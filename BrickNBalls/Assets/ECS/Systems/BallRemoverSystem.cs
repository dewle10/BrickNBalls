using Unity.Burst;
using Unity.Entities;
using Unity.Physics.Systems;
using Unity.Transforms;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(PhysicsSystemGroup))]
public partial struct BallRemoverSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Ball>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var gameState = SystemAPI.GetSingletonRW<GameState>();
        var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

        foreach (var (transform, entity) in SystemAPI.Query<RefRO<LocalTransform>>()
                 .WithAll<Ball>()
                 .WithEntityAccess())
        {
            if (transform.ValueRO.Position.z < gameState.ValueRO.RemoveBorder)
            {
                //CleanupSystem destroys entity with DestroyTag along with their gameobject companion
                ecb.AddComponent<DestroyTag>(entity);  
            }
        }
    }
}
