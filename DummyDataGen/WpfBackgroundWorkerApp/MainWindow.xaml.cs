using System.ComponentModel;
using System.Threading;
using System.Windows;

namespace WpfBackgroundWorkerApp
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public BackgroundWorker BgwTest { get; set; }
        public MainWindow()
        {
            InitializeComponent();

            BgwTest = new BackgroundWorker();
            BgwTest.DoWork += BgwTest_DoWork;
            BgwTest.RunWorkerCompleted += BgwTest_RunWorkerCompleted;
            BgwTest.ProgressChanged += BgwTest_ProgressChanged;

            BgwTest.WorkerReportsProgress = true;
            BgwTest.WorkerSupportsCancellation = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!BgwTest.IsBusy)
            {
                BgwTest.RunWorkerAsync(); //BackgroundWorker 실행
                LblResult.Content = "실행";
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (BgwTest.WorkerSupportsCancellation)
            {
                BgwTest.CancelAsync(); //BackgroundWorker 실행 취소
                LblResult.Content = "실행취소";
            }
        }


        //작업이 시작될 때 다른 스레드에서 실행할 이벤트 핸들러
        private void BgwTest_DoWork(object sender, DoWorkEventArgs e)
        {
            //var worker = sender as BackgroundWorker;
            var worker = (BackgroundWorker)sender;
            for (int i = 0; i <= 100; i++)
            {
                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    Thread.Sleep(20);
                    worker.ReportProgress(i);
                }
            }
        }

        //작업자의 스레드에서 일부 진행되었음을 나타낼 때 사용하는 이벤트 핸들러
        private void BgwTest_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //LblResult.Text = e.ProgressPercentage.ToString() + "%";
            PgbTest.Value = e.ProgressPercentage;
        }

        //작업자가 완료(성공, 실패 또는 취소) 되었을 때 발생하는 이벤트 핸들러
        private void BgwTest_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                LblResult.Content = "Canceled";
            }
            else if (e.Error != null)
            {
                LblResult.Content = "Error: " + e.Error.Message;
            }
            else
            {
                LblResult.Content = "Done";
            }
        }
    }
}
