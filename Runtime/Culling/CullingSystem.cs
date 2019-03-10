using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Linq;

namespace Aijai.Culling
{
    
    public class CullingSystem : MonoBehaviour
    {

        const int GrowthSize = 256;
        BoundingSphere[] boundingSpheres = new BoundingSphere[GrowthSize];
        ICullingSystemListener[] listeners = new ICullingSystemListener[GrowthSize];
        Dictionary<ICullingSystemListener, int> indices = new Dictionary<ICullingSystemListener, int>();
        
        int boundingSphereCount = 0;
        CullingGroup cullingGroup;

        [SerializeField] CullingDistanceBands DistanceBands;
        float[] m_distances = new float[0];
        
        private void Awake()
        {
            cullingGroup = new CullingGroup();
            cullingGroup.SetBoundingSpheres(boundingSpheres);
            cullingGroup.SetBoundingSphereCount(boundingSphereCount);

            if (DistanceBands != null)
            {
                m_distances = DistanceBands.GetDistances();
                cullingGroup.SetBoundingDistances(m_distances.OrderBy(x => x).ToArray());
            }

            cullingGroup.onStateChanged = OnStateChanged;
            cullingGroup.targetCamera = Camera.main;
            cullingGroup.SetDistanceReferencePoint(cullingGroup.targetCamera.transform);
            SceneManager.sceneLoaded += SceneLoaded;
        }

        private void SceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            cullingGroup.targetCamera = Camera.main;
            cullingGroup.SetDistanceReferencePoint(cullingGroup.targetCamera.transform);
        }

        private void OnDestroy()
        {
            cullingGroup.targetCamera = null;
            cullingGroup.onStateChanged -= OnStateChanged;
            cullingGroup.Dispose();
            cullingGroup = null;
            SceneManager.sceneLoaded -= SceneLoaded;
        }

        public void ReserveSphere(ICullingSystemListener listener, Vector3 position, float radius)
        {
            Debug.Assert(cullingGroup != null);
            if (cullingGroup == null)
                return;
            if (boundingSphereCount == boundingSpheres.Length)
            {
                int newSize = boundingSphereCount + GrowthSize;
                System.Array.Resize(ref boundingSpheres, newSize);
                System.Array.Resize(ref listeners, newSize);
                cullingGroup.SetBoundingSpheres(boundingSpheres);
            }
            indices.Add(listener, boundingSphereCount);
            listeners[boundingSphereCount] = listener;
            var bs = boundingSpheres[boundingSphereCount];
            bs.position = position;
            bs.radius = radius;
            boundingSpheres[boundingSphereCount] = bs;
            boundingSphereCount++;
            cullingGroup.SetBoundingSphereCount(boundingSphereCount);
        }

        public void SetSphere(ICullingSystemListener listener, Vector3 position)
        {
            Debug.Assert(cullingGroup != null);
            if (cullingGroup == null)
                return;
            var sphereIndex = indices[listener];
            var bs = boundingSpheres[sphereIndex];
            bs.position = position;
            boundingSpheres[sphereIndex] = bs;
        }

        public void SetSphere(ICullingSystemListener listener, Vector3 position, float radius)
        {
            if (cullingGroup == null)
                return;
            var sphereIndex = indices[listener];
            var bs = boundingSpheres[sphereIndex];
            bs.position = position;
            bs.radius = radius;
            boundingSpheres[sphereIndex] = bs;
        }

        public void ReleaseSphere(ICullingSystemListener listener)
        {
            if (cullingGroup == null)
                return;
            var sphereIndex = indices[listener];
            cullingGroup.EraseSwapBack(sphereIndex);
            indices[listeners[boundingSphereCount - 1]] = sphereIndex;
            CullingGroup.EraseSwapBack(sphereIndex, listeners, ref boundingSphereCount);
        }

        void OnStateChanged(CullingGroupEvent sphere)
        {
            var listener = listeners[sphere.index];
            listener.OnVisibilityChange(sphere);
        }

        const float RadToVol = (4f / 3f) * Mathf.PI;
        const float OneThird = 1f / 3f;
        const float FourPI = 4f * Mathf.PI;

        private void OnDrawGizmosSelected()
        {
            for (var i = 0; i < boundingSphereCount; i++)
            {
                var sphere = boundingSpheres[i];
                Gizmos.color = cullingGroup.IsVisible(i) ? new Color(0.5f,1f,1f, 0.75f) : new Color(1f, 1f, 1f, 0.25f);
                Gizmos.DrawSphere(sphere.position, sphere.radius);
                float baseVolume = RadToVol * (sphere.radius * sphere.radius * sphere.radius);

                int distanceIndex = cullingGroup.GetDistance(i);

                for (var a = 0; a < m_distances.Length; a++)
                {
                    float dist = m_distances[a];
                    float nextVolume = baseVolume + (RadToVol * (dist * dist * dist));
                    float nextRadius = Mathf.Pow(3f * (nextVolume / FourPI), OneThird);

                    
                    Gizmos.color = a == distanceIndex ? new Color(1f, 0.5f, 1f, 1f) : new Color(0f, 0.5f, 0f, 0.25f);
                    Gizmos.DrawWireSphere(sphere.position, nextRadius);
                }
            }
        }
    }
}
