   M S B A M L   `   `   `  ����   	Zubrilka2    Zubrilka2.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     -     � Title$  
Zubrilka 2��$	��800��$	��600�� � 	Resources    	TextStyle �� Property �� Value �� Path��%7   &	        �� .���  � �� �� $ FontSize�.���  � RowDefinitions �� $��* ��� $
��auto �.+��� $��0��$��Stretch=�.@��� $��Тестирование��.���  � ShowGridLines .   �� $
��auto ��� $��* ��� $��32 �.+��� -    ��Name$ 
spQuestion�� �� Orientation$ Vertical=�$��0��.+��� 
�������.��	Тема:�� #���   $��WrapWithOverflow=�
����������� $ QuestionView.Topic��� 
�������.��Документ:�� #���   $��WrapWithOverflow=�
����������� $ QuestionView.Doc��� 
�������.��Вопрос:�� #���   $��WrapWithOverflow=����� $ QuestionView.Text��� -   $��lAnswerOptions��
�������.��&$Варианты ответа(ов):�� $��1��.��&   System.Windows.Controls.DataGrid  -   	  Name$	 	dgAnswers��
   AutoGenerateColumns
 .  
������    RowBackground
 ����    GridLinesVisibility$
 None=�   HeadersVisibility$
 None=�$ �� IsSynchronizedWithCurrentItem$ FalseY�
������    SelectionUnit$ FullRow=�   SelectionMode$ Single=�����  �� ElementName$ 
spQuestion��$ Width�   	CellStyle ��  �� 
TargetType*  $System.Windows.Controls.DataGridCell"   �� Triggers P�  P� Property �@ 
IsSelected P� Value . . ���  � ��
 ���� ��  � ��
 �   ���  � ��$ 1q���  � ��
 ����    Columns 4  .System.Windows.Controls.DataGridTemplateColumn  ,  &System.Windows.Controls.DataGridColumn   Width5  /System.Windows.Controls.DataGridLengthConverter$ 30    CellTemplate �� .R��� $��10q�$��	14,14,0,0q�$��Center=� d� 	IsChecked �� $ Checked� �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�  $ *  �� .R��� -    �� Padding$ 10q�$��WrapWithOverflow=����� $ Color����� $ 
slFontSize��$ Value����� $
 Text��� $ 
Horizontal=�$��Right=�$��2��.+��� -   $��lResult��$��16(�$��	ExtraBold"�.��Результат�� -   $��	btExclude��$��3q�.��Исключить�� -   $��btCheck��$��3q�.��Проверить�� -	   $��btNext��$��3q�.��Следующий>�� $��Статистика��.��  -
   $��dgStatistic��
 .  $
 Cell=�   
IsReadOnly . $
 None=�   FrozenColumnCount$ 1��   $ 60  �� .R��� ���� $ Title�$ OneWay=�  $ 60     Header$  0�� �� .R��� $��Center=����� $ Questions[0]�$ OneWay=����� $ Questions[0].Color�$ OneWay=�  $ 60 $  1�� �� .R��� $��Center=����� $ Questions[1]�$ OneWay=����� $ Questions[1].Color�$ OneWay=�  $ 60 $  2�� �� .R��� $��Center=����� $ Questions[2]�$ OneWay=����� $ Questions[2].Color�$ OneWay=�  $ 60 $  3�� �� .R��� $��Center=����� $ Questions[3]�$ OneWay=����� $ Questions[3].Color�$ OneWay=�  $ 60 $  4�� �� .R��� $��Center=����� $ Questions[4]�$ OneWay=����� $ Questions[4].Color�$ OneWay=�  $ 60 $  5�� �� .R��� $��Center=����� $ Questions[5]�$ OneWay=����� $ Questions[5].Color�$ OneWay=�  $ 60 $  6�� �� .R��� $��Center=����� $ Questions[6]�$ OneWay=����� $ Questions[6].Color�$ OneWay=�  $ 60 $  7�� �� .R��� $��Center=����� $ Questions[7]�$ OneWay=����� $ Questions[7].Color�$ OneWay=�  $ 60 $  8�� �� .R��� $��Center=����� $ Questions[8]�$ OneWay=����� $ Questions[8].Color�$ OneWay=�  $ 60 $  9�� �� .R��� $��Center=����� $ Questions[9]�$ OneWay=����� $ Questions[9].Color�$ OneWay=��� $��Ввод данных���� $��Настройки��.���� $ Vertical=�.+��� $,��&Размер шрифта текста��.���� $ Vertical=�$��Center=�.+��� $ 
Horizontal=�$��Center=�$��Center=�.+��� $��8(�$��Center=�.��
Орёл�� -   $��
slFontSize��$	��200��$��Center=�! � Minimum$! 8Q�" � Maximum$" 36Q�# � Value# �� $ FontSize�$ TwoWay=�$ PropertyChanged=��� $��36(�.��Курица�� #���   $��Center=�.��KIСъешь ещё этих мягких французских булок�� $ 
Horizontal=�$��Right=�.+��� -   $��5q�.��'%Очистить статистику�� -   $��cbButtonMode��.��_]Одна кнопка проверки и переход на следующий вопрос�� -   $��cbShowRAnswersCount��.��OMПоказывать количество правильных ответов�� $��1��$��0q�.@�� .+��� -   $��lQCount��$��10(�$��0q�.��Кол-во заданных:�� -   $��
lRAllCount��$��10(�$��0q�.��,*Кол-во всех правильных:�� -   $��lLCount��$��10(�$��0q�.��Кол-во выученых:�� -   $��	lRPercent��$��10(�$��0q�.��&$Процент правильных:�� -   $��
lShowCount��$��10(�$��0q�.��!Кол-во появлений:�� -   $��lRCount��$��10(�$��0q�.��#!Кол-во правильных: