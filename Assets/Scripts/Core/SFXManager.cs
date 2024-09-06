using System;
using UnityEngine;

namespace Core
{
    public class SFXManager : MonoBehaviour
    {
        public static SFXManager instance;
        [SerializeField] private AudioSource sfxObject;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }

        public void PlaySFXClip(AudioClip audioClip, float volume, Transform spawnPoint)
        {
            // Make audio source
            AudioSource audioSource = Instantiate(sfxObject, spawnPoint.position, Quaternion.identity);
            audioSource.clip = audioClip;
            audioSource.volume = volume;

            // Play audio clip
            audioSource.Play();

            // Destroy audio source once sound done
            Destroy(audioSource.gameObject, audioSource.clip.length);
        }
        
        public void PlayRandomSFXClip(AudioClip[] audioClip, float volume, Transform spawnPoint)
        {
            // Make audio source
            AudioSource audioSource = Instantiate(sfxObject, spawnPoint.position, Quaternion.identity);
            audioSource.clip = audioClip[UnityEngine.Random.Range(0, audioClip.Length)];
            audioSource.volume = volume;

            // Play audio clip
            audioSource.Play();

            // Destroy audio source once sound done
            Destroy(audioSource.gameObject, audioSource.clip.length);
        }
    }
}