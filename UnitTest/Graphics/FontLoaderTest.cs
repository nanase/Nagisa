using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nagisa.Graphics;

namespace UnitTest.Graphics
{
    [TestClass]
    public class FontLoaderTest
    {
        [TestMethod]
        public void ConstructorTest1()
        {
            using (FontLoader font = new FontLoader("./resource/nanodigi.ttf"))
            {
                Assert.AreEqual(1, font.Families.Count);
                Assert.AreEqual("NanoDigi", font.Families[0].Name);
            }
        }

        [TestMethod]
        public void ConstructorTest2()
        {
            using (Stream stream = new FileStream("./resource/nanodigi.ttf", FileMode.Open, FileAccess.Read, FileShare.Read))
            using (FontLoader font = new FontLoader(stream, (int)stream.Length))
            {
                Assert.AreEqual(1, font.Families.Count);
                Assert.AreEqual("NanoDigi", font.Families[0].Name);
            }
        }
    }
}
