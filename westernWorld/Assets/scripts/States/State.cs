abstract public class State <T> {

	abstract public void Enter (T agent);
	abstract public void Execute (T agent);
	abstract public void Exit (T agent);

	//this is executes if the agent receive a message from dispatcher
	abstract public bool OnMessage(T agent, Telegram msg);
	abstract public bool OnSense(T agent, ModalityType modality);
}