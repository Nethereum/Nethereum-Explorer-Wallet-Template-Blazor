using System;
using System.Globalization;
using DynamicData;
using NethereumExplorer.Messages;
using NethereumExplorer.Services;
using ReactiveUI;

namespace NethereumExplorer.ViewModels
{
    public class BlocksViewModel : ReactiveObject
    {
        private readonly NewBlockProcessingService _newBlockProcessingService;
        private object _lockingObject = new object();
        private bool _loading;

        public BlocksViewModel(NewBlockProcessingService newBlockProcessingService)
        {
            _newBlockProcessingService = newBlockProcessingService;
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
                }
            );
        }

        public SourceCache<BlockViewModel, string> Blocks { get; set; } = new SourceCache<BlockViewModel, string>(t => t.Number.ToString(CultureInfo.InvariantCulture));

        public bool Loading
        {
            get { return _loading; }
            set { this.RaiseAndSetIfChanged(ref _loading, value); }
        }
    }
}
