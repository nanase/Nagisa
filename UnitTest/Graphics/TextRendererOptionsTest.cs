using System;
using System.Drawing;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nagisa.Graphics;

namespace UnitTest.Graphics
{
    [TestClass]
    public class TextRendererOptionsTest
    {
        private const string NanoDigiPath = "./resource/nanodigi.ttf";

        [TestMethod]
        public void LineHeightGetTest()
        {
            const int LineHeight = 10;

            using (FontLoader fontLoader = new FontLoader(NanoDigiPath))
            using (Font font = new Font(fontLoader.Families[0], 10.0f))
            using (TextRendererOptions options = new TextRendererOptions(font, LineHeight))
            {
                Assert.AreEqual(LineHeight, (int)options.LineHeight);
            }
        }

        [TestMethod]
        public void LineHeightSetTest()
        {
            const int LineHeight = 10;
            const int LineHeightModified = 12;

            using (FontLoader fontLoader = new FontLoader(NanoDigiPath))
            using (Font font = new Font(fontLoader.Families[0], 10.0f))
            using (TextRendererOptions options = new TextRendererOptions(font, LineHeight))
            {
                options.LineHeight = LineHeightModified;
                Assert.AreEqual(LineHeightModified, (int)options.LineHeight);
            }
        }
    }
}
