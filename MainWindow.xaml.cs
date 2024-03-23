using Microsoft.Win32;
using System.IO;
using System.Text;
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

        private void AddSudokuToGrid()
        {
            grid.Children.Add(sudoku);
            Grid.SetRow(sudoku, 3);
            Grid.SetColumn(sudoku, 1);
        }


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

                if (fileContent.Length == 81 && fileContent.All(char.IsDigit))
                    sudoku.LoadGame(fileContent);
                else
                    MessageBox.Show("Content of file doesn't match the requirements.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            sudoku.HideKeyboard();
            string rule = sudoku.GameRuleViolated();

            if (rule != "")
            {
                if (stopSavingGame(rule))
                    return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog.FileName = "game.txt";

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

        private void SolveButton_Click(object sender, RoutedEventArgs e)
        {
            sudoku.HideKeyboard();
            string rule = sudoku.GameRuleViolated();

            if (rule != "")
            {
                stopSolvingGame(rule);
                return;
            }

            sudoku.SolveGame();
        }


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

        private void stopSolvingGame(string rule)
        {
            MessageBox.Show($"The game can't be solved because it violates the rules. " +
                $"It contains multiple identical numbers within one {rule}.",
                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}