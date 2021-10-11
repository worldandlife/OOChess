using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyChess.Core
{
    public abstract class Player
    {
        public bool isWhiteSide { get; set; }
        public bool isHumanPlayer { get; set; }

    }

    public class HumanPlayer : Player
    {
        public HumanPlayer(bool whiteSide)
        {
            isWhiteSide = whiteSide;
            isHumanPlayer = true;
        }
    }

    public class ComputerPlayer : Player
    {
        public ComputerPlayer(bool whiteSide)
        {
            isHumanPlayer = whiteSide;
            isHumanPlayer = false;
        }
    }
}
