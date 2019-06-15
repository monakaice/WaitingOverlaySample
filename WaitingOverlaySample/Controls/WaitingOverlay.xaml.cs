using System;
using System.Windows;
using System.Windows.Threading;

namespace WaitingOverlaySample.Controls
{
    /// <summary>WaitingOverlay.xaml の相互作用ロジック</summary>
    public partial class WaitingOverlay
    {
        /// <summary>コンストラクタ。インスタンスを生成する</summary>
        public WaitingOverlay()
        {
            this.InitializeComponent();
            this.IsVisibleChanged += (s, e) =>
            {
                // 表示・非表示切替時に、Viewの位置が悪い。Viewのサイズ計算処理の動作するタイミングが悪いっぽい
                // 表示状態が切り替わったときに、微妙に遅らせて、再描画処理を呼び出す。
                // 参考サイト：http://geekswithblogs.net/ilich/archive/2012/10/16/running-code-when-windows-rendering-is-completed.aspx
                this.Dispatcher.BeginInvoke(
                    new Action(() => this.SubView.Redraw()),
                    DispatcherPriority.ContextIdle
                );
            };
        }

        /// <summary>依存性プロパティ</summary>
        /// 
        /// <remarks>
        /// SubViewにプロパティ値を受け渡す
        /// </remarks>
        /// 
        /// <see cref="https://qiita.com/tricogimmick/items/62cd9f5deca365a83858"/>
        public static readonly DependencyProperty OverlayTargetNameProperty = DependencyProperty.Register(
            "OverlayTargetName",
            typeof(string),
            typeof(WaitingOverlay),
            new FrameworkPropertyMetadata(null, OnOverlayTargetNameChanged)
            );

        /// <summary>依存性プロパティ</summary>
        /// 
        /// <remarks>
        /// SubViewにプロパティ値を受け渡す
        /// </remarks>
        /// 
        /// <see cref="https://qiita.com/tricogimmick/items/62cd9f5deca365a83858"/>
        public static readonly DependencyProperty FixedMessageProperty = DependencyProperty.Register(
            "FixedMessage",
            typeof(string),
            typeof(WaitingOverlay),
            new FrameworkPropertyMetadata("Waiting", OnFixedMessageChanged)
            );

        /// <summary>オーバレイするControlの名前</summary>
        public string OverlayTargetName
        {
            get => (string)this.GetValue(OverlayTargetNameProperty);
            set => this.SetValue(OverlayTargetNameProperty, value);
        }

        /// <summary>固定メッセージ</summary>
        public string FixedMessage
        {
            get => (string)this.GetValue(FixedMessageProperty);
            set => this.SetValue(FixedMessageProperty, value);
        }

        /// <summary>OverlayTargetNameが変更される度に実行する</summary>
        /// 
        /// <param name="obj">依存オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private static void OnOverlayTargetNameChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is WaitingOverlay myView)
            {
                myView.SubView.OverlayTargetName = myView.OverlayTargetName;
            }
        }

        /// <summary>FixMessageが変更される度に実行する</summary>
        /// 
        /// <param name="obj">依存オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private static void OnFixedMessageChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is WaitingOverlay myView)
            {
                ((WaitingOverlaySubViewModel)myView.SubView.DataContext).FixedMessage = myView.FixedMessage;
            }
        }
    }
}
