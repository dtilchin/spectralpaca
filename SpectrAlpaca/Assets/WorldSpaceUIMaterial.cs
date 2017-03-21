using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpaceUIMaterial : MonoBehaviour {
	public Material noZTestMaterial;


	void Start () {
		gameObject.GetComponentInChildren<CanvasRenderer> ().SetMaterial (noZTestMaterial, null);
	}
}
