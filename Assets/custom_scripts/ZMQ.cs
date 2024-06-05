using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NetMQ;
using NetMQ.Sockets;
using System;
using System.Threading;


public class ZMQ : MonoBehaviour
{
    private string server_address = "tcp://192.168.178.101:5557"; 
    public event Action<string> OnPythonResponse;
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
        using (socket = new RequestSocket())
            {
            socket.Connect(server_address);
            while (threadRunning)
            {
                if (boolTouchedCoin)
                {
                    socket.SendFrame("Touched coin!");
                    boolTouchedCoin = false;
                    string message = socket.ReceiveFrameString();
                    OnPythonResponse?.Invoke(message.ToString());
                    
                }
                else
                {
                    Thread.Sleep(100); 
                }

            }
            }
            
    }


    void ReactTouchedCoin(){
        Debug.Log("detected before the thread");
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
