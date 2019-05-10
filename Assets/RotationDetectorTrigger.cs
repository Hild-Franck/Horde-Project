using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationDetectorTrigger : MonoBehaviour {
    public bool triggered = false;
    private GameObject objectToRemove = null;

    public void HideObject() => objectToRemove.SetActive(false);
    public void RemoveObject() {
        Destroy(objectToRemove);
        Destroy(gameObject);
    }
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
