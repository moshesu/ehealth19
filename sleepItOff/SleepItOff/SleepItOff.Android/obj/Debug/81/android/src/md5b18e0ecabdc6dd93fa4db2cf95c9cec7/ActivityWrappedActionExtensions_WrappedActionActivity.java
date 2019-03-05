package md5b18e0ecabdc6dd93fa4db2cf95c9cec7;


public class ActivityWrappedActionExtensions_WrappedActionActivity
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"n_onDestroy:()V:GetOnDestroyHandler\n" +
			"";
		mono.android.Runtime.register ("Microsoft.Band.Portable.ActivityWrappedActionExtensions+WrappedActionActivity, Microsoft.Band.Portable", ActivityWrappedActionExtensions_WrappedActionActivity.class, __md_methods);
	}


	public ActivityWrappedActionExtensions_WrappedActionActivity ()
	{
		super ();
		if (getClass () == ActivityWrappedActionExtensions_WrappedActionActivity.class)
			mono.android.TypeManager.Activate ("Microsoft.Band.Portable.ActivityWrappedActionExtensions+WrappedActionActivity, Microsoft.Band.Portable", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);


	public void onDestroy ()
	{
		n_onDestroy ();
	}

	private native void n_onDestroy ();

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
