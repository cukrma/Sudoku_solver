using System.Windows;
using System.Windows.Controls;


namespace Sudoku_solver
{
    // helping class for transfering arguments throw event
    public class KeyboardUCEventArgs : EventArgs
    {
        public int? Value { get; }

        public KeyboardUCEventArgs(int? value)
        {
            this.Value = value;
        }
    }

    /// <summary>
    /// Interaction logic for KeyboardUC.xaml
    /// </summary>
    public partial class KeyboardUC : UserControl
    {
        public event EventHandler<KeyboardUCEventArgs>? keyboardButton_Click;

        public KeyboardUC()
        {
            InitializeComponent();

            CreateButtons();
        }

        // method for creating all buttons for the keyboard and setting their positions in the grid
        private void CreateButtons()
        {
            for (int i = 1; i <= 9; i++)
            {
                Button btn = CreateButton(i, i.ToString());
                grid.Children.Add(btn);

                int row = (i + 2) / 3;
                int col = ((i - 1) % 3) + 1;
                Grid.SetRow(btn, row);
                Grid.SetColumn(btn, col);
            }

            Button empty = CreateButton(0, "Empty", 20, new Thickness(4, 2, 4, 2)); // Empty button
            empty.Margin = new Thickness(0, 0, 22, 0); // space between Empty button and Cancel button
            Button cancel = CreateButton(null, "Cancel", 20, new Thickness(4, 2, 4, 2)); // Cancel button

            StackPanel stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Horizontal;
            stackPanel.Children.Add(empty);
            stackPanel.Children.Add(cancel);

            grid.Children.Add(stackPanel);
            Grid.SetRow(stackPanel, 5);
            Grid.SetColumn(stackPanel, 1);
            Grid.SetColumnSpan(stackPanel, 3);
        }

        // method for creating a single button with some default values
        private Button CreateButton(int? value, string content, double? fontSize = null, Thickness? thickness = null)
        {
            Button btn = new Button();
            btn.Name = $"key_{value}";
            btn.Content = content;
            btn.Click += ButtonClickHandler;

            if (fontSize != null)
                btn.FontSize = (double)fontSize;
            if (thickness != null)
                btn.Padding = (Thickness)thickness;

            return btn;
        }

        // handler for button click (sending arguments to its parent throw event)
        private void ButtonClickHandler(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                string name = btn.Name;
                try
                {
                    string subStr = name.Substring(4);
                    int? value = null;
                    if (subStr.Length > 0)
                        value = int.Parse(subStr);

                    var args = new KeyboardUCEventArgs(value); // creating arguments
                    keyboardButton_Click?.Invoke(this, args); // creating event with arguments that is send to its parent
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error occured while parsing value from button: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

    }
}
