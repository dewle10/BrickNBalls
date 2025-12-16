using Unity.Burst;
using Unity.Transforms;
using Unity.Entities;

public partial struct CubeRotationSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var deltaTime = SystemAPI.Time.DeltaTime;

        foreach (var (transform, rotationSpeed) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<RotationSpeed>>())
        {
            var radians = rotationSpeed.ValueRO.RadiansPerSecond * deltaTime;
            transform.ValueRW = transform.ValueRW.RotateY(radians);
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
