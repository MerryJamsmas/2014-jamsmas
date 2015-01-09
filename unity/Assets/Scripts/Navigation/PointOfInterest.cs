using UnityEngine;
using System.Collections;

public class PointOfInterest : MonoBehaviour {

	private Behaviour m_halo;

	// Use this for initialization
	void Start () {
		m_halo = GetComponent ("Halo") as Behaviour;
	}

	void OnMouseEnter () {
		if (m_halo != null) {
			m_halo.enabled = true;
		}
	}

	void OnMouseExit () {
		if (m_halo != null) {
			m_halo.enabled = false;
		}
	}
}
