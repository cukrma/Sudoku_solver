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
    /// Interaction logic for SudokuTableUC.xaml
    /// </summary>
    public partial class SudokuTableUC : UserControl
    {
        private List<Cell> cells = new List<Cell>();
        private List<Button> buttons = new List<Button>();
        private KeyboardUC keyboardUC = new KeyboardUC();
        private int clickedButtonId = 0;

        public List<Button> Buttons { get { return buttons; } }


        public SudokuTableUC()
        {
            InitializeComponent();

            CreateGridCellsAndButtons();
            CreateKeyboard();
        }

        public void LoadGame(string input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];
                int num = (int)char.GetNumericValue(c);

                Button btn = buttons.ElementAt(i);
                btn.Content = num == 0 ? "" : c;
            }
        }

        public string GameToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (Button btn in buttons)
            {
                string? content = btn.Content.ToString() == "" ? "0" : btn.Content.ToString();
                sb.Append(content);
            }
            return sb.ToString();
        }

        public string GameRuleViolated()
        {
            cells = SetCellsNumbers();
            string rule = CheckRules();
            return rule;
        }

        public void SolveGame()
        {
            SudokuSolver sudokuSolver = new SudokuSolver(cells);
            bool isSolved = sudokuSolver.Solve();

            if (!isSolved)
            {
                MessageBox.Show("The Sudoku was not successfully solved for an unknown reason.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            cells = sudokuSolver.SolvedCells;
            
            for (int i = 0; i < 81; i++)
            {
                try
                {
                    Cell cell = cells.ElementAt(i);
                    Button btn = buttons.ElementAt(i);
                    btn.Content = cell.Number.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error occured while setting buttons content: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            string rule = CheckRules();

            if (rule != "")
            {
                MessageBox.Show($"Game rules violated! {rule}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void CreateGridCellsAndButtons()
        {
            for (int row = 1; row <= 9; row++)
            {
                for (int col = 1; col <= 9; col++)
                {
                    Button btn = new Button();
                    int id = (row - 1) * 9 + col;
                    btn.Name = $"button_{id}";
                    btn.Content = "";
                    btn.BorderThickness = new Thickness(0.5);
                    btn.Click += ButtonClickHandler;
                    AddBorder(btn, row, col);
                    buttons.Add(btn);
                    grid.Children.Add(btn);

                    Grid.SetRow(btn, row - 1);
                    Grid.SetColumn(btn, col - 1);

                    Cell cell = new Cell(row, col, 0, id);
                    cells.Add(cell);
                }
            }
        }

        private void CreateKeyboard()
        {
            HideKeyboard();
            keyboardUC.keyboardButton_Click += onKeyboardButton_Click;
            grid.Children.Add(keyboardUC);

            Grid.SetRow(keyboardUC, 0);
            Grid.SetRowSpan(keyboardUC, 9);
            Grid.SetColumn(keyboardUC, 0);
            Grid.SetColumnSpan(keyboardUC, 9);
        }

        private List<Cell> SetCellsNumbers()
        {
            for (int i = 0; i < 81; i++)
            {
                try
                {
                    Button btn = buttons.ElementAt(i);
                    int num = btn.Content.ToString() == "" ? 0 : int.Parse(btn.Content.ToString());

                    Cell cell = cells.ElementAt(i);
                    cell.Number = num;
                    cell.Candidates.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unexpected error eccured while checking validity of the game: " + ex.Message,
                        "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }

            return cells;
        }

        private string CheckRules()
        {
            string rule = "";
            bool hasDuplicates = false;

            for (int index = 1; index <= 9; index++)
            {
                // check row
                hasDuplicates = cells
                    .Where(cell => cell.Row == index && cell.Number != 0)
                    .Select(cell => cell.Number)
                    .GroupBy(num => num)
                    .Any(group => group.Count() > 1);

                if (hasDuplicates)
                {
                    rule = "row";
                    break;
                }

                // check col
                hasDuplicates = cells
                    .Where(cell => cell.Col == index && cell.Number != 0)
                    .Select(cell => cell.Number)
                    .GroupBy(num => num)
                    .Any(group => group.Count() > 1);

                if (hasDuplicates)
                {
                    rule = "column";
                    break;
                }

                // check box
                hasDuplicates = cells
                    .Where(cell => cell.Box == index && cell.Number != 0)
                    .Select(cell => cell.Number)
                    .GroupBy(num => num)
                    .Any(group => group.Count() > 1);

                if (hasDuplicates)
                {
                    rule = "box";
                    break;
                }
            }
            return rule;
        }


        private void ButtonClickHandler(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                string name = btn.Name;
                try
                {
                    clickedButtonId = int.Parse(name.Substring(7));
                    keyboardUC.Visibility = Visibility.Visible;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error occured while parsing id from button: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void onKeyboardButton_Click(object sender, KeyboardUCEventArgs e)
        {
            HideKeyboard();
            int? num = e.Value;

            if (num != null)
            {
                Button btn = buttons.ElementAt(clickedButtonId - 1);
                btn.Content = num == 0 ? "" : num.ToString();
            }
        }

        private void AddBorder(Button btn, int row, int col)
        {
            if (row == 1 || row == 4 || row == 7)
                SetBorderThickness(btn, 3, "top");
            if (row == 9)
                SetBorderThickness(btn, 3, "bottom");
            if (col == 1 || col == 4 || col == 7)
                SetBorderThickness(btn, 3, "left");
            if (col == 9)
                SetBorderThickness(btn, 3, "right");
        }

        private void SetBorderThickness(Button btn, int thickness, string side)
        {
            Thickness newBorderThickness = new Thickness(btn.BorderThickness.Left,
                                                     btn.BorderThickness.Top,
                                                     btn.BorderThickness.Right,
                                                     btn.BorderThickness.Bottom);
            
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

            btn.BorderThickness = newBorderThickness;
        }

        public void HideKeyboard()
        {
            keyboardUC.Visibility = Visibility.Collapsed;
        }

    }
}
