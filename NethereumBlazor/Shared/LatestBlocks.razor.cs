using NethereumBlazor.ViewModels;
using ReactiveUI;
using System;
using System.Reactive;

namespace NethereumBlazor.Shared
{
    public partial class LatestBlocks
    {
        public LatestBlocks()
        {
            ViewModel = new BlocksViewModel();
        }

        protected override void OnInitialized()
        {
            System.Reactive.Linq.Observable.Timer(TimeSpan.FromMilliseconds(500), TimeSpan.FromMilliseconds(2000), RxApp.TaskpoolScheduler)
                    .Subscribe(_ => StateHasChanged());
        }
    }
}
