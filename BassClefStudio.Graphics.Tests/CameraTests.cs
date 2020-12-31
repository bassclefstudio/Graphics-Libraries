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
            Assert.AreEqual(testPt, testCam.TransformPoint(testPt), "The ViewCamera.Identity camera failed to correctly transform the test point.");
        }

        [TestMethod]
        public void TestIdentityCenter()
        {
            ViewCamera testCam = ViewCamera.IdentityCenter;
            var testPt = new Vector2(100, -400);
            Assert.AreEqual(ViewCamera.CenterConstant * testPt, testCam.TransformPoint(testPt), "The ViewCamera.IdentityCenter camera failed to correctly transform the test point.");
        }

        [TestMethod]
        public void TestScale()
        {
            float scale = 2;
            ViewCamera testCam = new ViewCamera(scale, Vector2.Zero);
            var testPt = new Vector2(100, -400);
            Assert.AreEqual(scale * testPt, testCam.TransformPoint(testPt), "The 2x ViewCamera camera failed to correctly transform the test point.");
        }

        [TestMethod]
        public void TestTranslate()
        {
            Vector2 translate = new Vector2(-100);
            ViewCamera testCam = new ViewCamera(1, translate);
            var testPt = new Vector2(100, -400);
            Assert.AreEqual(testPt - translate, testCam.TransformPoint(testPt), "The translate ViewCamera camera failed to correctly transform the test point.");
        }

        [TestMethod]
        public void TestTransformOrder()
        {
            Vector2 translate = new Vector2(-100);
            float scale = 2;
            ViewCamera testCam = new ViewCamera(scale, translate);
            var testPt = new Vector2(100, -400);
            Assert.AreEqual((testPt - translate) * scale, testCam.TransformPoint(testPt), "The translate/2x ViewCamera camera failed to correctly transform the test point.");
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
            ViewCamera viewCam = new ViewCamera(2, new Vector2(1, 1));
            ViewCamera drawCam = new ViewCamera(2, new Vector2(1, 1));

            ICamera testCam = new ComplexCamera(drawCam, viewCam);
            var testPt = new Vector2(1, -1);

            Vector2 manualCombine = (((testPt - drawCam.Translation) * drawCam.GetScale()) - viewCam.Translation) * viewCam.GetScale();
            Assert.AreEqual(manualCombine, testCam.TransformPoint(testPt), "The combined ViewCamera camera failed to correctly transform the test point.");
        }

        [TestMethod]
        public void TestCombineCameraFlip()
        {
            ViewCamera viewCam = new ViewCamera(2, new Vector2(1, 1), true);
            ViewCamera drawCam = new ViewCamera(2, new Vector2(1, 1));

            ICamera testCam = new ComplexCamera(drawCam, viewCam);
            var testPt = new Vector2(1, -1);

            Vector2 manualCombine = (((testPt - drawCam.Translation) * drawCam.GetScale()) - viewCam.Translation) * viewCam.GetScale();
            Assert.AreEqual(manualCombine, testCam.TransformPoint(testPt), "The combined ViewCamera camera failed to correctly transform the centered test point.");
        }
    }
}
