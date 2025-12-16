using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

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
        var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
        float removeBorderZ = -22.0f;

        foreach (var (transform, entity) in SystemAPI.Query<RefRO<LocalTransform>>()
                 .WithAll<Ball>()
                 .WithEntityAccess())
        {
            if (transform.ValueRO.Position.z < removeBorderZ)
            {
                ecb.AddComponent<DestroyTag>(entity);
            }
        }
    }
}
