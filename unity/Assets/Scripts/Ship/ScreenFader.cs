using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour {

	private Image m_mask;

	void Start () {
		m_mask = this.GetComponent<Image> ();
		if (m_mask == null) {
			Debug.LogError("Could not find Image on ScreenFader");
		}
	}

	public void SetFaded(bool faded) {
		m_mask.enabled = faded;
	}
}
