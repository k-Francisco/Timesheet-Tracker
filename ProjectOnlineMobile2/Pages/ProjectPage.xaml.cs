using ProjectOnlineMobile2.ViewModels;
using System;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjectOnlineMobile2.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProjectPage : ContentPage
	{

        bool didAppear;
        ProjectPageViewModel viewModel;

		public ProjectPage ()
		{
			InitializeComponent ();
            viewModel = this.BindingContext as ProjectPageViewModel;
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (!didAppear)
            {
                //1. load the projects that are stored in the database
                //2. get the projects from the server and sync with the local database
                viewModel.LoadProjectsFromDatabase();
                viewModel.SyncProjects();

                //to stop loading and syncing on every appearance of the page
                didAppear = true;
            }
        }
    }
}