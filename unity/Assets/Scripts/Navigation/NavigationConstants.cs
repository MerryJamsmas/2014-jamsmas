using UnityEngine;
using System.Collections;

public class NavigationConstants : MonoBehaviour {

	[SerializeField]
	private const float m_mapEdgeLeft = -2;

	[SerializeField]
	private const float m_mapEdgeRight = 2;

	[SerializeField]
	private const float m_mapEdgeTop = 2;

	[SerializeField]
	private const float m_mapEdgeBottom = -2;

	public static float mapEdgeLeft { get { return m_mapEdgeLeft; } }
	public static float mapEdgeRight { get { return m_mapEdgeRight;} }
	public static float mapEdgeTop { get { return m_mapEdgeTop;} }
	public static float mapEdgeBottom { get { return m_mapEdgeBottom;} }
}
