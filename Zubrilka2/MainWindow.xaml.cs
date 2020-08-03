using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;

namespace Zubrilka2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public CQuestionsList QuestionsList = new CQuestionsList();
        public CInterface FInterface;
        //public CQuestionView QuestionView;
        public CStatisticTableView Statistic;
        private COptions FOptions = new COptions();
        private bool MouseDownFlag = false;
        private bool OneButton = false;
        private bool CheckedQuestion = false;
        private bool ShowAnswersCount = false;
        public MainWindow() {
            InitializeComponent();
            QuestionsList.Load("Q.xml");
            Debug.Write(QuestionsList.Count.ToString());
            FOptions.Load("conf.xml");
            FInterface = new CInterface(FOptions);
            DataContext = FInterface;
            Init();
            Statistic = new CStatisticTableView(QuestionsList);
            Fill();
            Test();
        }

        public void Init() {
            //slFontSize.Value = FOptions.FontSize;
            cbButtonMode.IsChecked = FOptions.OneButton;
            CheckButtonMode();
            cbShowRAnswersCount.IsChecked = FOptions.ShowAnswersCount;
            ShowAnswersTitle();

        }

        public void ShowAnswersTitle() {
            if (ShowAnswersCount == true) {
                int c = FInterface.QuestionView.AnswersCount;
                int r = FInterface.QuestionView.RightAnswersCount;
                lAnswerOptions.Content = "Варианты ответа(ов) (" + r.ToString() + " правильных из " + c.ToString() + "):";

            } else {
                lAnswerOptions.Content = "Варианты ответа(ов):";
            }
        }
        public void Fill() {
            lResult.Content = "";
            dgAnswers.Background = Brushes.Transparent;
            CQuestion q = QuestionsList.GetRNDQuestion();
            CQuestionView qv = new CQuestionView(q, QuestionsList);
            FInterface.QuestionView = qv;
            dgAnswers.ItemsSource = FInterface.QuestionView.Answers;
            ShowAnswersTitle();
            dgStatistic.ItemsSource = null;
            dgStatistic.ItemsSource = Statistic.Rows;
            lQCount.Content = "Кол-во заданных: " + QuestionsList.AQCount.ToString();
            lRAllCount.Content = "Кол-во всех правильных: " + QuestionsList.RQCount.ToString();
            lLCount.Content = "Кол-во выученых: " + QuestionsList.LQCount.ToString();
            int p = Convert.ToInt32((Convert.ToDouble(QuestionsList.RQCount) / Convert.ToDouble(QuestionsList.AQCount)) * 100);
            lRPercent.Content = "Процент выученых: " + p.ToString() + "%";
            lShowCount.Content = "Кол-во появлений: " + q.AQCount.ToString();
            lRCount.Content = "Кол-во правильных: " + q.RQCount.ToString();
        }

        public void Test() {
            
        }

        public void Check() {
            if (FInterface.QuestionView.Check() == true) {
                lResult.Content = "Правильно";
                lResult.Foreground = Brushes.Green;
                dgAnswers.Background = Brushes.LightGreen;
            } else {
                lResult.Content = "Не правильно";
                lResult.Foreground = Brushes.Red;
                dgAnswers.Background = Brushes.LightPink;
            }




        }

        private void bNext_Click(object sender, RoutedEventArgs e) {
            if (FOptions.OneButton == true) {
                if (CheckedQuestion == false) {
                    Check();
                    btNext.Content = "Следующий >";
                    CheckedQuestion = true;
                } else {
                    Fill();
                    btNext.Content = "Проверить";
                    CheckedQuestion = false;
                }
            } else
                Fill();
        }

        private void bCheck_Click(object sender, RoutedEventArgs e) {
            Check();
        }

        private void dgStatistic_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            int r, c;
            CStatisticRowView rv = (dgStatistic.CurrentCell.Item as CStatisticRowView);
            if (rv != null) {
                r = rv.Row;
                c = dgStatistic.CurrentColumn.DisplayIndex;
                if ((c > 0) && ((r == 0) && (c > 1) || (r > 0))) {
                    QuestionWindow q = new QuestionWindow(rv.Questions[c - 1]);
                    q.ShowDialog();
                }

            }
            
            
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e) {
            MouseDownFlag = true;
        }

        private void TextBlock_MouseUp(object sender, MouseButtonEventArgs e) {
            if (MouseDownFlag == true) {
                TextBlock tb = sender as TextBlock;
                CAnswerView av = tb.DataContext as CAnswerView;
                av.Checked = !(av.Checked);
                MouseDownFlag = false;

            }
        }

        private void Window_Closed(object sender, EventArgs e) {
            QuestionsList.Save("Q.xml");
            FOptions.Save("Conf.xml");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e) {
            if (MessageBox.Show("Хотите очистить статистику?", "Вопрос", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                QuestionsList.ClearStatistic();
            Fill();
        }

        private void CheckButtonMode() {
            if (cbButtonMode.IsChecked == true) {
                btNext.Content = "Проверить";
                FOptions.OneButton = true;
                btCheck.Visibility = Visibility.Collapsed;
            } else {
                btNext.Content = "Следующий";
                FOptions.OneButton = false;
                btCheck.Visibility = Visibility.Visible;
            }
        }

        private void cbButtonMode_Click(object sender, RoutedEventArgs e) {
            CheckButtonMode();
        }

        private void cbShowRAnswersCount_Click(object sender, RoutedEventArgs e) {
            FOptions.ShowAnswersCount = (bool)cbShowRAnswersCount.IsChecked;
            ShowAnswersTitle();
        }

        private void btExclude_Click(object sender, RoutedEventArgs e) {
           // FInterface.QuestionView.Question.RQCount = 9;
        }

    }
}
