using UnityEngine;

namespace TGSJuice
{
    [AddComponentMenu("")]
    [JuiceLabel("Gameobject/Toggle", "Gameobject Icon")]
    [JuiceDescription("Enable or disable a gameobject")]
    public class TGSToggleJuice : TGSJuiceBase
    {
        [SerializeField] private GameObject target;
        [SerializeField] private bool enable;

        public override void Play()
        {
            target.SetActive(enable);
        }
    }
}