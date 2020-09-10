using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using PRMDesktopUI.EventModels;
using PRMDesktopUI.Library.Api;
using PRMDesktopUI.Library.Models;

namespace PRMDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
    {
        private SalesViewModel _salesVM;
        private IEventAggregator _events;
        private ILoggedInUserModel _user;
        private IAPIHelper _apiHelper;

        public ShellViewModel(SalesViewModel salesVM, IEventAggregator events, ILoggedInUserModel user, IAPIHelper apiHelper)
        {
            _events = events;
            _salesVM = salesVM;
            _user = user;
            _apiHelper = apiHelper;

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

        public void UserManageMent()
        {
            ActivateItem(IoC.Get<UserDisplayViewModel>());
        }

        public void SaleManagement()
        {
            ActivateItem(IoC.Get<SalesViewModel>());
        }

        public void LogOut()
        {
            _user.ResetUserModel();
            _apiHelper.ResetUserModel();
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




