using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToastController : MonoBehaviour {
	private string state;

	delegate void StateDelegate();
	private bool isFlying = false;

	private Dictionary<string, StateDelegate> stateFuncs = new Dictionary<string, StateDelegate> ();

	void Start () {
		stateFuncs.Add ("Flying", UpdateFlying);
	}

	void Update () {
		if (isFlying) {
			gameObject.transform.parent.position += new Vector3 (-3f, 0, 0) * Time.deltaTime;
		}
//		if (state != null && stateFuncs.ContainsKey (state)) {
//			stateFuncs [state] ();
//		}
	}

	public void OnStateChange(AnimatorStateInfo info) {
		foreach (string st in stateFuncs.Keys) {
			if (info.IsName (st)) {
				state = st;
				break;
			}
		}
	}


	private void UpdateFlying() {
		isFlying = true;
	}
}
