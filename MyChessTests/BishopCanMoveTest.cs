using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyChess.Core;
using System.Collections.Generic;
using static MyChess.Core.Figure;

namespace MyChessTests
{
    [TestClass]
    public class BishopCanMoveTest
    {
        [TestMethod]
        public void TestMethod4()
        {
            int expected = 8;
            int count = 0;
            var board = new Board();
            Cell startCell = board.getCell(7, 2);
            Cell endCell = board.getCell(2, 7);
            Figure sourceFigure = startCell.Figure;
            
            var res = sourceFigure.canMove(board, startCell, endCell);
                  



            Assert.AreEqual(true, res);
        }
    }
}