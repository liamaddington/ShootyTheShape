using ShootyTheShape.Levels;
using ShootyTheShape.Runtime;
using ShootyTheShape.Services.Content;
using ShootyTheShape.Services.Rendering;
using ShootyTheShape.Services.Spawning;

namespace ShootyTheShape.GameModes;

internal abstract class GameModeBaseClass : ILevel
{
	protected ISpawnService SpawnService { get; }
	protected IContentService ContentService { get; }
	protected IRenderService RenderService { get; }
	protected GameSession GameSession { get; }

	protected GameModeBaseClass(
		ISpawnService spawnService,
		IContentService contentService,
		IRenderService renderService,
		GameSession gameSession)
	{
		SpawnService = spawnService;
		ContentService = contentService;
		RenderService = renderService;
		GameSession = gameSession;
	}

	public abstract void CheckAndHandleWinCondition();
	public abstract void CheckAndHandleLoseCondition();
	public abstract void Update();
	public abstract void DrawHud();

	public abstract void ResetGameMode();

	public virtual IHud Hud { get; set; }

	public abstract void EnableHud();
	public abstract void DisableHud();

}
