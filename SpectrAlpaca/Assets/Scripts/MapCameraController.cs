using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCameraController : MonoBehaviour {
	void LateUpdate () {
		transform.rotation = Quaternion.LookRotation (Vector3.down, new Vector3(0.25f, 0f, 0.25f));
	}
}
