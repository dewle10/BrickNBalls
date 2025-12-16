using Unity.Entities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


[UpdateInGroup(typeof(InitializationSystemGroup), OrderLast = true)]
public partial class PlayerInputSystem : SystemBase
{
    private TanksInputSystemActions _actions;
    private Entity _playerEntity;

    protected override void OnCreate()
    {
        RequireForUpdate<Player>();
        RequireForUpdate<PlayerMoveInput>();

        _actions = new TanksInputSystemActions();
    }

    protected override void OnStartRunning()
    {
        _actions.Enable();
        _playerEntity = SystemAPI.GetSingletonEntity<Player>();
    }

    protected override void OnStopRunning()
    {
        _actions.Disable();
        _playerEntity = Entity.Null;
    }

    protected override void OnUpdate()
    {
        var curMoveInputs = _actions.Player.Move.ReadValue<Vector2>();

        SystemAPI.SetSingleton(new PlayerMoveInput { Value = curMoveInputs});
    }
}

