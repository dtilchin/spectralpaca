using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	public float speed;
	public float gravity = 30f;
	public float mouseOffsetY;
	public GameObject projectile;
	public GameObject enemy;
	public GameObject output;

	public float fireRate;

	private CharacterController cc;
	private float nextFire;

	void Start() {
		cc = GetComponent<CharacterController> ();
	}

	void Update() {
		Vector2 positionOnScreen = Camera.main.WorldToViewportPoint (transform.position);
		Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);
		float angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);
		transform.rotation =  Quaternion.Euler (new Vector3(0f, mouseOffsetY-angle, 0f));


		if (Input.GetButton ("Fire1") && Time.time > nextFire) {
			nextFire = Time.time + fireRate;
			Vector3 pos = new Vector3 (output.transform.position.x, output.transform.position.y, output.transform.position.z);
			Quaternion rot = Quaternion.Euler ( new Vector3(0f, transform.rotation.eulerAngles.y, 0f));
			Instantiate (projectile, pos, rot);
		}

		UpdateMotion ();
	}

	float AngleBetweenTwoPoints(Vector3 a, Vector3 b) {
		return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
	}

	void UpdateMotion() {
		Vector3 baseVec = new Vector3 (Input.GetAxis ("Horizontal"), 0.0f, Input.GetAxis ("Vertical"));
		Vector3 movement = Quaternion.Euler(new Vector3(0f, 45f, 0f)) * baseVec;
		movement.y -= gravity * Time.deltaTime;

		cc.Move(movement * Time.deltaTime * speed);
	} 
		

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag ("Pickup")) {
			other.gameObject.SetActive (false);
		}
	}
}
