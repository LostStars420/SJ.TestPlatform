   M S B A M L   `   `   `  ����   FTU.Monitor=    7FTU.Monitor.View.CombineTelesignalisationManagementView   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008       N� Title$,  &CombineTelesignalisationManagementView��.���  � ColumnDefinitions �� $��2* ��� $��* �.+��� $��组合遥信列表��$��1��$��3q����	#FFF0F0F0.���  � RowDefinitions �� $��* ��� $	��12* �.+�� $��0��.+��� $��3q� �� Orientation$ 
Horizontal=�$��Right=�.+��� $��60��$��删除������ *'%DeleteCombineTelesignalisationCommand+� $��1��.+�&   System.Windows.Controls.DataGrid  $��5q�   AutoGenerateColumns .     CanUserAddRows .     CanUserSortColumns .   �� SelectedIndex �� *#!CombineTelesignalisationGridIndex+���� *CombineTelesignalisationList+   Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn	   Header$	 序号��
   Width5  /System.Windows.Controls.DataGridLengthConverter$
 *    
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *Number+  $	 遥信名称��$

 1.5*  .  �� *Name+  $	 遥信ID��$

 1.1*  .  �� *ID+ �� StringFormat$ {0:X4}��  $	 内容��$

 auto  .  �� *	Comment+�� $��组合遥信��$��0��$��3q�$��Stretch=����	#FFF0F0F0.���  �� $
��1.5* ��� $��* �.+��� $��遥信原始点表��$��3q�$��0��$��Stretch=�
��� ���.���  �� $
��8.5* ��� $��* �.+�  $��0��$��5q� .   .   .   �� *	GridIndex+���� *TelesignalisationSourceList+   $	 序号��$
 *  .  �� *Number+  $	 遥信名称��$
 3*  .  �� *Name+  $	 遥信ID��$

 1.2*  .  �� *ID+$ {0:X4}��  $	 备注��$
 4*  .  �� *	Comment+�� $��1��$��选择��$��25��$��5q����� *%#SelectTelesignalisationPointCommand+�� $��新增组合遥信��$��3q�$��1��
��� ���.���� $ Vertical=�.+��� $ 
Horizontal=�.+��� $��逻辑运算符：��$��25��$��20��$��5q��� $	��与��$��20��$��30��$��5q�$	��AND������ *LogicOperationCommand+�� $	��或��$��20��$��30��$��5q�$��OR������ *LogicOperationCommand+�� $	��非��$��20��$��30��$��5q�$	��NOT������ *LogicOperationCommand+�� $��5q�$��50�� �� AcceptsReturn .  �� TextWrapping$
 Wrap=����� *!CombineTelesignalisationContent+�� $ 
Horizontal=�.+��� $��	名称：��$��25��$��20��$��5q��� $��5q�$��80������ *CombineTelesignalisationName+�� $��保存��$��20��$��30��$��5q����� *%#SaveCombineTelesignalisationCommand+�� $��10q�
����  �这是说明：�� 组合遥信内容说明.