using UnityEngine;
using System.Collections;

public class NavigationMap : MonoBehaviour {
	
	// These constants represent the bounds of the area in the scene
	// in which we allow GameObjects to exist.  If a GameObject travels
	// beyond these bounds, we Destroy it, and reinstantiate it if it needs
	// to move back inside this area.
	private const float m_sceneEdge = 20;
	public float sceneEdgeLeft { get { return -m_sceneEdge; } }
	public float sceneEdgeRight { get { return m_sceneEdge;} }
	public float sceneEdgeTop { get { return m_sceneEdge;} }
	public float sceneEdgeBottom { get { return -m_sceneEdge;} }

	// These contants represent the bounds of the map in map coordinates.
	// An object should not be permitted to move beyond the bounds of the map.
	private const float m_mapEdge = 25;
	public float mapEdgeLeft { get { return -m_mapEdge; } }
	public float mapEdgeRight { get { return m_mapEdge;} }
	public float mapEdgeTop { get { return m_mapEdge;} }
	public float mapEdgeBottom { get { return -m_mapEdge;} }

	private Vector2 m_mapCentre = Vector2.zero;
	public Vector2 mapCentre { get { return m_mapCentre; } }

	public float mouseScrollSpeed;
	public float keyScrollSpeed;

	private Camera m_camera;
	private NavigationShip m_navigationShip;
	private NavigationController m_navigationController;

	void Start() {
		GameObject cameraGameObject = GameObject.FindGameObjectWithTag ("NavigationCamera");
		if (cameraGameObject == null) {
			Debug.LogError ("Unable to find camera game object in NavigationMap");
		}
		else{
			m_camera = cameraGameObject.camera;
			if(m_camera == null){
				Debug.LogError ("Unable to extract camera object in NavigationMap");
			}
		}

		m_navigationController = FindObjectOfType<NavigationController> ();
		if (m_navigationController == null) {
			Debug.LogError ("Unable to find NavigationController in NavigationMap");
		}

		m_navigationShip = FindObjectOfType<NavigationShip> ();
		if (m_navigationShip == null) {
			Debug.LogError ("Unable to find NavigationShip in NavigationMap");
		}
	}

	void Update () {
		if (!m_navigationShip.shipIsMoving) {
			if (Input.GetAxis("Horizontal") != 0.0f || Input.GetAxis("Vertical") != 0.0f) {
				ScrollMap (keyScrollSpeed * Input.GetAxis("Horizontal"), keyScrollSpeed * Input.GetAxis("Vertical"));
			}
			else if (Input.GetMouseButton(2)) {
				ScrollMap (mouseScrollSpeed * Input.GetAxis("Mouse X"), mouseScrollSpeed * Input.GetAxis("Mouse Y"));
			}
		}
	}

	void FixedUpdate() {
		if (m_navigationShip.shipIsMoving) {
			GameObject shipGameObject = GameObject.FindGameObjectWithTag ("NavigationShip");
			if (shipGameObject != null) {
				// Move the ship one step towards its current destination
				Vector2 shipMapPosition = SceneToMap(shipGameObject.transform.position);
				Vector2 hopObjective = Vector2.MoveTowards(shipMapPosition, m_navigationShip.shipMovementDestination, 0.1f);
				MoveShip (shipMapPosition.x - hopObjective.x, shipMapPosition.y - hopObjective.y);
			}
		}
	}

	public bool MapCoordinatesAreInScene(Vector2 mapCoordinates) {
		return mapCoordinates.x > (m_mapCentre.x + sceneEdgeLeft)
			&& mapCoordinates.x < (m_mapCentre.x + sceneEdgeRight)
				&& mapCoordinates.y > (m_mapCentre.y + sceneEdgeBottom)
				&& mapCoordinates.y < (m_mapCentre.y + sceneEdgeTop);
	}
	
	public Vector3 MapToScene(Vector2 mapCoordinates) {
		return new Vector3 (mapCoordinates.x - m_mapCentre.x, mapCoordinates.y - m_mapCentre.y, 0.0f);
	}
	
	public Vector2 SceneToMap(Vector3 sceneCoordinates) {
		return new Vector2 (sceneCoordinates.x + m_mapCentre.x, sceneCoordinates.y + m_mapCentre.y);
	}

	public bool SceneCoordinatesAreInSceneBounds(Vector3 sceneCoordinates) {
		return sceneCoordinates.x <= sceneEdgeRight && sceneCoordinates.x >= sceneEdgeLeft 
			&& sceneCoordinates.y <= sceneEdgeTop && sceneCoordinates.y >= sceneEdgeBottom;
	}

	public bool MapCoordinatesAreInMapBounds(Vector2 mapCoordinates) {
		return mapCoordinates.x <= mapEdgeRight && mapCoordinates.x >= mapEdgeLeft 
			&& mapCoordinates.y <= mapEdgeTop && mapCoordinates.y >= mapEdgeBottom;
	}

	public void TranslateMap(float deltaX, float deltaY) {
		m_mapCentre.x -= deltaX;
		m_mapCentre.y -= deltaY;
	}

	public bool HasMouseCoordinates() {
		return m_camera.pixelRect.Contains (Input.mousePosition);
	}

	public Vector3 GetMouseSceneCoordinates() {
		return m_camera.ScreenToWorldPoint (Input.mousePosition);
	}

	public Vector2 GetMouseMapCoordinates() {
		return SceneToMap(m_camera.ScreenToWorldPoint (Input.mousePosition));
	}
	
	private void ScrollMap(float deltaX, float deltaY) {
		// Must translate map first
		TranslateMap (deltaX, deltaY);

		TranslateAsteroidGameObjects (deltaX, deltaY);
		TranslatePoiGameObjectsInScene (deltaX, deltaY);
		TranslateShipGameObjectInScene (deltaX, deltaY);
	}

	private void MoveShip(float deltaX, float deltaY) {
		// Must translate map first
		TranslateMap (deltaX, deltaY);

		TranslateAsteroidGameObjects (deltaX, deltaY);
		TranslatePoiGameObjectsInScene (deltaX, deltaY);
		m_navigationShip.ReduceFuel (Vector2.Distance(Vector2.zero, new Vector2(deltaX, deltaY)));
	}
	
	private void TranslateShipGameObjectInScene(float deltaX, float deltaY) {
		GameObject shipGameObject = GameObject.FindGameObjectWithTag ("NavigationShip");
		if (shipGameObject != null) {
			TranslateGameObjectInScene (shipGameObject, deltaX, deltaY);
		}
	}
	
	private void TranslateAsteroidGameObjects (float deltaX, float deltaY) {
		foreach (AsteroidField asteroidField in m_navigationController.allAsteroidFields) {
			foreach (Asteroid asteroid in asteroidField.allAsteroids) {
				if (asteroid.me) {
					TranslateGameObjectInScene(asteroid.me, deltaX, deltaY);
				}
				else if (MapCoordinatesAreInScene(asteroid.pos)) {
					m_navigationController.InstantiateAsteroid(asteroid);
				}
			}
		}
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
		if (!SceneCoordinatesAreInSceneBounds(position))
		{
			// TODO: Make sure that destroying a gameobject in this iterator doesn't create problems
			GameObject.Destroy(gObject);
		}
	}
}
