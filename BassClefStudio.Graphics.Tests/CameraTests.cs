using BassClefStudio.Graphics.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BassClefStudio.Graphics.Tests
{
    [TestClass]
    public class CameraTests
    {
        [TestMethod]
        public void TestIdentity()
        {
            ViewCamera testCam = ViewCamera.Identity;
            var testPt = new Vector2(100, -400);
            Assert.AreEqual(testPt, testCam.GetViewPoint(testPt), "The ViewCamera.Identity camera failed to correctly transform the test point.");
        }

        [TestMethod]
        public void TestIdentityCenter()
        {
            ViewCamera testCam = ViewCamera.IdentityFlip;
            var testPt = new Vector2(100, -400);
            Assert.AreEqual(Scaling.FlipConstant * testPt, testCam.GetViewPoint(testPt), "The ViewCamera.IdentityCenter camera failed to correctly transform the test point.");
        }

        [TestMethod]
        public void TestScale()
        {
            float scale = 2;
            ViewCamera testCam = new ViewCamera(scale, Vector2.Zero);
            var testPt = new Vector2(100, -400);
            Assert.AreEqual(scale * testPt, testCam.GetViewPoint(testPt), "The 2x ViewCamera camera failed to correctly transform the test point.");
        }

        [TestMethod]
        public void TestTranslate()
        {
            Vector2 translate = new Vector2(-100);
            ViewCamera testCam = new ViewCamera(1, translate);
            var testPt = new Vector2(100, -400);
            Assert.AreEqual(testPt - translate, testCam.GetViewPoint(testPt), "The translate ViewCamera camera failed to correctly transform the test point.");
        }

        [TestMethod]
        public void TestTransformOrder()
        {
            Vector2 translate = new Vector2(-100);
            float scale = 2;
            ViewCamera testCam = new ViewCamera(scale, translate);
            var testPt = new Vector2(100, -400);
            Assert.AreEqual((testPt - translate) * scale, testCam.GetViewPoint(testPt), "The translate/2x ViewCamera camera failed to correctly transform the test point.");
        }

        [TestMethod]
        public void TestViewFit()
        {
            Vector2 viewSize = new Vector2(100);
            Vector2 drawSize = new Vector2(200);
            ViewCamera testCam = new ViewCamera(viewSize, drawSize, ZoomType.FitAll);
            Assert.AreEqual(0.5, testCam.Scale, "FitAll ViewCamera has incorrect scale to fit draw-space 2x view-space.");
            Vector2 newDrawSize = new Vector2(50, 200);
            testCam = new ViewCamera(viewSize, newDrawSize, ZoomType.FitAll);
            Assert.AreEqual(0.5, testCam.Scale, "FitAll ViewCamera has incorrect scale to fit rectangular draw-space.");
        }

        [TestMethod]
        public void TestViewFill()
        {
            Vector2 viewSize = new Vector2(100);
            Vector2 drawSize = new Vector2(200);
            ViewCamera testCam = new ViewCamera(viewSize, drawSize, ZoomType.FillView);
            Assert.AreEqual(0.5, testCam.Scale, "FillView ViewCamera has incorrect scale to fill draw-space 2x view-space.");
            Vector2 newDrawSize = new Vector2(50, 200);
            testCam = new ViewCamera(viewSize, newDrawSize, ZoomType.FillView);
            Assert.AreEqual(2, testCam.Scale, "FillView ViewCamera has incorrect scale to fill with rectangular draw-space.");
        }

        [TestMethod]
        public void TestCombineCamera()
        {
            float[] scaling = new float[] { 2, 2 };
            Vector2[] translation = new Vector2[] { new Vector2(1, 1), new Vector2(1, 1) };
            ViewCamera viewCam = new ViewCamera(2, new Vector2(1, 1));
            ViewCamera drawCam = new ViewCamera(2, new Vector2(1, 1));

            ViewCamera testCam = drawCam + viewCam;
            var testPt = new Vector2(1, -1);

            Vector2 manualCombine = (((testPt - translation[0]) * scaling[0]) - translation[1]) * scaling[1];
            Assert.AreEqual(manualCombine, testCam.GetViewPoint(testPt), "The combined ViewCamera camera failed to correctly transform the test point.");
        }

        [TestMethod]
        public void TestCombineCameraFlip()
        {
            float[] scaling = new float[] { 2, 2 };
            Vector2[] translation = new Vector2[] { new Vector2(1, 1), new Vector2(1, 1) };
            ViewCamera viewCam = new ViewCamera(2, new Vector2(1, 1));
            ViewCamera drawCam = new ViewCamera(2, new Vector2(1, 1));

            ViewCamera testCam = drawCam + ViewCamera.IdentityFlip + viewCam;
            var testPt = new Vector2(1, -1);

            Vector2 manualCombine = (((testPt - translation[0]) * scaling[0] * Scaling.FlipConstant) - translation[1]) * scaling[1];
            Assert.AreEqual(manualCombine, testCam.GetViewPoint(testPt), "The combined ViewCamera camera failed to correctly transform the centered test point.");
        }
    }
}
