using System.Windows;
using System.Windows.Forms;

namespace WaitingOverlaySample.Controls
{
    /// <summary>WaitingOverlaySubView.xaml の相互作用ロジック</summary>
    public partial class WaitingOverlaySubView
    {
        /// <summary>コンストラクタ。インスタンスを生成する</summary>
        public WaitingOverlaySubView()
        {
            this.InitializeComponent();
            this._viewModel = new WaitingOverlaySubViewModel();
            this.DataContext = this._viewModel;
            this.AnimationPicture.SizeMode = PictureBoxSizeMode.StretchImage;
            this.AnimationPicture.Image = Properties.Resources.WaitingGif;

            this.Loaded += this.OnLoaded;
            this.Unloaded += this.OnUnloaded;
            this.IsVisibleChanged += this.OnIsVisibleChanged;
        }

        /// <summary>ViewModel</summary>
        private readonly WaitingOverlaySubViewModel _viewModel;

        /// <summary>ウィンドウ</summary>
        private Window _window;

        /// <summary>オーバレイするターゲットコントロール名</summary>
        public string OverlayTargetName { get; set; }

        /// <summary>再描画処理</summary>
        public void Redraw()
        {
            this.Resize();
        }

        /// <summary>リサイズするぞい</summary>
        private void Resize()
        {
            // 表示しているコントロールの真ん中に表示したいので、親を辿り、ターゲットのサイズを取得する
            FrameworkElement parentElement = this.Parent as FrameworkElement;
            while (parentElement?.Parent != null && parentElement.Parent is FrameworkElement parent)
            {
                if (parent.Name == this.OverlayTargetName)
                {
                    this._viewModel.Width = parent.ActualWidth;
                    this._viewModel.Height = parent.ActualHeight;
                    break;
                }

                parentElement = parent;
            }
        }

        /// <summary>コントロールがロードされたときに実行</summary>
        /// 
        /// <param name="sender">イベント送信元インスタンス</param>
        /// <param name="e">イベントデータ</param>
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this._window = Window.GetWindow(this);
            // VisualStudioのビジュアルビューで例外が発生して、見た目が表示出来なくなるのを防ぐ
            if (this._window != null)
            {
                this._window.SizeChanged += this.OnSizeChanged;
            }
        }

        /// <summary>コントロールがアンロードされたときに実行</summary>
        /// 
        /// <param name="sender">イベント送信元インスタンス</param>
        /// <param name="e">イベントデータ</param>
        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            // WindowインスタンスのSizeChangedイベントにコールバックを登録しまくるので、
            // これをしないとメモリリークか、ぬるぽするかもシレーヌ（未確認）
            if (this._window != null)
            {
                this._window.SizeChanged -= this.OnSizeChanged;
            }
        }

        /// <summary>アプリケーションのウィンドウサイズが変わったときに実行</summary>
        /// 
        /// <param name="sender">イベント送信元インスタンス</param>
        /// <param name="e">イベントデータ</param>
        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.Resize();
        }

        /// <summary>表示状態が変わったときに実行</summary>
        /// 
        /// <param name="sender">イベント送信元インスタンス</param>
        /// <param name="e">イベントデータ</param>
        private void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                this._viewModel.StartUpdatingDotLeader();
                this.Resize();
            }
            else
            {
                this._viewModel.StopUpdatingDotLeader();
            }
        }
    }
}
