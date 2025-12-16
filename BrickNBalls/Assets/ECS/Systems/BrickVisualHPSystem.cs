using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(PresentationSystemGroup))]
public partial class BrickVisualHPSystem : SystemBase
{
    private MaterialPropertyBlock _propBlock;
    private static readonly int ColorId = Shader.PropertyToID("_BaseColor");

    protected override void OnCreate()
    {
        _propBlock = new MaterialPropertyBlock();
    }

    protected override void OnUpdate()
    {
        Color color3HP = Color.red;
        Color color2HP = Color.yellow;
        Color color1HP = Color.green;
        foreach (var (brick, instance) in SystemAPI.Query<RefRO<Brick>, RefRO<CompanionInstance>>()
                .WithChangeFilter<Brick>())
        {
            Renderer renderer = instance.ValueRO.Instance.Value.GetComponent<Renderer>();
            if (renderer == null) return;

            Color targetColor = Color.white;
            switch (brick.ValueRO.Health)
            {
                case 3: targetColor = color3HP; break;
                case 2: targetColor = color2HP; break;
                case 1: targetColor = color1HP; break;
            }

            renderer.GetPropertyBlock(_propBlock);

            _propBlock.SetColor(ColorId, targetColor);

            renderer.SetPropertyBlock(_propBlock);
        }
    }
}