using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NavigationController : MonoBehaviour 
{

    List<AsteroidField> allAsteroidFields;

    public GameObject Asteroid;

	// Use this for initialization
	void Start() 
    {
        makeAsteroidFields();
        instantiateAllAsteroids();
	}
	
	// Update is called once per frame
	void Update()
    {
	
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
