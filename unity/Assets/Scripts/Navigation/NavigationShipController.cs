using UnityEngine;
using System.Collections;

public class NavigationShipController : MonoBehaviour {

	public static float m_maxFuel { get { return 50f; } }
	public static float m_minFuel { get { return 0f; } }
	public float m_remainingFuel = m_maxFuel;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey ("q")) {
			ReduceFuel(0.01f);
		}
		else if (Input.GetKey ("e")) {
			IncreaseFuel(0.01f);
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
}
