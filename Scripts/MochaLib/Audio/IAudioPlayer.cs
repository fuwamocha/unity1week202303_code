using UnityEngine;
namespace MochaLib.Audio
{
    public interface IAudioPlayer
    {
        public void Play(AudioClip audioClip, bool isLoop);
        public void Stop();
        public void SetVolume(float volume);
    }
}
