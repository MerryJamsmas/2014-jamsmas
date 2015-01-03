using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public float m_panSpeed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 cameraPosition = this.transform.position;

		if (Input.GetAxis ("Horizontal") > 0) {
			// Pan camera right
			cameraPosition.x += m_panSpeed;
		}
		else if (Input.GetAxis ("Horizontal") < 0) {
			// Pan camera left
			cameraPosition.x -= m_panSpeed;
		}

		if (Input.GetAxis ("Vertical") > 0) {
			// Pan camera up
			cameraPosition.y += m_panSpeed;
		}
		else if (Input.GetAxis ("Vertical") < 0) {
			// Pan camera down
			cameraPosition.y -= m_panSpeed;
		}
		this.transform.position = cameraPosition;
	}
}
