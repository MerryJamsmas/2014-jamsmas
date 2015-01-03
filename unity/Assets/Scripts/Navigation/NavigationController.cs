using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NavigationController : MonoBehaviour 
{

    List<AsteroidField> allAsteroidFields;

    public GameObject asteroidPrefab;

	public float cameraPanSpeed;
	private Camera m_camera;

	// The coordinates of the centre of the portion of the map currently displayed in the scene
	public Vector2 m_mapCentre = new Vector2(0, 0);

	// Use this for initialization
	void Start () {
		GameObject cameraGameObject = GameObject.FindGameObjectWithTag ("NavigationCamera");
		if (cameraGameObject == null) {
			Debug.Log ("Unable to find camera game object in navigation controller");
		}
		else{
			m_camera = cameraGameObject.camera;
			if(m_camera == null){
				Debug.Log ("Unable to extract camera object in navigation controller");
			}
		}

		makeAsteroidFields ();
		instantiateAllAsteroids ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			Vector3 objective = m_camera.ScreenToWorldPoint(Input.mousePosition);
			GameObject shipGameObject = GameObject.FindGameObjectWithTag ("NavigationShip");
			if (shipGameObject != null) {
				MoveShip (shipGameObject.transform.position.x - objective.x, shipGameObject.transform.position.y - objective.y);
			}
		}
		else if (Input.GetAxis("Horizontal") != 0.0f || Input.GetAxis("Vertical") != 0.0f) {
			MoveCamera (cameraPanSpeed * Input.GetAxis("Horizontal"), cameraPanSpeed * Input.GetAxis("Vertical"));
		}
	}

	private void MoveCamera(float deltaX, float deltaY) {
		TranslateMap (deltaX, deltaY);
		TranslateAsteroidGameObjects (deltaX, deltaY);
		TranslateShipGameObjectInScene (deltaX, deltaY);
	}

	private void MoveShip(float deltaX, float deltaY){
		TranslateMap (deltaX, deltaY);
		TranslateAsteroidGameObjects (deltaX, deltaY);
	}

	private void TranslateMap(float deltaX, float deltaY) {
		m_mapCentre.x -= deltaX;
		m_mapCentre.y -= deltaY;
	}

	private void TranslateShipGameObjectInScene(float deltaX, float deltaY) {
		GameObject shipGameObject = GameObject.FindGameObjectWithTag ("NavigationShip");
		if (shipGameObject != null) {
			TranslateGameObjectInScene (shipGameObject, deltaX, deltaY);
		}
	}

	private void TranslateAsteroidGameObjects (float deltaX, float deltaY) {
		foreach (AsteroidField asteroidField in allAsteroidFields) {
			foreach (Asteroid asteroid in asteroidField.allAsteroids) {
				if (asteroid.me) {
					TranslateGameObjectInScene(asteroid.me, deltaX, deltaY);
				}
				else if (MapCoordinatesAreInScene(asteroid.pos)) {
					asteroid.instantiate(Instantiate(asteroidPrefab, MapToScene(asteroid.pos), asteroid.rot) as GameObject);
				}
			}
		}
	}

	private bool MapCoordinatesAreInScene(Vector2 mapCoordinates) {
		return mapCoordinates.x > (m_mapCentre.x + NavigationConstants.mapEdgeLeft)
			&& mapCoordinates.x < (m_mapCentre.x + NavigationConstants.mapEdgeRight)
			&& mapCoordinates.y > (m_mapCentre.y + NavigationConstants.mapEdgeBottom)
			&& mapCoordinates.y < (m_mapCentre.y + NavigationConstants.mapEdgeTop);
	}

	public Vector3 MapToScene(Vector2 mapCoordinates) {
		return new Vector3 (mapCoordinates.x - m_mapCentre.x, mapCoordinates.y - m_mapCentre.y, 0.0f);
	}

	public Vector2 SceneToMap(Vector3 sceneCoordinates) {
		return new Vector2 (sceneCoordinates.x + m_mapCentre.x, sceneCoordinates.y + m_mapCentre.y);
	}

	private void TranslatePoiGameObjectsInScene(float deltaX, float deltaY) {
		GameObject[] poiGameObjects = GameObject.FindGameObjectsWithTag ("NavigationPOI");
		foreach (GameObject poiGameObject in poiGameObjects) {
			TranslateGameObjectInScene(poiGameObject, deltaX, deltaY);
		}
	}

	private void TranslateGameObjectInScene(GameObject gObject, float deltaX, float deltaY) {
		Vector3 position = gObject.transform.position;
		
		// Apply translation to gameobject to simulate camera movement
		position.x += deltaX;
		position.y += deltaY;
		gObject.transform.position = position;
		
		// Destroy game object if outside of bounds of map area
		if (position.x > NavigationConstants.mapEdgeRight || position.x < NavigationConstants.mapEdgeLeft 
		    || position.y > NavigationConstants.mapEdgeTop || position.y < NavigationConstants.mapEdgeBottom)
		{
			// TODO: Make sure that destroying a gameobject in this iterator doesn't create problems
			GameObject.Destroy(gObject);
		}
	}

    void makeAsteroidFields()
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
	
	void instantiateAllAsteroids()
    {
        foreach(AsteroidField af in allAsteroidFields)
            foreach(Asteroid a in af.allAsteroids)
                a.instantiate(Instantiate(asteroidPrefab, a.pos, a.rot) as GameObject);
    }
}
