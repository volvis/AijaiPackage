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
        static HashSet<string> CriticalErrors = new HashSet<string>();

        static HashSet<GizmoInstance> GizmoInstances = new HashSet<GizmoInstance>();

        static DevGUI Instance;

        static Color DefaultColor = new Color(0.149f, 0.012f, 0.224f, 0.6f);



        /// <summary>
        /// Display temporary data on screen
        /// </summary>
        public static void LogNotice(string value, float duration = 1f)
        {
            if (!Debug.isDebugBuild)
                return;
            TimedLogs.Add(new TimedLog() { Value = value, ExpirationTime = Time.timeSinceLevelLoad + duration });
            LogChanged();
        }

        /// <summary>
        /// Display temporary data bound to a key on screen.
        /// </summary>
        public static void LogValue(string key, object value, float duration = 10f)
        {
            if (!Debug.isDebugBuild)
                return;
            NamedLogs[key] = new TimedLog() { Value = value.ToString(), ExpirationTime = Time.timeSinceLevelLoad + duration};
            LogChanged();
        }

        /// <summary>
        /// Create a permanent error notice for the user
        /// </summary>
        /// <param name="notice"></param>
        public static void UserError(string notice)
        {
            if(CriticalErrors.Add(notice))
            {
                LogChanged();
            }
        }

        public static void DrawCircle(Vector3 position, float radius = 0.5f, float duration = 0.3f)
        {
            DrawCircle(position, radius, DefaultColor, duration);
        }

        public static void DrawCircle(Vector3 position, float radius, Color color, float duration = 0.3f)
        {
            GizmoInstances.Add(new GizmoInstance()
            {
                A = position,
                B = Vector3.up * radius,
                ExpirationTime = Time.timeSinceLevelLoad + duration,
                Color = color,
                Type = GizmoInstanceType.Circle
            });
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
            bool HasLogs = (TimedLogs.Count != 0 || NamedLogs.Count != 0 || CriticalErrors.Count != 0 || GizmoInstances.Count != 0);
            if (Instance == null)
            {
                if (HasLogs)
                {
                    var go = new GameObject("[DevLogScreen]");
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

        enum GizmoInstanceType
        {
            Circle
        }

        struct GizmoInstance
        {
            internal GizmoInstanceType Type;
            internal float ExpirationTime;
            internal Vector3 A;
            internal Vector3 B;
            internal Color Color;
        }

        class DevGUI : MonoBehaviour
        {
            GUIStyle m_style;
            Texture2D m_bg;

            private void Awake()
            {
                
                m_bg = new Texture2D(1, 1);
                m_bg.SetPixel(0, 0, DefaultColor);
                m_bg.Apply();

                m_style = new GUIStyle();
                m_style.normal.background = m_bg;
            }

            private void OnDrawGizmos()
            {
                var c = Gizmos.color;
                foreach (var g in GizmoInstances)
                {
                    Gizmos.color = g.Color;
                    switch (g.Type)
                    {
                        case GizmoInstanceType.Circle:
                            Gizmos.DrawWireSphere(g.A, g.B.y);
                            break;
                    }
                }
                Gizmos.color = c;

                GizmoInstances.RemoveWhere(x => x.ExpirationTime < Time.timeSinceLevelLoad);
            }

            private void OnGUI()
            {
                TrimLogs();

                GUILayout.BeginVertical(m_style, GUILayout.MinWidth(256f));

                foreach (var t in CriticalErrors)
                {
                    GUILayout.Label(string.Format("<b>[ERROR]</b> {0}", t));
                }

                foreach (var t in NamedLogs)
                {
                    GUILayout.Label(string.Format("<b>{0}</b>: {1}", t.Key, t.Value.Value));
                }

                foreach (var t in TimedLogs)
                {
                    GUILayout.Label(string.Format("- {0}", t.Value));
                }

                GUILayout.EndVertical();
            }
        }
    }
}
