using System;
using UnityEngine;

namespace TGSJuice
{
    [System.Serializable]
    [AddComponentMenu("")]
    public class TGSJuiceBase : MonoBehaviour
    {
        public TGSJuiceBase() { }
        public virtual Type ActionType { get; }
        public virtual void Play() { }
    }
}