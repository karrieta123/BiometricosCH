   M S B A M L   `   `   `  ����  PUE     PUE_UI.Views.Login.ucLogin   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      $	��260��5      
�������  � 	Resources  5	       �� MergedDictionaries �� Source�� 5
   
    5      �� 5      $. (../../Content/Styles/Login/dicLogin.xaml?�6%   �� 5      $, &../../Content/Styles/Home/dicHome.xaml?�6%   %    5      .��5      �� $��
2, 2, 2, 2q�6   $	��257��6*   
�������$
��Left=�6N   $	��260��6i   ��5   
   �� 5       �� Color$ 	LightGray��6    �� Opacity$ 1Q�60   ._�5   
   � ���#333333 � RowDefinitions 5      �� 5      $��30 �6    �� 5      $	��120 �6    �� 5      $��50 �6    �� 5      $
��auto �6    �� 5      $
��auto �6    �� 5      $��25 �6     � ColumnDefinitions 5!      �� 5"      $	��200 �6#   �� 5#      $��63 �6#   .+�5&      �� 
�������
�������$��2q�6?   $��2��6S   $��
31,5,32,85q�6g   $��1��6{   5(      �� 5*      $��0��6   $��0��6&    J� Opacity$	 0.9Q�66   5,      �� 5.      $��0��6   $��1��6&   $	 0.9Q�66   .+�5/      �� -   5         ��Name$ imgCloseWindow��5/         styleMinimize_Close#��[  60   $(��"../../Content/Images/All/close.png��6i   	 � ToolTip$	 Cerrar��6�   $��	0,10,26,0q�6�   50      �� 52      $��1��6   $��2��6&   ���#3333336:   �� 54      $��1��6   $��2��6&   $��	0,17,0,10q�6:   .+�55      �� -   5        $��
oLabelName��55        styleAuthenticationError#��[ 6,   
����������#333333$
��10px(�6�   $
��Bold"�6�   $��0 0 0 0q�6�   $�� ��6�   
 �� 
IsReadOnly
 . |�. 6�   56      �� 58      $��1��6   $��2��6&   $��	0,47,0,23q�6:   .+�59      ��   styleLblUserAuthentication#��[ 6   $��20,0,0,0q�6�   $
��Left=�6�   $	��243��6�    +� FocusedElement 6X   ��  �� ElementName$ txtUser��.��6�   
Usuario:6�   �� 5:      -   5        $��txtUser��5:        styleTxtUserAuthentication#��[ 6)   |�. 6�   5;      �� 5=      $��3��6   $��2��6&   �� 5A      $��2��6   $��0,5,0,25q�6-   $��2��6?   $��4��6L   ���#333333.+�5B      �� #��[ 6   $��Contraseña:��6X   $��Right=�6n   $��0q�6�   $	��243��6�   G� 5C      -   5         G�Name$ psbPasswordUser��5C        stylePswUserAuthentication#��[ 67   $��
0, 0, 0, 5q�6w   6�   5D      �� 5F      $��5��6   $��0��6&   $��
0,12,0,-20q�66   ���#333333.+�5G      �� -   5         ��Name$ lblLoginError��5G        styleInfoFooter#��[ 6/   $��	0,0,-57,0q�6d   .��5H      �� -   5         ��Name$ btnLogin��5H        LoginButton#��[ 6/   $��100px��6`   $��	140,0,5,0q�6n    J� RenderTransformOrigin$ 0.524,1.538&�6�   $��Login��6�   6�   5I      5J      5L      5M      5N      