   M S B A M L   `   `   `  ����  WPF_ATM    WPF_ATM.DepositWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$#  Внесення коштів��5   	   $	��200��6!   $	��400��6.   .��5      � $��10q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5	      $
��Auto �6   .+�5   
   �� $:��4Введіть суму для поповнення:��6   $��14(�5      $��Center=�6"   �� 5   
   -   5         ��Name$ DepositAmountTextBox��5      $��1��5      $��	0,10,0,10q�6   $��14(�5      $��Center=�5      $��Stretch=�5      �� 5   
    �� Orientation$ 
Horizontal=�6   $��Right=�5      $��2��5      $��0,10,0,0q�6"   .+�5      �� -   5         ��Name$ DepositConfirmButton��5      $��Підтвердити��5      $	��100��5      $��0,0,10,0q�5      5      �� 5      -   5        $ CancelButton��5      $��Скасувати��5      $	��100��5      5       5!      5"      5#      