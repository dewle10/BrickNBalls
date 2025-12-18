using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(PresentationSystemGroup))]
public partial class GoCompanionInstSystem : SystemBase
{
    protected override void OnCreate()
    {
        RequireForUpdate(
            SystemAPI.QueryBuilder()
                .WithAll<CompanionPrefab>()
                .WithNone<CompanionInstance>()
                .Build()
        );
    }
    protected override void OnUpdate()
    {
        var ecb = new EntityCommandBuffer(Allocator.Temp);

        foreach (var (prefab, ltw, entity)
                 in SystemAPI.Query<RefRO<CompanionPrefab>, RefRO<LocalToWorld>>()
                              .WithNone<CompanionInstance>()
                              .WithEntityAccess())
        {
            GameObject prefabGo = prefab.ValueRO.Prefab;
            if (prefabGo == null)
                continue;

            GameObject instance = Object.Instantiate(prefabGo);

            ecb.AddComponent(entity, new CompanionInstance
            {
                Instance = instance,
                Renderer = instance.GetComponent<Renderer>()
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
