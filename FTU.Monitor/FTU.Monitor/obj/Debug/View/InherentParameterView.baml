   M S B A M L   `   `   `  ����  FTU.Monitor,    &FTU.Monitor.View.InherentParameterView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5        N� Title$  InherentParameterView��5      .��5
      �  � RowDefinitions 5   
   �� 5      $��30 �6   5      �� 5      $��7* �6   5      �� 5      $��60 �6   5      .+�5   
   �� $��0��6    �� Orientation$ 
Horizontal=�6"   .+�5      �� $��25��6   .��6    当前终端序列号：6*   �� 5      $��20��66   $	��120��6B   $��3q�6N    �� 
IsReadOnly . ��6   �� *ProgramVersion+6n   5      &   System.Windows.Controls.DataGrid  5   
   $��1��6      AutoGenerateColumns .     CanUserAddRows .     CanUserSortColumns .  ��6!   �� *InherentParameterData+   Columns 5      4  .System.Windows.Controls.DataGridCheckBoxColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ 选择��6)   	   Width5  /System.Windows.Controls.DataGridLengthConverter$	 40 65   1  +System.Windows.Controls.DataGridBoundColumn
   Binding
 6@   �� *
Selected+ �� Mode$ TwoWay=�0  *System.Windows.Controls.DataGridTextColumn  5      $ 序号��6%   $	 40 61      
IsReadOnly . 
 6<   �� *Number+6W     5       $ 名称��6%   $		 120 61    . 
 6=   �� *Name+6V     5!      $ 点号��6%   $	 60 61    . 
 6<   �� *ID+6S     5"      $	 值��6%   $		 200 60    . 
 6<   �� *StringValue+6\     5#      $ 备注��6%   $		 250 61    . 
 6=   �� *	Comment+6Y   5%      � 5'   
   $��2��6    � ColumnDefinitions 5(      �� 5)      $��* �6#   �� 5*      $��2* �6#   .+�5-      �� $��0��6   $ Vertical=�6)   $	��Top=�6@   $��10q�6X   $
��Left=�6d   .+�5.      �� $��全选��6   $��25��6(   $��50��64    d� 	IsChecked 6?   �� *ComboxInherentChecked+��6k   �� *SelectAllInherentCommand+6�   5/      �� 51      $��1��6   $��10,10,10,10q�6)   .+�52      �� $��读取固有参数��6   $��25��6*   $	��150��66   $��
200,5,30,5q�6B   $��InherentParameterRead��6�   ��6V   �� *InherentParameterCommand+6�   53      54      56      57      