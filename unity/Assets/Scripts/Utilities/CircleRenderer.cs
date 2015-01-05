using UnityEngine;
using System.Collections;

public class CircleRenderer : MonoBehaviour {

	public int segments;
	public float xradius;
	public float yradius;
	public float z;
		
	LineRenderer line;

	void Start () {
		line = gameObject.GetComponent<LineRenderer>();
		line.SetVertexCount (segments + 1);
		line.useWorldSpace = false;

		// TODO: move this code to be specific to the Navigation Rangeometer
		line.sortingLayerName = "NavigationForeground";
	}

	void Update () {
		CreatePoints ();
	}

	void CreatePoints () {
		float x;
		float y;

		// TODO: move this animating angle code to be specific to the Navigation Rangeometer
		float angle = Time.frameCount * -2.0f;

		for (int i = 0; i < (segments + 1); i++) {
			x = Mathf.Sin (Mathf.Deg2Rad * angle) * xradius;
			y = Mathf.Cos (Mathf.Deg2Rad * angle) * yradius;

			line.SetPosition (i,new Vector3(x,z,y) );
			angle += (360f / segments);
		}
	}
}
