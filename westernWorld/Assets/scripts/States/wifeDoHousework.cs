using UnityEngine;
using System.Collections;

public sealed class DoHousework : State<Elsa> {

	static readonly DoHousework instance = new DoHousework();
	
	public static DoHousework Instance {
		get {
			return instance;
		}
	}
	
	static DoHousework () {}
	private DoHousework () {}
	
	public override void Enter (Elsa agent) {
		Debug.Log("Starting to Dohouse work...");
	}
	
	public override void Execute (Elsa agent) {
		Debug.Log("Moppin' the floor...");
		if (agent.NatureCall == true) agent.ChangeState(VisitBathroom.Instance);
	}
	
	public override void Exit (Elsa agent) {
		Debug.Log("Stopping doing house work...");
	}

	public override bool OnMessage(Elsa agent, Telegram msg){ return false; }
	public override bool OnSense(Elsa agent, ModalityType modality){return false;}
}
