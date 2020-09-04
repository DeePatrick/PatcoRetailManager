using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using PRMDesktopUI.EventModels;

namespace PRMDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
    {
        private readonly SalesViewModel _salesVM;
        private readonly IEventAggregator _events;

        public ShellViewModel(SalesViewModel salesVM, IEventAggregator events)
        {
            _events = events;
            _salesVM = salesVM;

            _events.Subscribe(this);

            ActivateItem(IoC.Get<LoginViewModel>());
        }

        public void Handle(LogOnEvent message)
        {
            ActivateItem(_salesVM);
        }
    }
}


