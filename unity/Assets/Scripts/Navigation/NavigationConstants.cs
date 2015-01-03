using UnityEngine;
using System.Collections;

public class NavigationConstants : MonoBehaviour {

	public static float m_mapEdge = 20;
	
	// These constants represent the bounds of the area in the scene
	// in which we allow GameObjects to exist.  If a GameObject travels
	// beyond these bounds, we Destroy it, and reinstantiate it if it needs
	// to move back inside this area.
	public static float mapEdgeLeft { get { return -m_mapEdge; } }
	public static float mapEdgeRight { get { return m_mapEdge;} }
	public static float mapEdgeTop { get { return m_mapEdge;} }
	public static float mapEdgeBottom { get { return -m_mapEdge;} }
}
