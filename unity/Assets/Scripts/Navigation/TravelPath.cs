using UnityEngine;
using System.Collections;

public class TravelPath : MonoBehaviour {

	private LineRenderer m_line;
	private NavigationShip m_navigationShip;
	private NavigationMap m_navigationMap;

	// Use this for initialization
	void Start () {
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
		float radius = m_navigationShip.GetRangeOnRemainingFuel();

		if (Vector2.Distance(m_navigationMap.SceneToMap(this.transform.position),
		                     m_navigationMap.GetMouseMapCoordinates ()) <= radius) {
			m_line.enabled = true;
			m_line.SetPosition (0, this.transform.position);
			Vector3 coords = m_navigationMap.MapToScene(m_navigationMap.GetMouseMapCoordinates ());
			m_line.SetPosition (1, new Vector3(coords.x, coords.y, 0.0f));
		}
		else {
			m_line.enabled = false;
		}
	}
}
