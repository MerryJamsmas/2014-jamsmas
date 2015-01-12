using UnityEngine;
using System.Collections;

public class ShipController : MonoBehaviour {

	public GameObject screenFader;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetFaded(bool isFaded) {
		ScreenFader fader = screenFader.GetComponent<ScreenFader> ();
		if (fader != null) {
			fader.SetFaded(isFaded);
		}
	}
}
