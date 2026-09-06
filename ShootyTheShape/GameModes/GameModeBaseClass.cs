using ShootyTheShape.Levels;
using ShootyTheShape.Services.Content;
using ShootyTheShape.Services.Spawning;

namespace ShootyTheShape.GameModes;

internal abstract class GameModeBaseClass : ILevel
{
	public ISpawnService SpawnService;
	public IContentService ContentService;

	public abstract void CheckAndHandleWinCondition();
	public abstract void CheckAndHandleLoseCondition();
	public abstract void Update();
	public abstract void DrawHud();

	public abstract void ResetGameMode();

	public virtual IHud Hud { get; set; }

	public abstract void EnableHud();
	public abstract void DisableHud();

	public void LoadSpawnService()
	{
		SpawnService = (ISpawnService)GameRoot.ServiceProvider.GetService(typeof(ISpawnService));
	}

	protected void LoadContentService()
	{
		ContentService = (IContentService)GameRoot.ServiceProvider.GetService(typeof(IContentService));
	}
}
