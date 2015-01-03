using UnityEngine;
using System.Collections;

public class UIGameManagerScript : MonoBehaviour 
{
    void Start()
    {
        Debug.Log("Loaded Scene: " + Application.loadedLevelName);
    }

    void Update()
    {
        if(Input.GetKey("n") && Application.loadedLevelName != "navigation")
        {
            Debug.Log("Loading Scene: navigation");
            Application.LoadLevel("navigation");
        }
        if(Input.GetKey("1") && Application.loadedLevelName != "room00")
        {
            Debug.Log("Loading Scene: room00");
            Application.LoadLevel("room00");
        }
    }
}
