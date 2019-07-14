using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aijai
{
    public static class MedianUtils
    {
        static List<float> set = new List<float>();

        public static Vector3 Median( IEnumerable<Vector3> data)
        {
            Vector3 m = Vector3.zero;
            var en = data.GetEnumerator();

            set.Clear();
            while(en.MoveNext())
            {
                set.Add(en.Current.x);
            }
            m.x = GetMedian(set);
            en.Reset();

            set.Clear();
            while (en.MoveNext())
            {
                set.Add(en.Current.y);
            }
            m.y = GetMedian(set);
            en.Reset();

            set.Clear();
            while (en.MoveNext())
            {
                set.Add(en.Current.z);
            }
            m.z = GetMedian(set);
            en.Reset();

            return m;
        }
        

        public static float Median(IEnumerable<float> data)
        {
            set.Clear();
            var enumerator = data.GetEnumerator();

            while (enumerator.MoveNext())
            {
                set.Add(enumerator.Current);
            }

            if (set.Count == 0)
                throw new EmptyData();

            return GetMedian(set);
        }


        static float GetMedian(List<float> set)
        {
            int count = set.Count;
            if (count == 1)
                return set[0];
            if (count == 2)
            {
                return (set[0] + set[1]) * 0.5f;
            }
            if (count == 3)
            {
                var minA = Mathf.Min(set[0], set[1]);
                var minB = Mathf.Min(set[1], set[2]);
                if (minB == minA)
                    return Mathf.Min(set[0], set[2]);
                return Mathf.Max(minA, minB);
            }

            set.Sort();

            return set[count / 2];
        }

        public class EmptyData : System.Exception { }
    }
}
