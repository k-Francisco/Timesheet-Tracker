package md5118534af8a7b4ed44d8122ee7f7f80e1;


public class CustomWebView
	extends android.webkit.WebViewClient
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onPageStarted:(Landroid/webkit/WebView;Ljava/lang/String;Landroid/graphics/Bitmap;)V:GetOnPageStarted_Landroid_webkit_WebView_Ljava_lang_String_Landroid_graphics_Bitmap_Handler\n" +
			"n_onPageFinished:(Landroid/webkit/WebView;Ljava/lang/String;)V:GetOnPageFinished_Landroid_webkit_WebView_Ljava_lang_String_Handler\n" +
			"";
		mono.android.Runtime.register ("ProjectOnlineMobile2.Android.Helpers.CustomWebView, ProjectOnlineMobile2.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", CustomWebView.class, __md_methods);
	}


	public CustomWebView ()
	{
		super ();
		if (getClass () == CustomWebView.class)
			mono.android.TypeManager.Activate ("ProjectOnlineMobile2.Android.Helpers.CustomWebView, ProjectOnlineMobile2.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public CustomWebView (md5768ea8aa860a9e68b0c928544b06741b.LoginActivity p0)
	{
		super ();
		if (getClass () == CustomWebView.class)
			mono.android.TypeManager.Activate ("ProjectOnlineMobile2.Android.Helpers.CustomWebView, ProjectOnlineMobile2.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "ProjectOnlineMobile2.Android.Activities.LoginActivity, ProjectOnlineMobile2.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", this, new java.lang.Object[] { p0 });
	}


	public void onPageStarted (android.webkit.WebView p0, java.lang.String p1, android.graphics.Bitmap p2)
	{
		n_onPageStarted (p0, p1, p2);
	}

	private native void n_onPageStarted (android.webkit.WebView p0, java.lang.String p1, android.graphics.Bitmap p2);


	public void onPageFinished (android.webkit.WebView p0, java.lang.String p1)
	{
		n_onPageFinished (p0, p1);
	}

	private native void n_onPageFinished (android.webkit.WebView p0, java.lang.String p1);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}