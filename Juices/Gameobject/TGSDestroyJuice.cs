using UnityEngine;

namespace TGSJuice
{
    [AddComponentMenu("")]
    [JuiceLabel("Gameobject/Destroy", "Gameobject Icon")]
    [JuiceDescription("Destroy a gameobject")]
    public class TGSDestroyJuice : TGSJuiceBase
    {
        [SerializeField] private GameObject gameObjectToDestroy;
        [SerializeField] private float delay = 0f;

        public override void Play()
        {
            Destroy(gameObjectToDestroy, delay);
        }
    }
}
