using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace TGSJuice
{
    [DisallowMultipleComponent]
    public class TGSJuices : MonoBehaviour
    {
        [SerializeReference]
        public List<TGSJuiceBase> juices = new List<TGSJuiceBase>();

        public void PlayAll()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                return;
#endif
            foreach (var juice in juices)
            {
                juice.Play();
            }
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
