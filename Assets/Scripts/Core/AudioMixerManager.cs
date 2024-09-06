using UnityEngine;
using UnityEngine.Audio;

namespace Core
{
    public class AudioMixerManager : MonoBehaviour
    {
        [SerializeField] private AudioMixer audioMixer;

        public void SetMasterVolume(float level)
        {
            audioMixer.SetFloat("mainAudioMixerMasterVolume", Mathf.Log10(level) * 20f);
        }

        public void SetMusicVolume(float level)
        {
            audioMixer.SetFloat("mainAudioMixerMusicVolume", Mathf.Log10(level) * 20f);
        }

        public void SetSFXVolume(float level)
        {
            audioMixer.SetFloat("mainAudioMixerSFXVolume", Mathf.Log10(level) * 20f);
        }
    }
}