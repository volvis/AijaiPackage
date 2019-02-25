using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace Aijai.Events
{
    [Serializable]
    public class UnityEventBool : UnityEvent<bool> { }

    [Serializable]
    public class UnityEventInt : UnityEvent<int> { }

    [Serializable]
    public class UnityEventFloat : UnityEvent<float> { }

    [Serializable]
    public class UnityEventVector2 : UnityEvent<Vector2> { }

    [Serializable]
    public class UnityEventVector3 : UnityEvent<Vector3> { }

    [Serializable]
    public class UnityEventString : UnityEvent<string> { }

    [Serializable]
    public class UnityEventComponent : UnityEvent<Component> { }

    [Serializable]
    public class UnityEventGameObject : UnityEvent<GameObject> { }
}