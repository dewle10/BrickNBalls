using Unity.Entities;

[UpdateInGroup(typeof(InitializationSystemGroup), OrderLast = true)]
public partial class GetInputSystem : SystemBase
{
    private InputActions _actions;

    protected override void OnCreate()
    {
        RequireForUpdate<Cannon>();
        RequireForUpdate<Inputs>();

        _actions = new InputActions();
    }

    protected override void OnStartRunning()
    {
        _actions.Enable();
    }

    protected override void OnStopRunning()
    {
        _actions.Disable();
    }

    protected override void OnUpdate()
    {
        var curShootInput = _actions.Cannon.Shoot.IsPressed();
        var curMoveInputs = _actions.Cannon.Move.ReadValue<float>();
        SystemAPI.SetSingleton(new Inputs { MoveValue = curMoveInputs, IsShooting = curShootInput });
    }
}

