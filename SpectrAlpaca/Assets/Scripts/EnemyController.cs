﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using PicaVoxel;

public class EnemyController : MonoBehaviour {
	public float speed = 2;
	public float attackRate = 1;

	private GameObject player;
	private CharacterController cc;
	private GameController gameController;


	// Attacking
	private bool isAttacking = false;
	private float lastAttack = 0;

	// Pathfinding
	private Path path;
	private Seeker seeker;
	private int currentWaypoint = 0;

	public float repathRate = 0.5f;
	private float lastRepath = 0;
	public float nextWaypointDistance = 3;

	void Start () {
		player = GameObject.FindWithTag("Player");
		cc = GetComponent<CharacterController> ();
		seeker = GetComponent<Seeker>();

		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		gameController = gameControllerObject.GetComponent <GameController>();
	}

	public void Update() {
		// Face player
		Vector3 direction = (player.transform.position - transform.position).normalized;
		// Only rotate around y axis; don't pitch up/down
		direction.y = 0;
		transform.rotation = Quaternion.LookRotation (direction) * Quaternion.Euler (0, 90, 0);

		UpdateAttack ();
		UpdatePath ();
	}

	private void UpdatePath() {
		if (Time.time - lastRepath > repathRate && seeker.IsDone ()) {
			lastRepath = Time.time + Random.value * repathRate * 0.5f;
			seeker.StartPath (transform.position, player.transform.position, OnPathComplete);
		}

		if (path == null) {
			return;
		}

		if (currentWaypoint > path.vectorPath.Count) {
			return;
		}
		if (currentWaypoint == path.vectorPath.Count) {
			currentWaypoint++;
			return;
		}

		// Direction to the next waypoint
		Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
		cc.SimpleMove(dir * speed);

		if ((transform.position - path.vectorPath[currentWaypoint]).sqrMagnitude < nextWaypointDistance*nextWaypointDistance) {
			currentWaypoint++;
			return;
		}
	}


	private void UpdateAttack() {
		if (isAttacking) {
			if (Time.time - lastAttack > attackRate) {
				gameController.OnHitPlayer ();
				lastAttack = Time.time;
			}
		}
	}


	public void OnHit() {
		GetComponent<Exploder>().Explode();
		Destroy (gameObject);
	}


	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag ("Player")) {
			isAttacking = true;
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.gameObject.CompareTag ("Player")) {
			isAttacking = false;
		}
	}


	public void OnPathComplete (Path p) {
		if (p.error) {
			Debug.Log ("Error  " + p.errorLog);
		} else {
			path = p;
			currentWaypoint = 0;
		}
	}


	void _SimplePlayerApproach_Update () {
		// Move towards player
		Vector3 dir = (player.transform.position - transform.position).normalized;
		dir.y = -1.0f; // gravity
		dir *= speed;
		cc.SimpleMove (dir);

		// Face player
		Vector3 direction = (player.transform.position - transform.position).normalized;
		// Only rotate around y axis; don't pitch up/down
		direction.y = 0;
		transform.rotation = Quaternion.LookRotation (direction) * Quaternion.Euler(0, 270, 0);
	}
}
