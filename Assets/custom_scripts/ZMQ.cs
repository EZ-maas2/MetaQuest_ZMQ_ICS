using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NetMQ;
using NetMQ.Sockets;


public class ZMQ : MonoBehaviour
{
    public string server_address = "tcp://*:5556";
    public event Action OnPythonResponse;

    private volatile bool threadRunning = true;
    private Thread thread;
    private RequestSocket socket; // we select Request socket because we are using the request - response communication pattern
    private volatile bool boolTouchedCoin = false;


    void Awake()
    {
       // subscribe to an event in TouchCoinStatic that would inform us that we need to send something to Python
        TouchCoinStatic.OnTouchedCoin += ReactTouchedCoin;
        thread = new Thread(new ThreadStart(RequestFunc)); 
        thread.Start();
    }

    void RequestFunc()
    {
        AsyncIO.ForceDotNet.Force();
        socket.Options.SendHighWatermark = 1000;
        socket.Bind(server_address);
        while (threadRunning)
        {
            if (boolTouchedCoin)
            {
                socket.SendMoreFrame("Coin").SendFrame("Touched coin!");
                boolTouchedCoin = false;
                message = socket.ReceiveMessage();
                if (message == "1")
                {
                    OnPythonResponse?.Invoke();
                }
            }

        }
            
    }


    void ReactTouchedCoin(){
        boolTouchedCoin = true;
    }

    void OnDestroy()
    {
        threadRunning = false;
        if (thread != null && thread.IsAlive)
        {
            thread.Interrupt();
            thread.Join();
        }
        NetMQConfig.Cleanup();
        TouchCoinStatic.OnTouchedCoin -= ReactTouchedCoin; // Unsubscribe to prevent leaks
    }
}
