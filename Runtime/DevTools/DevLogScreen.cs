using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Aijai.DevTools
{
    public static class DevLogScreen
    {
        static Dictionary<string, TimedLog> NamedLogs = new Dictionary<string, TimedLog>();
        static List<TimedLog> TimedLogs = new List<TimedLog>();
        static DevGUI Instance;

        

        public static void Log(string value, float duration = 1f)
        {
            if (!Debug.isDebugBuild)
                return;
            TimedLogs.Add(new TimedLog() { Value = value, ExpirationTime = Time.timeSinceLevelLoad + duration });
            LogChanged();
        }

        public static void Log(string key, string value, float duration = 5f)
        {
            if (!Debug.isDebugBuild)
                return;
            NamedLogs[key] = new TimedLog() { Value = value, ExpirationTime = Time.timeSinceLevelLoad + duration};
            LogChanged();
        }

        static void TrimLogs()
        {
            var dt = Time.timeSinceLevelLoad;
            TimedLogs = TimedLogs.Where(x => x.ExpirationTime > dt).ToList();
            NamedLogs = NamedLogs.Where(x => x.Value.ExpirationTime > dt).ToDictionary(t => t.Key, t => t.Value);
            LogChanged();
        }

        static void LogChanged()
        {
            bool HasLogs = (TimedLogs.Count != 0 || NamedLogs.Count != 0);
            if (Instance == null)
            {
                if (HasLogs)
                {
                    var go = new GameObject("[DevLogScreen]");
                    go.hideFlags = HideFlags.HideInHierarchy;
                    Instance = go.AddComponent<DevGUI>();
                }
            }
            else
            {
                if (!HasLogs)
                {
                    GameObject.Destroy(Instance.gameObject);
                    Instance = null;
                }
            }
        }

        struct TimedLog
        {
            internal string Value;
            internal float ExpirationTime;
        }

        class DevGUI : MonoBehaviour
        {
            private void OnGUI()
            {
                TrimLogs();
                
                GUILayout.BeginArea(new Rect(Screen.width - 256 - 8, 8, 256, Screen.height - 16), "Dev Log", GUI.skin.window);
                foreach (var t in NamedLogs)
                {
                    GUILayout.Label(string.Format("<b>{0}</b>: {1}", t.Key, t.Value.Value));
                }
                foreach (var t in TimedLogs)
                {
                    GUILayout.Label(string.Format("- {0}", t.Value));
                }
                GUILayout.EndArea();
            }
        }
    }
}
