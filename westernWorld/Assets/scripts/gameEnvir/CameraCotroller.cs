using UnityEngine;
using System.Collections;

public class CameraCotroller : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Vector3 m_mapsize = pathFinder.Instance.MapSize;
		m_mapsize /= 2;
		m_mapsize.z = -20;
		transform.localPosition = m_mapsize;
	}

}
