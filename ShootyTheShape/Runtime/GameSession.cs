using ShootyTheShape.Enums;
using ShootyTheShape.Managers;

namespace ShootyTheShape.Runtime;

public sealed class GameSession
{
	public GameState CurrentGameState { get; set; } = GameState.Playing;

	public void Reset()
	{
		EntityManager.RemoveAllEnemies();
		EntityManager.RemoveAllBosses();
		CurrentGameStats.KillCounter = 0;
		CurrentGameStats.RemainingLives = 3;
		CurrentGameStats.BossWave = false;
		CurrentGameStats.GameTimer.Restart();
	}
}
