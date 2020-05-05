using System;
using System.Globalization;
using DynamicData;
using NethereumBlazor.Messages;
using NethereumBlazor.Services;
using ReactiveUI;
using Splat;

namespace NethereumBlazor.ViewModels
{
    public class BlocksViewModel : ReactiveObject
    {
        private readonly NewBlockProcessingService _newBlockProcessingService;
        private object _lockingObject = new object();
        private bool _loading;
        public SourceCache<BlockViewModel, string> Blocks { get; set; } = new SourceCache<BlockViewModel, string>(t => t.Number.ToString(CultureInfo.InvariantCulture));

        public bool Loading
        {
            get { return _loading; }
            set { this.RaiseAndSetIfChanged(ref _loading, value); }
        }

        public BlocksViewModel(NewBlockProcessingService newBlockProcessingService = null)
        {
            _newBlockProcessingService = newBlockProcessingService ?? Locator.Current.GetService<NewBlockProcessingService>();

            MessageBus.Current.Listen<UrlChanged>().Subscribe(x =>
                {
                    lock (_lockingObject)
                    {
                        Loading = true;
                        Blocks.Clear();
                    }
                }
            );

            _newBlockProcessingService.Blocks.Connect().Subscribe(blockChanges =>
            {
                lock (_lockingObject)
                {
                    Loading = true;
                    Blocks.Edit(_ =>
                    {

                        Blocks.Clear();
                        foreach (var block in _newBlockProcessingService.Blocks.Items)
                        {
                            Blocks.AddOrUpdate(new BlockViewModel(block));
                        }

                    });
                    Loading = false;
                }
            });
        }
    }
}
