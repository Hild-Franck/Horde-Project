using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
   public GameObject attachedGrahic;

   public void Remove() {
      Destroy(attachedGrahic);
      Destroy(gameObject);
   }
}
