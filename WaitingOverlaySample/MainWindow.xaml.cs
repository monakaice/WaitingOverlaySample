namespace WaitingOverlaySample
{
    /// <summary>MainWindow.xaml の相互作用ロジック</summary>
    public partial class MainWindow
    {
        /// <summary>コンストラクタ。インスタンスを生成する</summary>
        public MainWindow()
        {
            this.InitializeComponent();
            this.DataContext = new MainWindowViewModel();
        }
    }
}
