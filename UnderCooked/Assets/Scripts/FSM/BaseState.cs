public class BaseState
{
    public string Name;

    protected StateMachine _stateMachine;


    public BaseState(string name, StateMachine stateMachine)
    {
        this.Name = name;
        this._stateMachine = stateMachine;
    }

    public virtual void Enter() { }
    public virtual void UpdateLogic() { }
    public virtual void UpdatePhysics() { }
    public virtual void Exit() { }

}
