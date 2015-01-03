using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NavigationController : MonoBehaviour 
{

    List<AsteroidField> allAsteroidFields;

    public GameObject Asteroid;

	public float cameraPanSpeed;
	private Camera m_camera;
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
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			Vector3 objective = m_camera.ScreenToWorldPoint(Input.mousePosition);
			GameObject shipGameObject = GameObject.FindGameObjectWithTag ("NavigationShip");
			MoveShip (shipGameObject.transform.position.x - objective.x, shipGameObject.transform.position.y - objective.y);
		}
		else{
			MoveCamera (cameraPanSpeed * Input.GetAxis("Horizontal"), cameraPanSpeed * Input.GetAxis("Vertical"));
	
		}
	}

	private void MoveCamera(float deltaX, float deltaY) {
		TranslatePoiGameObjectsInScene (deltaX, deltaY);
		TranslateShipGameObjectInScene (deltaX, deltaY);
	}

	private void MoveShip(float deltaX, float deltaY){
		TranslatePoiGameObjectsInScene (deltaX, deltaY);
	}

	private void TranslateShipGameObjectInScene(float deltaX, float deltaY) {
		GameObject shipGameObject = GameObject.FindGameObjectWithTag ("NavigationShip");
		if (shipGameObject != null) {
			TranslateGameObjectInScene (shipGameObject, deltaX, deltaY);
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
                a.instantiate(Instantiate(Asteroid, a.pos, a.rot) as GameObject);
    }
}
