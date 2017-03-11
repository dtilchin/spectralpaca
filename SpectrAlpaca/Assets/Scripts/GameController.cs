using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

public class GameController : MonoBehaviour {
	public Text scoreText;
	public Text healthText;
	public GameObject player;
	public GameObject spawnPointsParent;
	public GameObject goal;

	private int score = 0;
	private float playerHealth = 1;

	void Start () {
		SpawnGoal ();
	}

	private void SpawnGoal() {
		if (spawnPointsParent.transform.childCount == 0) {
			scoreText.text = "You am winner";
			RespawnPlayer ();
			return;
		}

		Transform spawn = spawnPointsParent.transform.GetChild (Random.Range (0, spawnPointsParent.transform.childCount)).transform;
		Instantiate (goal, spawn.position, spawn.rotation);
		Destroy (spawn.gameObject);
	}

	public void OnScore() {
		score += 1;
		scoreText.text = "Rainbows: " + score;

		SpawnGoal ();
	}

	public void OnHitPlayer() {
		playerHealth -= 0.1f;

		if (playerHealth <= 0) {
			// Game over! Reset health and drop from the sky
			RespawnPlayer();
		}

		healthText.text = "Health: " + (playerHealth * 100);

		Camera.main.GetComponent<ColorCorrectionCurves> ().saturation = playerHealth;
	}


	private void RespawnPlayer() {
		playerHealth = 1f;
		player.transform.position = new Vector3 (0, 15, 0);
	}
}
