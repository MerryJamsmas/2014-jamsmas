using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NavigationController : MonoBehaviour 
{
	public List<AsteroidField> allAsteroidFields { get; private set; }

    public GameObject asteroidPrefab;
	private GameObject asteroidParent;

	private NavigationShip m_navigationShip;
	private NavigationMap m_navigationMap;
	private Camera m_camera;

	// Use this for initialization
	void Start () {

		asteroidParent = GameObject.FindGameObjectWithTag ("AsteroidParentGameObject");
		if (asteroidParent == null) {
			Debug.LogError ("Unable to find AsteroidParentGameObject in NavigationController");
		}

		m_navigationShip = GameObject.FindObjectOfType<NavigationShip> ();
		if (m_navigationShip == null) {
			Debug.LogError("Unable to find NavigationShip in NavigationController");
		}

		m_navigationMap = GameObject.FindObjectOfType<NavigationMap> ();
		if (m_navigationMap == null) {
			Debug.LogError("Unable to find NavigationMap in NavigationController");
		}

		GameObject navigationCamera = GameObject.FindGameObjectWithTag("NavigationCamera");
		if (navigationCamera != null) {
			m_camera = navigationCamera.camera;
		}

		MakeAsteroidFields ();
		InstantiateAllAsteroids ();
	}

	public void ShowNavigationView () {
		m_camera.enabled = true;
	}

	public void HideNavigationView () {
		m_camera.enabled = false;
	}

    private void MakeAsteroidFields()
    {
        AsteroidField a;
        allAsteroidFields = new List<AsteroidField>();

        a = new AsteroidField(10.0f, 10.0f, 5.0f, 20);
        allAsteroidFields.Add(a);
        
        a = new AsteroidField(-5.0f, 5.0f, 3.0f, 10);
        allAsteroidFields.Add(a);
        
        a = new AsteroidField(0.0f, -10.0f, 8.0f, 200);
        allAsteroidFields.Add(a);
	}

	public void InstantiateAsteroid(Asteroid asteroid) {
		asteroid.instantiate(Instantiate(asteroidPrefab, m_navigationMap.MapToScene(asteroid.pos), asteroid.rot) as GameObject);
		asteroid.me.transform.parent = asteroidParent.transform;
	}

	private void InstantiateAllAsteroids()
    {
        foreach (AsteroidField af in allAsteroidFields) {
			foreach (Asteroid a in af.allAsteroids) {
				a.instantiate (Instantiate (asteroidPrefab, a.pos, a.rot) as GameObject);
				a.me.transform.parent = asteroidParent.transform;
			}
		}
    }
}
