using UnityEngine;
using System.Collections;

public class Reticule : MonoBehaviour {
	
	public GameObject target;
	public GameObject shipGameObject { get; set; }

	// Use this for initialization
	void Start () {
		shipGameObject = GameObject.FindGameObjectWithTag ("NavigationShip");
		if (shipGameObject == null) {
			Debug.LogError("Could not find NavigationShip GameObject by tag.");
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (target != null){
			this.transform.localRotation = Quaternion.identity;
			this.transform.localPosition = new Vector3(0f, 1.1f, 0f);

			//determine if the target is to our right or our left. If it's to the left, need to take the -angle for our rotation
			float dir = target.transform.position.x > shipGameObject.transform.position.x? -1f:1f;


			this.transform.RotateAround(shipGameObject.transform.position, Vector3.forward, 
			                            180-dir*Vector3.Angle (Vector3.up, shipGameObject.transform.position-target.transform.position));
		}
	}

}
