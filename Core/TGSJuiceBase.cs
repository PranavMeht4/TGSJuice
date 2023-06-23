using UnityEngine;

namespace TGSJuice
{
    [System.Serializable]
    [AddComponentMenu("")]
    public class TGSJuiceBase : MonoBehaviour
    {
        public TGSJuiceBase() { }
        public virtual void Play() { }
    }
}