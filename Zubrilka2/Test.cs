using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Zubrilka2
{
    public class Answer {
        private string FText;
        private bool FChecked;
        public Answer(string AText, bool AChecked) {
            FText = AText;
            FChecked = AChecked;
        }
        public bool Checked {
            get {
                return FChecked;
            }
            set {
                FChecked = value;
            }
        }
        public string Text {
            get {
                return FText;
            }
            set {
                FText = value;
            }
        }
    }
    public class Answers {
        private IList<Answer> FAnswers = new ObservableCollection<Answer>();
        public IList<Answer> Lst {
            get {
                return FAnswers;
            }
        }
    }
}
