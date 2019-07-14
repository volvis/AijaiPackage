using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Aijai;

namespace Tests
{
    public class MedianUtilsTests
    {
        // A Test behaves as an ordinary method
        [Test]
        public void FloatMedianEmptyData()
        {
            var emptyArray = new float[] { };
            Assert.Throws<MedianUtils.EmptyData>(() => MedianUtils.Median((IEnumerable<float>)emptyArray));
        }

        [Test]
        public void FloatMedianSingleData()
        {
            var singleArray = new float[] { 15f };
            Assert.True( MedianUtils.Median((IEnumerable<float>)singleArray) == 15f);
        }

        [Test]
        public void FloatMedianAverage()
        {
            var doubleArray = new float[] { 4f, 8f };
            Assert.True(MedianUtils.Median((IEnumerable<float>)doubleArray) == 6f);
        }

        [Test]
        public void FloatTripleMedian()
        {
            Assert.True(MedianUtils.Median(new float[] { 1f, 2f, 3f }) == 2f);
            Assert.True(MedianUtils.Median(new float[] { 2f, 1f, 3f }) == 2f);
            Assert.True(MedianUtils.Median(new float[] { 3f, 1f, 2f }) == 2f);
            Assert.True(MedianUtils.Median(new float[] { 1f, 3f, 2f }) == 2f);
            Assert.True(MedianUtils.Median(new float[] { 2f, 3f, 1f }) == 2f);
            Assert.True(MedianUtils.Median(new float[] { 3f, 2f, 1f }) == 2f);
        }

        [Test]
        public void FloatSortMedian()
        {
            Assert.True(MedianUtils.Median(new float[] { 1f, 2f, 3f, 10f }) == 3f);
            Assert.True(MedianUtils.Median(new float[] { 2f, 1f, 3f, 40f }) == 3f);
            Assert.True(MedianUtils.Median(new float[] { 3f, 1f, 2f, 45f }) == 3f);
            Assert.True(MedianUtils.Median(new float[] { 1f, 3f, 2f, 43f }) == 3f);
            Assert.True(MedianUtils.Median(new float[] { 2f, 3f, 1f, 44f }) == 3f);
            Assert.True(MedianUtils.Median(new float[] { 3f, 2f, 1f, 54f }) == 3f);
        }

        [Test]
        public void Vector3Median()
        {
            var a = new Vector3(1f, 2f, 3f);
            var b = new Vector3(2f, 3f, 1f);
            var c = new Vector3(3f, 1f, 2f);
            Assert.True(MedianUtils.Median(new Vector3[] { a }) == new Vector3(1f, 2f, 3f));
            Assert.True(MedianUtils.Median(new Vector3[] { a, b }) == new Vector3(1.5f, 2.5f, 2f));
            Assert.True(MedianUtils.Median(new Vector3[] { a, b, c }) == new Vector3(2f, 2f, 2f));
        }

    }
}
