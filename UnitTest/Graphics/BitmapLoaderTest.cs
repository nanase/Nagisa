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
    }
}
