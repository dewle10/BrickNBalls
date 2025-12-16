using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using Unity.VisualScripting;

[UpdateBefore(typeof(TransformSystemGroup))]
partial struct ShootingSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Inputs>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var input = SystemAPI.GetSingleton<Inputs>();
        double currentTime = SystemAPI.Time.ElapsedTime;
        var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

        foreach (var (cannon, transform) in
                     SystemAPI.Query<RefRW<Cannon>, RefRO<LocalToWorld>>())
        {
            if (cannon.ValueRO.BallAmount <= 0) continue;
            if (input.IsShooting)
            {
                if (currentTime - cannon.ValueRO.LastFireTime > 1)
                {
                    cannon.ValueRW.LastFireTime = currentTime;

                    cannon.ValueRW.BallAmount -= 1;
                    Entity projectile = ecb.Instantiate(cannon.ValueRO.BallPrefab);
                    float3 spawnPos = math.transform(transform.ValueRO.Value, cannon.ValueRO.SpawnPoint);
                    ecb.SetComponent(projectile, LocalTransform.FromPositionRotation(spawnPos, quaternion.identity));
                    float3 dir = transform.ValueRO.Forward;
                    ecb.SetComponent(projectile, new Unity.Physics.PhysicsVelocity
                    {
                        Linear = dir * cannon.ValueRO.BallSpeed,
                        Angular = float3.zero
                    });
                }
            }
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
