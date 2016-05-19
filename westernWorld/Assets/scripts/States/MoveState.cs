using UnityEngine;
using System.Collections;

public class MoveState : State<Bob> {

	static readonly MoveState instance = new MoveState();
	
	public static MoveState Instance {
		get {
			return instance;
		}
	}
	
	static MoveState () {}
	private MoveState () {}
	
	// let bob send message from at this state
	public override void Enter (Bob agent) {}
	
	public override void Execute (Bob agent) {
		agent.moveTo();
		if (agent.TargetArrival () == true && agent.Target == gameManager.instance.gameInfo.HomeAdd.position) {
			agent.ChangeState (WaitState.Instance);
			//Debug.Log("I choose waitState" + agent.Target+ gameManager.instance.gameInfo.HomeAdd.position);
		}
		if (agent.TargetArrival () == true && agent.Target == gameManager.instance.gameInfo.MineAdd.position) {
			agent.ChangeState (workState.Instance);
			//Debug.Log("I choose workstate"+ agent.Target+ gameManager.instance.gameInfo.MineAdd.position);
		}
	}
	
	public override void Exit (Bob agent) {}
	
	public override bool OnMessage(Bob agent, Telegram msg){ return false; }
	public override bool OnSense(Bob agent, ModalityType modality){return false;}
}
