using UnityEngine;

public sealed class EatState : State<Bob> {
	
	static readonly EatState instance = new EatState();
	
	public static EatState Instance {
		get {
			return instance;
		}
	}
	
	static EatState () {}
	private EatState () {}
	
	public override void Enter (Bob agent) {
		Debug.Log (agent.name +": Start to eat.....");
	}
	
	public override void Execute (Bob agent) {
		agent.CreateTime();
		agent.Target = gameManager.instance.gameInfo.MineAdd.position;
		agent.ChangeState(MoveState.Instance);
	}
	
	public override void Exit (Bob agent) {
		Debug.Log (agent.name + " : I have finish such nice dinner, gonna back to work.....");
	}

	public override bool OnMessage(Bob agent, Telegram msg){ return false; }
	public override bool OnSense(Bob agent, ModalityType modality){return false;}
}
