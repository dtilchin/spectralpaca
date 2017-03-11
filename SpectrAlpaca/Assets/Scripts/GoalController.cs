using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalController : MonoBehaviour {
	public GameObject goal;
	public GameObject goalFlag;

	private bool hasCreated = false;

	// Use this for initialization
	void Start () {
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag ("Player") && !hasCreated) {
			Quaternion rot = Quaternion.Euler ( new Vector3(0f, transform.rotation.eulerAngles.y + 180f, 0f));
			Instantiate (goal, transform.position, rot);
			hasCreated = true;

			Destroy (goalFlag.gameObject);
		}
	}
}
