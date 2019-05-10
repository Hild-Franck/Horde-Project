using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationDetectorTrigger : MonoBehaviour {
    public bool triggered = false;
    private Spike objectToRemove = null;

    public void HideObject() => objectToRemove.attachedGrahic.SetActive(false);
    public void RemoveObject() {
        objectToRemove.Remove();
        objectToRemove = null;
        triggered = false;
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Spike") {
            triggered = true;
            objectToRemove = other.GetComponent<Spike>();
        }
    }
    void OnTriggerExit(Collider other) {
        if (other.tag == "Spike") {
            triggered = false;
            objectToRemove.attachedGrahic.SetActive(true);
            objectToRemove = null;
        }
    }
}
