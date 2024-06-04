using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity;

public class TouchCoin : MonoBehaviour
{
    
    // this script is attached to the coin
    // Start is called before the first frame update
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Hand")){
            TouchCoinStatic.coinState = true;
        }
    }

}
