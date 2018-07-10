package md514fa525f96795d5cde788849feca9bd7;


public class SplashscreenActivity
	extends android.support.v7.app.AppCompatActivity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("ProjectOnlineMobile2.Droid.SplashscreenActivity, ProjectOnlineMobile2.Droid", SplashscreenActivity.class, __md_methods);
	}


	public SplashscreenActivity ()
	{
		super ();
		if (getClass () == SplashscreenActivity.class)
			mono.android.TypeManager.Activate ("ProjectOnlineMobile2.Droid.SplashscreenActivity, ProjectOnlineMobile2.Droid", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);

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
