using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyChess.Core;
using System;
using System.Collections.Generic;
using static MyChess.Core.Figure;

namespace MyChessTests
{
    [TestClass]
    public class FigurePathTest
    {
        [TestMethod]
        
        public void TestMethod5()
        {
            int stepDirectionX = Math.Sign(2 - 7);
            int stepDirectionY = Math.Sign(7 - 2);

            Assert.AreEqual(-1, stepDirectionX);
            Assert.AreEqual(1, stepDirectionY);
        }
    }
}