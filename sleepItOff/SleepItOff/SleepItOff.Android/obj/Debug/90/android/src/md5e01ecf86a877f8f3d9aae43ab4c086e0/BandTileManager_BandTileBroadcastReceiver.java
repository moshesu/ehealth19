package md5e01ecf86a877f8f3d9aae43ab4c086e0;


public class BandTileManager_BandTileBroadcastReceiver
	extends android.content.BroadcastReceiver
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onReceive:(Landroid/content/Context;Landroid/content/Intent;)V:GetOnReceive_Landroid_content_Context_Landroid_content_Intent_Handler\n" +
			"";
		mono.android.Runtime.register ("Microsoft.Band.Portable.Tiles.BandTileManager+BandTileBroadcastReceiver, Microsoft.Band.Portable", BandTileManager_BandTileBroadcastReceiver.class, __md_methods);
	}


	public BandTileManager_BandTileBroadcastReceiver ()
	{
		super ();
		if (getClass () == BandTileManager_BandTileBroadcastReceiver.class)
			mono.android.TypeManager.Activate ("Microsoft.Band.Portable.Tiles.BandTileManager+BandTileBroadcastReceiver, Microsoft.Band.Portable", "", this, new java.lang.Object[] {  });
	}


	public void onReceive (android.content.Context p0, android.content.Intent p1)
	{
		n_onReceive (p0, p1);
	}

	private native void n_onReceive (android.content.Context p0, android.content.Intent p1);

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
