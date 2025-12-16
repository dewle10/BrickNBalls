using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using static BrickSpawnerAuthoring;

partial struct BrickSpawnerSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<BrickSpawnerConfig>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var config = SystemAPI.GetSingleton<BrickSpawnerConfig>();
        var random = new Random(config.RandomSeed);
        var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

        float totalWidth = (config.Columns - 1) * config.Spacing.x;
        float totalDepth = (config.Rows - 1) * config.Spacing.y;

        float startX = config.CenterPosition.x - (totalWidth / 2f);
        float startZ = config.CenterPosition.z - (totalDepth / 2f);

        for (int x = 0; x < config.Columns; x++)
        {
            for (int z = 0; z < config.Rows; z++)
            {
                if (random.NextFloat() < config.SpawnChance)
                {
                    Entity brick = ecb.Instantiate(config.BrickPrefab);
                    ecb.SetComponent(brick, new Brick { Health = random.NextInt(1, 4) });

                    float posX = startX + (x * config.Spacing.x);
                    float posZ = startZ + (z * config.Spacing.y);
                    float3 position = new float3(posX, config.CenterPosition.y, posZ);

                    ecb.SetComponent(brick, LocalTransform.FromPosition(position));
                }
            }
        }
        state.Enabled = false;
    }
}
