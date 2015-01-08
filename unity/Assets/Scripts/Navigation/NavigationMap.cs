using UnityEngine;
using System.Collections;

public class NavigationMap : MonoBehaviour {

	public static float m_mapEdge = 20;
	
	// These constants represent the bounds of the area in the scene
	// in which we allow GameObjects to exist.  If a GameObject travels
	// beyond these bounds, we Destroy it, and reinstantiate it if it needs
	// to move back inside this area.
	public static float mapEdgeLeft { get { return -m_mapEdge; } }
	public static float mapEdgeRight { get { return m_mapEdge;} }
	public static float mapEdgeTop { get { return m_mapEdge;} }
	public static float mapEdgeBottom { get { return -m_mapEdge;} }

	public float scrollSpeed;

	private Vector2 m_mapCentre = Vector2.zero;
	public Vector2 mapCentre { get; set; }

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
				ScrollMap (scrollSpeed * Input.GetAxis("Horizontal"), scrollSpeed * Input.GetAxis("Vertical"));
			}
		}
	}

	void FixedUpdate() {
		if (m_navigationShip.shipIsMoving) {
			GameObject shipGameObject = GameObject.FindGameObjectWithTag ("NavigationShip");
			if (shipGameObject != null) {
				// Move the ship one step towards its current destination
				Vector2 shipMapPosition = SceneToMap(shipGameObject.transform.position);
				Vector2 hopObjective = Vector2.Lerp(shipMapPosition, m_navigationShip.shipMovementDestination, 0.1f);
				MoveShip (shipMapPosition.x - hopObjective.x, shipMapPosition.y - hopObjective.y);
			}
		}
	}

	public bool MapCoordinatesAreInScene(Vector2 mapCoordinates) {
		return mapCoordinates.x > (m_mapCentre.x + mapEdgeLeft)
			&& mapCoordinates.x < (m_mapCentre.x + mapEdgeRight)
				&& mapCoordinates.y > (m_mapCentre.y + mapEdgeBottom)
				&& mapCoordinates.y < (m_mapCentre.y + mapEdgeTop);
	}
	
	public Vector3 MapToScene(Vector2 mapCoordinates) {
		return new Vector3 (mapCoordinates.x - m_mapCentre.x, mapCoordinates.y - m_mapCentre.y, 0.0f);
	}
	
	public Vector2 SceneToMap(Vector3 sceneCoordinates) {
		return new Vector2 (sceneCoordinates.x + m_mapCentre.x, sceneCoordinates.y + m_mapCentre.y);
	}

	public bool SceneCoordinatesAreInBounds(Vector3 sceneCoordinates) {
		return sceneCoordinates.x <= mapEdgeRight && sceneCoordinates.x >= mapEdgeLeft 
			&& sceneCoordinates.y <= mapEdgeTop && sceneCoordinates.y >= mapEdgeBottom;
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
		TranslateMap (deltaX, deltaY);
		TranslateAsteroidGameObjects (deltaX, deltaY);
		TranslateShipGameObjectInScene (deltaX, deltaY);
	}

	private void MoveShip(float deltaX, float deltaY) {
		TranslateMap (deltaX, deltaY);
		TranslateAsteroidGameObjects (deltaX, deltaY);
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
	
	// Currently not in use.  Could be used to move game objects by tag rather than by held pointer.
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
		if (!SceneCoordinatesAreInBounds(position))
		{
			// TODO: Make sure that destroying a gameobject in this iterator doesn't create problems
			GameObject.Destroy(gObject);
		}
	}
}
