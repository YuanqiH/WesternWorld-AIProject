using UnityEngine;
using System.Collections;

public class VisitBathroom : State<Elsa> {

	static readonly VisitBathroom instance = new VisitBathroom();
	
	public static VisitBathroom Instance {
		get {
			return instance;
		}
	}
	
	static VisitBathroom () {}
	private VisitBathroom () {}
	
	public override void Enter (Elsa agent) {
		Debug.Log("Walkin' to the can. Need to powda mah pretty li'l nose...");
	}
	
	public override void Execute (Elsa agent) {
		Debug.Log("Ahhhhhh! Sweet relief!...");
		agent.NatureCall = false;
		// using blip state
		if (agent.NatureCall == false)
			agent.RevertPreviousState ();
	}
	
	public override void Exit (Elsa agent) {
		Debug.Log("Leavin' the john...");
	}

	public override bool OnMessage(Elsa agent, Telegram msg){ return false; }
	public override bool OnSense(Elsa agent, ModalityType modality){return false;}
}
