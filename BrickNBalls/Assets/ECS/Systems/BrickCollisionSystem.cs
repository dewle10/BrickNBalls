using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(PhysicsSystemGroup))]
public partial struct BrickCollisionSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Ball>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var gameState = SystemAPI.GetSingletonRW<GameState>();
        var simSingleton = SystemAPI.GetSingleton<SimulationSingleton>();
        var ecbSingleton = SystemAPI.GetSingleton<EndFixedStepSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

        double currentTime = SystemAPI.Time.ElapsedTime;

        var scoreCounter = new NativeReference<int>(Allocator.TempJob){Value = 0};
        var hitsCounter = new NativeReference<int>(Allocator.TempJob) {Value = 0};

        var job = new CollisionJob
        {
            BallLookup = SystemAPI.GetComponentLookup<Ball>(true),
            BrickLookup = SystemAPI.GetComponentLookup<Brick>(false),
            VelocityLookup = SystemAPI.GetComponentLookup<PhysicsVelocity>(false),
            Ecb = ecb,
            CurrentTime = currentTime,
            ScoreCounter = scoreCounter,
            HitsCounter = hitsCounter
        };

        state.Dependency = job.Schedule(simSingleton, state.Dependency);
        state.Dependency.Complete();

        if(hitsCounter.Value > 0)
        {
            gameState.ValueRW.Score += scoreCounter.Value;
            gameState.ValueRW.HitsThisFrame = hitsCounter.Value;
        }

        scoreCounter.Dispose();
        hitsCounter.Dispose();
    }

    [BurstCompile]
    struct CollisionJob : ICollisionEventsJob
    {
        [ReadOnly] public ComponentLookup<Ball> BallLookup;
        public ComponentLookup<Brick> BrickLookup;
        public ComponentLookup<PhysicsVelocity> VelocityLookup;

        public EntityCommandBuffer Ecb;
        public double CurrentTime;
        public NativeReference<int> ScoreCounter;
        public NativeReference<int> HitsCounter;

        public void Execute(CollisionEvent collisionEvent)
        {
            Entity entityA = collisionEvent.EntityA;
            Entity entityB = collisionEvent.EntityB;

            Entity ballEntity = Entity.Null;
            Entity brickEntity = Entity.Null;

            if (BallLookup.HasComponent(entityA))
            {
                ballEntity = entityA;
                HitsCounter.Value++;
            }
            else if (BrickLookup.HasComponent(entityA))
            {
                brickEntity = entityA;
            }

            if (BallLookup.HasComponent(entityB))
            {
                HitsCounter.Value++;
                if (ballEntity == Entity.Null) 
                    ballEntity = entityB;
            }
            else if (BrickLookup.HasComponent(entityB))
            {
                if (brickEntity == Entity.Null) 
                    brickEntity = entityB;
            }

            if (ballEntity != Entity.Null && brickEntity != Entity.Null)
            {
                HandleBallBrickColl(brickEntity);
            }

            ResetBallVelocity(ballEntity);
        }

        private void HandleBallBrickColl(Entity brickEntity)
        {
            if (!BrickLookup.HasComponent(brickEntity))
                return;

            var brick = BrickLookup[brickEntity];

            if (CurrentTime - brick.LastHitTime < 0.05)
            {
                return;
            }

            brick.LastHitTime = CurrentTime;
            brick.Health--;
            ScoreCounter.Value++;

            if (brick.Health <= 0)
            {
                Ecb.AddComponent<DestroyTag>(brickEntity);
            }
            else
            {
                BrickLookup[brickEntity] = brick;
            }
        }
        private void ResetBallVelocity(Entity ballEntity)
        {
            if (ballEntity != Entity.Null && VelocityLookup.HasComponent(ballEntity))
            {
                var physicsVel = VelocityLookup[ballEntity];
                var ballData = BallLookup[ballEntity];

                float3 currentLinear = physicsVel.Linear;
                currentLinear.y = 0f;
                if (math.lengthsq(currentLinear) > 0.001f)
                {
                    float3 direction = math.normalize(currentLinear);
                    physicsVel.Linear = direction * ballData.TargetSpeed;
                    VelocityLookup[ballEntity] = physicsVel;
                }
            }
        }
    }
}
