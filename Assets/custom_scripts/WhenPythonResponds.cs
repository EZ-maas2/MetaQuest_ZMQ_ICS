using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhenPythonResponds : MonoBehaviour
{
  public ZMQ zmq;
  public GameObject coinPrefab;
  public Vector2 xRange;
  public Vector2 zRange;
  public float y;

  void Awake(){
    // subscribe to the event in ZMQ that is triggered by the python response
    zmq.OnPythonResponse += ReactToPython;
  }

// this function will Destroy the current Coin object and create a new one somewhere else
  void ReactToPython(string msg)
  {
    Debug.Log($"message was {msg}==========================");
    
    Vector3 position = new Vector3(Random.Range(xRange.x, xRange.y), y, Random.Range(zRange.x, zRange.y));
    Quaternion randomRotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
    Instantiate(coinPrefab, position, randomRotation);
  }


  void  OnDestroy(){
    zmq.OnPythonResponse -= ReactToPython;
  }
}
