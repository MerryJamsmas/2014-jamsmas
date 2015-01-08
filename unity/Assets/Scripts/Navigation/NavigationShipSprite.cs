using UnityEngine;
using System.Collections;

public class NavigationShipSprite : MonoBehaviour {

	// The parameters of the halo aren't exposed to scripts :(
	// All we could do via code is turn the Halo on and off;
	// As an alternate option for a more complex Halo, we could use a point light with a Halo

	// private Component m_halo;

	private NavigationShip m_navigationShip;
	private NavigationController m_navigationController;
	private NavigationMap m_navigationMap;
	private Animator m_animator;	

	void Start () {
		m_navigationShip = FindObjectOfType<NavigationShip> ();
		if (m_navigationShip == null) {
			Debug.LogError("Could not find NavigationShip in NavigationShipSprite");
		}

		m_navigationController = GameObject.FindObjectOfType<NavigationController> ();
		if (m_navigationController == null) {
			Debug.LogError("Could not find NavigationController in NavigationShipSprite");
		}

		m_navigationMap = GameObject.FindObjectOfType<NavigationMap> ();
		if (m_navigationMap == null) {
			Debug.LogError("Could not find NavigationMap in NavigationShipSprite");
		}

		m_animator = GetComponent<Animator> ();
		if (m_animator == null) {
			Debug.LogError("Could not find Animator component of NavigationShipSprite");
		}
	}
	
	// Update is called once per frame
	void Update () {
		m_animator.SetBool("thrust", m_navigationShip.shipIsMoving);
	}

	public void RotateForDestination(Vector2 destination) {
		transform.localRotation = Quaternion.identity;
		float dir = m_navigationMap.MapToScene(destination).x > transform.position.x? -1f:1f;
		transform.Rotate(Vector3.forward, dir * Vector2.Angle(Vector2.up, destination - m_navigationMap.SceneToMap(transform.position)));
	}

	public void ClearRotation() {
		transform.localRotation = Quaternion.identity;
	}
}
