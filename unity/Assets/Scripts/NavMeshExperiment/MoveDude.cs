using UnityEngine;
using System.Collections;

public class MoveDude : MonoBehaviour {



	// Use this for initialization
	void Start () {
		//transform.rotation.Set (90, 45, 0, 1);
	
	}
	
	// Update is called once per frame
	void Update () {

	
	}

	void FixedUpdate ()
	{
		var camera = GameObject.FindGameObjectWithTag ("MainCamera");
		var pos = camera.camera.ScreenToWorldPoint (Input.mousePosition);
		if (Input.GetMouseButtonDown (0)) {
			var agent = GetComponent<NavMeshAgent> ();
			agent.SetDestination (new Vector3 (((int)pos.x)+0.5f, 0f, ((int)pos.z)+0.5f));

				
				}



	}
}
