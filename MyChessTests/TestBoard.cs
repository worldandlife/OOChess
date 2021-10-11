using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyChess.Core;

namespace MyChessTests
{
    [TestClass]
    public class TestBoard
    {
        [TestMethod]
        public void TestMethod2()
        {
            var board = new Board();

            var cell1 = board.isEmpty(7, 0);
            var cell2 = board.isEmpty(5, 0);

            Assert.AreEqual(false, cell1);
            Assert.AreEqual(true, cell2);
        }
    }
}
