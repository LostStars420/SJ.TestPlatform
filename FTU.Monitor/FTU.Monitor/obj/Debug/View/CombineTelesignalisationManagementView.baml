   M S B A M L   `   `   `  ����  FTU.Monitor=    7FTU.Monitor.View.CombineTelesignalisationManagementView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5        N� Title$,  &CombineTelesignalisationManagementView��5      .��5
      �  � ColumnDefinitions 5   
   �� 5      $��2* �6   �� 5      $��* �6   .+�5   
   �� $��组合遥信列表��6   $��1��6#   $��3q�63   ���	#FFF0F0F0.��5      �  � RowDefinitions 5      �� 5      $��* �6$   61   �� 5      $	��12* �6$   63   .+�5      � $��0��6   .+�5      �� $��3q�6!    �� Orientation$ 
Horizontal=�6,   $��Right=�6E   .+�5      �� $��60��6!   $��删除��6/   ��6<   �� *'%DeleteCombineTelesignalisationCommand+6x   5      5      � 5      $��1��6   .+�5      &   System.Windows.Controls.DataGrid  $��5q�6       AutoGenerateColumns .     CanUserAddRows .     CanUserSortColumns .   �� SelectedIndex 6+   �� *#!CombineTelesignalisationGridIndex+��6g   �� *CombineTelesignalisationList+   Columns 5!      0  *System.Windows.Controls.DataGridTextColumn  5"      ,  &System.Windows.Controls.DataGridColumn	   Header$	 序号��61   
   Width5  /System.Windows.Controls.DataGridLengthConverter$
 * 6>      
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn   Binding 6H   �� *Number+6c     5#      $	 遥信名称��61   $

 1.5* 6?    .  6L   �� *Name+6e     5$      $	 遥信ID��61   $

 1.1* 6?    .  6L   �� *ID+ �� StringFormat$ {0:X4}��6z     5%      $	 内容��61   $

 auto 6=    .  6J   �� *	Comment+6f   5(      5*      5+      5,      �� 5.   
   $��组合遥信��6   $��0��6!   $��3q�61   $��Stretch=�6<   ���	#FFF0F0F0.��5/      �  50      �� 51      $
��1.5* �6'   �� 52      $��* �6'   .+�55      �� $��遥信原始点表��6   $��3q�6+   $��0��66   $��Stretch=�6F   
��� ���.��56      �  58      �� 59      $
��8.5* �6/   �� 5:      $��* �6/   .+�5=        $��0��6#   $��5q�63    .   .   .   6>   �� *	GridIndex+��6b   �� *TelesignalisationSourceList+ 5>        5?   "   $	 序号��65   $
 * 6B    .  6L   �� *Number+6g     5@   "   $	 遥信名称��65   $
 3* 6C    .  6N   �� *Name+6g     5A   "   $	 遥信ID��65   $

 1.2* 6C    .  6P   �� *ID+$ {0:X4}��6~     5B   "   $	 备注��65   $
 4* 6A    .  6L   �� *	Comment+6h   5D      �� 5E      $��1��6!   $��选择��61   $��25��6>   $��5q�6J   ��6U   �� *%#SelectTelesignalisationPointCommand+6�   5G      5H      �� 5J      $��新增组合遥信��6   $��3q�6+   $��1��66   
��� ���.��5K      �� $ Vertical=�6!   .+�5M      �� $ 
Horizontal=�6%   .+�5N      �� $��逻辑运算符：��6$   $��25��65   $��20��6A   $��5q�6O   �� 5O      $	��与��6%   $��20��61   $��30��6=   $��5q�6I   $	��AND��6~   ��6T   �� *LogicOperationCommand+6�   �� 5P      $	��或��6%   $��20��61   $��30��6=   $��5q�6I   $��OR��6~   ��6T   �� *LogicOperationCommand+6�   �� 5Q      $	��非��6%   $��20��61   $��30��6=   $��5q�6I   $	��NOT��6~   ��6T   �� *LogicOperationCommand+6�   5R      �� 5T      $��5q�6"   $��50��6-    �� AcceptsReturn .  �� TextWrapping$
 Wrap=�6   ��69   �� *!CombineTelesignalisationContent+6�   �� 5V      $ 
Horizontal=�6%   .+�5W      �� $��	名称：��6$   $��25��62   $��20��6>   $��5q�6L   �� 5X      $��5q�6&   $��80��61   ��6<   �� *CombineTelesignalisationName+6m   �� 5Y      $��保存��6%   $��20��62   $��30��6>   $��5q�6J   ��6U   �� *%#SaveCombineTelesignalisationCommand+6�   5Z      �� 5\      $��10q�6$   
����  �这是说明：�� 5]   '   组合遥信内容说明.5_      5a      5b      5d      5e      5g      5i      