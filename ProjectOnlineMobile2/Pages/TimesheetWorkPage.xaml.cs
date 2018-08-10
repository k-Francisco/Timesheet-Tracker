using ProjectOnlineMobile2.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjectOnlineMobile2.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TimesheetWorkPage : ContentPage
	{
        TimesheetWorkPageViewModel viewModel;
		public TimesheetWorkPage ()
		{
			InitializeComponent ();
            viewModel = this.BindingContext as TimesheetWorkPageViewModel;
		}

        protected override void OnAppearing()
        {
            viewModel.LoadWorkFromDatabase();
            viewModel.SyncTimesheetLineWork();
        }

        protected override void OnDisappearing()
        {
            viewModel.OnExitPage();
        }
    }
}