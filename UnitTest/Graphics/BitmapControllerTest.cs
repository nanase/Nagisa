/* Nagisa - 2D Game Library with OpenTK */

/* LICENSE - The MIT License (MIT)
 
Copyright (c) 2014 Tomona Nanase

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using System;
using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nagisa.Graphics;

namespace UnitTest.Graphics
{
    [TestClass]
    public class BitmapControllerTest
    {
        private Bitmap smallBitmap;
        private Bitmap writtenBitmap;

        [TestInitialize]
        public void Initialize()
        {
            this.smallBitmap = new Bitmap(32, 32);
            this.writtenBitmap = new Bitmap(32, 32);
        }

        [TestCleanup]
        public void CleanUp()
        {
            this.smallBitmap.Dispose();
            this.writtenBitmap.Dispose();
        }

        #region -- Indexer Get --
        [TestMethod]
        public void IndexerGetTest()
        {
            Color expected = Color.FromArgb(0);

            using (BitmapController controller = new BitmapController(this.smallBitmap, ImageLockMode.ReadOnly))
            {
                Assert.AreEqual(controller[0, 0], expected);
                Assert.AreEqual(controller[31, 0], expected);
                Assert.AreEqual(controller[0, 31], expected);
                Assert.AreEqual(controller[31, 31], expected);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void IndexerGetTestExeption1()
        {
            using (BitmapController controller = new BitmapController(this.smallBitmap, ImageLockMode.ReadOnly))
                Assert.AreEqual(controller[-1, 0], Color.Transparent);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void IndexerGetTestExeption2()
        {
            using (BitmapController controller = new BitmapController(this.smallBitmap, ImageLockMode.ReadOnly))
                Assert.AreEqual(controller[0, -1], Color.Transparent);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void IndexerGetTestExeption3()
        {
            using (BitmapController controller = new BitmapController(this.smallBitmap, ImageLockMode.ReadOnly))
                Assert.AreEqual(controller[32, 0], Color.Transparent);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void IndexerGetTestExeption4()
        {
            using (BitmapController controller = new BitmapController(this.smallBitmap, ImageLockMode.ReadOnly))
                Assert.AreEqual(controller[0, 32], Color.Transparent);
        }
        #endregion

        #region -- Indexer Set --
        [TestMethod]
        public void IndexerSetTest()
        {
            Color expected = Color.FromArgb(0x12, 0x34, 0x56, 0x78);

            using (BitmapController controller = new BitmapController(this.writtenBitmap, ImageLockMode.ReadWrite))
            {
                controller[0, 0] = expected;
                controller[31, 0] = expected;
                controller[0, 31] = expected;
                controller[31, 31] = expected;

                Assert.AreEqual(controller[0, 0], expected);
                Assert.AreEqual(controller[31, 0], expected);
                Assert.AreEqual(controller[0, 31], expected);
                Assert.AreEqual(controller[31, 31], expected);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void IndexerSetTestExeption1()
        {
            using (BitmapController controller = new BitmapController(this.smallBitmap, ImageLockMode.ReadWrite))
                controller[-1, 0] = Color.Transparent;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void IndexerSetTestExeption2()
        {
            using (BitmapController controller = new BitmapController(this.smallBitmap, ImageLockMode.ReadWrite))
                controller[0, -1] = Color.Transparent;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void IndexerSetTestExeption3()
        {
            using (BitmapController controller = new BitmapController(this.smallBitmap, ImageLockMode.ReadWrite))
                controller[32, 0] = Color.Transparent;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void IndexerSetTestExeption4()
        {
            using (BitmapController controller = new BitmapController(this.smallBitmap, ImageLockMode.ReadWrite))
                controller[0, 32] = Color.Transparent;
        }
        #endregion

        #region -- public BitmapData BitmapData --
        [TestMethod]
        public void BitmapDataTest()
        {
            using (BitmapController controller = new BitmapController(this.smallBitmap, ImageLockMode.ReadOnly))
            {
                Assert.IsNotNull(controller.BitmapData);
                Assert.AreEqual(this.smallBitmap.Width, controller.BitmapData.Width);
                Assert.AreEqual(this.smallBitmap.Height, controller.BitmapData.Height);
                Assert.AreEqual(this.smallBitmap.PixelFormat, controller.BitmapData.PixelFormat);
            }
        }
        #endregion

        #region -- public Bitmap BaseBitmap --
        [TestMethod]
        public void BaseBitmapTest()
        {
            using (BitmapController controller = new BitmapController(this.smallBitmap, ImageLockMode.ReadOnly))
            {
                Assert.AreEqual(this.smallBitmap, controller.BaseBitmap);
            }
        }
        #endregion

        #region -- public bool IsDisposed --
        [TestMethod]
        public void IsDisposedTest()
        {
            BitmapController controller = null;
            try
            {
                controller = new BitmapController(this.smallBitmap, ImageLockMode.ReadOnly);
                Assert.IsFalse(controller.IsDisposed);
            }
            finally
            {
                if (controller != null)
                {
                    controller.Dispose();
                    Assert.IsTrue(controller.IsDisposed);
                    controller = null;
                }
            }
        }
        #endregion

        #region -- Constructor --
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorTest()
        {
            using (var controller = new BitmapController(null, ImageLockMode.ReadOnly)) { }
        }
        #endregion
    }
}
