   M S B A M L   `   `   `  ����F  BFTU.Monitor, Version=0.0.0.1, Culture=neutral, PublicKeyToken=nullM+clr-namespace:FTU.Monitor.CheckValueServiceFTU.Monitor.CheckValueService  5      &     FTU.Monitor.View.FileServiceView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      =local+clr-namespace:FTU.Monitor.CheckValueService     5        N� Title$  FileServiceView��5	       � 	Resources 5       �� 
TargetType �� Property �� Value �� CornerRadius �� Orientation ��Name �� ElementName	 �� Path
 J� Effect3  -System.Windows.Media.Effects.DropShadowEffect   Opacity   ShadowDepth   Color��%�  '��       �� 5   
   " ��6   .�5      ��  �@=�ErrorTemplate 5      �� 5      .R�5      �� $ 3��6!   
���  ��$��1q�6D   ._�5      �� $ Vertical=�6)   .+�5   "   �� $ Adorned��6<   6O   �� 5   "   #��z��6,   
���  ����5   &   �� 5   *   $ Adorned��62   $8	 2AdornedElement.(Validation.Errors)[0].ErrorContent�6H   6�   
 5   &     5   *   $	 0.6Q�6;   $ 3Q�6I   $ Black��6Y   5   #   5      5      5      5      5       5!      .��5#      �  � RowDefinitions 5$   
   �� 5%      $��* �6   6)   �� 5&      $��60 �6   6*   �� 5'      $	��120 �6   6+   .+�5*   
   �� $��召唤目录��6   $��0��6!   $��3q�6.   
�������.��5+      �  5,      �� 5-      $��30 �6$   62   �� 5.      $��* �6$   61   .+�50      �� $ 
Horizontal=�6   $��0��66   .+�51      �� $��3q�6   $��2q�6'   .��63   	目录ID:6:   �� 53      $��3q�6   $��2q�6)   $��80��65   $��Center=�6C   .��6^   ��54      �� 55      $	 DirectoryID�6&    �� NotifyOnValidationError .  �� UpdateSourceTrigger$ PropertyChanged=�57   "    �� ValidationRules 58   "   3   -FTU.Monitor.CheckValueService.NumberRangeRule  59   &   
   Min$ 0��6<   
   Max$ 
2147483646��6D   6W   5;      .��5<   (   5=      �� 5>      $��3q�6   $��2q�6'   .��63   
目录名:69   �� 5?      $��3q�6   $��2q�6)   $	��100��65   ��6D   �� *DirectoryName+6e   �� 5@      $��3q�6   $��2q�6'   .��63   起始时间:6:   �� 5A      -   5        $��txtStartTime��5A   D   $��3q�6   $��2q�6)   $	��120��65   ��6X   �� *	StartTime+ �� StringFormat$! {0:yyyy-MM-dd HH:mm:ss.fff}��6�   �� 5B      $��3q�6   $��2q�6'   .��63   结束时间69   �� 5C      -   5        $��
txtEndTime��5C   D   $��3q�6   $��2q�6)   $	��120��65   ��6V   �� *	EndTime+$! {0:yyyy-MM-dd HH:mm:ss.fff}��6�   �� 5D      $��3q�6    $��2q�6+   $��Center=�67    d� 	IsChecked 6R   �� *TimeChecked+.��6t   满足时间段6{   �� 5E      $��3q�6    $��2q�6+   $��Center=�67    6R   �� *DefaultChecked+.��6w   默认目录6}   �� 5F      $��3,3,3,3q�6   $��3q�6.   $��ReadDirectory��6a   ��6:   �� *FileServiceCommand+.��6�   召唤目录6�   5G      &   System.Windows.Controls.DataGrid  5I      -   5          Name$ dg��5I      $��1��6�      AutoGenerateColumns .     CanUserAddRows .     CanUserSortColumns .     AreRowDetailsFrozen .    EnableRowVirtualization .  ��6'   �� *
FileData+ �� SelectedIndex 6H   �� *	GridIndex+   CurrentCell 6m   �� *
CellInfo+ �� Mode$ TwoWay=�   	CellStyle 5J      �� 5K      *  $System.Windows.Controls.DataGridCell"  6    .�5L      ��  � ��$ 0q�6@   �� 5M       � ��$ 85��69     �� Triggers  5N      P� 5O   "   ! P� Property! �@ 
IsSelected" P� Value" . . �5P   &   ��  � ��
 ��ح�6C   �� 5Q   &    � ��
 �����6C   5R   #   5T      #   Columns# 5W      4  .System.Windows.Controls.DataGridCheckBoxColumn  5X      ,  &System.Windows.Controls.DataGridColumn$   Header$$ 选择��61   %   Width5  /System.Windows.Controls.DataGridLengthConverter$
% 0.3* 6=   1  +System.Windows.Controls.DataGridBoundColumn&   Binding& 6J   �� *
Selected+$ TwoWay=�0	  *System.Windows.Controls.DataGridTextColumn	  5Y      $$ 目录ID��6-   $
% 0.3* 6;   '   
IsReadOnly' . & 6H   �� *IDOfDirectory+6j   	  5Z      $$ 序号��6-   $
% 0.3* 69   ' . & 6F   �� *Number+6a   	  5\      $$ 	文件名��6-   $
% 1.5* 6:   ' . & 6G   �� *Name+6`   	  5]      $$ 文件大小��6-   $
% 0.8* 6;   ' . & 6H   �� *Size+6a   	  5_      $$ 文件时间��6-   $% * 6;   ' . & 6E   �� *Time+6^   	  5`      $$ 备注��6-   $% * 69   ' . & 6C   �� *Remark+6^   ( � ContextMenu( 5k      �� 5l      ) �� 	StaysOpen) . .@�5m      m� $��清空��6'   * m� CommandParameter$* Clear��6Z   + m� Command+ 63   �� *FileServiceCommand+6u   5n      5p      5q      5r      �� 5s   
   $��读取文件��6   $��1��6!   $��3q�6.   
�������.��5t      �� $ 
Horizontal=�6   .+�5w      �� $��5,10,3,3q�6   $��全选��6-   $��25��6:   $��50��6F   $��SelectAllCommand��6�    6Q   �� *ComboxChecked+��6u   �� *FileServiceCommand+6�   �� 5x      $��30,3,3,3q�6   $	��120��6+   $��ReadFile��6a   ��6:   �� *FileServiceCommand+.��6}   读取文件6�   �� 5y      $��30,3,3,3q�6   $	��120��6+   $��OpenFolderPath��6a   ��6:   �� *FileServiceCommand+.��6�   打开录波文件目录6�   5z      5{      �� 5|   
   $��	写文件��6   $��2��6    $��3q�6-   
�������.��5}      �� $ 
Horizontal=�6   .+�5~      �  5      �� 5�      $��60 �6(   66   �� 5�      $��60 �6(   66   .+�5�      �� $ 
Horizontal=�6!   $��0��6:   .+�5�      �� $��	3,17,3,17q�6    $��2q�63   .��6?   写文件名:6F   �� 5�      $��	5,17,3,17q�6"   $��2q�65   $	��100��6A   ��6P   �� *WriteFileName+6q   �� 5�      $��
30,17,3,17q�6!   $��
SelectFile��6\   ��65   �� *FileServiceCommand+.��6z   选择文件6�   �� 5�      $��
30,17,3,17q�6!   $��UpdateStart��6\   ��65   �� *FileServiceCommand+.��6{   启动升级6�   �� 5�      $��
30,17,3,17q�6!   $��	WriteFile��6\   ��65   �� *FileServiceCommand+.��6y   	写文件6~   �� 5�      $��
30,17,3,17q�6!   $��UpdateCancel��6\   ��65   �� *FileServiceCommand+.��6|   终止升级6�   �� 5�      $��
30,17,3,17q�6!   $��
UpdateOver��6\   ��65   �� *FileServiceCommand+.��6z   升级结束6�   5�      � 5�      $��1��6   .+�5�      � -   5        $
��pBar��5�   &   , � Minimum$, 0Q�62   - � Maximum$	- 100Q�6>   $��0,0,0,0q�6L   $��50��6^   $
��1000��6j   5�      5�      5�      5�      5�      5�      