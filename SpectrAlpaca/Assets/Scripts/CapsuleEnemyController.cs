using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class CapsuleEnemyController : MonoBehaviour {
	public float speed;

	private GameObject player;
	private CharacterController cc;


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
	}

	public void Update() {
		// Face player
		Vector3 direction = (player.transform.position - transform.position).normalized;
		// Only rotate around y axis; don't pitch up/down
		direction.y = 0;
		transform.rotation = Quaternion.LookRotation (direction) * Quaternion.Euler(0, 270, 0);


		if (Time.time - lastRepath > repathRate && seeker.IsDone ()) {
			lastRepath = Time.time + Random.value*  repathRate * 0.5f;
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
		Vector3 dir = (path.vectorPath[currentWaypoint]-transform.position).normalized;
		dir *= speed;
		// Note that SimpleMove takes a velocity in meters/second, so we should not multiply by Time.deltaTime
		cc.SimpleMove(dir);

		// The commented line is equivalent to the one below, but the one that is used
		// is slightly faster since it does not have to calculate a square root
		//if (Vector3.Distance (transform.position,path.vectorPath[currentWaypoint]) < nextWaypointDistance) {
		if ((transform.position-path.vectorPath[currentWaypoint]).sqrMagnitude < nextWaypointDistance*nextWaypointDistance) {
			currentWaypoint++;
			return;
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


	void _Update () {
		// Move towards player
		Vector3 dir = (player.transform.position - transform.position).normalized;
		dir.y = -1.0f;
		dir *= speed;
		cc.SimpleMove (dir);

		// Face player
		Vector3 direction = (player.transform.position - transform.position).normalized;
		// Only rotate around y axis; don't pitch up/down
		direction.y = 0;
		transform.rotation = Quaternion.LookRotation (direction) * Quaternion.Euler(0, 270, 0);
	}
}
