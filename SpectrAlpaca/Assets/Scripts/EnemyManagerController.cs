using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManagerController : MonoBehaviour {
	public GameObject enemy;
	public GameObject spawnPointsParent;
	public float spawnDelay = 2f;
	public float spawnRate = 10f;

	void Start () {
		InvokeRepeating ("Spawn", spawnDelay, spawnRate);
	}
	

	void Spawn () {
		Transform spawn = spawnPointsParent.transform.GetChild (Random.Range (0, spawnPointsParent.transform.childCount)).transform;
		Instantiate (enemy, spawn.position, spawn.rotation);
	}
}
