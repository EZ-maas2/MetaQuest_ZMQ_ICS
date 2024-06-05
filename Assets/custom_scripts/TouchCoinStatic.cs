using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity;
using System;

public static class TouchCoinStatic 
{
    // this script is not attached to any GameObject
    // it just helps us with subscribing to the coin state change from the ZMQ class

    public static event Action OnTouchedCoin;

    private static bool _coinState;

    public static bool coinState
    {
        get { return _coinState;}
        set {
            _coinState = value;
            if (_coinState)
            {
                Debug.Log("coin state got  switched to positive  --------------------");
                OnTouchedCoin?.Invoke();
                coinState = false; // immediately return the coin state back to false after sending the message to python
            }
        }
    }

}
