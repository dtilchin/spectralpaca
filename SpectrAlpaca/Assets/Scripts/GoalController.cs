using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PicaVoxel;

public class GoalController : MonoBehaviour {
	public GameObject goal;
	public GameObject goalFlag;
	public GameObject mapIcon;
	public GameObject progressBar;

	public float timeNeeded = 3f;

	private bool hasCreated = false;

	private GameController gameController;
	private Image progressBarImage;
	private Color colorEnd;
	private Material transitionMat;
	private float progress = 0f;
	private bool playerIsIn = false;
	private bool finished = false;

	void Start () {
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		gameController = gameControllerObject.GetComponent <GameController>();

		progressBarImage = progressBar.GetComponent<Image> ();

		// Set up rainbow
		Volume volume = goal.GetComponent<Volume> ();
		colorEnd = volume.Material.GetColor("_Tint");
		transitionMat = new Material (volume.Material);
		volume.Material = transitionMat;
		volume.UpdateAllChunks ();
	}



	void Update () {
		if (finished || !goal.activeSelf) {
			return;
		}

		float mult = playerIsIn ? 1 : -1;

		progress += (Time.deltaTime * mult) / timeNeeded;

		if (progress >= 1f) {
			finished = true;
			gameController.OnScore ();

			//progressBar.SetActive (false);

			//mapIcon.GetComponent<ColorCorrectionCurves> ().saturation = 0f;
		}

		progress = Mathf.Max (progress, 0f);

		progressBarImage.fillAmount = progress;

		transitionMat.SetColor("_Tint", Color.Lerp (Color.clear, colorEnd, progress));
	}


	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag ("Player")) {
			playerIsIn = true;
			if (!goal.activeSelf) {
				//Quaternion rot = Quaternion.Euler ( new Vector3(0f, transform.rotation.eulerAngles.y + 180f, 0f));
				goal.SetActive(true);
				Destroy (goalFlag.gameObject);
			}
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.gameObject.CompareTag ("Player")) {
			playerIsIn = false;
		}
	}
}
