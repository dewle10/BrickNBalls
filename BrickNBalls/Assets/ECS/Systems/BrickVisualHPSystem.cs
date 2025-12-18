using Unity.Entities;
using UnityEngine;

[UpdateInGroup(typeof(PresentationSystemGroup))]
public partial class BrickVisualHPSystem : SystemBase
{
    private MaterialPropertyBlock _propBlock;
    private EntityQuery _textureQuery;

    protected override void OnCreate()
    {
        _propBlock = new MaterialPropertyBlock();
        _textureQuery = GetEntityQuery(ComponentType.ReadOnly<BrickTextures>());
        RequireForUpdate(_textureQuery);
    }

    protected override void OnUpdate()
    {
        var textureConfig = _textureQuery.GetSingleton<BrickTextures>();

        foreach (var (brick, instance) in SystemAPI.Query<RefRO<Brick>, RefRO<CompanionInstance>>()
                .WithChangeFilter<Brick>())
        {
            Renderer renderer = instance.ValueRO.Renderer;
            if (renderer == null) return;

            Texture2D targetTexture = null;
            switch (brick.ValueRO.Health)
            {
                case 3: targetTexture = textureConfig.Texture3HP; break;
                case 2: targetTexture = textureConfig.Texture2HP; break;
                case 1: targetTexture = textureConfig.Texture1HP; break;
            }

            if (targetTexture == null) return;

            renderer.GetPropertyBlock(_propBlock);
            _propBlock.SetTexture("_BaseMap", targetTexture);
            renderer.SetPropertyBlock(_propBlock);
        }
    }
}