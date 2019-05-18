using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallPart : MonoBehaviour {
		public void StartAnimation(float waitingAnimation) {
			StartCoroutine(WaitAndAnimate(waitingAnimation));
		}
		private IEnumerator WaitAndAnimate(float waitingAnimation) {
			yield return new WaitForSeconds(waitingAnimation);
			GetComponent<Animation>().Play();
	}
}
