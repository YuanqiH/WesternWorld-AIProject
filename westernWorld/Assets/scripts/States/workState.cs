using UnityEngine;
using System.Collections;

public class workState: State<Bob> {
		
	static readonly workState instance = new workState();
		
	public static workState Instance {
			get {
				return instance;
			}
		}
		
	static workState () {}
	private workState () {}
		
		// let bob send message from at this state
		public override void Enter (Bob agent) {
			
		}
		
		public override void Execute (Bob agent) {
		Debug.Log (agent.name+" : I am enjoying my work......");
		}
		
		public override void Exit (Bob agent) {
		}
		
	public override bool OnMessage(Bob agent, Telegram msg){ return false;} 
	public override bool OnSense(Bob agent, ModalityType modality){return false;}
}
