using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnPathScript : MonoBehaviour {

	private EditorPathScript PathToFollow;
	public int CurrentWayPointID = 0;
	public float speed;
	private float reachDistance = 1.0f;
//	public float rotationSpeed = 5.0f;

	Vector3 last_position;
	Vector3 current_position;
	private bool backward = false;
	// Use this for initialization
	void Start () {
		last_position = transform.position;
		PathToFollow = GameObject.FindWithTag (this.gameObject.name).GetComponent<EditorPathScript>();
	}

	// Update is called once per frame
	void Update () {
		float distance = Vector3.Distance (PathToFollow.path_objs [CurrentWayPointID].position, transform.position);
		transform.position = Vector3.MoveTowards (transform.position, PathToFollow.path_objs [CurrentWayPointID].position, Time.deltaTime * speed);

		if (distance <= reachDistance) {
			if (backward && CurrentWayPointID != 0)
				CurrentWayPointID--; // go backward
			else 
			{
				backward = false;
				CurrentWayPointID++; // go forward
			}
		}

		if (CurrentWayPointID >= PathToFollow.path_objs.Count) 
		{
			CurrentWayPointID = PathToFollow.path_objs.Count - 2; // reach the end point
			backward = true;
		}
	}
}
