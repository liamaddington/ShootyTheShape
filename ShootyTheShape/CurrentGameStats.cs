using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootyTheShape;
public static class CurrentGameStats
{
	public static int KillCounter = 0;
	public static int BossKillCounter = 0;

	public static Stopwatch GameTimer = new Stopwatch();

	public static TimeSpan BestTime = new TimeSpan(0, 0, 0);

	public static int RemainingLives = 3;

	public static bool BossWave = false;
}
