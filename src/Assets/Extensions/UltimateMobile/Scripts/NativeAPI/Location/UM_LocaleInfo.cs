using UnityEngine;
using System.Collections;

public class UM_LocaleInfo {

	private ISN_Locale _IOSLocale;
	private AN_Locale _ANLocale;

	private UM_LocaleInfo(){}

	public UM_LocaleInfo(ISN_Locale locale) {
		_IOSLocale= locale;
	}

	public UM_LocaleInfo(AN_Locale locale) {
		_ANLocale = locale;
	}

	public string CountryCode {
		get {
			switch (Application.platform) {
			case RuntimePlatform.Android:
				return _ANLocale.CountryCode;
			case RuntimePlatform.IPhonePlayer:
				return _IOSLocale.CountryCode;
			}
			return string.Empty;
		}
	}
	
	public string DisplayCountry {
		get {
			switch (Application.platform) {
			case RuntimePlatform.Android:
				return _ANLocale.DisplayCountry;
			case RuntimePlatform.IPhonePlayer:
				return _IOSLocale.DisplayCountry;
			}
			return string.Empty;
		}
	}
	
	public string LanguageCode {
		get {
			switch (Application.platform) {
			case RuntimePlatform.Android:
				return _ANLocale.LanguageCode;
			case RuntimePlatform.IPhonePlayer:
				return _IOSLocale.LanguageCode;
			}
			return string.Empty;
		}
	}
	
	public string DisplayLanguage {
		get {
			switch (Application.platform) {
			case RuntimePlatform.Android:
				return _ANLocale.DisplayLanguage;
			case RuntimePlatform.IPhonePlayer:
				return _IOSLocale.DisplayLanguage;
			}
			return string.Empty;
		}
	}

}
