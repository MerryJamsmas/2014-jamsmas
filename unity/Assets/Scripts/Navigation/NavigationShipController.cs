using UnityEngine;
using System.Collections;

public class NavigationShipController : MonoBehaviour {

	public static float m_maxFuel { get { return 100f; } }
	public static float m_minFuel { get { return 0f; } }
	private float m_remainingFuel = m_maxFuel;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey ("q")) {
			ReduceFuel(1f);
		}
	}

	public void ReduceFuel(float delta) {
		m_remainingFuel = Mathf.Max (0f, m_remainingFuel - delta);
	}

	public float GetRangeOnRemainingFuel() {
		// TODO: make this dependent on the mass of the ship!
		return m_remainingFuel;
	}

	public float GetRangeOnMaxFuel() {
		// TODO: make this dependent on the mass of the ship!
		return m_maxFuel;
	}
}
