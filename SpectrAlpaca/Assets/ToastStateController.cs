using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToastStateController : StateMachineBehaviour {

	// PRAGMA MARK - StateMachineBehaviour Lifecycle
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		this.Animator = animator;

		this.OnStateEntered();

		ToastController toast = animator.gameObject.GetComponent<ToastController> ();
		if (toast != null) {
			toast.OnStateChange (stateInfo);
		}

		this._active = true;
	}

	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		this.Animator = animator;

		this._active = false;
		this.OnStateExited();
	}

	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		this.OnStateUpdated();
	}


	// PRAGMA MARK - Internal
	private bool _active = false;

	protected Animator Animator { get; private set; }

	void OnDisable() {
		if (this._active) {
			this.OnStateExited();
		}
	}

	private void OnStateEntered() {
		
		//Animator.gameObject.transform.position.y += 0.5f * Time.deltaTime;
	}

	private void OnStateExited() {}
	private void OnStateUpdated() {}
}
