using UnityEngine;

namespace TGSJuice
{
    [AddComponentMenu("")]
    [JuiceLabel("Play Sound", "AudioSource Icon")]
    [JuiceDescription("Play any audio clip")]
    public class TGSPlayAudioJuice : TGSJuiceBase
    {
        public float Volume = 1;
        public AudioClip AudioClip;

        private GameObject _audioSourceObject;
        private AudioSource _audioSource;

        public override void Play()
        {
            if (AudioClip != null)
            {
                if (_audioSourceObject == null)
                {
                    _audioSourceObject = new GameObject("AudioSource");
                    _audioSource = _audioSourceObject.AddComponent<AudioSource>();
                    _audioSource.playOnAwake = false;
                }

                _audioSource.clip = AudioClip;
                _audioSource.volume = Volume;
                _audioSource.PlayOneShot(AudioClip);
            }
            else
            {
                Debug.LogError($"audioClip is null");
            }
        }
    }
}