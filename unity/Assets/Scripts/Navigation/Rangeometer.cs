using UnityEngine;
using System.Collections;

public class Rangeometer : MonoBehaviour {

	private NavigationShip m_navigationShip;

	public int lineSegments;
	
	private LineRenderer line;
	private float startAngle = 0.0f;

	// Use this for initialization
	void Start () {
		m_navigationShip = FindObjectOfType<NavigationShip> ();
		if (m_navigationShip == null) {
			Debug.LogError("Could not find NavigationShip!");
		}

		line = gameObject.GetComponent<LineRenderer>();
		line.SetVertexCount (lineSegments + 1);
		line.useWorldSpace = false;
		line.sortingLayerName = "NavigationForeground";
	}

	void FixedUpdate () {
		startAngle -= 1.0f;
		CreatePoints ();
	}

	void CreatePoints () {
		float radius = m_navigationShip.GetRangeOnRemainingFuel();
		float angle = startAngle;

		for (int i = 0; i < (lineSegments + 1); i++) {
			float x = Mathf.Sin (Mathf.Deg2Rad * angle) * radius;
			float y = Mathf.Cos (Mathf.Deg2Rad * angle) * radius;
			
			line.SetPosition (i,new Vector3(x,0.0f,y) );
			angle += (360f / lineSegments);
		}
	}
}
