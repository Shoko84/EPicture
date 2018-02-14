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
    public class GraphicUtilsTests
    {
        [TestMethod()]
        public void LoadImageTest()
        {
            try
            {
                GraphicUtils graphUtils = new GraphicUtils();
            }
            catch
            {
                Assert.Fail();
            }
        }
    }
}