using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class BrickSpawnerAuthoring : MonoBehaviour
{
    public GameObject BrickPrefab;
    public int Columns = 20;
    public int Rows = 11;
    public Vector2 Spacing = new(2.5f, 1.5f);
    public Vector3 CenterPosition = new(0, 0, 5);
    [Range(0f, 1f)] public float SpawnChance = 0.4f;
    [Range(0f, 0.5f)] public float Chance1hp = 0.45f;
    [Range(0f, 0.5f)] public float Chance2hp = 0.3f;

    class Baker : Baker<BrickSpawnerAuthoring>
    {
        public override void Bake(BrickSpawnerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new BrickSpawnerConfig
            {
                BrickPrefab = GetEntity(authoring.BrickPrefab, TransformUsageFlags.Dynamic),
                Columns = authoring.Columns,
                Rows = authoring.Rows,
                Spacing = authoring.Spacing,
                CenterPosition = authoring.CenterPosition,
                SpawnChance = authoring.SpawnChance,
                Chance1hp = authoring.Chance1hp,
                Chance2hp = authoring.Chance2hp,
            });
        }
    }

    public struct BrickSpawnerConfig : IComponentData
    {
        public Entity BrickPrefab;
        public int Columns;
        public int Rows;
        public float2 Spacing;
        public float3 CenterPosition;
        public float SpawnChance;
        public float Chance1hp;
        public float Chance2hp;
    }
}