using UnityEngine;
using System.Collections;

public class RangeometerController : MonoBehaviour {

	private NavigationShipController m_navigationShipController;

	// Use this for initialization
	void Start () {
		m_navigationShipController = FindObjectOfType<NavigationShipController> ();
		if (m_navigationShipController == null) {
			Debug.LogError("Could not find NavigationShipController!");
		}
	}
	
	// Update is called once per frame
	void Update () {
		// TODO: Fuel minimum should not draw as a circle of radius 0, it should draw as a circle that
		// nicely fits the ship

		// TODO: Also, fix "range / 50" to be a representation of the actual distance the player can travel
		// on the map with the remaining fuel

		float range = m_navigationShipController.GetRangeOnRemainingFuel();
		this.transform.localScale = new Vector3(range / 50, this.transform.localScale.y, range / 50);
	}
}
