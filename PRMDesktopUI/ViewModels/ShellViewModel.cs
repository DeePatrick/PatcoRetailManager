﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using PRMDesktopUI.EventModels;
using PRMDesktopUI.Library.Models;

namespace PRMDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
    {
        private readonly SalesViewModel _salesVM;
        private readonly IEventAggregator _events;
        private ILoggedInUserModel _user;

        public ShellViewModel(SalesViewModel salesVM, IEventAggregator events, ILoggedInUserModel user)
        {
            _events = events;
            _salesVM = salesVM;
            _user = user;

            _events.Subscribe(this);

            ActivateItem(IoC.Get<LoginViewModel>());
        }
        public void ExitApplication()
        {
            TryClose();
        }

        public bool IsLoggedIn
        {
            get {
                bool output = false;
                if (_user.Token?.Length > 0)
                {
                    output = true;
                }
                return output;
            }

        }

        public void LogOut()
        {
            _user.LogOffUser();
            ActivateItem(IoC.Get<LoginViewModel>());
            NotifyOfPropertyChange(() => IsLoggedIn);
        }
        public void Handle(LogOnEvent message)
        {
            ActivateItem(_salesVM);
            NotifyOfPropertyChange(() => IsLoggedIn);
        }
    }
}


