using Prism.Mvvm;
using System.Timers;

namespace WaitingOverlaySample.Controls
{
    /// <summary>WaitingOverlaySubView用のViewModel機能を提供する</summary>
    public class WaitingOverlaySubViewModel : BindableBase
    {
        /// <summary>コンストラクタ。インスタンスを開始する</summary>
        public WaitingOverlaySubViewModel()
        {
            // タイマ間隔、ピリオド数は適当
            this._updateDotLeaderTimer = new Timer(200 /*ms*/)
            {
                AutoReset = true
            };
            this._updateDotLeaderTimer.Elapsed += (s, e) =>
            {
                int count = e.SignalTime.Second % 6;
                this.DotLeader = new string('.', count);
            };
        }

        /// <summary>リーダー更新タイマ</summary>
        private readonly Timer _updateDotLeaderTimer;

        /// <summary>固定メッセージ。プロパティ用</summary>
        private string _fixedMessage;

        /// <summary>リーダー。プロパティ用</summary>
        private string _dotLeader;

        /// <summary>Controlの幅。プロパティ用</summary>
        private double _width;

        /// <summary>Controlの高さ。プロパティ用</summary>
        private double _height;

        /// <summary>固定メッセージ</summary>
        public string FixedMessage
        {
            get => this._fixedMessage;
            set => this.SetProperty(ref this._fixedMessage, value);
        }

        /// <summary>リーダー</summary>
        public string DotLeader
        {
            get => this._dotLeader;
            set => this.SetProperty(ref this._dotLeader, value);
        }

        /// <summary>Controlの幅</summary>
        public double Width
        {
            get => this._width;
            set => this.SetProperty(ref this._width, value);
        }

        /// <summary>Controlの高さ</summary>
        public double Height
        {
            get => this._height;
            set => this.SetProperty(ref this._height, value);
        }

        /// <summary>リーダーの更新を開始する</summary>
        public void StartUpdatingDotLeader() => this._updateDotLeaderTimer.Start();

        /// <summary>リーダの更新を停止する</summary>
        public void StopUpdatingDotLeader() => this._updateDotLeaderTimer.Stop();

    }
}
