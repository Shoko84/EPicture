using Microsoft.VisualStudio.TestTools.UnitTesting;
using epicture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epicture.Tests
{
    [TestClass()]
    public class PictureViewerTests
    {
        PictureViewer pictureViewer;

        [TestMethod()]
        public void PictureViewerTest()
        {
            try
            {
                pictureViewer = new PictureViewer();
            }
            catch
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void SetCurrentPageTest()
        {
            //Assert.Fail();
        }

        [TestMethod()]
        public void SetPicturesTest()
        {
            //Assert.Fail();
        }

        [TestMethod()]
        public void SetPicturesTest1()
        {
            //Assert.Fail();
        }
    }
}