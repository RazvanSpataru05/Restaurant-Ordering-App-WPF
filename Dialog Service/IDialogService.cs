using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantOrderingApp.Dialog_Service
{
    public interface IDialogService
    {
        void ShowLoginWindow();
        void ShowRestaurantWindow();
        void CloseLoginWindow();
        bool ShowConfirmationDialog(string message);
    }
}
