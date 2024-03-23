using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
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


        public bool Solve()
        {
            SetInitialCandidates();
            bool stop = ExistEmptyCellWithoutCandidates();

            if (stop)
                return false;

            FillCellsWithOneCandidate();

            stop = ExistEmptyCellWithoutCandidates();

            if (stop)
                return false;

            if (CountOfEmptyCells(cells) == 0) // all cells have numbers, sudoku is solved
                return true;

            stop = !SolveProcess();

            if (stop)
                return false;

            return true;
        }

        private bool SolveProcess()
        {
            List<Cell> cellsToCompute;

            try
            {
                cellsToCompute = cells.Where(item => item.Number == 0).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error ordering cells to solve the game: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            StringBuilder config = new StringBuilder("");
            StringBuilder config2 = new StringBuilder("");
            int emptyCellsCount = CountOfEmptyCells(cellsToCompute);
            bool goingNext = true;
            int step = 0;

            while (emptyCellsCount > 0)
            {
                step++;

                if (step == 100)
                    Console.WriteLine("");

                try
                {
                    if (goingNext)
                    {
                        int index = config.Length;
                        Cell currentCell = cellsToCompute[index];
                        if (currentCell.Id == 58 && currentCell.Candidates.Contains(4))
                            Console.WriteLine("");
                        if (currentCell.Candidates.GroupBy(num => num).Any(group => group.Count() > 1))
                            Console.WriteLine("");

                        if (currentCell.Candidates.Count == 0)
                        {
                            goingNext = false;
                            continue;
                        }
                        int candidate = currentCell.Candidates.First();
                        ModifyCandidatesOfNeighborCells(currentCell, newNumber: candidate);
                        currentCell.Number = candidate;
                        //currentCell.Candidates.Clear();
                        config.Append(candidate);
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
                        if (currentCell.Id == 58 && currentCell.Candidates.Contains(4))
                            Console.WriteLine("");
                        if (currentCell.Candidates.GroupBy(num => num).Any(group => group.Count() > 1))
                            Console.WriteLine("");

                        List<int> candidates = currentCell.Candidates;
                        int candidateIndex = candidates.IndexOf(lastCandidate);
                        if (candidateIndex < candidates.Count - 1) // some candidates to try remain
                        {
                            int candidate = currentCell.Candidates[candidateIndex + 1];
                            ModifyCandidatesOfNeighborCells(currentCell, "replace", candidate);
                            currentCell.Number = candidate;
                            config.Append(candidate);
                            goingNext = true;
                        }
                        else
                        {
                            ModifyCandidatesOfNeighborCells(currentCell, "add");
                            currentCell.Number = 0;
                            //SetCandidatesForCell(currentCell);
                        }
                    }
                    emptyCellsCount = CountOfEmptyCells(cells);
                    if (config == config2)
                    {
                        MessageBox.Show("Error solving the game, program is inside a cycle.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                    config2.Clear();
                    config2.Append(config);
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

        private void CreateSolvedCells(List<Cell> computedCells)
        {
            solvedCells = cells;

            foreach (Cell cell in computedCells)
            {
                Cell? c = solvedCells.FirstOrDefault(item => item.Id == cell.Id);
                if (c != null)
                {
                    c.Number = cell.Number;
                }
            }
        }

        private void SetInitialCandidates()
        {
            foreach (Cell cell in cells)
            {
                if (cell.Number != 0) // cell already has a number so there are no candidates
                    continue;

                SetCandidatesForCell(cell);
            }
        }

        private void SetCandidatesForCell(Cell cell)
        {
            List<int> candidates = new() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            try
            {
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

        private bool ExistEmptyCellWithoutCandidates()
        {
            int count = 0;

            try
            {
                count = cells.Where(item => item.Number == 0 && item.Candidates.Count == 0).Count();
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

        private void FillCellsWithOneCandidate()
        {
            List<Cell> cellsWithOneCandidate = cells.Where(item => item.Candidates.Count == 1).ToList();
            List<Cell> cc = cells.Where(item => item.Number == 0).ToList();

            while (cellsWithOneCandidate.Count > 0)
            {
                Cell cell = cellsWithOneCandidate.First();
                int num = cell.Candidates.First();
                ModifyCandidatesOfNeighborCells(cell, newNumber: num);
                cell.Number = num;
                //cell.Candidates.Clear();

                cellsWithOneCandidate = cells.Where(item => item.Number == 0 && item.Candidates.Count == 1).ToList();
            }

            cc = cells.Where(item => item.Number == 0).ToList();
        }

        private void ModifyCandidatesOfNeighborCells(Cell cell, string action = "remove", int newNumber = 0)
        {
            if (action == "remove")
            {
                foreach (Cell item in cells.Where(item => (item.Row == cell.Row || item.Col == cell.Col || item.Box == cell.Box)
                && item.Number == 0 && item.Id != cell.Id))
                {
                    item.Candidates.Remove(newNumber);
                }
                //cell.Candidates.Add(newNumber);
                //cell.Candidates = cell.Candidates.Order().ToList();
            }
            else if (action == "replace")
            {
                //ModifyCandidatesOfNeighborCells(cell, "add", cell.Number);
                //ModifyCandidatesOfNeighborCells(cell, "remove", newNumber);
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
                    //if (item.Candidates.Contains(cell.Number))
                    //    continue;
                    //item.Candidates.Add(cell.Number);
                    //item.Candidates = item.Candidates.Order().ToList();
                }
            }
        }

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
