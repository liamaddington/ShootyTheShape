using ShootyTheShape.Services.Audio.Enums;

namespace ShootyTheShape.Services.Audio;

public interface IAudioService
{
	void LoadBgm();

	void LoadSfx();

	void PlaySong(BackgroundMusic song);

	void PlayExplosionSfx();

	void PlayShotSfx();

	void PlaySpawnSfx();

	void PauseSong();

	void UnpauseSong();

	void SetBgmVolume(float volume);

	void MuteBgm();

	void UnmuteBgm();

	void SetSfxVolume(float volume);

	void MuteSfx();

	void UnmuteSfx();
}
