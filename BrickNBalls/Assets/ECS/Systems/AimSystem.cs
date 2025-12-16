using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateBefore(typeof(TransformSystemGroup))]
partial struct AimSystem : ISystem
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
        foreach (var (cannon, transform) in
                     SystemAPI.Query<RefRW<Cannon>, RefRW<LocalTransform>>())
        {
            if (input.MoveValue == 0) continue;

            float rot = SystemAPI.Time.DeltaTime * cannon.ValueRO.AimSpeed * input.MoveValue;
            float limit = cannon.ValueRO.MaxYAngle;
            float newAngle = cannon.ValueRO.CurrentYAngle + rot;
            newAngle = math.clamp(newAngle, -limit, limit);
            cannon.ValueRW.CurrentYAngle = newAngle;
            transform.ValueRW.Rotation = quaternion.RotateY(newAngle);
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
