using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationDetectorTrigger : MonoBehaviour {
    public bool triggered = false;
    private GameObject objectToRemove = null;

    public void RemoveObject() => objectToRemove.SetActive(false);
    void OnTriggerEnter(Collider other) {
        if (other.tag == "Spike") {
            triggered = true;
            objectToRemove = other.GetComponent<Spike>().attachedGrahic;
        }
    }
    void OnTriggerExit(Collider other) {
        if (other.tag == "Spike") {
            triggered = false;
            objectToRemove.SetActive(true);
            objectToRemove = null;
        }
    }
}
