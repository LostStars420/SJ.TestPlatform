   M S B A M L   `   `   `  ����  FTU.Monitor    FTU.Monitor.View.COSView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5        N� Title$	  COS��5      .��5
      �  � RowDefinitions 5   
   �� 5      $��30 �6   6*   �� 5      $��* �6   6)   .+�5   
   � $��0��6   .+�5      �� $��3q�6    �� Orientation$ 
Horizontal=�6$   $��Right=�6=   .+�5      �� $��0,0,5,0q�6   $��60��6*   $��导入��68   $��
ImportData��6d   ��6E   �� *
COSCommand+6�   �� 5      $��60��6   $��导出��6'   $��
ExportData��6S   ��64   �� *
COSCommand+6s   5      5      � 5   
   $��1��6   .+�5      &   System.Windows.Controls.DataGrid  -   5          Name$ dg��5      $��0��6#      AutoGenerateColumns .     CanUserAddRows .     CanUserSortColumns .     EnableRowVirtualization .  ��60   �� *	COSData+   RowStyle 5      �� 5      	 �� 
TargetType)  #System.Windows.Controls.DataGridRow"	  6   
 �� Triggers
 5      �� 5       �� Binding 6*   �� *Value+ �� Value 16D   .a�5   "   ��  �� Property � �� �� Value �	#FFD2F4F96S   5      5         Columns 5"      0  *System.Windows.Controls.DataGridTextColumn  5#      ,  &System.Windows.Controls.DataGridColumn   Header$ 序号��6)      Width5  /System.Windows.Controls.DataGridLengthConverter$ 40 65      
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn   Binding 6D   �� *Number+6`     5$      $ 点号��6)   $ 60 65    .  6A   �� *ID+6X     5%      $ 内容��6)   $	 150 65    .  6A   �� *	Content+6]     5&      $	 值��6)   $ 50 65    .  6@   �� *Value+6Z     5'      $ 备注��6)   $	 100 65    .  6A   �� *	Comment+6]    � ContextMenu 5*      �� 5+      .@�5,      m� $��清除��6#    m� CommandParameter$ Clear��6N    m� Command 6/   �� *
COSCommand+5-      5.      50      51      52      53      