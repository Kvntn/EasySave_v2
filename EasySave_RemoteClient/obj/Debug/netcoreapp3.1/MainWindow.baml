   M S B A M L   `   `   `  ����P  LEasySave_RemoteClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:EasySave_RemoteClientEasySave_RemoteClient  5   	   &     EasySave_RemoteClient.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   5local#clr-namespace:EasySave_RemoteClient     5   	     � 
ResizeMode$  NoResize=�5   	    � Title$* $EasySave - For your cleanest backups��5	   	   $	��466��66   $
��1000��6C   .��5
      � $��0,3,0,0q�6    � ColumnDefinitions 5   
   �� 5      $
��269* �6   �� 5      $
��231* �6   .+�5   
   �� -   5         ��Name$ 
outputSave��5      $
��Left=�6(   $��30��6C   $��
99,357,0,0q�6O   $
��Wrap=�6c   $	��Top=�6w   $	��268��6�   $��1��6�   �� 5   
   -   5         ��Name$ listbox_backup��5      $
��Left=�6J   $	��311��6e   $��
47,101,0,0q�6r   $	��Top=�6�   $	��311��6�    �� SelectionMode$ Multiple=�6�   $ �� IsSynchronizedWithCurrentItem$ FalseY�6�    � DataContext$  ��6�   $��Verdana)�6  $��14(�6#  ��6*   �� *	StrList+��6�   Z� 6#  �� 5   
   -   5        $ txt_Use_Copy��5      $
��Left=�6*   $��61��6E   $
��Wrap=�6Q   $	��Top=�6e   $	��172��6}   $��Verdana)�6�   $��20(�6�   $��Italic$�6�   $��
271,14,0,0q�6�    �� TextAlignment$ Center=�6�   �� 6�   $�Connect��6�   �� 6   $� ��6  �� 6  $	�to ��6  �� 6   $�remote server :��6$  6>  �� 5   
   -   5        	 ��Name$	 input_ip��5      $
��Left=�6$   $��29��6?   $��
456,22,0,0q�6K   
 �� TextWrapping$

 Wrap=�6_   $	��Top=�6s   $	��207��6�   4  .System.Windows.Automation.AutomationProperties   IsRequiredForForm$
 True��6�   $��18(�6�   $��Verdana)�6�   $��2��6�   �� 5   
   -   5         ��Name$ button_Connect��5      $��Connect��6)   $
��Left=�6;   $��25��6V   $	��Top=�6b   $��71��6z   $��
140,24,0,0q�6�   $��1��6�   �� 5   
   $
��Wrap=�6   $��Verdana)�6(   $��36(�6=   $��Italic$�6K    J� RenderTransformOrigin$ 0.5,0.5&�6^   $��	0,8,311,0q�6~   $��57��6�   $	��Top=�6�   $��Right=�6�   $	��190��6�   z�5      V� 5      .~�5      �� �� 5       �� AngleX$ -0.929Q�6$   T� 5       T� X$ 0.983Q�6)    T� Y$ 1.445Q�63   5      �� 5   *   $�EasySave��6.   �� 6@   �� 6L   6S   �� 5   
   $
��Wrap=�6   $��Verdana)�6(   $��Italic$�6=   $��
0,59,300,0q�6P   $��32��6d   $	��Top=�6p   $��Right=�6�   $	��200��6�   �� 6�    � Language$ fr-fr�6�   $ �For your cleanest backups.��6�   6�   �� 5   
   -   5        $ 
Button_Add��5      $	��Add��6%   $
��Left=�63   $��68��6N   $	��Top=�6Z   $��46��6r   $��376,223,0,0q�6}   6�   &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ dataGrid��5         
IsReadOnly$
 true��6B    �� AlternationCount$ 2��6U      AutoGenerateColumns$ False��6l   $��433,100,128,37q�6�   $��2��6�   ��6%   �� *grid+   Columns 5!      0  *System.Windows.Controls.DataGridTextColumn  5#      1  +System.Windows.Controls.DataGridBoundColumn   Binding 6%   �� *Name+,  &System.Windows.Controls.DataGridColumn   Header 5$      �� 5%      -   5        $
 Name��5%   $   $.��(Name                                    ��62   5'        5)       6%   �� *	Percent+ 5*      �� 5+      -	   5        $ Percent��5+   $   $��%��65   5-      4  .System.Windows.Controls.DataGridTemplateColumn  51       52      �� 53      $��Play ��6$      CellTemplate 55      �� 56      .R�57      �� -
   5        $
��Play��57   %   
 � Tag 6H   �� *Name+58      5:        5;       5<      �� 5=      $��Pause��6$    5?      �� 5@      .R�5A      �� -   5        $��Pause��5A   %    6I   �� *Name+5B      5D        5F       5G      �� 5H      $
��Stop��6$    5J      �� 5K      .R�5L      �� -   5        $
��Stop��5L   %    6G   �� *Name+5M      5O      5R      �� 5S   
   -   5        $ Button_Start��5S      $�� ��6'   $��1��62   $
��Left=�6B   $��40��6]   $	��Top=�6i   $��39��6�   $��378,181,0,0q�6�   $��0,0,0,0q�6�   ��5T      �� 5U       �� ImageSource$ /res/play.png��6   5W      �� 5X   
   -   5        $ Button_Pause��5X      $�� ��6'   $��1��62   $
��Left=�6B   $��40��6]   $	��Top=�6i   $��40��6�   $��376,241,0,0q�6�   $��0,0,0,0q�6�   $
}�True��6�   ��5Y      �� 5Z      $ /res/pause.png��6   5\      �� 5]   
   -   5        $ Button_Stop��5]      $�� ��6&   $��1��61   $
��Left=�6A   $��39��6\   $	��Top=�6h   $��39��6�   $��377,301,0,0q�6�   $��0,0,0,0q�6�    J� SnapsToDevicePixels$
 True��6�   ��5^      �� 5_      $ /res/square.png��6   5a      �� 5b   
   -   5        $ button_Disconnect��5b      $��
Disconnect��6,   $
��Left=�6A   $��25��6\   $	��Top=�6h   $��70��6�   $��
216,24,0,0q�6�   $��1��6�   5d      5e      