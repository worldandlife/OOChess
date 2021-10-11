using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyChess.Core;

namespace MyChessTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var p1 = new HumanPlayer(true);
            var p2 = new HumanPlayer(false);
            var game = new Game(p1,p2);

            var res = game.playerMove(p1, 7, 1, 5, 0);
                
            Assert.AreEqual(true, res);
        }
    }
}
