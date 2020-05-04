using NethereumBlazor.ViewModels;
using ReactiveUI;
using System;

namespace NethereumBlazor.Shared
{
    public partial class Toasts
    {
        public Toasts()
        {
            ViewModel = new ToastsViewModel();
        }

        protected override void OnInitialized()
        {
            //refreshing the seconds countdown
            System.Reactive.Linq.Observable.Timer(TimeSpan.FromMilliseconds(500), TimeSpan.FromMilliseconds(2000), RxApp.TaskpoolScheduler)
                .Subscribe(_ =>
                {
                    if (ViewModel.Toasts.Count > 0)
                    {
                        StateHasChanged();
                    }
                });
        }
    }
}
