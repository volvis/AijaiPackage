using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Aijai.DevTools
{
    public static class DevInputListener
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void Initialize()
        {
            if (Debug.isDebugBuild == false)
                return;
            
            UpdateHandler.OnUpdate += UpdateHandler_OnUpdate;
        }

        public static event System.Action<string> OnDevInput;

        internal static StringBuilder stringBuilder;
        static bool listen;
        static DevInputGUI gui;

        private static void UpdateHandler_OnUpdate(float obj)
        {
            if (listen == false)
            {
                if (Input.GetKeyUp(KeyCode.Tab))
                {
                    stringBuilder = new StringBuilder();
                    listen = true;
                    var go = new GameObject("DevInputGui"+Random.Range(int.MinValue, int.MaxValue));
                    go.hideFlags = HideFlags.HideInHierarchy;
                    gui = go.AddComponent<DevInputGUI>();
                }
            }
            else
            {
                if (Input.GetKeyUp(KeyCode.KeypadEnter) || Input.GetKeyUp(KeyCode.Return))
                {
                    listen = false;
                    string message = stringBuilder.ToString().Trim().ToLower();
                    
                    //Debug.Log(message);
                    int sceneIndex;
                    if (int.TryParse(message, out sceneIndex))
                    {
                        if (sceneIndex >= 0 && sceneIndex < SceneManager.sceneCountInBuildSettings)
                        {
                            SceneManager.LoadScene(sceneIndex);
                        }
                    }
                    else
                    {
                        if (OnDevInput != null)
                            OnDevInput.Invoke(message);
                    }
                }
                else if (Input.GetKeyUp(KeyCode.Escape))
                {
                    listen = false;
                }
                else if (Input.GetKeyDown(KeyCode.Backspace) && stringBuilder.Length > 0)
                {
                    stringBuilder.Length = stringBuilder.Length - 1;
                }
                else
                {
                    stringBuilder.Append(Input.inputString);

                    if (stringBuilder.Length > 64)
                    {
                        Debug.Log("Debug message too long");
                        listen = false;
                    }
                }

                if (listen == false)
                    GameObject.Destroy(gui.gameObject);
            }
        }

        class DevInputGUI : MonoBehaviour
        {
            private void OnGUI()
            {
                GUILayout.BeginArea(new Rect(8, 8, 512, 50), "Dev Input", GUI.skin.window);
                GUILayout.Label(DevInputListener.stringBuilder.ToString());
                GUILayout.EndArea();
            }
        }
    }

}
