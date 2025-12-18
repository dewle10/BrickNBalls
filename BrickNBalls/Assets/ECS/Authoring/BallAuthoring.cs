using Unity.Entities;
using UnityEngine;

class BallAuthoring : MonoBehaviour
{
    public float TargetSpeed = 20f;

    class Baker : Baker<BallAuthoring>
    {
        public override void Bake(BallAuthoring authoring)
        {
            var entity = GetEntity(authoring, TransformUsageFlags.Dynamic);

            AddComponent(entity, new Ball
            {
                TargetSpeed = authoring.TargetSpeed
            });
        }
    }
}

public struct Ball : IComponentData
{
    public float TargetSpeed;
}