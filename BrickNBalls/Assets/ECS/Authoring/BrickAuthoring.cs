using Unity.Entities;
using UnityEngine;

public class BrickAuthoring : MonoBehaviour
{
    public int Health = 1; 
    class Baker : Baker<BrickAuthoring>
    {
        public override void Bake(BrickAuthoring authoring)
        {
            var entity = GetEntity(authoring, TransformUsageFlags.Dynamic);

            AddComponent(entity, new Brick
            {
                Health = authoring.Health,
                LastHitTime = 0
            });
            AddComponent<CompanionStaticTag>(entity);
        }
    }
}

public struct Brick : IComponentData
{
    public int Health; // 1-3
    public double LastHitTime;
}
