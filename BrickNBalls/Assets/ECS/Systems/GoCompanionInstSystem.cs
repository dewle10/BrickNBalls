using Unity.Entities;
using Unity.Collections;
using UnityEngine;
using Unity.Transforms;

[UpdateInGroup(typeof(PresentationSystemGroup))]
public partial class GoCompanionInstSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var ecb = new EntityCommandBuffer(Allocator.Temp);

        foreach (var (prefab, ltw, entity)
                 in SystemAPI.Query<RefRO<CompanionPrefab>, RefRW<LocalToWorld>>()
                              .WithNone<CompanionInstance>()
                              .WithEntityAccess())
        {
            GameObject prefabGo = prefab.ValueRO.Prefab;
            if (prefabGo == null)
                continue;

            GameObject instance = Object.Instantiate(prefabGo);

            ecb.AddComponent(entity, new CompanionInstance
            {
                Instance = instance
            });

            instance.transform.SetPositionAndRotation(
                ltw.ValueRO.Position,
                ltw.ValueRO.Rotation
            );
        }

        ecb.Playback(EntityManager);
        ecb.Dispose();
    }
}
