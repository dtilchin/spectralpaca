using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour {
	public float speed;
	public float ttl;

	private float createdAt;

	void Start () {
		Rigidbody rb = GetComponent<Rigidbody> ();
		rb.velocity = transform.forward * speed;

		createdAt = Time.time;
	}

	void Update() {
		if (Time.time > createdAt + ttl) {
			Destroy (gameObject);
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag ("Enemy")) {
			other.gameObject.GetComponent<EnemyController> ().OnHit ();
			Destroy (gameObject);
		} else if (other.gameObject.CompareTag ("Map")) {
			Destroy (gameObject);
		}
	}
}
