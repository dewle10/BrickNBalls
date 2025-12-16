using Unity.Entities;
using UnityEngine;

[UpdateInGroup(typeof(PresentationSystemGroup))]
public partial class CleanupSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

        foreach (var (visual, entity)
                 in SystemAPI.Query<RefRO<CompanionInstance>>()
                              .WithAll<DestroyTag>()
                              .WithEntityAccess())
        {
            GameObject go = visual.ValueRO.Instance;
            if (go != null) Object.Destroy(go);

            ecb.DestroyEntity(entity);
        }
        ecb.Playback(EntityManager);
        ecb.Dispose();
    }
}
