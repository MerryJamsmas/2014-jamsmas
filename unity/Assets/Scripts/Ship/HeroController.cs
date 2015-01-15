using UnityEngine;
using System.Collections;
using Pathfinding;

public class HeroController : MonoBehaviour {

	public GameObject target;

	private Seeker m_seeker;
	private Path m_path;
	private Animator m_animator;

	private int m_currentWaypoint = 0;
	private float m_speed = 5f;

	// Use this for initialization
	void Start () {

		m_seeker = GetComponent<Seeker> ();
		if (m_seeker == null) {
			Debug.LogError("Could not find Seeker in HeroController");
		}

		if (target == null) {
			Debug.LogError("No pathfinding target for seeker!");
		}

		m_animator = GetComponent<Animator> ();
		if (m_animator == null) {
			Debug.LogError("Could not find Animator in HeroController");
		}

		m_path = m_seeker.StartPath (this.transform.position, target.transform.position);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (m_path == null) {
			//We have no path to move after yet
			m_animator.SetBool ("walking", false);
			return;
		}
		
		if (m_currentWaypoint >= m_path.vectorPath.Count) {
			Debug.Log ("End Of Path Reached");
			m_animator.SetBool ("walking", false);
			return;
		}

		Vector3 dir = (m_path.vectorPath[m_currentWaypoint] - transform.position).normalized;
		if (Vector3.Distance(transform.position, m_path.vectorPath[m_currentWaypoint]) < m_speed * Time.fixedDeltaTime) {
			transform.position = m_path.vectorPath[m_currentWaypoint];
		}
		else
		{
			transform.position = transform.position + (dir * m_speed * Time.fixedDeltaTime);
		}

		//Check if we are close enough to the next waypoint
		//If we are, proceed to follow the next waypoint
		if (Vector3.Distance (transform.position, m_path.vectorPath [m_currentWaypoint]) < 0.001f) {
			m_currentWaypoint++;
		}

		float angle = Vector3.Angle(Vector3.down, dir);
		if (angle < 45) { 
			m_animator.SetInteger ("facing", 0);
		}
		else if (angle > 135) {
			m_animator.SetInteger ("facing", 2);
		}
		else if (dir.x < 0) {
			m_animator.SetInteger ("facing", 1);
		}
		else if (dir.x > 0) {
			m_animator.SetInteger ("facing", 3);
		}
		m_animator.SetBool ("walking", true);

	}
}
