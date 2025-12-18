using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using static BrickSpawnerAuthoring;

[UpdateInGroup(typeof(InitializationSystemGroup))]
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
        var configEntity = SystemAPI.GetSingletonEntity<BrickSpawnerConfig>();
        if (SystemAPI.HasComponent<SpawnedTag>(configEntity)) return;
        var config = SystemAPI.GetSingleton<BrickSpawnerConfig>();

        var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

        float totalWidth = (config.Columns - 1) * config.Spacing.x;
        float totalDepth = (config.Rows - 1) * config.Spacing.y;

        float startX = config.CenterPosition.x - (totalWidth / 2f);
        float startZ = config.CenterPosition.z - (totalDepth / 2f);

        uint seed = (uint)(SystemAPI.Time.ElapsedTime * 100000) + 1;
        var random = new Random(seed);

        for (int x = 0; x < config.Columns; x++)
        {
            for (int z = 0; z < config.Rows; z++)
            {
                if ((z >= 9 && z <= 11) && (x == 3 || x == 4 || x == 15 || x == 16)) continue; //Space for columns

                if (random.NextFloat() < config.SpawnChance)
                {
                    Entity brick = ecb.Instantiate(config.BrickPrefab);

                    float hpRoll = random.NextFloat();
                    int finalHp;
                    float chance2hp = config.Chance1hp + config.Chance2hp;

                    if (hpRoll < config.Chance1hp)
                    {
                        finalHp = 1;
                    }
                    else if (hpRoll < chance2hp) 
                    {
                        finalHp = 2;
                    }
                    else
                    {
                        finalHp = 3;
                    }
                    ecb.SetComponent(brick, new Brick { Health = finalHp });

                    float posX = startX + (x * config.Spacing.x);
                    float posZ = startZ + (z * config.Spacing.y);
                    float3 position = new float3(posX, config.CenterPosition.y, posZ);

                    ecb.SetComponent(brick, LocalTransform.FromPosition(position));
                }
            }
        }
        ecb.AddComponent<SpawnedTag>(configEntity);
    }
}
