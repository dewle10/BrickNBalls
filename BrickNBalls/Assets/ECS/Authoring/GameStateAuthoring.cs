using Unity.Entities;
using UnityEngine;

public class GameStateAuthoring : MonoBehaviour
{
    public int BallAmount = 10;
    public float RemoveBorder = -24f;

    class Baker : Baker<GameStateAuthoring>
    {
        public override void Bake(GameStateAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new GameState
            {
                BallAmount = authoring.BallAmount,
                RemoveBorder = authoring.RemoveBorder,
            });
            AddComponent<Inputs>(entity);
        }
    }
}
public struct GameState : IComponentData
{
    public int Score;
    public int BallAmount;
    public bool IsGameOver;
    public int HitsThisFrame;
    public bool IsShooting;
    public float RemoveBorder;
}