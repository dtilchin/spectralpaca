using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowerBoundController : MonoBehaviour {
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag ("Player")) {
			other.transform.position = new Vector3 (0, 10, 0);
		} else if (other.gameObject.CompareTag ("Enemy")) {
			Destroy (other.gameObject);
		}
	}
}
