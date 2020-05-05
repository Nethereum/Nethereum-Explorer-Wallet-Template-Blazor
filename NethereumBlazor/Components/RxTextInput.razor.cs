using System.ComponentModel;

namespace NethereumBlazor.Components
{
    public partial class RxTextInput : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _text = string.Empty;

        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Text)));
                StateHasChanged();
            }
        }

        public string Error { get; set; }
    }
}
