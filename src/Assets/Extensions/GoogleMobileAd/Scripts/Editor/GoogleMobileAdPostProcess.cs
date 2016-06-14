//#define CODE_DISABLED

using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Collections;

public class GoogleMobileAdPostProcess  {
	

	[PostProcessBuild(49)]
	public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject) {


		#if UNITY_IPHONE && !CODE_DISABLED


		string StoreKit = "StoreKit.framework";
		if(!ISDSettings.Instance.ContainsFreamworkWithName(StoreKit)) {
			ISD_Framework F =  new ISD_Framework();
			F.Name = StoreKit;
			ISDSettings.Instance.Frameworks.Add(F);
		}

		string CoreTelephony = "CoreTelephony.framework";
		if(!ISDSettings.Instance.ContainsFreamworkWithName(CoreTelephony)) {
			ISD_Framework F =  new ISD_Framework();
			F.Name = CoreTelephony;
			ISDSettings.Instance.Frameworks.Add(F);
		}

		string AdSupport = "AdSupport.framework";
		if(!ISDSettings.Instance.ContainsFreamworkWithName(AdSupport)) {
			ISD_Framework F =  new ISD_Framework();
			F.Name = AdSupport;
			ISDSettings.Instance.Frameworks.Add(F);
		}


		string MessageUI = "MessageUI.framework";
		if(!ISDSettings.Instance.ContainsFreamworkWithName(AdSupport)) {
			ISD_Framework F =  new ISD_Framework();
			F.Name = MessageUI;
			ISDSettings.Instance.Frameworks.Add(F);
		}
	

		string EventKit = "EventKit.framework";
		if(!ISDSettings.Instance.ContainsFreamworkWithName(AdSupport)) {
			ISD_Framework F =  new ISD_Framework();
			F.Name = EventKit;
			ISDSettings.Instance.Frameworks.Add(F);
		}

		string EventKitUI = "EventKitUI.framework";
		if(!ISDSettings.Instance.ContainsFreamworkWithName(EventKitUI)) {
			ISD_Framework F =  new ISD_Framework();
			F.Name = EventKitUI;
			ISDSettings.Instance.Frameworks.Add(F);
		}

		/*
		string linkerFlasg = "-ObjC";
		if(!ISDSettings.Instance.linkFlags.Contains(linkerFlasg)) {
			ISDSettings.Instance.linkFlags.Add(linkerFlasg);
		}
		*/



		#endif
	}

}
