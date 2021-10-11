using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MyChess.Core.Figure;

namespace MyChess.Core
{
   
    public abstract class Figure
    {
        public bool isKilled { get; set; } = false;
        public bool isWhite { get; set; } = false;

        public Figure(bool white)
        {
            isWhite = white;
        }

        public abstract bool canMove(Board board, Cell start, Cell end);

        private bool isKingBeingAttacked(Board board, Cell start, Cell end)
        {
            var newBoard = board.Clone();

            newBoard.getCell(end.X, end.Y).Figure = newBoard.getCell(start.X, start.Y).Figure;
            newBoard.getCell(start.X, start.Y).Figure = null;

            Cell kingCell = null;
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    var cell = newBoard.getCell(i, j);
                    var figure = cell.Figure;
                    if(figure!=null && figure.isWhite==isWhite && figure is King)
                    {
                        var king = (King)figure;
                        kingCell = cell;
                    }
                }

                    for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    var figure = newBoard.getCell(i, j).Figure;
                    if (figure != null && figure.isWhite!= isWhite && figure.canMove(newBoard, newBoard.getCell(i, j), kingCell))
                    {
                        return true;
                    }
                }
            return false;
        }
        
        public List<Cell> getAllowedCells(Board board, Cell start)
        {
            List<Cell> cells = new List<Cell>();
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    var end = board.getCell(i, j);
                    var res = canMove(board, start, end);
                    if (res)
                    {
                        cells.Add(end);
                    }
                }
            return cells;
        }
        public class King : Figure
        {
            public bool isAttacked { get; set; } = false;
            public bool isCastlingDone { get; set; } = false;

            public King(bool white) : base(white)
            {
            }
            public override bool canMove(Board board, Cell start, Cell end)
            {
                // we can't move the figure to a Cell that
                // has a figure of the same color
                if (end.Figure != null && end.Figure.isWhite == isWhite)
                {
                    return false;
                }

                int x = Math.Abs(start.X - end.X);
                int y = Math.Abs(start.Y - end.Y);
                if (y < 2 && (x + y == 1 || x == y))
                {
                    // check if this move will not result in the king
                    // being attacked if so return true
                    if (!board.isCloned && isKingBeingAttacked(board, start, end)) return false;

                    return true;
                }
                //return this.isValidCastling(board, start, end);
                return false;
            }

            private bool isValidCastling(Board board, Cell start, Cell end)
            {

                if (isCastlingDone)
                {
                    return false;
                }
                
                int x = Math.Abs(start.X - end.X);
                int y = Math.Abs(start.Y - end.Y);
                if (x==0 && y==2 && !isCastlingDone)
                {
                    // check if this move will not result in the king
                    // being attacked if so return true
                    if (!board.isCloned && isKingBeingAttacked(board, start, end)) return false;
                    return true;
                }
                return false;
            }

            public bool isCastlingMove(Cell start, Cell end)
            {
                // check if the starting and
                // ending position are correct
                return Math.Abs(start.Y - end.Y) == 2;
            }

            public bool IsAttacked(Board board, Cell kingCell)
            {
      
                for (int i = 0; i < 8; i++)
                    for (int j = 0; j < 8; j++)
                    {
                        var figure = board.getCell(i, j).Figure;
                        if (figure != null && figure.isWhite != isWhite && figure.canMove(board, board.getCell(i, j), kingCell))
                        {
                            return true;
                        }
                    }
                return false;
            }
        }
        public class Queen : Figure
        {
            public Queen(bool white) : base(white)
            {
            }
            public override bool canMove(Board board, Cell start, Cell end)
            {
                // we can't move the figure to a Cell that
                // has a figure of the same color
                //if (end.Figure != null && end.Figure.isWhite == isWhite)
                //{
                //    return false;
                //}
                int x = Math.Abs(start.X - end.X);
                int y = Math.Abs(start.Y - end.Y);
                if (x == y || x * y == 0)
                {
                    var pathLength = x == y ? x : x + y;

                    int stepDirectionX = Math.Sign(end.X - start.X);
                    int stepDirectionY = Math.Sign(end.Y - start.Y);
                    x = start.X; y = start.Y;
                    for (int i = 1; i < pathLength; i++)
                    {
                        x += stepDirectionX;
                        y += stepDirectionY;

                        if (board.isEmpty(x, y)) continue;
                        else return false;

                    }
                    
                    if (board.isEmpty(end.X, end.Y)) {
                        if (!board.isCloned && isKingBeingAttacked(board, start, end)) return false;
                        return true;
                    } 

                    if(end.Figure.isWhite != isWhite)
                    {
                        if (!board.isCloned && isKingBeingAttacked(board, start, end)) return false;
                        return true;
                    }

                }
                return false;
            }

        }
        public class Bishop : Figure
        {
            public Bishop(bool white) : base(white)
            {
            }

            public override bool canMove(Board board, Cell start, Cell end)
            {
                //if (end.Figure != null && end.Figure.isWhite == isWhite)
                //{
                //    return false;
                //}
                int x = Math.Abs(start.X - end.X);
                int y = Math.Abs(start.Y - end.Y);
                if (x == y)
                {
                    int pathLength = x;
                    int stepDirectionX = Math.Sign(end.X - start.X);
                    int stepDirectionY = Math.Sign(end.Y - start.Y);
                    x = start.X; y = start.Y;
                    for (int i = 1; i < pathLength; i++)
                    {
                        x += stepDirectionX;
                        y += stepDirectionY;

                        if (board.isEmpty(x, y)) continue;
                        else return false;

                    }
                    if (board.isEmpty(end.X, end.Y))
                    {
                        if (!board.isCloned && isKingBeingAttacked(board, start, end)) return false;
                        return true;
                    }

                    if (end.Figure.isWhite != isWhite)
                    {
                        if (!board.isCloned && isKingBeingAttacked(board, start, end)) return false;
                        return true;
                    }

                }
                return false;
            }
        }
        public class Knight : Figure
        {

            public Knight(bool white) : base(white)
            {
            }

            public override bool canMove(Board board, Cell start, Cell end)
            {
                // we can't move the figure to a Cell that
                // has a figure of the same color
                if (end.Figure != null && end.Figure.isWhite == isWhite)
                {
                    return false;
                }

                int x = Math.Abs(start.X - end.X);
                int y = Math.Abs(start.Y - end.Y);
                if (x * y == 2)
                {              
                    if (!board.isCloned && isKingBeingAttacked(board, start, end)) return false;
                    return true;
                }
                return false;
            }
        }
        public class Rook : Figure
        {
            public Rook(bool white) : base(white)
            {
            }
            public override bool canMove(Board board, Cell start, Cell end)
            {
                // we can't move the figure to a Cell that
                // has a figure of the same color
                //if (end.Figure != null && end.Figure.isWhite == isWhite)
                //{
                //    return false;
                //}
                int x = Math.Abs(start.X - end.X);
                int y = Math.Abs(start.Y - end.Y);
                if (x * y == 0)
                {
                    int pathLength = x + y;
                    int stepDirectionX = Math.Sign(end.X - start.X);
                    int stepDirectionY = Math.Sign(end.Y - start.Y);
                    x = start.X; y = start.Y;
                    for (int i = 1; i < pathLength; i++)
                    {
                        x += stepDirectionX;
                        y += stepDirectionY;

                        if (board.isEmpty(x, y)) continue;
                        else return false;

                    }
                    if (board.isEmpty(end.X, end.Y))
                    {
                        if (!board.isCloned && isKingBeingAttacked(board, start, end)) return false;
                        return true;
                    }

                    if (end.Figure.isWhite != isWhite)
                    {
                        if (!board.isCloned && isKingBeingAttacked(board, start, end)) return false;
                        return true;
                    }

                }
                return false;

            }

        }
        public class Pawn : Figure
        {
            public bool isFirstMoveDone { get; set; } = false;
            public Pawn(bool white) : base(white)
            {
            }
            public override bool canMove(Board board, Cell start, Cell end)
            {


                int x = start.X - end.X;
                int y = Math.Abs(start.Y - end.Y);



                if ((isWhite ? x == 1 : x == -1) && y < 2)
                {
                    if (y == 0 && !board.isEmpty(end.X, end.Y))
                    {
                        return false;
                    }
                    if (y == 1 && board.isEmpty(end.X, end.Y))
                    {
                        var pawn = board.EnPassatPawns.Find(item => item.Pawn.Equals(this));
                        if (pawn!=null && this.Equals(pawn.Pawn) && pawn.EnPassatCell.X==end.X && pawn.EnPassatCell.Y == end.Y)
                        {   
                                if (!board.isCloned && isKingBeingAttacked(board, start, end)) return false;
                                return true;
                        }
            
                        return false;
                    }

                    if (!board.isCloned && isKingBeingAttacked(board, start, end)) return false;
                    return true;
                }

                return isValidLongMove(board, start, end);
            }
            private bool isValidLongMove(Board board, Cell start, Cell end)
            {

                if (isFirstMoveDone)
                {
                    return false;
                }

                int pawnDirecrionX = start.X - end.X;
                int x = Math.Abs(start.X - end.X);
                int y = Math.Abs(start.Y - end.Y);

                if ((isWhite ? pawnDirecrionX == 2 : pawnDirecrionX == -2) && y == 0)
                {
                    int stepDirectionX = Math.Sign(end.X - start.X);
                    int stepDirectionY = Math.Sign(end.Y - start.Y);
                    x = start.X; y = start.Y;
                    for (int i = 1; i < 2; i++)
                    {
                        x += stepDirectionX;
                        y += stepDirectionY;

                        if (board.isEmpty(x, y)) continue;
                        else return false;

                    }
                    
                    if (board.isEmpty(end.X, end.Y)) 
                    {
                        if (!board.isCloned && isKingBeingAttacked(board, start, end)) return false;
                        return true;
                    }
                    
                }
                return false;
            }
            public void setEnPassatPawns(Board board, Cell start, Cell end)
            {
                List<EnPassatPawn> pawns = new List<EnPassatPawn>();                
                var dir = 1;
                int stepDirecrionX = Math.Sign(end.X - start.X);
                Cell enPassatCell = board.getCell(start.X + stepDirecrionX, start.Y);
                for (int i = 0; i < 2; i++)
                {
                    var resY = end.Y + dir;
                    if (resY > 7 || resY < 0)
                        resY = end.Y;
                    Cell cell = board.getCell(end.X, resY);
                    if (cell.Figure != null && cell.Figure is Pawn && cell.Figure.isWhite!=isWhite)
                    {
                        var obj = new EnPassatPawn((Pawn)cell.Figure, enPassatCell) ;
                        pawns.Add(obj);
                    }
                        
                    dir = -1;
                }
                board.EnPassatPawns = pawns;
            }
            public bool isLongMove(Cell start, Cell end)
            {
                return Math.Abs(start.X - end.X) == 2;
            }
            public bool isEnPassatMove(Cell start, Cell end)
            {
                return Math.Abs(start.X - end.X) == Math.Abs(start.Y - end.Y) && end.Figure==null;
            }
        }

    }
    public class EnPassatPawn
    {
        public Cell EnPassatCell { get; set; }
        public Pawn Pawn { get; set; }

        public EnPassatPawn(Pawn pawn, Cell EnPassatCell)
        {

            this.EnPassatCell = EnPassatCell;
            this.Pawn = pawn;
        }
    }

}