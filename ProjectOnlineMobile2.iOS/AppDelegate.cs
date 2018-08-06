using Foundation;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using SpevoCore.Services.Token_Service;
using System;
using UIKit;
using Xamarin.Forms;

namespace ProjectOnlineMobile2.iOS
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
	[Register("AppDelegate")]
	public class AppDelegate : UIApplicationDelegate
	{
        // class-level declarations
        public static AppDelegate appDelegate;
        public static UIStoryboard Storyboard = UIStoryboard.FromName("Main", null);
        public override UIWindow Window
		{
			get;
			set;
		}

        public string UserName, UserEmail, TimesheetPeriod;

		public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
		{
            AppCenter.Start("41b6b5a7-bb94-45c6-b42b-8f35f74f0376", typeof(Analytics), typeof(Crashes));

            Forms.Init();

            appDelegate = this;

            UINavigationBar.Appearance.SetTitleTextAttributes(new UITextAttributes()
            {
                TextColor = UIColor.White
            });
            UINavigationBar.Appearance.LargeTitleTextAttributes = new UIStringAttributes()
            {
                ForegroundColor = UIColor.White
            };
            UINavigationBar.Appearance.Translucent = false;
            UINavigationBar.Appearance.TintColor = UIColor.White;
            UINavigationBar.Appearance.BarTintColor = UIColor.FromRGBA(49, 117, 47, 1);
            UINavigationBar.Appearance.BackgroundColor = UIColor.FromRGBA(49, 117, 47, 1);

            MessagingCenter.Instance.Subscribe<ProjectOnlineMobile2.Models2.UserModel>(this, "UserInfo", (user)=> {
                UserName = user.UserName;
                UserEmail = user.UserEmail;
            });

            MessagingCenter.Instance.Subscribe<String>(this, "TimesheetPeriod", (tsp) => {
                TimesheetPeriod = tsp;
            });

            if (string.IsNullOrWhiteSpace(Settings.CookieString))
            {
                var controller = Storyboard.InstantiateViewController("LoginController") as LoginController;
                Window.RootViewController = controller;
                Window.MakeKeyAndVisible();
            }
            else
            {
                var controller = Storyboard.InstantiateViewController("TabBarController") as TabBarController;
                Window.RootViewController = controller;
                Window.MakeKeyAndVisible();
            }
            return true;
		}

		public override void OnResignActivation(UIApplication application)
		{
			// Invoked when the application is about to move from active to inactive state.
			// This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) 
			// or when the user quits the application and it begins the transition to the background state.
			// Games should use this method to pause the game.
		}

		public override void DidEnterBackground(UIApplication application)
		{
			// Use this method to release shared resources, save user data, invalidate timers and store the application state.
			// If your application supports background exection this method is called instead of WillTerminate when the user quits.
		}

		public override void WillEnterForeground(UIApplication application)
		{
			// Called as part of the transiton from background to active state.
			// Here you can undo many of the changes made on entering the background.
		}

		public override void OnActivated(UIApplication application)
		{
			// Restart any tasks that were paused (or not yet started) while the application was inactive. 
			// If the application was previously in the background, optionally refresh the user interface.
		}

		public override void WillTerminate(UIApplication application)
		{
			// Called when the application is about to terminate. Save data, if needed. See also DidEnterBackground.
		}
	}
}

