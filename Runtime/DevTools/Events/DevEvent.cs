using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Aijai.DevTools
{

    /**
     * todo: DevEvents for bool, int and float
     * */
    public class DevEvent : NamedDevEventBase
    {
        [SerializeField] UnityEvent Action;

        override protected void OnDevInput(string[] obj)
        {
            Action.Invoke();
        }
    }

    public abstract class DevEventBase : MonoBehaviour
    {
        private void OnEnable()
        {
            if (Debug.isDebugBuild)
                DevInputListener.OnDevInput += OnDevInputRaw;
        }

        private void OnDisable()
        {
            if (Debug.isDebugBuild)
                DevInputListener.OnDevInput -= OnDevInputRaw;
        }

        public abstract void OnDevInputRaw(string value);
    }

    public abstract class NamedDevEventBase : DevEventBase
    {
        [SerializeField] protected string Command;

        

        private void DevInputListener_OnDevInput(string obj)
        {

            var split = obj.Split(new char[] { ' ' }, 2);
            if (split[0] == Command)
            {
                if (split.Length == 1 || split[1] == gameObject.name.ToLower())
                {
                    OnDevInput(split);
                }
            }
            
        }

        

        protected abstract void OnDevInput(string[] obj);
    }

}
