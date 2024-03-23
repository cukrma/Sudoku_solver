using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku_solver
{
    public class Cell
    {
        private readonly int id;
        private readonly int row;
        private readonly int col;
        private readonly int box;
        private int number;
        //private List<int> candidates;

        public int Id { get { return id; } }
        public int Row { get { return row; } }
        public int Col { get { return col; } }
        public int Box { get { return box; } }
        public int Number { get { return number; } }
        //public List<int> Candidates { get { return candidates; } set { candidates = value; } }


        public Cell(int row, int col, int number = 0, int id = 0)
        {
            this.id = id != 0 ? id : (row - 1) * 9 + col;
            this.row = row;
            this.col = col;
            this.box = 1 + (((row-1) / 3) * 3) + ((col-1) / 3);
            this.number = number;

            //candidates = number != 0 ? new List<int>() : new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        }

        public void Clear()
        {
            number = 0;
            //candidates.AddRange([1, 2, 3, 4, 5, 6, 7, 8, 9]);
        }

        public void SetNumber(int num)
        {
            //if (number == num)
            //    return;
            //else if (number == 0 && num != 0)
            //{
            //    //candidates.Clear();
            //    // TODO
            //    // upravit kandidaty v sousednich bunkach
            //}
            //else if (number != 0 && num == 0)
            //{
            //    // TODO
            //    // vytvorit kandidaty pro tuto bunku
            //    // upravit kandidaty v sousednich bunkach
            //}
            //else
            //{
            //    // TODO
            //    // upravit kandidaty v sousednich bunkach
            //}

            number = num;

        }



    }
}
