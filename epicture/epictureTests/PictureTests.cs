using Microsoft.VisualStudio.TestTools.UnitTesting;
using epicture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlickrNet;

namespace epicture.Tests
{
    [TestClass()]
    public class PictureTests
    {
        [TestMethod()]
        public void PictureTest()
        {
            try
            {
                Picture picture = new Picture(new Photo());
            }
            catch
            {
                Assert.Fail();
            }
        }
    }
}