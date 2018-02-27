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
	public GameObject completedIcon;

	public float timeNeeded = 3f;

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

			progressBar.SetActive (false);
			completedIcon.SetActive (true);

			Renderer mapRenderer = mapIcon.GetComponent<Renderer>();
			Material iconMat = new Material (mapRenderer.material);
			Color mapColor = iconMat.color;
			mapColor.a = 0.25f;
			iconMat.color = mapColor;
			mapRenderer.material = iconMat;
		}

		progress = Mathf.Max (progress, 0f);

		progressBarImage.fillAmount = progress;

		transitionMat.SetColor("_Tint", Color.Lerp (Color.clear, colorEnd, progress));
	}


	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag ("Player")) {
			playerIsIn = true;
			if (!goal.activeSelf) {
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
