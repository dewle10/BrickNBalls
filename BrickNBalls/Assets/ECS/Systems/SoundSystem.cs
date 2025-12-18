using Unity.Entities;
using UnityEngine;

[UpdateInGroup(typeof(PresentationSystemGroup))]
public partial class SoundSystem : SystemBase
{
    protected override void OnCreate()
    {
        RequireForUpdate<GameState>();
    }
    protected override void OnUpdate()
    {
        if (SoundManager.Instance == null) return;
        if (!SystemAPI.HasSingleton<GameState>()) return;

        var gameState = SystemAPI.GetSingletonRW<GameState>();

        if (gameState.ValueRO.HitsThisFrame > 0)
        {
            SoundManager.Instance.PlayHitSound();
            gameState.ValueRW.HitsThisFrame = 0;
        }
        if (gameState.ValueRO.IsShooting)
        {
            SoundManager.Instance.PlayShootSound();
            gameState.ValueRW.IsShooting = false;
        }
    }
}