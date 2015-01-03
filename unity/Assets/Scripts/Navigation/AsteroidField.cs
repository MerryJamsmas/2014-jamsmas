using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AsteroidField : MonoBehaviour 
{
    public GameObject Asteroid;
    //Vector3 pos;
    //float size;

	// Use this for initialization
	void Start () 
	{
        Debug.Log("Loading Asteroid Field...");
        MakeAnAsteroidField(10.0F, 10.0F, 100, 9.0F);
        Debug.Log("Asteroid Field Loaded.");
	}
	
	// Update is called once per frame
	void Update () 
	{
	}

    void MakeAnAsteroidField(float x, float y, int n, float R)
    {
        int i;
        float r2, dx, dy, phi, scale;
        GameObject o;
        float maxScale = 2.0F;
        float minScale = 0.5F;

        for(i = 0; i < n; i++)
        {
            r2 = 2*R*R;
            dx = 0.0F;
            dy = 0.0F;
            while(r2 > R*R)
            {
                dx = Random.Range(-R, R);
                dy = Random.Range(-R, R);
                r2 = dx*dx + dy*dy;
            }
            Vector3 pos = new Vector3(x+dx, y+dy, 0.0F);
            phi = Random.Range(0.0F,360.0F);
            Quaternion rot = Quaternion.AngleAxis(phi,Vector3.forward);
            scale = minScale*Mathf.Pow(maxScale/minScale,Random.Range(0.0F,1.0F));

            o = Instantiate(Asteroid, pos, rot) as GameObject;
            o.transform.localScale = new Vector3(scale, scale, scale);
        }
    }
}
