using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial struct PlayerSystem : ISystem
{
    // Because OnUpdate accesses a managed object (the camera), we cannot Burst compile 
    // this method, so we don't use the [BurstCompile] attribute here.

    public void OnUpdate(ref SystemState state)
    {
        var deltaTime = SystemAPI.Time.DeltaTime;

        foreach (var (playerTransform, moveInput, moveSpeed) in
                 SystemAPI.Query<RefRW<LocalTransform>, RefRO<PlayerMoveInput>, RefRO<PlayerMoveSpeed>>()
                     .WithAll<Player>())
        {
            // move the player tank
            playerTransform.ValueRW.Position.xz += moveInput.ValueRO.Value /** moveSpeed.ValueRO.Value*/ * deltaTime;

            if (math.lengthsq(moveInput.ValueRO.Value) > float.Epsilon)
            {
                var forward = new float3(moveInput.ValueRO.Value.x, 0f, moveInput.ValueRO.Value.y);
                playerTransform.ValueRW.Rotation = quaternion.LookRotation(forward, math.up());
            }

            // move the camera to follow the player
            var cameraTransform = Camera.main.transform;
            cameraTransform.position = playerTransform.ValueRO.Position;
            cameraTransform.position -= 10.0f * (Vector3)playerTransform.ValueRO.Forward();  // move the camera back from the player
            cameraTransform.position += new Vector3(0, 5f, 0);  // raise the camera by an offset
            cameraTransform.LookAt(playerTransform.ValueRO.Position);  // look at the player
        }
    }
}