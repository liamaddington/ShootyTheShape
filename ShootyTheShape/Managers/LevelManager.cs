using ShootyTheShape.Levels;
using ShootyTheShape.Levels.NG.Aratif.WaveLevels;

namespace ShootyTheShape.Managers;

public static class LevelManager
{
	public static int CurrentLevel = 1;

	public static ILevel CurrentLoadedLevel;

	public static void LoadCurrentLevel()
	{
		switch (CurrentLevel)
		{
			case 1://TODO Add more levels
				CurrentLoadedLevel = new AratifWaveMission1();
				break;
			case 2:
				CurrentLoadedLevel = new AratifWaveMission1();
				break;
			case 3:
				break;
			case 4:
				break;
			case 5:
				break;
			case 6:
				break;
			case 7:
				break;
		}
	}
	public static void Update()
	{
		CurrentLoadedLevel.Update();
		CurrentLoadedLevel.DrawHud();
	}
}
