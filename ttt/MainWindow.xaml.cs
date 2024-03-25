using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ttt
{
    public partial class MainWindow : Window
    {
        public static int[,] board = { 
                                { 0, 0, 0, 0, -1000, -33, -30, -50 },
                                { 0, 0, 0, 0, 0,    0, 0, 0 },
                                { 0, 0, 0, 0, 0,    0, 0, 0 },
                                { 0, 0, 0, 0, 0,    0, 0, 0 },
                                { 0, 0, 0, 0, 0,    0, 0, 0 },
                                { 0, 0, 0, 0, 0,    0, 0, 0 },
                                { 0, 0, 0, 0, 10,   0, 0, 0 },
                                { 0, 0, 0, 0, 1000, 0, 0, 0 }  
        };

        bool WhiteP = false, WhiteK = false;
        double DeltaX, DeltaY;
        int Pindex1 = 6, Pindex2 = 4, Kindex1 = 7, Kindex2 = 4, top = 0, left = 0;
        int BKlocT = 0, BKlocL = 6, BBlocT = 0, BBlocL = 5, BRlocT = 0, BRlocL = 7, BKinglocT = 0, BKinglocL = 4;
        int BKlocTtemp = 0, BKlocLtemp = 6, BBlocTtemp = 0, BBlocLtemp = 5, BRlocTtemp = 0, BRlocLtemp = 7;
        bool BishopCheker = false, RookChecker = false, BishopChekersec = false, RookCheckersec = false, BishopChekertrd = false, RookCheckertrd = false, KingChecker;
        bool RandMoveChecker = false;
        public MainWindow()
        {
            InitializeComponent();
        }

        void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (WhiteP)
                WhitePawn.Margin = new Thickness(e.GetPosition(this).X - DeltaX,
                e.GetPosition(this).Y - DeltaY, 0, 0);
            if (WhiteK)
                WhiteKing.Margin = new Thickness(e.GetPosition(this).X - DeltaX,
                e.GetPosition(this).Y - DeltaY, 0, 0);
        }

        void WhitePawn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == e.LeftButton)
            {
                StackPanel.SetZIndex(WhitePawn, 1);
                StackPanel.SetZIndex(WhiteKing, 0);
                if (!WhiteP)
                {
                    top = (int)WhitePawn.Margin.Top;
                    left = (int)WhitePawn.Margin.Left;
                }
                WhiteP = true;
                DeltaX = e.GetPosition(this).X - WhitePawn.Margin.Left;
                DeltaY = e.GetPosition(this).Y - WhitePawn.Margin.Top;
            }
        }

        void WhitePawn_MouseUp(object sender, MouseButtonEventArgs e)
        {
            WhiteP = false;
            if (top - (int)(WhitePawn.Margin.Top + 25) / 50 * 50 == 50 && ((left - (int)(WhitePawn.Margin.Left + 25) / 50 * 50 == 0) || (left - (int)(WhitePawn.Margin.Left + 25) / 50 * 50 == 50 && board[Pindex1 - 1, Pindex2 - 1] != 0) || (left - (int)(WhitePawn.Margin.Left + 25) / 50 * 50 == -50 && board[Pindex1 - 1, Pindex2 + 1] != 0)))
            {
                WhitePawn.Margin = new Thickness((int)(WhitePawn.Margin.Left + 25) / 50 * 50, (int)(WhitePawn.Margin.Top + 25) / 50 * 50, 0, 0);
                board[(int)(WhitePawn.Margin.Top + 25) / 50, (int)(WhitePawn.Margin.Left + 25) / 50] = 10;
                board[Pindex1, Pindex2] = 0;
                Pindex1 = (int)(WhitePawn.Margin.Top + 25) / 50;
                Pindex2 = (int)(WhitePawn.Margin.Left + 25) / 50;
                BlackMoves();
            } else
            {
                WhitePawn.Margin = new Thickness(left, top, 0, 0);
            }
        }

        void WhiteKing_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == e.LeftButton)
            {
                StackPanel.SetZIndex(WhiteKing, 1);
                StackPanel.SetZIndex(WhitePawn, 0);
                if (!WhiteK)
                {
                    top = (int)WhiteKing.Margin.Top;
                    left = (int)WhiteKing.Margin.Left;
                }
                WhiteK = true;
                DeltaX = e.GetPosition(this).X - WhiteKing.Margin.Left;
                DeltaY = e.GetPosition(this).Y - WhiteKing.Margin.Top;
            }
        }

        void WhiteKing_MouseUp(object sender, MouseButtonEventArgs e)
        {
            WhiteK = false;
            if ((top - (int)(WhiteKing.Margin.Top + 25) / 50 * 50 == 50 || top - (int)(WhiteKing.Margin.Top + 25) / 50 * 50 == -50 || top - (int)(WhiteKing.Margin.Top + 25) / 50 * 50 == 0) && (left - (int)(WhiteKing.Margin.Left + 25) / 50 * 50 == -50 || left - (int)(WhiteKing.Margin.Left + 25) / 50 * 50 == 0 || left - (int)(WhiteKing.Margin.Left + 25) / 50 * 50 == 50))
            {
                WhiteKing.Margin = new Thickness((int)(WhiteKing.Margin.Left + 25) / 50 * 50, (int)(WhiteKing.Margin.Top + 25) / 50 * 50, 0, 0);
                board[(int)(WhiteKing.Margin.Top + 25) / 50, (int)(WhiteKing.Margin.Left + 25) / 50] = 1000;
                board[Kindex1, Kindex2] = 0;
                Kindex1 = (int)(WhiteKing.Margin.Top + 25) / 50;
                Kindex2 = (int)(WhiteKing.Margin.Left + 25) / 50;
                BlackMoves();
            }
            else
            {
                WhiteKing.Margin = new Thickness(left, top, 0, 0);
            }
        }

        void BlackMoves()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (((Math.Abs(BKlocT - i) == 2 && Math.Abs(BKlocL - j) == 1) || (Math.Abs(BKlocT - i) == 1 && Math.Abs(BKlocL - j) == 2)) && board[i, j] == 1000)
                    {
                        board[BKlocT, BKlocL] = 0;
                        BKlocT = i;
                        BKlocL = j;
                        board[BKlocT, BKlocL] = -30;
                        MyBnFigure.Margin = new Thickness(j * 50, i * 50, 0, 0);
                        return;
                    }
                    else if (Math.Abs(BBlocT - i) == Math.Abs(BBlocL - j) && board[i, j] == 1000)
                    {
                        BishopChekersec = false;
                        for (int l = 1; l < Math.Abs(i - BBlocT); l++)
                        {
                            if (i > BBlocT && j < BBlocL && board[BBlocT + l, BBlocL - l] != 0)
                            {
                                BishopChekersec = true;
                                break;
                            }
                            if (i > BBlocT && j > BBlocL && board[BBlocT + l, BBlocL + l] != 0)
                            {
                                BishopChekersec = true;
                                break;
                            }
                            if (i < BBlocT && j < BBlocL && board[BBlocT - l, BBlocL - l] != 0)
                            {
                                BishopChekersec = true;
                                break;
                            }
                            if (i < BBlocT && j > BBlocL && board[BBlocT - l, BBlocL + l] != 0)
                            {
                                BishopChekersec = true;
                                break;
                            }
                        }
                        if (!BishopChekersec)
                        {
                            board[BBlocT, BBlocL] = 0;
                            BBlocT = i;
                            BBlocL = j;
                            board[BBlocT, BBlocL] = -33;
                            MyWbFigure.Margin = new Thickness(j * 50, i * 50, 0, 0);
                            return;
                        }
                    }
                    else if ((j == BRlocL || i == BRlocT) && board[i, j] == 1000)
                    {
                        RookCheckersec = false;
                        if (j == BRlocL)
                        {
                            for (int l = 1; l < Math.Abs(i - BRlocT); l++)
                            {
                                if (i > BRlocT && board[BRlocT + l, j] != 0)
                                {
                                    RookCheckersec = true;
                                    break;
                                }
                                if (i < BRlocT && board[BRlocT - l, j] != 0)
                                {
                                    RookCheckersec = true;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            for (int l = 1; l < Math.Abs(j - BRlocL); l++)
                            {
                                if (j > BRlocL && board[i, BRlocL + l] != 0)
                                {
                                    RookCheckersec = true;
                                    break;
                                }
                                if (j < BRlocL && board[i, BRlocL - l] != 0)
                                {
                                    RookCheckersec = true;
                                    break;
                                }
                            }
                        }
                        if (!RookCheckersec)
                        {
                            board[BRlocT, BRlocL] = 0;
                            BRlocT = i;
                            BRlocL = j;
                            board[BRlocT, BRlocL] = -50;
                            BlackRook.Margin = new Thickness(j * 50, i * 50, 0, 0);
                            return;
                        }
                    }
                }
            }
            BKlocTtemp = BKlocT;
            BKlocLtemp = BKlocL;
            BBlocTtemp = BBlocT;
            BBlocLtemp = BBlocL;
            BRlocTtemp = BRlocT;
            BRlocLtemp = BRlocL;
            BlackCheckersec();
        }

        void BlackCheckersec()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (((Math.Abs(BKlocTtemp - i) == 2 && Math.Abs(BKlocLtemp - j) == 1) || (Math.Abs(BKlocTtemp - i) == 1 && Math.Abs(BKlocLtemp - j) == 2)) && board[i, j] >= 0)
                    {
                        if (Knight(i, j) == 11)
                        {
                            return;
                        }
                    }
                    if (Math.Abs(BBlocTtemp - i) == Math.Abs(BBlocLtemp - j) && board[i, j] >= 0)
                    {
                        BishopCheker = false;
                        for (int l = 1; l < Math.Abs(i - BBlocTtemp); l++)
                        {
                            if (i > BBlocTtemp && j < BBlocLtemp && board[BBlocTtemp + l, BBlocLtemp - l] != 0)
                            {
                                BishopCheker = true;
                            }
                            if (i > BBlocTtemp && j > BBlocLtemp && board[BBlocTtemp + l, BBlocLtemp + l] != 0)
                            {
                                BishopCheker = true;
                            }
                            if (i < BBlocTtemp && j < BBlocLtemp && board[BBlocTtemp - l, BBlocLtemp - l] != 0)
                            {
                                BishopCheker = true;
                            }
                            if (i < BBlocTtemp && j > BBlocLtemp && board[BBlocTtemp - l, BBlocLtemp + l] != 0)
                            {
                                BishopCheker = true;
                            }
                        }
                        if (!BishopCheker)
                        {
                            if (Bishop(i, j) == 11)
                            {
                                return;
                            }
                        }
                    }
                    if ((j == BRlocLtemp || i == BRlocTtemp) && board[i, j] >= 0)
                    {
                        RookChecker = false;
                        if (j == BRlocLtemp)
                        {
                            for (int l = 1; l < Math.Abs(i - BRlocTtemp); l++)
                            {
                                if (i > BRlocTtemp && board[BRlocTtemp + l, j] != 0)
                                {
                                    RookChecker = true;
                                    break;
                                }
                                if (i < BRlocTtemp && board[BRlocTtemp - l, j] != 0)
                                {
                                    RookChecker = true;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            for (int l = 1; l < Math.Abs(j - BRlocLtemp); l++)
                            {
                                if (j > BRlocLtemp && board[i, BRlocLtemp + l] != 0)
                                {
                                    RookChecker = true;
                                    break;
                                }
                                if (j < BRlocLtemp && board[i, BRlocLtemp - l] != 0)
                                {
                                    RookChecker = true;
                                    break;
                                }
                            }
                        }
                        if (!RookChecker)
                        {
                            if (Rook(i, j) == 11)
                            {
                                return;
                            }
                        }
                    }
                }
            }
            RandomMove();
        }

        int Knight(int BKlocTtemp, int BKlocLtemp)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (((Math.Abs(BKlocTtemp - i) == 2 && Math.Abs(BKlocLtemp - j) == 1) || (Math.Abs(BKlocTtemp - i) == 1 && Math.Abs(BKlocLtemp - j) == 2)) && board[i, j] == 1000)
                    {
                        if (board[BKlocTtemp, BKlocLtemp] >= 0)
                        {
                            board[BKlocT, BKlocL] = 0;
                            BKlocT = BKlocTtemp;
                            BKlocL = BKlocLtemp;
                            board[BKlocT, BKlocL] = -30;
                            MyBnFigure.Margin = new Thickness(BKlocLtemp * 50, BKlocTtemp * 50, 0, 0);
                            return 11;
                        }
                    }
                }
            }
            return 0;
        }

        int Bishop(int BBlocTtemp, int BBlocLtemp)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (Math.Abs(BBlocTtemp - i) == Math.Abs(BBlocLtemp - j) && board[i, j] == 1000)
                    {
                        for (int l = 1; l < Math.Abs(Kindex1 - BBlocTtemp); l++)
                        {
                            if (Kindex1 > BBlocTtemp && Kindex2 < BBlocLtemp && board[BBlocTtemp + l, BBlocLtemp - l] != 0)
                            {
                                return 0;
                            }
                            if (Kindex1 > BBlocTtemp && Kindex2 > BBlocLtemp && board[BBlocTtemp + l, BBlocLtemp + l] != 0)
                            {
                                return 0;
                            }
                            if (Kindex1 < BBlocTtemp && Kindex2 < BBlocLtemp && board[BBlocTtemp - l, BBlocLtemp - l] != 0)
                            {
                                return 0;
                            }
                            if (Kindex1 < BBlocTtemp && Kindex2 > BBlocLtemp && board[BBlocTtemp - l, BBlocLtemp + l] != 0)
                            {
                                return 0;
                            }
                        }
                        if (board[BBlocTtemp, BBlocLtemp] >= 0)
                        {
                            board[BBlocT, BBlocL] = 0;
                            BBlocT = BBlocTtemp;
                            BBlocL = BBlocLtemp;
                            board[BBlocT, BBlocL] = -33;
                            MyWbFigure.Margin = new Thickness(BBlocLtemp * 50, BBlocTtemp * 50, 0, 0);
                            return 11;
                        }
                    }
                }
            }
            return 0;
        }

        int Rook(int BRlocTtemp, int BRlocLtemp)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if ((j == BRlocLtemp || i == BRlocTtemp) && board[i, j] == 1000)
                    {
                        if (j == BRlocLtemp)
                        {
                            for (int l = 1; l < Math.Abs(Kindex1 - BRlocTtemp); l++)
                            {
                                if (Kindex1 > BRlocTtemp && board[BRlocTtemp + l, j] != 0)
                                {
                                    return 0;
                                }
                                if (Kindex1 < BRlocTtemp && board[BRlocTtemp - l, j] != 0)
                                {
                                    return 0;
                                }
                            }
                        }
                        else
                        {
                            for (int l = 1; l < Math.Abs(Kindex2 - BRlocLtemp); l++)
                            {
                                if (Kindex2 > BRlocLtemp && board[i, BRlocLtemp + l] != 0)
                                {
                                    return 0;
                                }
                                if (Kindex2 < BRlocLtemp && board[i, BRlocLtemp - l] != 0)
                                {
                                    return 0;
                                }
                            }
                        }
                        if (board[BRlocTtemp, BRlocLtemp] >= 0)
                        {
                            board[BRlocT, BRlocL] = 0;
                            BRlocT = BRlocTtemp;
                            BRlocL = BRlocLtemp;
                            board[BRlocT, BRlocL] = -50;
                            BlackRook.Margin = new Thickness(BRlocLtemp * 50, BRlocTtemp * 50, 0, 0);
                            return 11;
                        }
                    }
                }
            }
            return 0;
        }

        void RandomMove()
        {
            RandMoveChecker = false;
            Random rnd = new Random();
            while (!RandMoveChecker)
            {
                int rndRow = rnd.Next(0, 8);
                int rndColumn = rnd.Next(0, 8);

                if (((Math.Abs(BKlocT - rndRow) == 2 && Math.Abs(BKlocL - rndColumn) == 1) || (Math.Abs(BKlocT - rndRow) == 1 && Math.Abs(BKlocL - rndColumn) == 2)) && board[rndRow, rndColumn] >= 0)
                {
                    board[BKlocT, BKlocL] = 0;
                    BKlocT = rndRow;
                    BKlocL = rndColumn;
                    board[BKlocT, BKlocL] = -30;
                    MyBnFigure.Margin = new Thickness(rndColumn * 50, rndRow * 50, 0, 0);
                    RandMoveChecker = true;
                    return;
                }
                else if (Math.Abs(BBlocT - rndRow) == Math.Abs(BBlocL - rndColumn) && board[rndRow, rndColumn] >= 0)
                {
                    BishopChekertrd = false;
                    for (int l = 1; l < Math.Abs(rndRow - BBlocT); l++)
                    {
                        if (rndRow > BBlocT && rndColumn < BBlocL && board[BBlocT + l, BBlocL - l] != 0)
                        {
                            BishopChekertrd = true;
                            break;
                        }
                        if (rndRow > BBlocT && rndColumn > BBlocL && board[BBlocT + l, BBlocL + l] != 0)
                        {
                            BishopChekertrd = true;
                            break;
                        }
                        if (rndRow < BBlocT && rndColumn < BBlocL && board[BBlocT - l, BBlocL - l] != 0)
                        {
                            BishopChekertrd = true;
                            break;
                        }
                        if (rndRow < BBlocT && rndColumn > BBlocL && board[BBlocT - l, BBlocL + l] != 0)
                        {
                            BishopChekertrd = true;
                            break;
                        }
                    }
                    if (!BishopChekertrd)
                    {
                        board[BBlocT, BBlocL] = 0;
                        BBlocT = rndRow;
                        BBlocL = rndColumn;
                        board[BBlocT, BBlocL] = -33;
                        MyWbFigure.Margin = new Thickness(rndColumn * 50, rndRow * 50, 0, 0);
                        RandMoveChecker = true;
                        return;
                    }
                }
                else if ((rndColumn == BRlocL || rndRow == BRlocT) && board[rndRow, rndColumn] >= 0)
                {
                    RookCheckertrd = false;
                    if (rndColumn == BRlocL)
                    {
                        for (int l = 1; l < Math.Abs(rndRow - BRlocT); l++)
                        {
                            if (rndRow > BRlocT && board[BRlocT + l, rndColumn] != 0)
                            {
                                RookCheckertrd = true;
                                break;
                            }
                            if (rndRow < BRlocT && board[BRlocT - l, rndColumn] != 0)
                            {
                                RookCheckertrd = true;
                                break;
                            }
                        }
                    }
                    else
                    {
                        for (int l = 1; l < Math.Abs(rndColumn - BRlocL); l++)
                        {
                            if (rndColumn > BRlocL && board[rndRow, BRlocL + l] != 0)
                            {
                                RookCheckertrd = true;
                                break;
                            }
                            if (rndColumn < BRlocL && board[rndRow, BRlocL - l] != 0)
                            {
                                RookCheckertrd = true;
                                break;
                            }
                        }
                    }
                    if (!RookCheckertrd)
                    {
                        board[BRlocT, BRlocL] = 0;
                        BRlocT = rndRow;
                        BRlocL = rndColumn;
                        board[BRlocT, BRlocL] = -50;
                        BlackRook.Margin = new Thickness(rndColumn * 50, rndRow * 50, 0, 0);
                        RandMoveChecker = true;
                        return;
                    }
                }
                else if (((Math.Abs(rndColumn - BKinglocL) == 1 && Math.Abs(rndRow - BKinglocT) == 1) || (Math.Abs(rndColumn - BKinglocL) == 0 && Math.Abs(rndRow - BKinglocT) == 1) || (Math.Abs(rndColumn - BKinglocL) == 1 && Math.Abs(rndRow - BKinglocT) == 0)) && board[rndRow, rndColumn] >= 0)
                {
                    KingChecker = false;
                    for (int i = rndRow - 1; i < rndRow + 2; i++) 
                    {
                        for(int j = rndColumn - 1; j < rndColumn + 2; j++)
                        {
                            if (i > -1 && i < 8 && j > -1 && j < 8)
                            {
                                if (board[i, j] == -1000)
                                {
                                    KingChecker = true;
                                }
                            }
                        }
                    }
                    if (!KingChecker && board[rndRow + 1, rndColumn + 1] != 10 && board[rndRow + 1, rndColumn - 1] != 10)
                    {
                        board[BKinglocT, BKinglocL] = 0;
                        BKinglocT = rndRow;
                        BKinglocL = rndColumn;
                        board[BKinglocT, BKinglocL] = -50;
                        BlackKing.Margin = new Thickness(rndColumn * 50, rndRow * 50, 0, 0);
                        RandMoveChecker = true;
                        return;
                    }
                }
            }
        }
    }
}