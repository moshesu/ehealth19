package md5f911216a49f3643a985d6c4c7bdf5abb;


public class BandClientExtensions_BandConnectionCallback
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.microsoft.band.BandConnectionCallback
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onStateChanged:(Lcom/microsoft/band/ConnectionState;)V:GetOnStateChanged_Lcom_microsoft_band_ConnectionState_Handler:Microsoft.Band.IBandConnectionCallbackInvoker, Microsoft.Band.Android\n" +
			"";
		mono.android.Runtime.register ("Microsoft.Band.BandClientExtensions+BandConnectionCallback, Microsoft.Band.Android", BandClientExtensions_BandConnectionCallback.class, __md_methods);
	}


	public BandClientExtensions_BandConnectionCallback ()
	{
		super ();
		if (getClass () == BandClientExtensions_BandConnectionCallback.class)
			mono.android.TypeManager.Activate ("Microsoft.Band.BandClientExtensions+BandConnectionCallback, Microsoft.Band.Android", "", this, new java.lang.Object[] {  });
	}


	public void onStateChanged (com.microsoft.band.ConnectionState p0)
	{
		n_onStateChanged (p0);
	}

	private native void n_onStateChanged (com.microsoft.band.ConnectionState p0);

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
