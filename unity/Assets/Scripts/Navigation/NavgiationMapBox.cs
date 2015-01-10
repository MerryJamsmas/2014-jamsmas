using UnityEngine;
using System.Collections;

public class NavgiationMapBox : MonoBehaviour {

	private NavigationMap m_map;
	private LineRenderer[] m_lineRenderers;

	// Use this for initialization
	void Start () {
		m_map = FindObjectOfType<NavigationMap> ();
		if (m_map == null) {
			Debug.LogError("Could not find NavigationMap in NavigationMapBox");
		}

		m_lineRenderers = GetComponentsInChildren<LineRenderer> ();
		if (m_lineRenderers.Length < 4) {
			Debug.LogError("Too few LineRenderers in NavigationMapBox");
		}
	}

	private bool LeftMapEdgeIsInSceneBounds() {
		return (m_map.mapEdgeLeft > (m_map.sceneEdgeLeft + m_map.mapCentre.x))
			&& (m_map.mapEdgeLeft < (m_map.sceneEdgeRight + m_map.mapCentre.x));
	}

	private bool RightMapEdgeIsInSceneBounds() {
		return (m_map.mapEdgeRight > (m_map.sceneEdgeLeft + m_map.mapCentre.x))
			&& (m_map.mapEdgeRight < (m_map.sceneEdgeRight + m_map.mapCentre.x));
	}

	private bool TopMapEdgeIsInSceneBounds() {
		return (m_map.mapEdgeTop > (m_map.sceneEdgeBottom + m_map.mapCentre.y))
			&& (m_map.mapEdgeTop < (m_map.sceneEdgeTop + m_map.mapCentre.y));
	}

	private bool BottomMapEdgeIsInSceneBounds() {
		return (m_map.mapEdgeBottom > (m_map.sceneEdgeBottom + m_map.mapCentre.y))
			&& (m_map.mapEdgeBottom < (m_map.sceneEdgeTop + m_map.mapCentre.y));
	}

	// Update is called once per frame
	void Update () {
		if (LeftMapEdgeIsInSceneBounds ()) {
			m_lineRenderers[0].enabled = true;
			m_lineRenderers[0].SetPosition(0, m_map.MapToScene(new Vector2(m_map.mapEdgeLeft, Mathf.Min(m_map.mapEdgeTop, m_map.sceneEdgeTop + m_map.mapCentre.y))));
			m_lineRenderers[0].SetPosition(1, m_map.MapToScene(new Vector2(m_map.mapEdgeLeft, Mathf.Max(m_map.mapEdgeBottom, m_map.sceneEdgeBottom + m_map.mapCentre.y))));
		}
		else {
			m_lineRenderers[0].enabled = false;
		}

		if (RightMapEdgeIsInSceneBounds ()) {
			m_lineRenderers[1].enabled = true;
			m_lineRenderers[1].SetPosition(0, m_map.MapToScene(new Vector2(m_map.mapEdgeRight, Mathf.Min(m_map.mapEdgeTop, m_map.sceneEdgeTop + m_map.mapCentre.y))));
			m_lineRenderers[1].SetPosition(1, m_map.MapToScene(new Vector2(m_map.mapEdgeRight, Mathf.Max(m_map.mapEdgeBottom, m_map.sceneEdgeBottom + m_map.mapCentre.y))));
		}
		else {
			m_lineRenderers[1].enabled = false;
		}

		if (TopMapEdgeIsInSceneBounds ()) {
			m_lineRenderers[2].enabled = true;
			m_lineRenderers[2].SetPosition(0, m_map.MapToScene(new Vector2(Mathf.Max(m_map.mapEdgeLeft, m_map.sceneEdgeLeft + m_map.mapCentre.x), m_map.mapEdgeTop)));
			m_lineRenderers[2].SetPosition(1, m_map.MapToScene(new Vector2(Mathf.Min(m_map.mapEdgeRight, m_map.sceneEdgeRight + m_map.mapCentre.x), m_map.mapEdgeTop)));
		}
		else {
			m_lineRenderers[2].enabled = false;
		}

		if (BottomMapEdgeIsInSceneBounds ()) {
			m_lineRenderers[3].enabled = true;
			m_lineRenderers[3].SetPosition(0, m_map.MapToScene(new Vector2(Mathf.Max(m_map.mapEdgeLeft, m_map.sceneEdgeLeft + m_map.mapCentre.x), m_map.mapEdgeBottom)));
			m_lineRenderers[3].SetPosition(1, m_map.MapToScene(new Vector2(Mathf.Min(m_map.mapEdgeRight, m_map.sceneEdgeRight + m_map.mapCentre.x), m_map.mapEdgeBottom)));
		}
		else {
			m_lineRenderers[3].enabled = false;
		}
	}
}
