using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Sudoku_solver
{
    /// <summary>
    /// Interaction logic for CellUC.xaml
    /// </summary>
    public partial class CellUC : UserControl
    {
        private readonly int id;
        private readonly int row;
        private readonly int col;
        private readonly int area;
        private int number;
        private List<int> candidates;

        public int Id { get { return id; } }
        public int Row { get { return row; } }
        public int Col { get { return col; } }
        public int Area { get { return area; } }
        public int Number { get { return number; } }
        public List<int> Candidates { get { return candidates; } set { candidates = value; } }


        public CellUC(int row, int col, int number = 0)
        {
            InitializeComponent();

            this.id = id != 0 ? id : (row - 1) * 9 + col;
            this.row = row;
            this.col = col;
            this.area = 1 + (((row - 1) / 3) * 3) + ((col - 1) / 3);
            this.number = number;
            this.Content = number != 0 ? number.ToString() : null;
            candidates = number != 0 ? new List<int>() : new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            AddBorder(row, col);
        }


        private void AddBorder(int row, int col)
        {
            if (row == 1 || row == 4 || row == 7)
                SetBorderThickness(3, "top");
            if (row == 9)
                SetBorderThickness(3, "bottom");
            if (col == 1 || col == 4 || col == 7)
                SetBorderThickness(3, "left");
            if (col == 9)
                SetBorderThickness(3, "right");
        }


        private void SetBorderThickness(int thickness, string side)
        {
            Thickness newBorderThickness = new Thickness(BorderThickness.Left,
                                                     BorderThickness.Top,
                                                     BorderThickness.Right,
                                                     BorderThickness.Bottom);

            switch (side)
            {
                case "left":
                    {
                        newBorderThickness.Left = thickness;
                        break;
                    }
                case "top":
                    {
                        newBorderThickness.Top = thickness;
                        break;
                    }
                case "right":
                    {
                        newBorderThickness.Right = thickness;
                        break;
                    }
                case "bottom":
                    {
                        newBorderThickness.Bottom = thickness;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            this.BorderThickness = newBorderThickness;
        }


        public void Clear()
        {
            number = 0;
            candidates.AddRange([1, 2, 3, 4, 5, 6, 7, 8, 9]);
            this.Content = null;
        }


        public void SetNumber(int num)
        {
            if (number == num)
                return;
            else if (number == 0 && num != 0)
            {
                candidates.Clear();
                // TODO
                // upravit kandidaty v sousednich bunkach
            }
            else if (number != 0 && num == 0)
            {
                // TODO
                // vytvorit kandidaty pro tuto bunku
                // upravit kandidaty v sousednich bunkach
            }
            else
            {
                // TODO
                // upravit kandidaty v sousednich bunkach
            }

            number = num;
            this.Content = number != 0 ? number.ToString() : null;

        }
    }
}
