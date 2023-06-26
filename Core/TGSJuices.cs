using System.Collections.Generic;
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
            foreach (var juice in juices)
            {
                juice.Play();
            }
        }
    }
}
