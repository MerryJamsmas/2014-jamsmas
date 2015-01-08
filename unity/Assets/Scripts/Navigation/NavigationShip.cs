using UnityEngine;
using System.Collections;

public class NavigationShip : MonoBehaviour {

	public static float m_maxFuel { get { return 50f; } }
	public static float m_minFuel { get { return 0f; } }
	public float m_remainingFuel = m_maxFuel;

	public Vector2 shipMovementDestination { get; private set; }
	public bool shipIsMoving { get; private set; }

	private NavigationMap m_navigationMap;
	private NavigationShipSprite m_navigationShipSprite;
	
	// Use this for initialization
	void Start () {
		m_navigationMap = GameObject.FindObjectOfType<NavigationMap> ();
		if (m_navigationMap == null) {
			Debug.LogError("Unable to find NavigationMap in NavigationShip");
		}

		m_navigationShipSprite = GameObject.FindObjectOfType<NavigationShipSprite> ();
		if (m_navigationShipSprite == null) {
			Debug.LogError("Unable to find NavigationShipSprite in NavigationShip");
		}

		shipMovementDestination = Vector2.zero;
		shipIsMoving = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey ("q")) {
			ReduceFuel(0.01f);
		}
		else if (Input.GetKey ("e")) {
			IncreaseFuel(0.01f);
		}
		else if (!shipIsMoving && Input.GetMouseButtonDown(0) && m_navigationMap.HasMouseCoordinates()) {
			Vector2 mapDestination = m_navigationMap.GetMouseMapCoordinates();
			Vector2 shipMapPosition = m_navigationMap.SceneToMap(transform.position);
			
			if (Vector2.Distance(mapDestination, shipMapPosition) < GetRangeOnRemainingFuel()) {
				SetShipDestination(mapDestination);
			}
		}
	}

	void FixedUpdate() {
		if (shipIsMoving && Vector2.Distance (shipMovementDestination, m_navigationMap.SceneToMap(this.transform.position)) < 0.05) {
			shipIsMoving = false;
			shipMovementDestination = Vector2.zero;
			m_navigationShipSprite.ClearRotation();
		}
	}

	public void ReduceFuel(float delta) {
		m_remainingFuel = Mathf.Max (0f, m_remainingFuel - delta);
	}

	public void IncreaseFuel(float delta) {
		m_remainingFuel = Mathf.Min (m_maxFuel, m_remainingFuel + delta);
	}

	// The travel range (in map units) on the ship's remaining tank of fuel
	public float GetRangeOnRemainingFuel() {
		// TODO: make this dependent on the mass of the ship!
		return m_remainingFuel;
	}

	// The travel range (in map units) on the ship's full tank of fuel
	public float GetRangeOnMaxFuel() {
		// TODO: make this dependent on the mass of the ship!
		return m_maxFuel;
	}

	private void SetShipDestination(Vector2 shipDestination) {
		shipMovementDestination = shipDestination;
		shipIsMoving = true;
		m_navigationShipSprite.RotateForDestination (shipDestination);
	}
}
