using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyChess.Core
{
    public class Cell
    {
        public Figure Figure { get; set; }
        public int X { get; set;}
        public int Y { get; set; }

        public Cell(int x, int y, Figure figure)
        {
            Figure = figure;
            X = x;
            Y = y;
        }
    }
}
