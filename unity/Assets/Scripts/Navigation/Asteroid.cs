using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Asteroid
{
    public GameObject me;
    public Vector2 pos;
    public Quaternion rot;
    float size;
    
    public Asteroid()
    {
        pos = new Vector2(0,0);
        size = 1.0f;
        rot = Quaternion.identity;
    }

    public Asteroid(float x, float y, float r, float phi)
    {
        pos = new Vector2(x, y);
        size = r;
        rot = Quaternion.AngleAxis(phi, Vector3.forward);
    }

    public void instantiate(GameObject astPreFab)
    {
        me = astPreFab;
        me.transform.localScale = new Vector3(size, size, size);
    }

    public void destroy()
    {
        me = null;
    }
}
