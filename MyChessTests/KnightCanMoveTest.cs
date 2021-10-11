using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyChess.Core;
using System.Collections.Generic;
using static MyChess.Core.Figure;

namespace MyChessTests
{
    [TestClass]
    public class KnightCanMoveTest
    {
        [TestMethod]
        public void TestMethod3()
        {
            int expected = 6;
            int count = 0;
            var board = new Board();
            Cell startCell = board.getCell(4, 3);
            startCell.Figure = new Knight(true);
            Figure sourceFigure = startCell.Figure;
            for (int i = 0; i < 8; i++)
            for (int j = 0; j < 8; j++)
                {
                    var endCell = board.getCell(i, j);
                    var res = sourceFigure.canMove(board, startCell, endCell);
                    if(res)
                    {
                        count++;
                    }
                }

            
            
            
            Assert.AreEqual(expected, count);
        }
    }
}