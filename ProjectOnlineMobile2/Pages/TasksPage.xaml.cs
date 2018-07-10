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
	public partial class TasksPage : ContentPage
	{
        bool didAppear;
        TasksPageViewModel viewModel;

		public TasksPage ()
		{
			InitializeComponent ();
            viewModel = this.BindingContext as TasksPageViewModel;
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (!didAppear)
            {
                //1. Load user tasks from the local database
                //2. Sync user tasks
                viewModel.LoadAssignmentsFromDatabase();
                viewModel.SyncUserTasks();

                //to prevent from syncing the next time this page appears
                didAppear = true;
            }
        }
    }
}