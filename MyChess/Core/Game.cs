using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MyChess.Core.Figure;

namespace MyChess.Core
{
    public class Game
    {
        public List<Player> players = new List<Player>();
        public Board Board { get; set; }
        public Player CurrentTurn { get; set; }
        public GameStatus Status { get; set; }
        private List<Move> movesPlayed = new List<Move>();


        public void Init(Player p1, Player p2)
        {
            players.Add(p1);
            players.Add(p2);

            Board = new Board();

            if (p1.isWhiteSide)
            {
                this.CurrentTurn = p1;
            }
            else
            {
                this.CurrentTurn = p2;
            }

            movesPlayed.Clear();
        }
        public bool isEnd()
        {
            return Status != GameStatus.ACTIVE;
        }

        public bool playerMove(Player player, int startX, int startY, int endX, int endY)
        {
            if (isEnd())
                return false;

            Cell startCell = Board.getCell(startX, startY);
            Cell endCell = Board.getCell(endX, endY);
            Move move = new Move(player, startCell, endCell);
            return this.makeMove(move, player);
        }

        private bool makeMove(Move move, Player player)
        {
            Figure sourceFigure = move.Start.Figure;
            if (sourceFigure == null)
            {
                return false;
            }

            // valid player
            if (player != CurrentTurn)
            {
                return false;
            }

            if (sourceFigure.isWhite != player.isWhiteSide)
            {
                return false;
            }

            // valid move?
            if (!sourceFigure.canMove(Board, move.Start, move.End))
            {
                return false;
            }

            // kill?
            Figure destFigure = move.End.Figure;
            if (destFigure != null)
            {
                destFigure.isKilled = true;
                move.FigureKilled = destFigure;
            }
            Board.EnPassatPawns.Clear();
            // pawn first move?
            if (sourceFigure != null && sourceFigure is Pawn)
            {
                var pawn = (Pawn)sourceFigure;
                if(!pawn.isFirstMoveDone)
                {
                    pawn.isFirstMoveDone = true;
                    if(pawn.isLongMove(move.Start, move.End))
                    {
                        pawn.setEnPassatPawns(Board, move.Start, move.End);
                    }
                }
                if(pawn.isEnPassatMove(move.Start, move.End))
                {
                    int stepDirecrionX = Math.Sign(move.End.X - move.Start.X);
                    Board.getCell(move.End.X - stepDirecrionX, move.End.Y).Figure = null;
                }
              
            }

            // castling?
            if (sourceFigure != null && sourceFigure is King)
            {

                var king = (King)sourceFigure;
                if (!king.isCastlingDone)
                {
                    king.isCastlingDone = true;
                    if (king.isCastlingMove(move.Start, move.End))
                    {
                        move.isCastlingMove = true;
                    }
                }
                 
                
            }

            // store the move
            movesPlayed.Add(move);

            // move Figure from the start cell to end cell
            move.End.Figure = move.Start.Figure;
            move.Start.Figure = null;

            //is king attacked after move?
            var kingCell = Board.getKingCell(!CurrentTurn.isWhiteSide);
            var enemyKing = (King)kingCell.Figure;
            enemyKing.isAttacked = enemyKing.IsAttacked(Board, kingCell) ? true : false;

            

            if (enemyKing.isAttacked && getAllAllowedCells().Count==0)
            {
                if (CurrentTurn.isWhiteSide)
                {
                    Status = GameStatus.WHITE_WIN;
                }
                else
                {
                    Status = GameStatus.BLACK_WIN;
                }
            } 
            else 
            if(!enemyKing.isAttacked && getAllAllowedCells().Count == 0)
            {
                Status = GameStatus.STALEMATE;
            }

            // set the current turn to the other player
            if (this.CurrentTurn == players[0])
            {
                this.CurrentTurn = players[1];
            }
            else
            {
                this.CurrentTurn = players[0];
            }

            return true;
        }
            private List<Cell> getAllAllowedCells()
            {
            List<Cell> allAllowedCells = new List<Cell>();
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    var cell = Board.getCell(i, j);
                    var figure = cell.Figure;
                    if (figure != null && figure.isWhite != CurrentTurn.isWhiteSide)
                    {
                        allAllowedCells.AddRange(figure.getAllowedCells(Board, cell));
                    }
                }
                return allAllowedCells;
            }

    }

    public enum GameStatus
    {
        ACTIVE,
        BLACK_WIN,
        WHITE_WIN,
        FORFEIT,
        STALEMATE,
        RESIGNATION
    }
}
