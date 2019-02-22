using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Aijai.Events
{
    public class RandomEvent : MonoBehaviour, IUpdateListener
    {
        public UnityEvent[] Events;
        int m_last = -1;

        public float MinDelay;
        public float MaxDelay;
        [SerializeField] bool ForceUniquePick;
        [SerializeField] bool InvokeOnEnable;

        float m_timer;

        void OnValidate()
        {
            MinDelay = Mathf.Max(0f, MinDelay);
            MaxDelay = Mathf.Max(MinDelay, MaxDelay);
        }

        void IUpdateListener.Update(float deltaTime)
        {
            m_timer -= deltaTime;
            if (m_timer <= 0f)
            {
                m_timer = Random.Range(MinDelay, MaxDelay);

                if (ForceUniquePick)
                {
                    int index = Random.Range(0, Events.Length);
                    if (index == m_last)
                    {
                        index = (index++) % Events.Length;
                    }
                    if (index == m_last)
                        return;
                    Events[index].Invoke();
                    m_last = index;
                }
                else
                {
                    Events[Random.Range(0, Events.Length)].Invoke();
                }
            }
        }

        void OnEnable()
        {
            if (InvokeOnEnable == false)
                m_timer = Random.Range(MinDelay, MaxDelay);

            if (Events.Length != 0)
                UpdateHandler.Register(this);
        }

        void OnDisable()
        {
            UpdateHandler.Unregister(this);
        }
    }

}
