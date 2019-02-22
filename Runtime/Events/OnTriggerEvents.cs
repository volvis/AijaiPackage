using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Aijai.Events
{
    public class OnTriggerEvents : MonoBehaviour
    {
        [SerializeField] LayerMask m_layerMask = ~0;

        public GameObjectUnityEvent OnEnter;
        public GameObjectUnityEvent OnExit;

        [SerializeField] float m_delayedActivation = 1f/15f;


        private IEnumerator Start()
        {
            var mask = m_layerMask;
            m_layerMask = 0;
            yield return new WaitForSeconds(m_delayedActivation);
            m_layerMask = mask;
        }

        private void OnValidate()
        {
            m_delayedActivation = Mathf.Max(0f, m_delayedActivation);
        }

        public void Log(GameObject go)
        {
            if (Debug.isDebugBuild)
                Debug.Log(go);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (LayerMatch(other))
            {
                OnEnter.Invoke(other.gameObject);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (LayerMatch(other))
            {
                OnExit.Invoke(other.gameObject);
            }
        }

        bool LayerMatch(Collider other)
        {
            return (m_layerMask == (m_layerMask | (1 << other.gameObject.layer)));
        }
    }

    [System.Serializable]
    public class GameObjectUnityEvent : UnityEvent<GameObject> { }

}
