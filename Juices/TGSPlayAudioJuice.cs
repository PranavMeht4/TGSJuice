using UnityEngine;

namespace TGSJuice
{
    [AddComponentMenu("")]
    [JuiceLabel("Play Sound")]
    [JuiceDescription("Play any audio clip")]
    public class TGSPlayAudioJuice : TGSJuiceBase
    {
        public float Volume;
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
                }

                _audioSource.clip = AudioClip;
                _audioSource.volume = Volume;
                _audioSource.Play();
            }
            else
            {
                Debug.LogError($"audioClip is null");
            }
        }
    }
}