using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//============================
//purpose of this is as a medium to call Addsignal() and display() region and path
public class signalManager : MonoBehaviour {

	public Signal m_signal;
	public bool showRangeOn;
	public Color HighlightColor;
	public double lifetime;
	public double ticktime;
	private Dictionary<Vector3, GameObject> tileDicts;
	public GameObject rangeSphere;
	// Use this for initialization
	void Start () {
		showRangeOn = true;
		ticktime = 0;
		transform.position = pathFinder.Instance.MapSize/2;
		m_signal = GetComponent<Signal> ();
		tileDicts = pathFinder.Instance.tileDicts;
	}
	
	// Update is called once per frame
	void Update () {
		displayRange ();
		if (ticktime > 0) {
			ReginalSenceManager.Instance.resetSensePath ();
			ReginalSenceManager.Instance.AddSignal (m_signal); // using a* here
			ReginalSenceManager.Instance.displaySensePath ();
			ticktime -= Time.deltaTime;
		} else { // clear
			ReginalSenceManager.Instance.resetSensePath ();
		}

	}

	public void displayRange(){ 
		if (showRangeOn) {
			rangeSphere.GetComponent<MeshRenderer> ().enabled = true;
			rangeSphere.transform.position = transform.localPosition;
			rangeSphere.transform.localScale = 2 * new Vector3 (m_signal.maximumRange, m_signal.maximumRange, m_signal.maximumRange);

		} else
			rangeSphere.GetComponent<MeshRenderer> ().enabled = false;
	}

	
	public void OnShowRange(){
		showRangeOn = !showRangeOn;
	}

	public void ReTrigger(){
		ticktime = lifetime;
	}

	private Vector3 floor(Vector3 vec){
		vec.x = Mathf.FloorToInt (vec.x);
		vec.y = Mathf.FloorToInt (vec.y);
		vec.z = Mathf.FloorToInt (vec.z);
		vec.x =Mathf.Clamp (vec.x, 0, pathFinder.Instance.MapSize.x);
		vec.y =Mathf.Clamp (vec.y, 0, pathFinder.Instance.MapSize.y);
		return vec;
	}
}
