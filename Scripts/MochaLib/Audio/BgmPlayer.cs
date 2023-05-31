using System;
using UnityEngine;
using UnityEngine.UI;
namespace MochaLib.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class BgmPlayer : MonoBehaviour, IAudioPlayer
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private Slider bgmSlider;

        private void Reset()
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSource.playOnAwake = false;
        }
        private void Start()
        {
            bgmSlider.onValueChanged.AddListener(SetVolume);
            bgmSlider.value = 0.2f;
        }
        public void Play(AudioClip audioClip, bool isLoop)
        {
            if (audioClip == null) throw new NullReferenceException($"AudioClip {audioClip} is null.");
            _audioSource.clip = audioClip;
            _audioSource.loop = isLoop;
            _audioSource.Play();
        }

        public void Stop()
        {
            _audioSource.Stop();
        }

        public void SetVolume(float volume)
        {
            if (volume is < 0 or > 1)
            {
                throw new ArgumentOutOfRangeException($"Volume {volume} is out of range.");
            }
            _audioSource.volume = volume;
        }
    }
}
