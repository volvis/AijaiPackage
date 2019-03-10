using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aijai.Culling
{
    public interface ICullingSystemListener
    {
        void OnVisibilityChange(CullingGroupEvent visible);
    }
}
