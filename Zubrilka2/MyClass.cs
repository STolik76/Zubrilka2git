using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Zubrilka2 {
    //Ответ
    public class CAnswer {
        public string Text { get; set; }
        public bool Right { get; set; }
        public void Load(XmlNode ANode) {
            Text = ANode.Attributes["text"].Value;
            Right = Convert.ToBoolean(ANode.Attributes["right"].Value);
        }
    }
    //Вопрос
    public class CQuestion {
        private Random FRnd = new Random();
        private XmlNode FQuestionNode;
        public string Text { get; set; }
        public List<CAnswer> AnswersList = new List<CAnswer>();
        public string Topic { get; set; }
        public string Doc { get; set; }
        public int AQCount { get; set; }
        public int RQCount { get; set; }
        public int TGCount { get; set; }
        public DateTime LastTime { get; set; }
        public void Load(XmlNode ANode) {
            FQuestionNode = ANode;
            AQCount = Convert.ToInt32(ANode.Attributes["aqcount"].Value);
            RQCount = Convert.ToInt32(ANode.Attributes["rqcount"].Value);
            TGCount = Convert.ToInt32(ANode.Attributes["tgcount"].Value);
            Text = ANode.Attributes["text"].Value;
            foreach (XmlNode n in ANode.ChildNodes) {
                CAnswer a = new CAnswer();
                a.Load(n);
                AnswersList.Add(a);
            }
        }
        public void Save() {
            FQuestionNode.Attributes["aqcount"].Value = AQCount.ToString();
            FQuestionNode.Attributes["rqcount"].Value = RQCount.ToString();
            FQuestionNode.Attributes["tgcount"].Value = TGCount.ToString();
        }
        //Возвращает вопрос с перемешанными ответами
        public CQuestion GetTestQuestion() {
            CQuestion q = new CQuestion();
            q.AQCount = AQCount;
            q.RQCount = RQCount;
            q.Text = Text;
            q.Topic = Topic;
            q.Doc = Doc;
            int c = AnswersList.Count;
            for (int i = 0; i < c; i++)
                q.AnswersList.Add(null);
            int ca = 0;
            do {
                int p = FRnd.Next(c);
                if (q.AnswersList[p] == null) {
                    q.AnswersList[p] = AnswersList[ca];
                    ca++;
                }
            } while (ca < c);
            LastTime = DateTime.Now;
            return q;
        }
        //Проверяет недавно ли появлялся вопрос
        public bool IsLately(int AMinInterval) {
            TimeSpan ts = DateTime.Now - LastTime;
            if (ts.Seconds <= AMinInterval)
                return true;
            else
                return false;
        }
        public override string ToString() {
            return "zzz";
        }
    }
    //Список вопросов
    public class CQuestionsList {
        private XmlDocument FXmlDoc = new XmlDocument();
        private List<CQuestion> FQuestionList = new List<CQuestion>();
        private CQuestion FCurrentQuestion;
        private XmlNode FRootNode;
        public int Threshold { get; set; }
        public int MinInterval { get; set; }
        public int AQCount;
        public int RQCount;
        private Random FRnd = new Random();
        public void Load(string AFile) {
            FXmlDoc.Load(AFile);
            FRootNode = FXmlDoc.SelectSingleNode("/questions");
            Threshold = Convert.ToInt32(FRootNode.Attributes["threshold"].Value);
            MinInterval = Convert.ToInt32(FRootNode.Attributes["mininterval"].Value);
            AQCount = Convert.ToInt32(FRootNode.Attributes["aqcount"].Value);
            RQCount = Convert.ToInt32(FRootNode.Attributes["rqcount"].Value);
            string ct;
            string cd;
            foreach (XmlNode tn in FRootNode.ChildNodes) {
                ct = tn.Attributes["text"].Value;
                foreach (XmlNode dn in tn.ChildNodes) {
                    cd = dn.Attributes["text"].Value;
                    foreach (XmlNode qn in dn.ChildNodes) {
                        CQuestion q = new CQuestion();
                        q.Load(qn);
                        q.Topic = ct;
                        q.Doc = cd;
                        FQuestionList.Add(q);
                    }
                }
            }
        }
        public void Save(string AFile) {
            foreach (CQuestion q in FQuestionList)
                q.Save();
            FRootNode.Attributes["aqcount"].Value = AQCount.ToString();
            FRootNode.Attributes["rqcount"].Value = RQCount.ToString();
            FRootNode.Attributes["mininterval"].Value = MinInterval.ToString();
            FXmlDoc.Save(AFile);
        }
        public int LQCount {
            get {
                int c = 0;
                foreach (CQuestion q in FQuestionList) {
                    if (q.TGCount >= Threshold)
                        c++;
                }
                return c;
            }
        }
        public CQuestion GetRNDQuestion() {
            int qlc = FQuestionList.Count;
            int ic = 0;
            do {
                int p = FRnd.Next(qlc);
                FCurrentQuestion = FQuestionList[p];
                ic++;
            } while (((FCurrentQuestion.TGCount >= Threshold) || (FCurrentQuestion.IsLately(MinInterval) != true)) && (ic < 1000));
            if (ic == 1000) {
                foreach (CQuestion q in FQuestionList)
                    if (q.TGCount < Threshold) {
                        FCurrentQuestion = q;
                        break;
                    }
            }
            FCurrentQuestion.AQCount++;
            AQCount++;
            return FCurrentQuestion.GetTestQuestion();
        }
        public void SetRight() {
            FCurrentQuestion.RQCount++;
            FCurrentQuestion.TGCount++;
            RQCount++;
        }
        public void SetWrong() {
            FCurrentQuestion.TGCount = 0;
        }
        public void Exclude() {
            FCurrentQuestion.TGCount = Threshold + 1;
        }
        public CQuestion this[int AIndex] {
            get {
                return FQuestionList[AIndex];
            }
        }
        public int Count {
            get {
                return FQuestionList.Count;
            }
        }
        public void ClearStatistic() {
            foreach (CQuestion q in FQuestionList) {
                q.TGCount = 0;
                q.RQCount = 0;
                q.AQCount = 0;
            }
            AQCount = 0;
            RQCount = 0;
        }
    }
    //Классы обвертки, для трансляции ворпосов в интерефейс
    public class CAnswerView : INotifyPropertyChanged {
        private string FText;
        private bool FChecked = false;
        private double FFontSize;
        private bool FRight;
        private string FColor = "Black";
        public event PropertyChangedEventHandler PropertyChanged;
        public CAnswerView(CAnswer AAnswer) {
            FText = AAnswer.Text;
            FRight = AAnswer.Right;
        }
        public string Text {
            get {
                return FText;
            }
        }
        public string Color {
            get {
                return FColor;
            }
            set {
                FColor = value;
                if (PropertyChanged != null)
                   PropertyChanged(this, new PropertyChangedEventArgs("Color"));
            }
        }
        public double FontSize {
            get {
                return FFontSize;
            }
            set {
                FFontSize = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("FontSize"));
            }
        }

        public bool Checked {
            get {
                return FChecked;
            }
            set {
                FChecked = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Checked"));
            }
        }
        public void Check() {
            if (FRight == true)
                Color = "Green";
            else
                Color = "Black";
        }
        public bool Right {
            get {
                return FRight;
            }
        }
        public override string ToString() {
            string s;
            if (FRight == true)
                s = "- (Верно) ";
            else
                s = "- ";
            s += FText;
            return s;
        }
    }

    public class CQuestionView {
        private CQuestion FQuestion;
        private CQuestionsList FParentQuestionsList;
        //public event PropertyChangedEventHandler PropertyChanged;
        private IList<CAnswerView> FAnswers = new ObservableCollection<CAnswerView>();
        public CQuestionView(CQuestion AQuestion, CQuestionsList AParentQuestionsList) {
            FQuestion = AQuestion;
            FParentQuestionsList = AParentQuestionsList;
            foreach (CAnswer a in AQuestion.AnswersList) {
                CAnswerView av = new CAnswerView(a);
                FAnswers.Add(av);
            }
        }
        public bool Check() {
            bool f = true;
            foreach (CAnswerView a in FAnswers) {
                a.Check();
                if (a.Checked != a.Right)
                    f = false;
            }
            if (f == true)
                FParentQuestionsList.SetRight();
            else
                FParentQuestionsList.SetWrong();
            return f;
        }
        public override string ToString() {
            return FQuestion.AQCount.ToString() + "/" + FQuestion.RQCount.ToString() + "/" + FQuestion.TGCount.ToString();
        }
        public IList<CAnswerView> Answers {
            get {
                return FAnswers;
            }
        }
        public int AnswersCount {
            get {
                return FAnswers.Count;
            }
        }
        public int RightAnswersCount {
            get {
                int i = 0;
                foreach (CAnswerView a in FAnswers) {
                    if (a.Right == true)
                        i++;
                }
                return i;
            }
        }
        public string Text {
            get {
                return FQuestion.Text;
            }
        }
        public string Topic {
            get {
                return FQuestion.Topic;
            }
        }
        public string Doc {
            get {
                return FQuestion.Doc;
            }
        }
        public CQuestion Question {
            get {
                return FQuestion;
            }
        }
        public string Color {
            get {
                string c = "Gray";
                if (FQuestion.AQCount > 0)
                    c = "Red";
                if (FQuestion.RQCount > 0)
                    c = "Yellow";
                if (FQuestion.TGCount > FParentQuestionsList.Threshold)
                    c = "Green";
                return c;
            }
        }        
    }

    //Класс обвертка, для отображаения статистики
    public class CStatisticRowView {
        private CQuestionView[] FQuestions = new CQuestionView[10];
        private int FRow;
        public CStatisticRowView(int ARow) {
            FRow = ARow;
        }
        private void Refresh() {
            /*
            for (int i = 0; i < 10; i++) {
                if (FQuestions[i] != null) {
                    FValues[i] = FQuestions[i].AQCount.ToString() + "/" + FQuestions[i].RQCount.ToString() + "/" + FQuestions[i].TGCount.ToString();
                    FColors[i] = "Red";
                    if (FQuestions[i].RQCount > 0)
                        FColors[i] = "Yellow";
                    if (FQuestions[i].TGCount >= FParentQuestionsList.Threshold)
                        FColors[i] = "Green";
                } else {
                    FValues[i] = "-";
                    FColors[i] = "Gray";
                }
            }
             */
        }
        public CQuestionView[] Questions {
            get {
                //Refresh();
                return FQuestions;
            }
        }
        public string Title {
            get {
                if (FRow == 0)
                    return "x";
                else
                    return FRow.ToString() + "x";
            }
        }
        public int Row {
            get {
                return FRow;
            }
        }

        public void SetQuestions(CQuestionView AQuestions, int ACol) {
            FQuestions[ACol] = AQuestions;
        }
    }


    public class CStatisticTableView {
        private IList<CStatisticRowView> FRows = new ObservableCollection<CStatisticRowView>();
        public CStatisticTableView(CQuestionsList AParentQuestionsList) {
            int r1;
            r1 = (int)(AParentQuestionsList.Count - 1) / 10;
            for (int r = 0; r < r1 + 1; r++) {
                CStatisticRowView scrv = new CStatisticRowView(r);
                for (int c = 0; c < 10; c++) {
                    int i = r * 10 + c;
                    if (i <= AParentQuestionsList.Count) {
                        if ((r==0) && (c==0))
                            scrv.SetQuestions(null, c);
                        else
                            scrv.SetQuestions(new CQuestionView(AParentQuestionsList[i - 1], AParentQuestionsList), c);
                    }
                    /*
                    if ((r * 10 + c < AParentQuestionsList.Count) && (r * 10 + c != 0))
                        scrv.SetQuestions(new CQuestionView(AParentQuestionsList[r * 10 + c], AParentQuestionsList), c);
                    else
                        scrv.SetQuestions(null, c);
                    */
                }
                FRows.Add(scrv);
            }
        }
        public IList<CStatisticRowView> Rows {
            get {
                return FRows;
            }
        }
    }

    public class COptions {
        //private string FFileName;
        private XmlDocument FXmlDoc = new XmlDocument();
        private XmlNode FRootNode;
        //private Single FFontSize;
        //private bool FOneButton;
        //private bool FShowAnswersCount;
        public COptions() {

        }
        public void Load(string AFileName){
            FXmlDoc.Load(AFileName);
            FRootNode = FXmlDoc.SelectSingleNode("/Options");
            FontSize = Convert.ToDouble(FRootNode["FontSize"].InnerText);
            OneButton = Convert.ToBoolean(FRootNode["OneButton"].InnerText);
            ShowAnswersCount = Convert.ToBoolean(FRootNode["ShowAnswersCount"].InnerText);
        }
        public void Save(string AFileName) {
            FRootNode["FontSize"].InnerText = FontSize.ToString();
            FRootNode["OneButton"].InnerText = OneButton.ToString();
            FRootNode["ShowAnswersCount"].InnerText = ShowAnswersCount.ToString();
            FXmlDoc.Save(AFileName);
        }
        public Double FontSize { get; set; }
        public bool OneButton { get; set; }
        public bool ShowAnswersCount { get; set; }
    }

    public class CInterface : INotifyPropertyChanged {
        private COptions FOptions;
        private CQuestionView FQuestionView;
        public event PropertyChangedEventHandler PropertyChanged;
        public CInterface(COptions AOptions) {
            FOptions = AOptions;
        }
        public CQuestionView QuestionView {
            get {
                return FQuestionView;
            }
            set {
                FQuestionView = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("QuestionView"));
            }
        }
        public double FontSize {
            get {
                return FOptions.FontSize;
            }
            set {
                FOptions.FontSize = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("FontSize"));
            }
        }

    }
}
