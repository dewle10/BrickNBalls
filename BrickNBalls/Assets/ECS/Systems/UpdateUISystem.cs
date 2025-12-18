using Unity.Entities;
using UnityEngine;

[UpdateInGroup(typeof(PresentationSystemGroup))]
public partial class UpdateUISystem : SystemBase
{
    private int _lastScore = -1;
    private int _lastBalls = -1;

    protected override void OnCreate()
    {
        RequireForUpdate<GameState>();
    }

    protected override void OnUpdate()
    {
        if (GameUIManager.Instance == null) return;
        var gameState = SystemAPI.GetSingleton<GameState>();
        int currentScore = gameState.Score;
        int currentBalls = gameState.BallAmount;

        if (currentScore != _lastScore)
        {
            GameUIManager.Instance.UpdateScore(currentScore);
            _lastScore = currentScore;
        }
        if (currentBalls != _lastBalls)
        {
            GameUIManager.Instance.UpdateBalls(currentBalls);
            _lastBalls = currentBalls;
        }
    }
}