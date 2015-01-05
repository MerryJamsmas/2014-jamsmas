using UnityEngine;
using System.Collections;

public class RangeometerController : MonoBehaviour {

	private NavigationShipController m_navigationShipController;

	public int lineSegments;
	
	LineRenderer line;

	// Use this for initialization
	void Start () {
		m_navigationShipController = FindObjectOfType<NavigationShipController> ();
		if (m_navigationShipController == null) {
			Debug.LogError("Could not find NavigationShipController!");
		}

		line = gameObject.GetComponent<LineRenderer>();
		line.SetVertexCount (lineSegments + 1);
		line.useWorldSpace = false;
		line.sortingLayerName = "NavigationForeground";
	}
	
	// Update is called once per frame
	void Update () {
		CreatePoints ();
	}

	void CreatePoints () {
		float radius = m_navigationShipController.GetRangeOnRemainingFuel();
		float angle = Time.frameCount * -2.0f;

		for (int i = 0; i < (lineSegments + 1); i++) {
			float x = Mathf.Sin (Mathf.Deg2Rad * angle) * radius;
			float y = Mathf.Cos (Mathf.Deg2Rad * angle) * radius;
			
			line.SetPosition (i,new Vector3(x,0.0f,y) );
			angle += (360f / lineSegments);
		}
	}
}
