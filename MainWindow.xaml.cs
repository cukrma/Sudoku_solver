using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;


namespace Sudoku_solver
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SudokuTableUC sudoku = new SudokuTableUC();

        public MainWindow()
        {
            InitializeComponent();

            AddSudokuToGrid();

        }

        // method for adding Sudoku component in the grid at specific position
        private void AddSudokuToGrid()
        {
            grid.Children.Add(sudoku);
            Grid.SetRow(sudoku, 3);
            Grid.SetColumn(sudoku, 1);
        }

        // handler for clear button that clears all sudoku
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            sudoku.HideKeyboard();

            try
            {
                sudoku.Buttons.ForEach(btn => btn.Content = "");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unexpected error has occured: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // handler for load button that loads a game from a file
        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            sudoku.HideKeyboard();

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            string fileContent = "";
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                try
                {
                    fileContent = File.ReadAllText(filePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error reading file: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                if (fileContent.Length == 81 && fileContent.All(char.IsDigit)) // check if the content of file is in valid format
                    sudoku.LoadGame(fileContent);
                else
                    MessageBox.Show("Content of file doesn't match the requirements.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // handler for save button that saves current state of a game into a file
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            sudoku.HideKeyboard();
            string rule = sudoku.GameRuleViolated(); // checking if the current state of a game is valid

            if (rule != "") // if its not valid, then it goes inside this if block
            {
                if (stopSavingGame(rule)) // if user doenst want to continue saving, the saving is canceled
                    return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog.FileName = "game.txt"; // pre-defined name for saving file

            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;
                try
                {
                    string contentToWrite = sudoku.GameToString();
                    File.WriteAllText(filePath, contentToWrite);
                    MessageBox.Show("File saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving file: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // handler for solve button that runs a solving algorithm
        private void SolveButton_Click(object sender, RoutedEventArgs e)
        {
            sudoku.HideKeyboard();
            string rule = sudoku.GameRuleViolated(); // check if the start state of a game is valid

            if (rule != "")
            {
                stopSolvingGame(rule);
                return;
            }

            sudoku.SolveGame(); // run the solving algorithm
        }

        // helping method that informs user that the state of a game is invalid and asks if he/she wants to continue saving proccess
        private bool stopSavingGame(string rule)
        {
            MessageBoxResult result = MessageBox.Show($"The game you want to save violates the rules. " +
                $"It contains multiple identical numbers within one {rule}. Do you still want to proceed with the save?",
                "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
                return false;
            else
                return true;
        }

        // helping method that informs user that the state of a game is invalid and so the game cant be solved
        private void stopSolvingGame(string rule)
        {
            MessageBox.Show($"The game can't be solved because it violates the rules. " +
                $"It contains multiple identical numbers within one {rule}.",
                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}