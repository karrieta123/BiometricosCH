   M S B A M L   `   `   `  ����   Saraff.Twain.Wpf.Sample2+    %Saraff.Twain.Wpf.Sample2.TwainSample2   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     -     � Title$  Captura de Documentos��$	��545��$	��719��$	��483��$	��300��
������� � 	Resources    TwainSources  Resolutions  
PixelTypes  XferMech  ImageFileFormats �� 
TargetType �� Property �� Value �� RelativeSource �� AncestorType �� Path �� 
TargetType	 � LayoutTransform
 �� Triggers :� RoutedEvent �� 
TargetName �� TargetProperty	 W� To g� Duration �� Triggers �� Value �� KeyTime T� Y J� RenderTransformOrigin'  !System.Windows.VisualStateManager   VisualStateGroups%  System.Windows.VisualStateGroup   Name   States   System.Windows.VisualState   Name   
Storyboard	 �� To A�Name � Offset � Color��%�
  &	        &	      &	 
     &	      &	      '��      '�� �     '9� �    '	�     '�� '    �� �� �� �� �� �� " ��.���  � �� ��  �� *FindAncestor+" �$ 
Background���  � ��$ Disabled=���  � ��
 ���� ��  � ��$ 1,0,1,1q��� " ��.���  � �� ��  �� *FindAncestor+" �$ 
Foreground���  �@�ToolTip ��  �� *Self+$ Content���  �@^�InitialShowDelay$
 1000����  �@^�ShowDuration$
 2000����  � �� �� " ��.R�� $��Stretch=�$��Stretch=�	 �� scaleTransform��.+��� #��z����. ���� $ Content� �� *TemplatedParent+
 :� $ 
MouseEnter��[��� .���� .��W� $ scaleTransform��$ ScaleX�$	 1.2X�$ 0:0:0.1D�W� $ scaleTransform��$ ScaleY�$	 1.2X�$ 0:0:0.1D��� " 9�.���  � �� ��  �� *FindAncestor+" �$ 
Foreground� :� $ 
MouseEnter��[��� .���� .���� $ 
IsExpanded�$ 0:0:0D�.p�s�  . $ 0:0:0��:� $ 
MouseLeave��[��� .���� .���� $ 
IsExpanded�$ 0:0:0D�.p�s�  .  $ 0:0:0���� " 	�.���  � z� T� $ 50Q���  � �� �� " 	�.R�� $��Center=�$��Center=�$ .5,.5&�z�V� .~��� scaleTransform��   $ CommonStates��.   $ 	MouseOver��. �� .���� $ 
StopColor1��$ Color�$ DarkBlueX�$ 0:0:.2D��� $ 
StopColor2��$ Color�$
 BlueX�$ 0:0:.2D�  $ 	Unchecked��  $ Indeterminate��  $ Checked��. �� .���� $ 
StopColor1��$ Color�$
 BlueX�$ 0:0:.2D��� $ 
StopColor2��$ Color�$ DeepSkyBlueX�$ 0:0:.2D�W� $ scaleTransform��$ ScaleX�$ .9X�$ 0D�W� $ scaleTransform��$ ScaleY�$ .9X�$ 0D�.+�A� $	 but����
� .��� 
StopColor1��$ 0Q�$ Wheat��� 
StopColor2��$ 1Q�$ White��(� .���� ��. #��z��#��z���� " ��.���  � �� �� " ��.R�� $��Center=�$��Center=�$ .5,.5&�	 �� scaleTransform��   $ CommonStates��.   $ Normal��  $ 	MouseOver��. �� .���� $ 
StopColor1��$ Color�$ WhiteX�$ 0:0:.2D��� $ 
StopColor2��$ Color�$ WhiteX�$ 0:0:.2D�  $ Pressed��. �� .���� $ 
StopColor1��$ Color�$ DeepSkyBlueX�$ 0:0:.2D��� $ 
StopColor2��$ Color�$	 RedX�$ 0:0:.2D�W� $ scaleTransform��$ ScaleX�$ .5X�$ 0D�W� $ scaleTransform��$ ScaleY�$ .5X�$ 0D�  $ Disabled��. �� $ Color�.���� $ 
StopColor1��$
 GrayX�$ 0:0:.2D��� $ 
StopColor2��$ 	#FF962323X�$ 0:0:.2D�.+�A� ��
� .��� 
StopColor1��$	 0.7Q�$ 	RoyalBlue��� 
StopColor2��$ 1Q�$ White���� ��. #��z��#��z��.��� ����  �� *TemplatedParent+$ 
Background� � RowDefinitions �� $	��35* ��� $	��40* ��� $	��60* ��� $
��245* � � ColumnDefinitions �� $
��285* �$	��430���� $��30 ��� $��80 ��� $��75 ��� $��90 ��� $��65 ��� $��* ��� $��50 �.+�9� -   $��dataSourcesExpander��$	��Top=�$	��Top=�  K� ZIndex$  1��$��3������ .R��� $��CharacterEllipsis=����� ! �� Source#! [  $ CurrentItem�.���� -   $��listbox������ #! [  �� $��0��$
��Left=�$��1��$��5,0,0,0q�.+��� .��Lista de Documentos�� -   $��cmbdocumentos��$	��130��$��25������ .R��� .+��� ���� $ NOMBRE��� $��0��$
��Left=�$��3��$��	5,-90,0,0q�.+��� -   $��acquireButton��$��50��$	��100��$  1��$��7q�.���� $=��7/Saraff.Twain.Wpf.Sample2;component/Images/scanner2.png���� -   " ��Name$
" list��$	��150��$	��180��$��5,0,0,0q�
���   �$��1q�� $��7��$��1��$��3������  �� *TemplatedParent+$ 
Background� �� $��6* ��� $��* �$��41��# � Triggers# :� $ Grid.MouseEnter��[��� .���� .��W� $ byWidthButton��$ RenderTransform.Y�$	 -20X�$ 0:0:.3D�W� $ byHeightButton��$ RenderTransform.Y�$	 -20X�$ 0:0:.3D�$ g� 	BeginTime$$ 0:0:.08X�W� $ defaultButton��$ RenderTransform.Y�$	 -20X�$ 0:0:.3D�$$ 0:0:.16X�W� $ bySizeButton��$ RenderTransform.Y�$	 -20X�$ 0:0:.3D�$$ 0:0:.24X�:� $ Grid.MouseLeave��[��� .���� .��W� $ byWidthButton��$ RenderTransform.Y�$ 50X�$ 0:0:.3D�W� $ byHeightButton��$ RenderTransform.Y�$ 50X�$ 0:0:.3D�$$ 0:0:0.08X�W� $ defaultButton��$ RenderTransform.Y�$ 50X�$ 0:0:.3D�$$ 0:0:0.16X�W� $ bySizeButton��$ RenderTransform.Y�$ 50X�$ 0:0:.3D�$$ 0:0:0.24X�.+��� -   $��scrol��$��Disabled=�$��Disabled=�$��2��$��
237,0,53,0q�����  �� *TemplatedParent+$ 
Background�.���� -   $��	scanImage��$
��Fill=�$��30,0,0,0q�� $��1�� �� �� $��50 ��� $��50 ��� $��50 ��� $��50 ��� .+�	� -	   $��byWidthButton��$��1��.���� $:��4/Saraff.Twain.Wpf.Sample2;component/Images/width.png��	� -
   $��byHeightButton��$��2��.���� $;��5/Saraff.Twain.Wpf.Sample2;component/Images/height.png��	� -   $��defaultButton��$��3��.���� $;��5/Saraff.Twain.Wpf.Sample2;component/Images/100%25.png��	� -   $��bySizeButton��$��4��% d� 	IsChecked$
% TrueY�.���� $;��5/Saraff.Twain.Wpf.Sample2;component/Images/bySize.png���� -   $��152,204,0,193q�$
��Left=�$��50��$��30��$ 1.485,0.449&�.���� $6��0/Saraff.Twain.Wpf.Sample2;component/Images/-.png��$��20��$��20��$ -0.193,0.393&��� -   $��	Finalizar��$
��Left=�$��
47,340,0,0q�$	��Top=�$��75��