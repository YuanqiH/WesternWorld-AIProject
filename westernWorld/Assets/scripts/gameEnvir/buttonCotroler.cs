using UnityEngine;
using System.Collections;

public class buttonCotroler : MonoBehaviour {

	public 	void onDisplay(){
		pathFinder.Instance.onShowpath = !pathFinder.Instance.onShowpath;
		Debug.Log ("the onshow is " + pathFinder.Instance.onShowpath);
	}

	public void OnFind(){
		pathFinder.Instance.OnFind = true;
		Debug.Log ("the onfind is"+ pathFinder.Instance.OnFind);
	}


}
