using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Events;

namespace TGSJuice
{
    [DisallowMultipleComponent]
    public class TGSJuices : MonoBehaviour
    {
        public float Delay = 0f;
        public UnityEvent OnStarted;
        public UnityEvent OnEnded;

        [SerializeReference]
        public List<TGSJuiceBase> juices = new List<TGSJuiceBase>();

        public void PlayAll()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                return;
#endif

            StartCoroutine(PlayAllWithDelay());
        }

        private IEnumerator PlayAllWithDelay()
        {
            yield return new WaitForSeconds(Delay);
            OnStarted?.Invoke();

            foreach (var juice in juices)
            {
                juice.Play();
            }

            OnEnded?.Invoke();
        }


#if UNITY_EDITOR
        protected virtual void OnDestroy()
        {
            if (!Application.isPlaying)
            {
                foreach (var juice in juices)
                {
                    DestroyImmediate(juice);
                }
            }
        }
#endif
    }
}