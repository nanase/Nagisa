using System;
using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nagisa.Graphics;

namespace UnitTest.Graphics
{
    [TestClass]
    public class BitmapLoaderTest
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

        [TestMethod]
        public void BaseBitmapTest()
        {
            using (BitmapLoader loader = new BitmapLoader(this.smallBitmap))
            {
                Assert.IsNotNull(loader.BaseBitmap);
                Assert.AreEqual(this.smallBitmap, loader.BaseBitmap);
            }
        }

        [TestMethod]
        public void GraphicsTest()
        {
            using (BitmapLoader loader = new BitmapLoader(this.smallBitmap))
            {
                Assert.IsNotNull(loader.Graphics);
            }
        }

        [TestMethod]
        public void Scan0Test()
        {
            using (BitmapLoader loader = new BitmapLoader(this.smallBitmap))
            {
                Assert.AreNotEqual(IntPtr.Zero, loader.Scan0);
            }
        }
    }
}
