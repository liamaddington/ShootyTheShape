using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using ShootyTheShape.Services.Audio.Enums;

namespace ShootyTheShape.Services.Audio;

internal class AudioService : ContentManager, IAudioService
{
	public IDictionary<BackgroundMusic, Song> BGM = new Dictionary<BackgroundMusic, Song>();

	public Song Music { get; set; }

	private Random _random { get; } = new Random();

	public List<SoundEffect> Explosions = new List<SoundEffect>();
	public SoundEffect Explosion { get { return Explosions[_random.Next(Explosions.Count)]; } }

	public List<SoundEffect> Shots = new List<SoundEffect>();
	public SoundEffect Shot { get { return Shots[_random.Next(Shots.Count)]; } }

	public List<SoundEffect> Spawns = new List<SoundEffect>();
	public SoundEffect Spawn { get { return Spawns[_random.Next(Spawns.Count)]; } }

	private string contentRoot = "Sound/";

	public AudioService(IServiceProvider serviceProvider, string contentRoot)
		: base(serviceProvider, contentRoot)
	{
		MuteSfx();
	}

	public void LoadBgm()
	{
		BGM.Add(BackgroundMusic.MainMenu, base.Load<Song>(contentRoot + "MainBattleTheme"));
	}

	public void LoadSfx()
	{
		for (int i = 1; i <= 8; ++i)
		{
			Explosions.Add(base.Load<SoundEffect>("Sound/explosion-0" + i)); //TODO, Finish explosion list population
		}
		for (int i = 1; i <= 4; ++i)
		{
			Shots.Add(base.Load<SoundEffect>("Sound/shoot-0" + i));
		}
		for (int i = 1; i <= 8; ++i)
		{
			Spawns.Add(base.Load<SoundEffect>("Sound/spawn-0" + i));
		}
	}

	public void PlaySong(BackgroundMusic song)
	{
		BGM.TryGetValue(song, out Song songToPlay);
		MediaPlayer.Play(songToPlay);
	}

	public void PauseSong()
	{
		MediaPlayer.Pause();
	}

	public void UnpauseSong()
	{
		MediaPlayer.Resume();
	}

	public void SetBgmVolume(float volume)
	{
		MediaPlayer.Volume = volume;
	}

	public void MuteBgm()
	{
		MediaPlayer.IsMuted = true;
	}

	public void UnmuteBgm()
	{
		MediaPlayer.IsMuted = false;
	}

	public void SetSfxVolume(float volume)
	{
		SoundEffect.MasterVolume = volume;
	}

	private float _volumeBeforeMuting = 0.5f;

	public void MuteSfx()
	{
		_volumeBeforeMuting = SoundEffect.MasterVolume;//TODO; See if there is a better way to mute and unmute SFX specifically
		SoundEffect.MasterVolume = 0;
	}

	public void UnmuteSfx()
	{
		SoundEffect.MasterVolume = _volumeBeforeMuting;
	}

	public void PlayExplosionSfx()
	{
		Explosion.Play(0f, _random.NextFloat(-0.2f, 0.2f), 0);
	}

	public void PlayShotSfx()
	{
		Shot.Play(0.2f, _random.NextFloat(-0.2f, 0.2f), 0);
	}

	public void PlaySpawnSfx()
	{
		throw new NotImplementedException();
	}
}
