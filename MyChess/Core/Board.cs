using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MyChess.Core.Figure;
using static MyChess.Core.Figure.Bishop;

namespace MyChess.Core
{
    public class Board
    {
        public List<Cell> Cells { get; set; } = new List<Cell>();
        public List<EnPassatPawn> EnPassatPawns { get; set; } = new List<EnPassatPawn>();

        public bool isCloned = false;
        public Board()
        {
            this.resetBoard();
        }
        public Board Clone()
        {
            List<Cell> newCells = new List<Cell>(Cells.Count);

            Cells.ForEach((item) =>
            {
                newCells.Add(new Cell(item.X, item.Y, item.Figure));
            });
            return new Board
            {
                Cells = newCells,
                isCloned = true
            };
        }
        public Cell getCell(int x, int y)
        {

            if (x < 0 || x > 7 || y < 0 || y > 7)
            {
                throw new Exception("Index out of bound");
            }
            
            return Cells.Find(cell => cell.X == x && cell.Y == y);
        }
        public Cell getKingCell(bool white)
        {
            return Cells.Find(cell => cell.Figure is King && cell.Figure.isWhite == white);
        }
        public bool isEmpty(int x, int y)
        {
            var cell = getCell(x, y);
            var figure = cell.Figure;
            if(figure==null)
            {
                return true;
            }
            return false;
        }

        public void resetBoard()
        {
            // initialize white figures
            Cells.Add(new Cell(7, 0, new Rook(true)));
            Cells.Add(new Cell(7, 1, new Knight(true)));
            Cells.Add(new Cell(7, 2, new Bishop(true)));
            Cells.Add(new Cell(7, 3, new Queen(true)));
            Cells.Add(new Cell(7, 4, new King(true)));
            Cells.Add(new Cell(7, 5, new Bishop(true)));
            Cells.Add(new Cell(7, 6, new Knight(true)));
            Cells.Add(new Cell(7, 7, new Rook(true)));

            for (int j = 0; j < 8; j++)
            {
                Cells.Add(new Cell(6, j, new Pawn(true)));
            }

            // initialize black figures
            Cells.Add(new Cell(0, 0, new Rook(false)));
            Cells.Add(new Cell(0, 1, new Knight(false)));
            Cells.Add(new Cell(0, 2, new Bishop(false)));
            Cells.Add(new Cell(0, 3, new Queen(false)));
            Cells.Add(new Cell(0, 4, new King(false)));
            Cells.Add(new Cell(0, 5, new Bishop(false)));
            Cells.Add(new Cell(0, 6, new Knight(false)));
            Cells.Add(new Cell(0, 7, new Rook(false)));

            for (int j = 0; j < 8; j++)
            {
                Cells.Add(new Cell(1, j, new Pawn(false)));
            }

            // initialize remaining cells without any figure
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Cells.Add(new Cell(i, j, null));
                }
            }
            //getCell(7, 4).Figure = new King(true);
            //getCell(0, 4).Figure = new King(false);
            //getCell(0, 0).Figure = new Rook(false);
            //getCell(7, 7).Figure = new Rook(true);
            //getCell(7, 0).Figure = new Rook(true);
            //getCell(0, 7).Figure = new Rook(false);
            //getCell(0, 3).Figure = new Queen(false);

        }
    }
}
