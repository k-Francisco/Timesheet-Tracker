﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;

namespace ProjectOnlineMobile2.Droid
{
    public class DialogHelper
    {
        private MainActivity _activity;
        public DialogHelper(MainActivity activity) {
            _activity = activity;
        }

        public void DisplayUserInfo(string userName, string userEmail)
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(_activity);
            alert.SetTitle(userName);
            alert.SetMessage(userEmail);
            alert.SetPositiveButton("Logout", (senderAlert, args) => {
                MessagingCenter.Instance.Send<String>("", "ClearAll");

                Intent intent = new Intent(_activity, typeof(LoginActivity));
                _activity.StartActivity(intent);
                _activity.Finish();
            });

            alert.SetNegativeButton("Close", (senderAlert, args) => {

            });

            Dialog dialog = alert.Create();
            dialog.Show();
        }

        public void DisplayPeriodDetails(string period)
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(_activity);
            alert.SetMessage(period);
            alert.SetPositiveButton("Change Period", (senderAlert, args) => {
                MessagingCenter.Instance.Send<String>("", "OpenPeriodPicker");
            });

            alert.SetNegativeButton("Close", (senderAlert, args) => {

            });

            Dialog dialog = alert.Create();
            dialog.Show();
        }

        public void DisplaySubmitTimesheetDialog()
        {
            EditText comment = new EditText(_activity);

            AlertDialog.Builder alert = new AlertDialog.Builder(_activity);
            alert.SetTitle("Comment");
            alert.SetView(comment);

            alert.SetPositiveButton("Submit", (senderAlert, args) => {
                MessagingCenter.Instance.Send<String>(comment.Text, "SubmitTimesheet");
            });

            alert.SetNegativeButton("Cancel", (senderAlert, args) => {

            });

            Dialog dialog = alert.Create();
            dialog.Show();
        }

        public void DisplayCreateTimesheetDialog(string message, string periodId, string positiveButton, string negativeButton)
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(_activity);
            alert.SetMessage(message);
            alert.SetPositiveButton(positiveButton, (senderAlert, args) => {
                MessagingCenter.Instance.Send<String>(periodId, "CreateTimesheet");
            });

            alert.SetNegativeButton(negativeButton, (senderAlert, args) => {

            });

            Dialog dialog = alert.Create();
            dialog.Show();
        }

    }
}