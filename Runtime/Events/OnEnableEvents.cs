using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Aijai.Events
{
    public class OnEnableEvents : MonoBehaviour
    {
        public UnityEvent m_onEnable;
        public UnityEvent m_onDisable;

        private void OnEnable()
        {
            m_onEnable.Invoke();
        }

        private void OnDisable()
        {
            m_onDisable.Invoke();
        }

        public void Log(string value)
        {
            if (Debug.isDebugBuild)
                Debug.Log(value);
        }
    }

}
