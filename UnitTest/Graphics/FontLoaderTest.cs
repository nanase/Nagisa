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
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorExceptionTest1()
        {
            using (FontLoader font = new FontLoader(null)) { }

            Assert.Fail();
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

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorException1Test2()
        {
            using (Stream stream = new FileStream("./resource/nanodigi.ttf", FileMode.Open, FileAccess.Read, FileShare.Read))
            using (FontLoader font = new FontLoader(null, (int)stream.Length)) { }

            Assert.Fail();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ConstructorException2Test2()
        {
            using (Stream stream = new FileStream("./resource/nanodigi.ttf", FileMode.Open, FileAccess.Read, FileShare.Read))
            using (FontLoader font = new FontLoader(stream, -1)) { }

            Assert.Fail();
        }

        [TestMethod]
        public void IsDisposedTest()
        {
            FontLoader loader = null;
            try
            {
                loader = new FontLoader("./resource/nanodigi.ttf");
                Assert.IsFalse(loader.IsDisposed);
            }
            finally
            {
                if (loader != null)
                {
                    loader.Dispose();
                    Assert.IsTrue(loader.IsDisposed);
                    loader = null;
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void FamiliesExceptionTest()
        {
            FontLoader loader = null;
            try
            {
                loader = new FontLoader("./resource/nanodigi.ttf");
            }
            finally
            {
                if (loader != null)
                {
                    loader.Dispose();

                    // Fire!
                    var families = loader.Families;

                    loader = null;
                }
                else
                {
                    Assert.Fail();
                }
            }
        }
    }
}
