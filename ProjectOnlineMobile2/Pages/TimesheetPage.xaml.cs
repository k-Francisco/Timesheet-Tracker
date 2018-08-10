using ProjectOnlineMobile2.ViewModels;
using System;
using LineModel = ProjectOnlineMobile2.Models2.LineModel.LineModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ProjectOnlineMobile2.Models2.LineModel;

namespace ProjectOnlineMobile2.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TimesheetPage : ContentPage
	{
        TimesheetPageViewModel viewModel;
        bool didAppear;
        const string EDIT_TASK = "Edit Task";
        const string DELETE_TASK = "Delete Task";
        const string CANCEL_BUTTON = "Cancel";

        public TimesheetPage ()
		{
			InitializeComponent ();

            viewModel = this.BindingContext as TimesheetPageViewModel;

            MessagingCenter.Instance.Subscribe<String>(this,"OpenPeriodPicker",(s)=> {
                periodPicker.Focus();
            });

            MessagingCenter.Instance.Subscribe<String>(this, "OpenProjectPicker", (s)=> {
                projectPicker.Focus();
            });

            MessagingCenter.Instance.Subscribe<String>(this, "CloseProjectPicker", (s)=> {
                projectPicker.Unfocus();
            });

            MessagingCenter.Instance.Subscribe<LineModel>(this, "EditComment", (line)=> {
                DisplayEditCommentAlert(line);
            });
            
		}

        private void DisplayEditCommentAlert(LineModel line)
        {
            MessagingCenter.Instance.Send<LineModel>(line, "DisplayEditLineCommentAlert");
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.SyncTimesheetLines();
            if (!didAppear)
            {
                didAppear = true;
            }
        }
    }
}