using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aijai.Culling
{
    [CreateAssetMenu(fileName = "CullingDistanceBands", menuName = "Aijai/Culling/Culling Distance Bands")]
    public class CullingDistanceBands : ScriptableObject
    {
        [SerializeField] float[] m_distances = new float[] { 0f, 5f, 20f, 40f };

        public float[] GetDistances()
        {
            return (float[])m_distances.Clone();
        }
    }
}
