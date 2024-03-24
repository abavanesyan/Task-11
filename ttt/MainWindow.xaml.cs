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
    [SuppressUnmanagedCodeSecurity]
    public static class ConsoleManager
    {
        private const string Kernel32_DllName = "kernel32.dll";

        [DllImport(Kernel32_DllName)]
        private static extern bool AllocConsole();

        [DllImport(Kernel32_DllName)]
        private static extern bool FreeConsole();

        [DllImport(Kernel32_DllName)]
        private static extern IntPtr GetConsoleWindow();

        [DllImport(Kernel32_DllName)]
        private static extern int GetConsoleOutputCP();

        public static bool HasConsole
        {
            get { return GetConsoleWindow() != IntPtr.Zero; }
        }

        /// <summary>
        /// Creates a new console instance if the process is not attached to a console already.
        /// </summary>
        public static void Show()
        {
            //#if DEBUG
            if (!HasConsole)
            {
                AllocConsole();
                InvalidateOutAndError();
            }
            //#endif
        }

        /// <summary>
        /// If the process has a console attached to it, it will be detached and no longer visible. Writing to the System.Console is still possible, but no output will be shown.
        /// </summary>
        public static void Hide()
        {
            //#if DEBUG
            if (HasConsole)
            {
                SetOutAndErrorNull();
                FreeConsole();
            }
            //#endif
        }

        public static void Toggle()
        {
            if (HasConsole)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }

        static void InvalidateOutAndError()
        {
            Type type = typeof(System.Console);

            System.Reflection.FieldInfo _out = type.GetField("_out",
                System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

            System.Reflection.FieldInfo _error = type.GetField("_error",
                System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

            System.Reflection.MethodInfo _InitializeStdOutError = type.GetMethod("InitializeStdOutError",
                System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

            Debug.Assert(_out != null);
            Debug.Assert(_error != null);

            Debug.Assert(_InitializeStdOutError != null);

            _out.SetValue(null, null);
            _error.SetValue(null, null);

            _InitializeStdOutError.Invoke(null, new object[] { true });
        }

        static void SetOutAndErrorNull()
        {
            Console.SetOut(TextWriter.Null);
            Console.SetError(TextWriter.Null);
        }
    }
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

        bool BnFigureClicked, WbFigureClicked, WhiteP = false, WhiteK = false;
        double DeltaX, DeltaY;
        int Pindex1 = 6, Pindex2 = 4, Kindex1 = 7, Kindex2 = 4, top = 0, left = 0;
        int BKlocT = 0, BKlocL = 6, BBlocT = 0, BBlocL = 5, BRlocT = 0, BRlocL = 7;
        int BKlocTtemp = 0, BKlocLtemp = 6, BBlocTtemp = 0, BBlocLtemp = 5, BRlocTtemp = 0, BRlocLtemp = 7;
        bool BishopCheker = false, RookChecker = false;
        public MainWindow()
        {
            InitializeComponent();
        }

        void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (BnFigureClicked)
                MyBnFigure.Margin = new Thickness(e.GetPosition(this).X - DeltaX,
                e.GetPosition(this).Y - DeltaY, 0, 0);
            if (WbFigureClicked)
                MyWbFigure.Margin = new Thickness(e.GetPosition(this).X - DeltaX,
                e.GetPosition(this).Y - DeltaY, 0, 0);
            if (WhiteP)
                WhitePawn.Margin = new Thickness(e.GetPosition(this).X - DeltaX,
                e.GetPosition(this).Y - DeltaY, 0, 0);
            if (WhiteK)
                WhiteKing.Margin = new Thickness(e.GetPosition(this).X - DeltaX,
                e.GetPosition(this).Y - DeltaY, 0, 0);
        }
        void MyBnFigure_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == e.LeftButton)
            {
                StackPanel.SetZIndex(MyBnFigure, 1);
                StackPanel.SetZIndex(MyWbFigure, 0);
                BnFigureClicked = true;
                DeltaX = e.GetPosition(this).X - MyBnFigure.Margin.Left;
                DeltaY = e.GetPosition(this).Y - MyBnFigure.Margin.Top;
            }
        }
        void MyBnFigure_MouseUp(object sender, MouseButtonEventArgs e)
        {
            BnFigureClicked = false;
            MyBnFigure.Margin = new Thickness((int)(MyBnFigure.Margin.Left + 25) / 50 * 50,(int)(MyBnFigure.Margin.Top + 25) / 50 * 50, 0, 0);
        }
        void MyWbFigure_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == e.LeftButton)
            {
                StackPanel.SetZIndex(MyWbFigure, 1);
                StackPanel.SetZIndex(MyBnFigure, 0);
                WbFigureClicked = true;
                DeltaX = e.GetPosition(this).X - MyWbFigure.Margin.Left;
                DeltaY = e.GetPosition(this).Y - MyWbFigure.Margin.Top;
            }
        }
        void MyWbFigure_MouseUp(object sender, MouseButtonEventArgs e)
        {
            WbFigureClicked = false;
            MyWbFigure.Margin = new Thickness((int)(MyWbFigure.Margin.Left + 25) / 50 * 50,
            (int)(MyWbFigure.Margin.Top + 25) / 50 * 50, 0, 0);
        }

        void WhitePawn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == e.LeftButton)
            {
                StackPanel.SetZIndex(WhitePawn, 1);
                StackPanel.SetZIndex(MyWbFigure, 0);
                StackPanel.SetZIndex(MyBnFigure, 0);
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
                StackPanel.SetZIndex(MyWbFigure, 0);
                StackPanel.SetZIndex(MyBnFigure, 0);
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
                        board[BBlocT, BBlocL] = 0;
                        BBlocT = i;
                        BBlocL = j;
                        board[BBlocT, BBlocL] = -33;
                        MyWbFigure.Margin = new Thickness(j * 50, i * 50, 0, 0);
                        return;
                    }
                    else if ((j == BRlocL || i == BRlocT) && board[i, j] == 1000)
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
    }
}