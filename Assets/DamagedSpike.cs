using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagedSpike : MonoBehaviour {
  public GameObject spikeTop;
  void Start() {
    spikeTop.GetComponent<Rigidbody>().AddTorque((Vector3.right * 75) + Vector3.forward * Random.Range(-50f, 50f));
  }
}
