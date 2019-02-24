using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

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

        static StringBuilder stringBuilder;
        static bool listen;

        private static void UpdateHandler_OnUpdate(float obj)
        {
            if (listen == false)
            {
                if (Input.GetKeyUp(KeyCode.Tab))
                {
                    stringBuilder = new StringBuilder();
                    listen = true;
                }
            }
            else
            {
                if (Input.GetKeyUp(KeyCode.KeypadEnter) || Input.GetKeyUp(KeyCode.Return))
                {
                    listen = false;
                    string message = stringBuilder.ToString().Trim().ToLower();
                    
                    Debug.Log(message);
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
                else
                {
                    stringBuilder.Append(Input.inputString);
                    if (stringBuilder.Length > 64)
                    {
                        Debug.Log("Debug message too long");
                        listen = false;
                    }
                }
            }
        }
    }

}
