using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class CannonAuthoring : MonoBehaviour
{
    public GameObject BallPrefab;
    public float BallSpeed = 20f;
    public float AimSpeed = 1f;
    public float3 SpawnPoint;
    public float MaxYAngle = 86f;

    class Baker : Baker<CannonAuthoring>
    {
        public override void Bake(CannonAuthoring authoring)
        {
            var entity = GetEntity(authoring, TransformUsageFlags.Dynamic);

            AddComponent(entity, new Cannon
            {
                BallPrefab = GetEntity(authoring.BallPrefab, TransformUsageFlags.Dynamic),
                BallSpeed = authoring.BallSpeed,
                AimSpeed = authoring.AimSpeed,
                SpawnPoint = authoring.SpawnPoint,
                LastFireTime = 0,
                CurrentYAngle = authoring.transform.rotation.y,
                MaxYAngle = math.radians(authoring.MaxYAngle)
            });
        }
    }
}

public struct Cannon : IComponentData
{
    public Entity BallPrefab;
    public float BallSpeed;
    public float AimSpeed;
    public float3 SpawnPoint;
    public double LastFireTime;
    public float CurrentYAngle;
    public float MaxYAngle;
}
