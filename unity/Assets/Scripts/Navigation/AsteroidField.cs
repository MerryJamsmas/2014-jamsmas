using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AsteroidField : MonoBehaviour 
{
    public Transform Asteroid;

	// Use this for initialization
	void Start () 
	{
        Debug.Log("Loading Asteroid Field...");
        Instantiate(Asteroid, new Vector3(1,1,0), Quaternion.identity);
        Debug.Log("Asteroid Field Loaded.");
	}
	
	// Update is called once per frame
	void Update () 
	{
	}
}
