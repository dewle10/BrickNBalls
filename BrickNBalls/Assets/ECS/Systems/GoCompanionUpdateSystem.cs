using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(PresentationSystemGroup))]
public partial class GoCompanionUpdateSystem : SystemBase
{
    protected override void OnCreate()
    {
        RequireForUpdate<CompanionInstance>();
    }

    protected override void OnUpdate()
    {
        foreach (var (ltw, prefab)
                 in SystemAPI.Query<RefRO<LocalToWorld>, RefRO<CompanionInstance>>()
                 .WithNone<CompanionStaticTag>())
        {
            GameObject prefabGo = prefab.ValueRO.Instance;
            if (prefabGo == null) continue;

            prefabGo.transform.SetPositionAndRotation(
                ltw.ValueRO.Position,
                ltw.ValueRO.Rotation
            );
        }
    }
}
