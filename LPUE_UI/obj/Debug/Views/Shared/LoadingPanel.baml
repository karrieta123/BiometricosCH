   M S B A M L   `   `   `  ����>  :PUE, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:PUE.Views.SharedPUE.Views.Shared  5      d `PresentationFramework.Classic, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35jMclr-namespace:Microsoft.Windows.Themes;assembly=PresentationFramework.ClassicMicrosoft.Windows.Themes 5      #    PUE.Views.Shared.LoadingPanel   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      .ctrclr-namespace:PUE.Views.Shared     5      fclassicThemeMclr-namespace:Microsoft.Windows.Themes;assembly=PresentationFramework.Classic     5      -   5        $��90��5        J� Opacity$	  0.9Q�5	       � 	Resources 5         panelBackground �� EndPoint �� 
StartPoint � Color � Offset  basicButton �� 
TargetType �� Property �� Value5  /Microsoft.Windows.Themes.ClassicBorderDecorator	   ClassicBorderBrush
 �� 
TargetType  Name   BorderBrush   BorderThickness   BorderStyle   
Background J� SnapsToDevicePixels �� Triggers P� Property P� Value �� 
TargetName  closeButton �� BasedOn  panelMessageText  panelSubMessageText��%�	  &	        &	 �     &	 A    7  &	 �    &	     �� 5   
   $ 0.5,1&�66   $ 0.5,0&�6G   .��5      � $ Black��6   $ 1Q�6)   � 5      $ 	#FF1B1B1B��6   5      �� 5   
   " ��6$   .�5      ��  � ��# � ��6+   �� 5       � ��# � ��6+   �� 5       � ��# Z	 6,   �� 5       � ��$ 3q�60   �� 5       � ��$ Center=�6;   �� 5       � ��$ Center=�69   �� 5       � ��$ 0,0,1,1q�6(   �� 5       � �� 5      �� 5      "
 ��6&   .R�5        $ ContentContainer��6>   # z��6X   $ 0q�6�   $
 None=�6�   # z��6�    . ._�5      �� #��z��6/   #��z��6r   ��. # z 6�   #��z��6�   5       5       P� 5!       �@J�IsKeyboardFocused . . �5"   "   �� $ ContentContainer��6@    �@ BorderStyle$ RaisedFocused=�6^   5#      P� 5$       �@��IsDefaulted . . �5%   "   �� $ ContentContainer��6@    �@ BorderStyle$ RaisedFocused=�6^   5&      P� 5'       � �� . . �5(   "   �� $ ContentContainer��6@    �@ BorderStyle$ RaisedPressed=�6^   5)      P� 5*       �@d�	IsChecked$
 TrueY�6H   . �5+   "   �� $ ContentContainer��6@    �@ BorderStyle$ RaisedPressed=�6^   5,      P� 5-       � {� .  . �5.   "   �� # � ��6?    � ��5/      51      53      54      �� 56   
   8   " ��6K   .�57      ��  � ��$	 Top=�62   �� 58       � ��$ Verdana)�6+   �� 59       � ��$ 9(�6)   �� 5:       � ��$
 Bold"�6+   �� 5;       � ��
 �����6+   �� 5<       �@�Cursor$
 Hand��6'   �� 5=       � ��$
 15,7q�6'   �� 5>       � �� 6+   Z� 5?      �� 5A   
   " ��6)   .�5B      ��  � ��$
 Left=�64   �� 5C       � ��$ 0,11,0,0q�6'   �� 5D       � ��$ WrapWithOverflow=�6-   �� 5E       � ��$
 Bold"�6+   �� 5F       � ��$ 16(�6)   �� 5G       � ��$	 Top=�62   �� 5H       � ��$ Arial)�6+   �� 5I       �@J�SnapsToDevicePixels . 64   5J      �� 5L   
   " ��6,   .�5M      ��  � ��$
 Left=�64   �� 5N       � ��$	 0,5q�6'   �� 5O       � ��$
 Wrap=�6-   �� 5P       � ��$ 12(�6)   �� 5Q       � ��$	 Top=�62   �� 5R       � ��$ Arial)�6+   �� 5S       �@J�SnapsToDevicePixels . 64   5T      5U      .��5W      ]� #��[  6   .+�5X   
   F� ����    `���
ט`��	$��1��62   $��
0,0.25,0,0q�6=    �� Stretch$
 Fill=�6Q   $	��Top=�6`   
���   �6u   F� 5Y   
   ����    `���
ט`��	$��1��62   $��
0,0.25,0,0q�6=   $
 Fill=�6Q   $	��Top=�6`   ���	#FF2F2F2F6u   �� 5[   
   -   5         ��Name$ btnClose��5[      $��Right=�5\      #��[ 5]       � ToolTip$ Close��5^      $��Cerrar X��5`      *   $PUE.Views.Shared.CircularProgressBar  5b   
   -   5          Name$ progressBar��5b   "   $
��Left=�5c      $
��Left=�5d      $	��Top=�5e      $��45��5f      $��18,19q�5g      �� 5i   
   $	��Top=�6   $��
9, 0, 0, 0q�6-   .+�5j      �� -   5         ��Name$ 
txtMessage��5j      #��[ 5k      ���#016EBF$	  0.7Q�5m      �� 5o      -   5        $ txtSubMessage��5o      #��[ 5p      
�������$��
3, 5, 0, 0q�5r      5s      5t      5u      