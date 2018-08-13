using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjectOnlineMobile2.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddProjectPage : ContentPage
	{
		public AddProjectPage ()
		{
			InitializeComponent ();

            MessagingCenter.Instance.Subscribe<string>(this,"SaveProject", (s) => {
                var parameters = new string[] { ProjectNameEntry.Text,
                    ProjectDescriptionEntry.Text,
                    String.Format("{0:MM/dd/yyyy}", ProjectDate.Date),
                    ProjectType.SelectedItem.ToString()};

                MessagingCenter.Instance.Send<string[]>(parameters, "AddProject");
            });

            ProjectType.Items.Add("Enterprise Project");
            ProjectType.Items.Add("Task List Project");
            ProjectType.SelectedIndex = 0;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            ProjectNameEntry.Text = "";
            ProjectDescriptionEntry.Text = "";
            ProjectDate.Date = DateTime.Now;
            ProjectType.SelectedIndex = 0;
        }
    }
}