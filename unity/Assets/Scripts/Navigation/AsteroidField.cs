using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AsteroidField
{
    public List<Asteroid> allAsteroids;

    Vector3 pos;
    float radius;
    
    public AsteroidField()
    {
        pos = new Vector3(0,0,0);
        radius = 1.0f;
        allAsteroids = new List<Asteroid>();
        build(1);
    }

    public AsteroidField(float x, float y, float r, int n)
    {
        pos = new Vector3(x, y, 0.0f);
        radius = r;
        allAsteroids = new List<Asteroid>();
        build(n);
    }

	// Use this for initialization
	//void Start () 
	//{
      //  Debug.Log("Loading Asteroid Field...");
      //  MakeAnAsteroidField(10.0F, 10.0F, 100, 9.0F);
      //  Debug.Log("Asteroid Field Loaded.");
	//}
	
	// Update is called once per frame
	//void Update () 
	//{
	//}

    void build(int n)
    {
        int i;
        float r2, dx, dy, phi, scale;
        float maxScale = 2.0F;
        float minScale = 0.5F;

        for(i = 0; i < n; i++)
        {
            r2 = 2*radius*radius;
            dx = 0.0F;
            dy = 0.0F;
            while(r2 > radius*radius)
            {
                dx = Random.Range(-radius, radius);
                dy = Random.Range(-radius, radius);
                r2 = dx*dx + dy*dy;
            }
            phi = Random.Range(0.0F,360.0F);
            scale = minScale*Mathf.Pow(maxScale/minScale,Random.Range(0.0F,1.0F));

            allAsteroids.Add(new Asteroid(pos.x+dx, pos.y+dy, scale, phi));
        }
    }
}
