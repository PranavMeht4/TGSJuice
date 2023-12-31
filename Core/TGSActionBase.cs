using UnityEngine;
using System.Collections;

namespace TGSJuice
{
    public abstract class TGSActionBase<T> : MonoBehaviour
    {
        public delegate void ActionDelegate(T actionParams);
        public static event ActionDelegate ActionInvoked;

        public int ChannelId;

        protected virtual void OnEnable()
        {
            ActionInvoked += PerformAction;
        }

        protected virtual void OnDisable()
        {
            ActionInvoked -= PerformAction;
        }

        public static void InvokeAction(T actionParams)
        {
            ActionInvoked?.Invoke(actionParams);
        }

        protected abstract void PerformAction(T actionParams);
    }
}