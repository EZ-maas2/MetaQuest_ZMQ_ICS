using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NetMQ;
using NetMQ.Sockets;
using System;
using System.Threading;


public class ZMQ : MonoBehaviour
{
    private string server_address = "tcp://127.0.0.1:5557"; // this is a standard localhost ip , change it to teh ip of your laptop
    public event Action<string> OnPythonResponse;
    private volatile bool threadRunning = true;
    private Thread thread;
    private RequestSocket socket; // we select Request socket because we are using the request - response communication pattern
    private volatile bool boolTouchedCoin = false;


    void Awake() // this executes when the game loads
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
            socket.Connect(server_address); // this connects us to the server that we created on our laptop
            while (threadRunning)
            {
                if (boolTouchedCoin)
                {
                    socket.SendFrame("Touched coin!"); // send message to laptop
                    boolTouchedCoin = false;
                    string message = socket.ReceiveFrameString(); // receive message back
                    OnPythonResponse?.Invoke(message.ToString());  // react to receiving a message back by notifying a subscriber in WhenPythonResponds.cs (this creates a  new coin)
                }
                else
                {
                    Thread.Sleep(100); // thread sleeps for 100 ms to reduce the CPU load
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
