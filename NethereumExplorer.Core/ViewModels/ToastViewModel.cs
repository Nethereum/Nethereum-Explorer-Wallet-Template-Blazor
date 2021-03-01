using System;
using ReactiveUI;

namespace NethereumExplorer.ViewModels
{
    public class ToastViewModel : ReactiveObject
    {
        private string _key;

        public string Key
        {
            get => _key;
            set => this.RaiseAndSetIfChanged(ref _key, value);
        }

        private string _title;

        public string Title
        {
            get => _title;
            set => this.RaiseAndSetIfChanged(ref _title, value);
        }

        private string _message;

        public string Message
        {
            get => _message;
            set => this.RaiseAndSetIfChanged(ref _message, value);
        }

        private DateTime _received;

        public DateTime Received
        {
            get => _received;
            set => this.RaiseAndSetIfChanged(ref _received, value);
        }

        private string _link;

        public string RelativeLink
        {
            get => _link;
            set => this.RaiseAndSetIfChanged(ref _link, value);
        }


        private string _linkMessage;

        public string LinkMessage
        {
            get => _linkMessage;
            set => this.RaiseAndSetIfChanged(ref _linkMessage, value);
        }


        private bool _failed;

        public bool Failed
        {
            get => _failed;
            set => this.RaiseAndSetIfChanged(ref _failed, value);
        }

    }
}
