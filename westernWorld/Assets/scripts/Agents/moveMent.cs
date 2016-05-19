using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class moveMent : MonoBehaviour {

	public float speed;
	// Use this for initialization
	private Vector3 m_start;
	private Vector3 m_target;
	private List<Location> m_path;
	void Start () {
		m_path = new List<Location> ();
		setPathTarget (new Vector3 ((16 - 1) / 2, (16 - 1) / 2, 0.0f));
	}
	
	// Update is called once per frame
	public void Move () {
		setPathStart (); // update the startpoint
		if (pathFinder.Instance.OnFind) {
			clearLastPathColor();
			getPath ();
			displayPath ();
		}
		MoveMethod ();
	}

	public void setPathStart(){
		Vector3 localStart = transform.localPosition;
		localStart = TOOLS.PositionClamp (localStart);
		localStart.z = 0;
		this.m_start = localStart;
	}

	public void setPathTarget(Vector3 target){
		target = TOOLS.PositionClamp (target);
		target.z = 0;
		this.m_target = target;
	}

	public void getPath(){
		pathFinder.Instance.pathFinding (this.m_start,this.m_target, ref m_path);
		if (m_path.Count == 0)
			Debug.LogError ("there is no path " + "start/end point " + m_start + m_target );
	}

	public void displayPath(){
		if (this.m_path.Count > 0)
			pathFinder.Instance.DisplayPath (this.m_path, GraphicType.CostGraph);
		else
			Debug.LogError ("no path to display");
	}

	public void clearLastPathColor(){
		pathFinder.Instance.ResetPathColor (this.m_path);
	}

	public void MoveMethod(){
		float step = speed * Time.deltaTime;
		Transform currentTransform = GetComponentInParent<Transform> ();
		Vector3 targetLocation;
		if (this.m_path.Count > 1) {
			targetLocation = new Vector3 (this.m_path[1].x, this.m_path[1].y, currentTransform.localPosition.z);
		}else if(this.m_path.Count > 0 ){
			targetLocation = new Vector3 (this.m_path[0].x, this.m_path [0].y, currentTransform.localPosition.z);
		}
		else
			targetLocation = currentTransform.localPosition;

		transform.localPosition = Vector3.MoveTowards (currentTransform.localPosition, targetLocation, step);

// ===================================================================
// little animation
		Vector2 dir = (Vector2)(targetLocation - currentTransform.localPosition); 
		if (Mathf.Abs (dir.x) > Mathf.Abs (dir.y))
			dir.y = 0; 
		else 
			dir.x =0;
		//Debug.Log("direction"+ dir);
		GetComponent<Animator> ().SetFloat ("Dir_R", dir.x);
		GetComponent<Animator> ().SetFloat ("Dir_D", dir.y);
	}



}
