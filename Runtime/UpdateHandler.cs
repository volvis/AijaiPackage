using UnityEngine;
using UnityEngine.Experimental.LowLevel;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Aijai
{
    public static class UpdateHandler
    {
        static HashSet<IUpdateListener> m_listeners = new HashSet<IUpdateListener>();
        static IUpdateListener[] m_listenersArray = new IUpdateListener[0];
        static bool m_dirty;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Start()
        {
            var custloop = new PlayerLoopSystem()
            {
                type = typeof(UpdateHandler),
                updateDelegate = CustomUpdate
            };

            var defaultPlayerLoop = PlayerLoop.GetDefaultPlayerLoop();
            var updateSystemList = defaultPlayerLoop.subSystemList[4];  // Four is the default Update loop
            var update = updateSystemList.subSystemList;

            // Possibly faster than creating a List from array, appending said list and creating an array from List?
            Array.Resize(ref update, update.Length + 1);
            update[update.Length - 1] = custloop;

            updateSystemList.subSystemList = update;
            defaultPlayerLoop.subSystemList[4] = updateSystemList;

            PlayerLoop.SetPlayerLoop(defaultPlayerLoop);
        }

        public static event Action<float> OnUpdate;

        public static void Register(IUpdateListener listener)
        {
            m_dirty = m_listeners.Add(listener);
        }

        public static void Unregister(IUpdateListener listener)
        {
            m_dirty = m_listeners.Remove(listener);
        }

        static void CustomUpdate()
        {
            if (m_dirty)
            {
                m_dirty = false;
                m_listenersArray = m_listeners.ToArray();
            }

            float dt = Time.deltaTime;
            for (var i = 0; i < m_listenersArray.Length; i++)
            {
                m_listenersArray[i].Update(dt);
            }

            if (OnUpdate != null)
                OnUpdate.Invoke(dt);
        }
    }

    public interface IUpdateListener
    {
        void Update(float deltaTime);
    }

}
