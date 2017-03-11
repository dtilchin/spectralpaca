using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PicaVoxel;

public class RainbowController : MonoBehaviour {
	public float timeNeeded = 1f;

	private GameController gameController;
	private Color colorEnd;
	private Material transitionMat;
	private float progress = 0f;
	private bool playerIsIn = false;
	private bool finished = false;

	void Start () {
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		gameController = gameControllerObject.GetComponent <GameController>();

		Volume volume = gameObject.GetComponent<Volume> ();
		colorEnd = volume.Material.GetColor("_Tint");
		transitionMat = new Material (volume.Material);
		volume.Material = transitionMat;
		volume.UpdateAllChunks ();
	}

	void Update () {
		if (finished) {
			return;
		}

		float mult = playerIsIn ? 1 : -1;

		progress += (Time.deltaTime * mult) / timeNeeded;

		if (progress >= 1f) {
			finished = true;
			gameController.OnScore ();
			//Destroy (gameObject);
		}

		progress = Mathf.Max (progress, 0f);

		transitionMat.SetColor("_Tint", Color.Lerp (Color.clear, colorEnd, progress));
	}
		
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag ("Player")) {
			playerIsIn = true;
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.gameObject.CompareTag ("Player")) {
			playerIsIn = false;
		}
	}
}
