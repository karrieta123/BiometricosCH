   M S B A M L   `   `   `  ����   PUE     PUE_UI.Views.Login.ucLogin   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     $	��260��
�������  � 	Resources   �� MergedDictionaries �� Source��  �� $. (../../Content/Styles/Login/dicLogin.xaml?��� $, &../../Content/Styles/Home/dicHome.xaml?�%    .���� $��
2, 2, 2, 2q�$	��257��
�������$
��Left=�$	��260������  �� Color$ 	LightGray�� �� Opacity$ 1Q�._�� ���#333333 � RowDefinitions �� $��30 ��� $	��120 ��� $��50 ��� $
��auto ��� $
��auto ��� $��25 � � ColumnDefinitions �� $	��200 ��� $��63 �.+��� 
�������
�������$��2q�$��2��$��
31,5,32,85q�$��1���� $��0��$��0�� J� Opacity$	 0.9Q��� $��0��$��1��$	 0.9Q�.+��� -    ��Name$ imgCloseWindow��   styleMinimize_Close#��[  $(��"../../Content/Images/All/close.png��	 � ToolTip$	 Cerrar��$��	0,10,26,0q��� $��1��$��2�����#333333�� $��1��$��2��$��	0,17,0,10q�.+��� -   $��
oLabelName��  styleAuthenticationError#��[ 
����������#333333$
��10px(�$
��Bold"�$��0 0 0 0q�$�� ��
 �� 
IsReadOnly
 . |�. �� $��1��$��2��$��	0,47,0,23q�.+���   styleLblUserAuthentication#��[ $��20,0,0,0q�$
��Left=�$	��243�� +� FocusedElement ��  �� ElementName$ txtUser��.��
Usuario:�� -   $��txtUser��  styleTxtUserAuthentication#��[ |�. �� $��3��$��2���� $��2��$��0,5,0,25q�$��2��$��4�����#333333.+��� #��[ $��Contraseña:��$��Right=�$��0q�$	��243��G� -    G�Name$ psbPasswordUser��  stylePswUserAuthentication#��[ $��
0, 0, 0, 5q��� $��5��$��0��$��
0,12,0,-20q����#333333.+��� -    ��Name$ lblLoginError��  styleInfoFooter#��[ $��	0,0,-57,0q�.���� -    ��Name$ btnLogin��  LoginButton#��[ $��100px��$��	140,0,5,0q� J� RenderTransformOrigin$ 0.524,1.538&�$��Login��