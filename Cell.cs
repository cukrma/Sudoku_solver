

namespace Sudoku_solver
{
    public class Cell
    {
        private readonly int id;
        private readonly int row;
        private readonly int col;
        private readonly int box;
        private int number;
        private List<int> candidates = new List<int>();

        public int Id { get { return id; } }
        public int Row { get { return row; } }
        public int Col { get { return col; } }
        public int Box { get { return box; } }
        public int Number { get { return number; } set { number = value; } }
        public List<int> Candidates { get { return candidates; } set { candidates = value; } }


        public Cell(int row, int col, int number = 0, int id = 0)
        {
            this.id = id != 0 ? id : (row - 1) * 9 + col;
            this.row = row;
            this.col = col;
            this.box = 1 + (((row-1) / 3) * 3) + ((col-1) / 3);
            this.number = number;
        }

        // method for "clearing" the cell
        public void Clear()
        {
            number = 0;
        }
    }
}
