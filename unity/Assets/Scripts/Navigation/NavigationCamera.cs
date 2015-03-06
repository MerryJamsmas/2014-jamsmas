using UnityEngine;
using System.Collections;

public class NavigationCamera : MonoBehaviour {

	public float zoomSpeed;
	private float minCameraSize = 2.0f;
	private float maxCameraSize = 20.0f;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		// TODO: Explore ways to make this camera scroll feel more "zoomy" or smoother
		// Maybe add some inertia too the camera movement too make it feel less snappy?

		if (Input.GetAxis("Mouse ScrollWheel") > 0.0f) {
			GetComponent<Camera>().orthographicSize = Mathf.Max (GetComponent<Camera>().orthographicSize - zoomSpeed * Input.GetAxis("Mouse ScrollWheel"), minCameraSize);

		}
		else if (Input.GetAxis("Mouse ScrollWheel") < 0.0f) {
			GetComponent<Camera>().orthographicSize = Mathf.Min (GetComponent<Camera>().orthographicSize - zoomSpeed * Input.GetAxis("Mouse ScrollWheel"), maxCameraSize);
		}
	}
}
