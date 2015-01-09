using UnityEngine;
using System.Collections;

public class TravelPath : MonoBehaviour {

	private CircleCollider2D m_collider;
	private LineRenderer m_line;
	private NavigationShip m_navigationShip;
	private NavigationMap m_navigationMap;

	// Use this for initialization
	void Start () {
		m_collider = GetComponent<CircleCollider2D> ();
		if (m_collider == null) {
			Debug.LogError("Could not find CircleCollider2D for TravelPath");
		}

		m_line = GetComponent<LineRenderer> ();
		if (m_line == null) {
			Debug.LogError("Could not find LineRenderer for TravelPath");
		}
		m_line.sortingLayerName = "NavigationForeground";

		m_navigationShip = FindObjectOfType<NavigationShip> ();
		if (m_navigationShip == null) {
			Debug.LogError("Could not find NavigationShip for TravelPath");
		}

		m_navigationMap = GameObject.FindObjectOfType<NavigationMap> ();
		if (m_navigationMap == null) {
			Debug.LogError("Unable to find NavigationMap for TravelPath");
		}
	}
	
	// Update is called once per frame
	void Update () {
		m_collider.radius = m_navigationShip.GetRangeOnRemainingFuel();
	}

	void OnMouseOver() {
		m_line.enabled = true;
		m_line.SetPosition (0, this.transform.position);
		Vector3 coords = m_navigationMap.MapToScene(m_navigationMap.GetMouseMapCoordinates ());
		m_line.SetPosition (1, new Vector3(coords.x, coords.y, 0.0f));
	}

	void OnMouseExit() {
		m_line.enabled = false;
	}
}
