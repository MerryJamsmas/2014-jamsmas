using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NavigationController : MonoBehaviour 
{

    List<AsteroidField> allAsteroidFields;
    public GameObject asteroidPrefab;
	private GameObject asteroidParent;

	public float cameraPanSpeed;
	private Camera m_camera;

	private bool m_shipIsMoving = false;
	public Vector2 m_shipMovementDestination;

	// The coordinates of the centre of the portion of the map currently displayed in the scene
	public Vector2 m_mapCentre = new Vector2(0, 0);

	private NavigationShipController m_navigationShipController;

	// Use this for initialization
	void Start () {
		GameObject cameraGameObject = GameObject.FindGameObjectWithTag ("NavigationCamera");
		if (cameraGameObject == null) {
			Debug.LogError ("Unable to find camera game object in NavigationController");
		}
		else{
			m_camera = cameraGameObject.camera;
			if(m_camera == null){
				Debug.LogError ("Unable to extract camera object in NavigationController");
			}
		}

		asteroidParent = GameObject.FindGameObjectWithTag ("AsteroidParentGameObject");
		if (asteroidParent == null) {
			Debug.LogError ("Unable to find AsteroidParentGameObject in NavigationController");
		}

		m_navigationShipController = GameObject.FindObjectOfType<NavigationShipController> ();
		if (m_navigationShipController == null) {
			Debug.LogError("Unable to find NavigationShipController in NavigationController");
		}

		makeAsteroidFields ();
		instantiateAllAsteroids ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!m_shipIsMoving) {
			if (Input.GetMouseButtonDown(0)) {
				GameObject shipGameObject = GameObject.FindGameObjectWithTag ("NavigationShip");
				if (shipGameObject != null) {
					Vector2 mapDestination = SceneToMap(m_camera.ScreenToWorldPoint(Input.mousePosition));
					Vector2 shipMapPosition = SceneToMap(shipGameObject.transform.position);

					if (Vector2.Distance(mapDestination, shipMapPosition) < m_navigationShipController.GetRangeOnRemainingFuel()) {
						SetShipDestination(SceneToMap(m_camera.ScreenToWorldPoint(Input.mousePosition)));

						// TODO: Refactor to move all this ship sprite manipulation to a separate class! (probably NavigationShipController)
						GameObject shipSpriteGameObject = GameObject.FindGameObjectWithTag ("NavigationShipSprite");
						if (shipSpriteGameObject != null) {
							// Rotate the ship to face it's current destination
							shipSpriteGameObject.transform.localRotation = Quaternion.identity;
							float dir = MapToScene(m_shipMovementDestination).x > shipSpriteGameObject.transform.position.x? -1f:1f;
							shipSpriteGameObject.transform.Rotate(Vector3.forward, dir * Vector3.Angle(Vector3.up, MapToScene(m_shipMovementDestination) - shipSpriteGameObject.transform.position));
							
							Animator animator = shipSpriteGameObject.GetComponent<Animator>();
							animator.SetBool("thrust", true);
						}
					}
				}
			}
			else if (Input.GetAxis("Horizontal") != 0.0f || Input.GetAxis("Vertical") != 0.0f) {
				MoveCamera (cameraPanSpeed * Input.GetAxis("Horizontal"), cameraPanSpeed * Input.GetAxis("Vertical"));
			}
		}
	}

	void FixedUpdate() {
		if (m_shipIsMoving) {
			GameObject shipGameObject = GameObject.FindGameObjectWithTag ("NavigationShip");
			if (shipGameObject != null) {
				// Move the ship one step towards its current destination
				Vector2 shipMapPosition = SceneToMap(shipGameObject.transform.position);
				Vector2 hopObjective = Vector2.Lerp(shipMapPosition, m_shipMovementDestination, 0.1f);
				MoveShip (shipMapPosition.x - hopObjective.x, shipMapPosition.y - hopObjective.y);
				if (Vector2.Distance(m_shipMovementDestination, shipMapPosition) < 0.05) {
					m_shipIsMoving = false;

					// TODO: Refactor to move all this ship sprite manipulation to a separate class! (probably NavigationShipController)
					GameObject shipSpriteGameObject = GameObject.FindGameObjectWithTag ("NavigationShipSprite");
					if (shipSpriteGameObject != null) {
						Animator animator = shipSpriteGameObject.GetComponent<Animator>();
						animator.SetBool("thrust", false);
					}
				}
			}
		}
		else {
			// TODO: Refactor to move all this ship sprite manipulation to a separate class! (probably NavigationShipController)
			// Rotate the ship to face up
			GameObject shipSpriteGameObject = GameObject.FindGameObjectWithTag ("NavigationShipSprite");
			if (shipSpriteGameObject != null) {
				shipSpriteGameObject.transform.localRotation = Quaternion.identity;
			}
		}
	}

	private void MoveCamera(float deltaX, float deltaY) {
		TranslateMap (deltaX, deltaY);
		TranslateAsteroidGameObjects (deltaX, deltaY);
		TranslateShipGameObjectInScene (deltaX, deltaY);
	}

	private void SetShipDestination(Vector3 shipDestination) {
		m_shipMovementDestination = shipDestination;
		m_shipIsMoving = true;
	}

	private void MoveShip(float deltaX, float deltaY) {
		TranslateMap (deltaX, deltaY);
		TranslateAsteroidGameObjects (deltaX, deltaY);
		m_navigationShipController.ReduceFuel (Vector2.Distance(Vector2.zero, new Vector2(deltaX, deltaY)));
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
					asteroid.me.transform.parent = asteroidParent.transform;
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
        foreach (AsteroidField af in allAsteroidFields) {
			foreach (Asteroid a in af.allAsteroids) {
				a.instantiate (Instantiate (asteroidPrefab, a.pos, a.rot) as GameObject);
				a.me.transform.parent = asteroidParent.transform;
			}
		}
    }
}
