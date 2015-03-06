using UnityEngine;
using System.Collections;

public class ShipController : MonoBehaviour {

	public GameObject screenFader;

	private GameObject currentRoom = null;
	private GameObject roomToLoad = null;
	private int loadingRoomDelay = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (loadingRoomDelay == 2) {
			loadingRoomDelay = 1;
		}
		else if (loadingRoomDelay == 1) {
			currentRoom = GameObject.Instantiate (roomToLoad) as GameObject;
			roomToLoad = null;
			loadingRoomDelay = 0;
		}
	}

	public void SetFaded(bool isFaded) {
		ScreenFader fader = screenFader.GetComponent<ScreenFader> ();
		if (fader != null) {
			fader.SetFaded(isFaded);
		}
	}

	public void SetCurrentRoom(GameObject roomPrefab) {
		if (currentRoom != null) {
			GameObject.Destroy(currentRoom);
		}
		loadingRoomDelay = 2;
		roomToLoad = roomPrefab;
	}
}
