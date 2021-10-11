using MyChess.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;
using static MyChess.Core.Figure;
using static MyChess.Core.Figure.Bishop;

namespace MyChess
{
    public partial class MainForm : Form
    {
        private Game game;
        private Player p1;
        private Player p2;
        private PictureBox[,] pbs = new PictureBox[8, 8];
        private Image figuresAtlas;
        private List<Image> figuresSprites = new List<Image>();

        public MainForm()
        {
            InitializeComponent();

            Init();
            p1 = new HumanPlayer(true);
            p2 = new HumanPlayer(false);
            game = new Game();
            game.Init(p1, p2);
            Build(game);
        }

        void Init()
        {
            string path = Path.GetFullPath(Directory.GetCurrentDirectory() + "\\..\\..\\Textures\\");

            figuresAtlas = new Bitmap(path + "AllFiguresAtlas.png");

            for (int i = 0; i < 12; i++)
            {
                var sprite = new Bitmap(100, 100);
                var gr = Graphics.FromImage(sprite);
                gr.DrawImage(figuresAtlas, new Rectangle(0, 0, 100, 100), 330 * (i > 5 ? i - 6 : i), (i > 5 ? 340 : 0), 350, 350, GraphicsUnit.Pixel);
                figuresSprites.Add(sprite);
            }


            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    pbs[i, j] = new PictureBox { Parent = this, Size = new Size(100, 100), Top = i * 100, Left = j * 100, BorderStyle = BorderStyle.FixedSingle, Tag = new Point(i, j), Cursor = Cursors.Hand, SizeMode = PictureBoxSizeMode.StretchImage };
                    pbs[i, j].Click += pb_Click;
                }

            new Button { Parent = this, Top = 850, Text = "Reset" }.Click += delegate { game = new Game(); game.Init(p1, p2); Build(game); };
        }

        private void Build(Game game)
        {
            Clear();
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    var figure = game.Board.getCell(i, j).Figure;
                    if (figure != null)
                    {
                        if (figure is King)
                        {
                            if (figure.isWhite)
                            {
                                pbs[i, j].Image = figuresSprites[0];
                            }
                            else
                            {
                                pbs[i, j].Image = figuresSprites[6];
                            }
                        }
                        else if (figure is Queen)
                        {
                            if (figure.isWhite)
                            {
                                pbs[i, j].Image = figuresSprites[1];
                            }
                            else
                            {
                                pbs[i, j].Image = figuresSprites[7];
                            }
                        }
                        else if (figure is Bishop)
                        {
                            if (figure.isWhite)
                            {
                                pbs[i, j].Image = figuresSprites[2];
                            }
                            else
                            {
                                pbs[i, j].Image = figuresSprites[8];
                            }
                        }
                        else if (figure is Knight)
                        {
                            if (figure.isWhite)
                            {
                                pbs[i, j].Image = figuresSprites[3];
                            }
                            else
                            {
                                pbs[i, j].Image = figuresSprites[9];
                            }
                        }
                        else if (figure is Rook)
                        {
                            if (figure.isWhite)
                            {
                                pbs[i, j].Image = figuresSprites[4];
                            }
                            else
                            {
                                pbs[i, j].Image = figuresSprites[10];
                            }
                        }
                        else if (figure is Pawn)
                        {
                            if (figure.isWhite)
                            {
                                pbs[i, j].Image = figuresSprites[5];
                            }
                            else
                            {
                                pbs[i, j].Image = figuresSprites[11];
                            }
                        }
                    }

                    else
                    {
                        pbs[i, j].Image = null;
                    }

                }

        }
        bool isMove = false;
        Point prevCell;
        void pb_Click(object sender, EventArgs e)
        {
            var p = ((Point)(sender as Control).Tag);
            var startCell = game.Board.getCell(p.X, p.Y);
            var sourceFigure = startCell.Figure;

            if (sourceFigure == null && !isMove)
            {
                return;
            }
            if (game.isEnd() && !isMove)
            {
                return;
            }

                if (sourceFigure != null && sourceFigure.isWhite != game.CurrentTurn.isWhiteSide && !isMove)
            {
                return;
            }

            if (sourceFigure != null)
            {
                var allowedCells = sourceFigure.getAllowedCells(game.Board, startCell);
                ShowPossibleMoves(allowedCells);
            }

            if (!isMove)
            {
                pbs[p.X, p.Y].BackColor = Color.Gold;
                prevCell = p;
                isMove = true;
                return;
            }

            if (sourceFigure != null && sourceFigure.isWhite == game.CurrentTurn.isWhiteSide)
            {
                isMove = false;
                Build(game);
                pb_Click(sender, e);
                return;
            }


            game.playerMove(game.CurrentTurn, prevCell.X, prevCell.Y, p.X, p.Y);
            isMove = false;
            Build(game);

            if (game.isEnd())
                MessageBox.Show(game.Status.ToString());
        }
        void Clear()
        {
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    if ((i + j) % 2 == 0)
                        pbs[i, j].BackColor = Color.White;
                    else
                    {
                        pbs[i, j].BackColor = Color.LightSkyBlue;
                    }
                }
        }
        void ShowPossibleMoves(List<Cell> cells)
        {
            Clear();
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    foreach (Cell cell in cells)
                        if (cell.X == i && cell.Y == j)
                            pbs[i, j].BackColor = Color.Yellow;
                }
        }
    }

}

