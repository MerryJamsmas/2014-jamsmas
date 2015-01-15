using UnityEngine;
using System.Collections;

public class Setup : MonoBehaviour {

	public GameObject wallPrefab;
	

	// Use this for initialization
	void Start () {


		Instantiate(wallPrefab, new Vector3(0.5f,0,-0.5f), Quaternion.identity);
		Instantiate(wallPrefab, new Vector3(1.5f,0,-0.5f), Quaternion.identity);
		Instantiate(wallPrefab, new Vector3(2.5f,0,-0.5f), Quaternion.identity);
		Instantiate(wallPrefab, new Vector3(3.5f,0,-0.5f), Quaternion.identity);
		Instantiate(wallPrefab, new Vector3(4.5f,0,-0.5f), Quaternion.identity);

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
