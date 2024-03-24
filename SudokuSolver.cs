using System.Text;
using System.Windows;


namespace Sudoku_solver
{
    public class SudokuSolver
    {
        private List<Cell> cells;
        private List<Cell> solvedCells = new List<Cell>();

        public List<Cell> SolvedCells { get { return solvedCells; } set { solvedCells = value; } }


        public SudokuSolver(List<Cell> cells)
        { 
            this.cells = cells;
        }

        // main method that ensures solving the sudoku game
        public bool Solve()
        {
            SetInitialCandidates(); // setting candidates (possible numbers) for each cell that is empty
            bool stop = ExistEmptyCellWithoutCandidates(); // if there is empty cell that has none candidates, it cant be solved

            if (stop)
                return false;

            FillCellsWithOneCandidate(); // setting number for all cells with just one candidate

            stop = ExistEmptyCellWithoutCandidates();

            if (stop)
                return false;

            if (CountOfEmptyCells(cells) == 0) // if all cells have numbers, sudoku is solved
                return true;

            stop = !SolveProcess();

            if (stop)
                return false;

            return true;
        }

        // method with its own solving algorithm
        private bool SolveProcess()
        {
            List<Cell> cellsToCompute; // all cells that are empty at the start of solving proccess

            try
            {
                cellsToCompute = cells.Where(item => item.Number == 0).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error ordering cells to solve the game: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            StringBuilder config = new StringBuilder(""); // current configuration
            int emptyCellsCount = CountOfEmptyCells(cellsToCompute);
            bool goingNext = true; // true = going to calculate next new cell; false = going back and trying other candidates

            while (emptyCellsCount > 0)
            {
                try
                {
                    if (goingNext)
                    {
                        int index = config.Length;
                        Cell currentCell = cellsToCompute[index];

                        if (currentCell.Candidates.Count == 0)
                        {
                            goingNext = false;
                            continue;
                        }
                        SetNewCandidateForCell(currentCell, config);
                    }
                    else
                    {
                        if (config.Length == 0) // we tried all possible options and none of them succeed
                            return false;

                        char c = config[config.Length - 1];
                        int lastCandidate = (int)char.GetNumericValue(c);
                        config.Remove(config.Length - 1, 1);

                        int index = config.Length;
                        Cell currentCell = cellsToCompute[index];

                        List<int> candidates = currentCell.Candidates;
                        int candidateIndex = candidates.IndexOf(lastCandidate);
                        if (candidateIndex < candidates.Count - 1) // some candidates to try remain
                        {
                            SetNewCandidateForCell(currentCell, config, candidateIndex + 1, "replace");
                            goingNext = true;
                        }
                        else
                        {
                            ModifyCandidatesOfNeighborCells(currentCell, "add");
                            currentCell.Number = 0;
                        }
                    }
                    emptyCellsCount = CountOfEmptyCells(cells);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error solving the game: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }

            CreateSolvedCells(cellsToCompute);

            return true;
        }

        // method for setting a new candidate for a cell, modifying the neighbor cells candidates and modifying the config
        private void SetNewCandidateForCell(Cell currentCell, StringBuilder config, int candidateIndex = 0, string action = "remove")
        {
            int candidate = currentCell.Candidates[candidateIndex];
            ModifyCandidatesOfNeighborCells(currentCell, action, newNumber: candidate);
            currentCell.Number = candidate;
            config.Append(candidate);
        }

        // method that settings solvedCells for its final values
        private void CreateSolvedCells(List<Cell> computedCells)
        {
            solvedCells = cells; // original cells

            foreach (Cell cell in computedCells) // cells that were computed during solving proccess
            {
                Cell? c = solvedCells.FirstOrDefault(item => item.Id == cell.Id);
                if (c != null)
                {
                    c.Number = cell.Number;
                }
            }
        }

        // method for setting candidates before the start of solving proccess
        private void SetInitialCandidates()
        {
            foreach (Cell cell in cells)
            {
                if (cell.Number != 0) // cell already has a number so there are no candidates
                    continue;

                SetCandidatesForCell(cell);
            }
        }

        // method for setting candidates for input cell
        private void SetCandidatesForCell(Cell cell)
        {
            List<int> candidates = new() { 1, 2, 3, 4, 5, 6, 7, 8, 9 }; // all posible numbers

            try
            {
                // all numbers that are in the same row, column or box as the input cell
                List<int> occupiedNumbers = cells.Where(item => item.Row == cell.Row || item.Col == cell.Col || item.Box == cell.Box)
                    .Select(item => item.Number)
                    .GroupBy(num => num)
                    .Select(group => group.Key)
                    .ToList();

                cell.Candidates = candidates.Except(occupiedNumbers).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occured while setting candidates for a cell: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // method that finds out if exists an empty cell with no candidates
        private bool ExistEmptyCellWithoutCandidates()
        {
            int count = 0;

            try
            {
                count = cells.Where(item => item.Number == 0 && item.Candidates.Count == 0).Count();
                if (count > 0)
                    MessageBox.Show("Current game state has no valid solution.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occured while checking validation of empty cells: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (count == 0)
                return false;
            else
                return true;
        }

        // method for filling all empty cells with just one candidate
        private void FillCellsWithOneCandidate()
        {
            List<Cell> cellsWithOneCandidate = cells.Where(item => item.Candidates.Count == 1).ToList();

            while (cellsWithOneCandidate.Count > 0)
            {
                Cell cell = cellsWithOneCandidate.First();
                int num = cell.Candidates.First();
                ModifyCandidatesOfNeighborCells(cell, newNumber: num); // all neighbor cell candidates has to modified
                cell.Number = num;

                cellsWithOneCandidate = cells.Where(item => item.Number == 0 && item.Candidates.Count == 1).ToList();
            }
        }

        // method for modifying candidates of neighbor cells when the input cell change its number
        private void ModifyCandidatesOfNeighborCells(Cell cell, string action = "remove", int newNumber = 0)
        {
            if (action == "remove")
            {
                foreach (Cell item in cells.Where(item => (item.Row == cell.Row || item.Col == cell.Col || item.Box == cell.Box)
                && item.Number == 0 && item.Id != cell.Id))
                {
                    item.Candidates.Remove(newNumber);
                }
            }
            else if (action == "replace")
            {
                cell.Number = newNumber;
                foreach (Cell item in cells.Where(item => (item.Row == cell.Row || item.Col == cell.Col || item.Box == cell.Box) && item.Number == 0))
                {
                    SetCandidatesForCell(item);
                }
            }
            else if (action == "add")
            {
                cell.Number = newNumber;
                foreach (Cell item in cells.Where(item => (item.Row == cell.Row || item.Col == cell.Col || item.Box == cell.Box) && item.Number == 0))
                {
                    SetCandidatesForCell(item);
                }
            }
        }

        // method that returns count of empty cells in the sudoku
        private int CountOfEmptyCells(List<Cell> input)
        {
            int count = 0;
            try
            {
                count = input.Where(item => item.Number == 0).Count();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occured while counting empty cells: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return count;
        }

    }
}
