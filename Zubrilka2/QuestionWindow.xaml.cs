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
using System.Windows.Shapes;

namespace Zubrilka2 {
    /// <summary>
    /// Логика взаимодействия для QuestionWindow.xaml
    /// </summary>
    public partial class QuestionWindow : Window {
        public QuestionWindow(CQuestionView AQuestionView) {
            InitializeComponent();
            DataContext = AQuestionView;
            lbAnswers.ItemsSource = AQuestionView.Answers;
        }
    }
}
