using System;
using Plugin.Maui.Audio;

namespace Skadi.Services;

public class SoundService
{
    IAudioPlayer audioPlayer;

    public SoundService()
    {
        Stream track = FileSystem.OpenAppPackageFileAsync("bell.mp3").Result;
        audioPlayer = AudioManager.Current.CreatePlayer(track);
    }

    public void Play()
    {
        if (audioPlayer != null)
            audioPlayer.Play();
    }
}
