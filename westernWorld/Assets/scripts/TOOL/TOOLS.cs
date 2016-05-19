using UnityEngine;
using System.Collections;

public static class TOOLS  {

	public static Vector3 PositionClamp(Vector3 location){
		location.x =Mathf.FloorToInt(location.x+0.5f);
		location.y =Mathf.FloorToInt(location.y+0.5f);
		location.z = Mathf.FloorToInt (location.z + 0.5f);
		return location;
	}

	
}
