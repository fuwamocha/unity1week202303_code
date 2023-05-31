using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace MochaLib.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class SePlayer : MonoBehaviour, IAudioPlayer
    {
        [SerializeField] private AudioClip seAudioClip;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private Slider seSlider;
        [SerializeField] private EventTrigger eventTrigger;
        private void Reset()
        {
            _audioSource = GetComponent<AudioSource>();
        }
        private void Start()
        {
            _audioSource.playOnAwake = false;

            eventTrigger.triggers = new List<EventTrigger.Entry>
            {
                new()
                {
                    eventID = EventTriggerType.PointerUp,
                    callback = new EventTrigger.TriggerEvent()
                }
            };
            eventTrigger.triggers[0].callback.AddListener(data => Play(seAudioClip, false));
            seSlider.onValueChanged.AddListener(SetVolume);
            seSlider.value = 0.3f;
        }
        public void Play(AudioClip audioClip, bool isLoop)
        {
            if (audioClip == null) throw new NullReferenceException($"AudioClip {audioClip} is null.");
            if (isLoop) throw new Exception("SEPlayer can't play looped audio.");
            _audioSource.PlayOneShot(audioClip);
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
        public void Play(AudioClip audioClip)
        {
            if (audioClip == null) throw new NullReferenceException($"AudioClip {audioClip} is null.");
            _audioSource.PlayOneShot(audioClip);
        }
    }
}
