using ShootyTheShape.Levels;
using ShootyTheShape.Levels.NG.Aratif.WaveLevels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootyTheShape.Managers;
public static class LevelManager
{
	public static int currentLevel = 1;

	public static ILevel currentLoadedLevel;

	public static void LoadCurrentLevel()
	{
		switch (currentLevel)
		{
			case 1://TODO Add more levels
				currentLoadedLevel = new AratifWaveMission1();
				break;
			case 2:
				currentLoadedLevel = new AratifWaveMission1();
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
		currentLoadedLevel.Update();
		currentLoadedLevel.DrawHud();
	}


}