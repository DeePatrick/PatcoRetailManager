using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

            _events.SubscribeOnPublishedThread(this);

            ActivateItemAsync(IoC.Get<LoginViewModel>(), new CancellationToken());
        }
        public void ExitApplication()
        {
            TryCloseAsync();
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
            ActivateItemAsync(IoC.Get<UserDisplayViewModel>(),  new CancellationToken());
        }

        public void SaleManagement()
        {
            ActivateItemAsync(IoC.Get<SalesViewModel>(), new CancellationToken());
        }

        public async Task LogOut()
        {
            _user.ResetUserModel();
            _apiHelper.ResetUserModel();
            await ActivateItemAsync(IoC.Get<LoginViewModel>(), new CancellationToken());
            NotifyOfPropertyChange(() => IsLoggedIn);
        }

        public async Task HandleAsync(LogOnEvent message, CancellationToken cancellationToken)
        {
            await ActivateItemAsync(_salesVM, cancellationToken);
            NotifyOfPropertyChange(() => IsLoggedIn);
        }
    }
}




