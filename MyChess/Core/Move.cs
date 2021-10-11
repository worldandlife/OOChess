using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyChess.Core
{
    public class Move
    {
        public Player Player { get; set; }
        public Cell Start { get; set; }
        public Cell End { get; set; }
        public Figure FigureMoved { get; set; }
        public Figure FigureKilled { get; set; }
        public bool isCastlingMove { get; set; } = false;

        public Move(Player player, Cell start, Cell end)
        {
            this.Player = player;
            this.Start = start;
            this.End = end;
            this.FigureMoved = start.Figure;
        }

    }
    
}
