   M S B A M L   `   `   `  ����H  DSudoku_solver, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:Sudoku_solverSudoku_solver  5   	       Sudoku_solver.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:Sudoku_solver     5   	     � Title$  Sudoku solver��5   	   $	��600��6   $	��900��6,   $	��600��5	   	   $	��900��6   ���	#FFE4E4E4$��25(�5
       $��SemiBold"�6.   .��5      � -   5         �Name$
 grid��5       � ColumnDefinitions 5   
   �� 5      $��20 �6   �� 5      $��4* �6   �� 5      $��50 �6   �� 5      $��* �6   �� 5      $��20 �6    � RowDefinitions 5   
   �� 5      $��20 �6   �� 5      $
��auto �6   �� 5      $��50 �6   �� 5      $��* �6   �� 5      $��20 �6   .+�5   
   �� $��Sudoku solver��6   $��1��6)   $��1��69   $��50(�5      $
��Bold"�6"   ���	#FF1B1B1B$��Center=�5   +   �� 5    
   $��3��6   $��3��6%   $��1��62    �� Orientation$ Vertical=�5!      $��Center=�6,   $	��200��5"      .+�5#      �� -   5        $��Clear��5#       � Cursor$
 Hand��6%    � ToolTip$ Clear all sudoku��5$      60   �� 5%      -   5        $
��Load��5%      $��0 25 0 0q�6$   $
 Hand��66   $ Load saved game from file��5&      5'      �� 5(      -   5        $
��Save��5(      $��0 25 0 0q�6$   $
 Hand��66   $ Save current game to file��5)      5*      �� 5+      -   5        $��Solve��5+      $��0 25 0 0q�6%   $
 Hand��67   $ Solve current game��5,      5-      5.      52      53      