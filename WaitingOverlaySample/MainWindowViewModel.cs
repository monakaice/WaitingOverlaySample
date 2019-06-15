using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Timers;

namespace WaitingOverlaySample
{
    /// <summary>MainWindow用のViewModel機能を提供する</summary>
    public class MainWindowViewModel : BindableBase
    {
        /// <summary>コンストラクタ。インスタンスを生成する</summary>
        public MainWindowViewModel()
        {
            bool CanExecute() => !this.IsWaiting;
            Expression<Func<bool>> observers = () => this.IsWaiting;

            this.Wait5SecCommand = new DelegateCommand(this.Wait5Sec, CanExecute)
                .ObservesProperty(observers);
            this.DrummingCommand = new DelegateCommand(this.Drumming, CanExecute)
                .ObservesProperty(observers);
        }

        /// <summary>待機中かどうか表す。プロパティ用</summary>
        private bool _isWaiting;

        /// <summary>待機中のメッセージ。プロパティ用</summary>
        private string _waitingMessage;

        /// <summary>待機中かどうか表す</summary>
        public bool IsWaiting
        {
            get => this._isWaiting;
            set => this.SetProperty(ref this._isWaiting, value);
        }

        /// <summary>待機中のメッセージ</summary>
        public string WaitingMessage
        {
            get => this._waitingMessage;
            set => this.SetProperty(ref this._waitingMessage, value);
        }

        /// <summary>５秒待ちコマンド</summary>
        public DelegateCommand Wait5SecCommand { get; }

        /// <summary>どんちきコマンド</summary>
        public DelegateCommand DrummingCommand { get; }

        /// <summary>５秒間待ってやる</summary>
        private async void Wait5Sec()
        {
            this.IsWaiting = true;
            {
                this.WaitingMessage = "５秒お待ち";
                await Task.Delay(5000 /*ms*/);
            }
            this.IsWaiting = false;
        }

        /// <summary>どんちき└(＾ω＾)┐♫┌(＾ω＾)┘♫どんちき</summary>
        private async void Drumming()
        {
            string MakeMessage()
            {
                bool even = (DateTime.Now.Second % 2) == 0;
                return even ? "└(＾ω＾)┐♫どんちき" : "┌(＾ω＾)┘♫どんちき";
            }

            this.IsWaiting = true;
            {
                this.WaitingMessage = MakeMessage();
                Timer timer = new Timer(1000 /*ms*/)
                {
                    AutoReset = true
                };
                timer.Elapsed += (s, e) => this.WaitingMessage = MakeMessage();
                timer.Start();
                await Task.Delay(10000 /*ms*/);
                timer.Stop();
                timer = null;
            }
            this.IsWaiting = false;
        }

    }
}
