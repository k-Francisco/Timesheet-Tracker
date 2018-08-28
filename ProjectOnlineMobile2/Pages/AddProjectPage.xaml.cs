using ProjectOnlineMobile2.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjectOnlineMobile2.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddProjectPage : ContentPage
    {

        private ProjectPageViewModel viewModel;

        public AddProjectPage()
        {
            InitializeComponent();

            viewModel = BindingContext as ProjectPageViewModel;

            MessagingCenter.Instance.Subscribe<string>(this, "SaveProject", (s) =>
            {
                if (string.IsNullOrWhiteSpace(ProjectNameEntry.Text))
                {
                    this.DisplayAlert(null, "Please fill in the name of the project", "OK");
                }
                else
                {
                    var parameters = new string[] { ProjectNameEntry.Text,
                    ProjectDescriptionEntry.Text,
                    String.Format("{0:MM/dd/yyyy}", ProjectDate.Date),
                    ProjectType.SelectedItem.ToString()};

                    viewModel.AddProject(parameters);
                }
            });

            ProjectType.Items.Add("Enterprise Project");
            ProjectType.Items.Add("Task List Project");
            ProjectType.SelectedIndex = 0;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            System.Diagnostics.Debug.WriteLine("disappeared!");
            ProjectNameEntry.Text = "";
            ProjectDescriptionEntry.Text = "";
            ProjectDate.Date = DateTime.Now;
            ProjectType.SelectedIndex = 0;
        }
    }
}