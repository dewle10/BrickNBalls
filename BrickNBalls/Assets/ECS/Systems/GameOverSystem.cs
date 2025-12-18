using Unity.Entities;

[UpdateInGroup(typeof(PresentationSystemGroup))]
public partial class GameOverSystem : SystemBase
{
    private EntityQuery _activeBallsQuery;
    private float _gameOverTimer = 0f;
    private float _gameOverDelay = 0.1f;

    protected override void OnCreate()
    {
        RequireForUpdate<GameState>();

        _activeBallsQuery = GetEntityQuery(
            ComponentType.ReadOnly<Ball>(),
            ComponentType.Exclude<DestroyTag>()
        );
    }

    protected override void OnStartRunning()
    {
        _gameOverTimer = 0f;
    }

    protected override void OnUpdate()
    {
        var playerState = SystemAPI.GetSingletonRW<GameState>();

        if (playerState.ValueRO.IsGameOver || playerState.ValueRO.BallAmount > 0) return;

        int activeBallsCount = _activeBallsQuery.CalculateEntityCount();

        if (activeBallsCount <= 0)
        {
            _gameOverTimer += SystemAPI.Time.DeltaTime;
            if (_gameOverTimer > _gameOverDelay)
            {
                playerState.ValueRW.IsGameOver = true;
                GameUIManager.Instance.ShowGameOverPanel(playerState.ValueRO.Score);
            }
        }
        else _gameOverTimer = 0f;
    }
}