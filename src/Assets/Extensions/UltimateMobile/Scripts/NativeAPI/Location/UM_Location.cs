using UnityEngine;
using System;
using System.Collections;

public class UM_Location : SA_Singleton<UM_Location> {

	public static event Action<UM_LocaleInfo> OnLocaleLoaded = delegate{};

	public void GetLocale() {
		switch (Application.platform) {
		case RuntimePlatform.IPhonePlayer:
			IOSNativeUtility.OnLocaleLoaded += HandleOnLocaleLoaded_IOS;
			IOSNativeUtility.Instance.GetLocale();
			break;
		case RuntimePlatform.Android:
			AndroidNativeUtility.LocaleInfoLoaded += HandleLocaleInfoLoaded_Android;
			AndroidNativeUtility.Instance.LoadLocaleInfo();
			break;
		}
	}

	void HandleLocaleInfoLoaded_Android (AN_Locale locale)
	{
		AndroidNativeUtility.LocaleInfoLoaded -= HandleLocaleInfoLoaded_Android;
		OnLocaleLoaded(new UM_LocaleInfo(locale));
	}

	void HandleOnLocaleLoaded_IOS (ISN_Locale locale)
	{
		IOSNativeUtility.OnLocaleLoaded -= HandleOnLocaleLoaded_IOS;
		OnLocaleLoaded(new UM_LocaleInfo(locale));
	}

}
