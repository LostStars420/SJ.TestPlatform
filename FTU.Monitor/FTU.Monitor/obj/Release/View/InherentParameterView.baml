   M S B A M L   `   `   `  ����   FTU.Monitor,    &FTU.Monitor.View.InherentParameterView   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008       N� Title$  InherentParameterView��.���  � RowDefinitions �� $��30 ��� $��7* ��� $��60 �.+��� $��0�� �� Orientation$ 
Horizontal=�.+��� $��25��.��当前终端序列号：�� $��20��$	��120��$��3q� �� 
IsReadOnly . ���� *ProgramVersion+&   System.Windows.Controls.DataGrid  $��1��   AutoGenerateColumns .     CanUserAddRows .     CanUserSortColumns .  ���� *InherentParameterData+   Columns 4  .System.Windows.Controls.DataGridCheckBoxColumn  ,  &System.Windows.Controls.DataGridColumn   Header$ 选择��	   Width5  /System.Windows.Controls.DataGridLengthConverter$	 40 1  +System.Windows.Controls.DataGridBoundColumn
   Binding
 �� *
Selected+ �� Mode$ TwoWay=�0  *System.Windows.Controls.DataGridTextColumn  $ 序号��$	 40    
IsReadOnly . 
 �� *Number+  $ 名称��$		 120  . 
 �� *Name+  $ 点号��$	 60  . 
 �� *ID+  $	 值��$		 200  . 
 �� *StringValue+  $ 备注��$		 250  . 
 �� *	Comment+� $��2�� � ColumnDefinitions �� $��* ��� $��2* �.+��� $��0��$ Vertical=�$	��Top=�$��10q�$
��Left=�.+��� $��全选��$��25��$��50�� d� 	IsChecked �� *ComboxInherentChecked+���� *SelectAllInherentCommand+�� $��1��$��10,10,10,10q�.+��� $��读取固有参数��$��25��$	��150��$��
200,5,30,5q�$��InherentParameterRead������ *InherentParameterCommand+