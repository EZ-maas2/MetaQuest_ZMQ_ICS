using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Unity;

public class TouchCoin : MonoBehaviour
{
    
    
    // this script is attached to the coin
    // Start is called before the first frame update
    void OnTriggerEnter(Collider collider)
    {
        if (collider.name  == "HandCollider")
        {
            Debug.Log($"correct tag -----------------------------");
            TouchCoinStatic.coinState = true;
            Destroy(GameObject.FindGameObjectWithTag("Coin"));

        }
    }

}
