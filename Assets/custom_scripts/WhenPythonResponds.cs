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
  void ReactToPython()
  {
    Destroy(GameObject.FindGameObjectsWithTag("Coin"));
    position = new Vector3(Random.Range(xRange), y, Random.Range(zRange));
    Instantiate(coinPrefab, position, Quaternion.identity);
  }


  OnDestroy(){
    zmq.OnPythonResponse -= ReactToPython;
  }
}
