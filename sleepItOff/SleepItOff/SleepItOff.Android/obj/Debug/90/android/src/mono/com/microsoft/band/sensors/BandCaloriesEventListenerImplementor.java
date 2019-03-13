package mono.com.microsoft.band.sensors;


public class BandCaloriesEventListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.microsoft.band.sensors.BandCaloriesEventListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onBandCaloriesChanged:(Lcom/microsoft/band/sensors/BandCaloriesEvent;)V:GetOnBandCaloriesChanged_Lcom_microsoft_band_sensors_BandCaloriesEvent_Handler:Microsoft.Band.Sensors.IBandCaloriesEventListenerInvoker, Microsoft.Band.Android\n" +
			"";
		mono.android.Runtime.register ("Microsoft.Band.Sensors.IBandCaloriesEventListenerImplementor, Microsoft.Band.Android", BandCaloriesEventListenerImplementor.class, __md_methods);
	}


	public BandCaloriesEventListenerImplementor ()
	{
		super ();
		if (getClass () == BandCaloriesEventListenerImplementor.class)
			mono.android.TypeManager.Activate ("Microsoft.Band.Sensors.IBandCaloriesEventListenerImplementor, Microsoft.Band.Android", "", this, new java.lang.Object[] {  });
	}


	public void onBandCaloriesChanged (com.microsoft.band.sensors.BandCaloriesEvent p0)
	{
		n_onBandCaloriesChanged (p0);
	}

	private native void n_onBandCaloriesChanged (com.microsoft.band.sensors.BandCaloriesEvent p0);

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
