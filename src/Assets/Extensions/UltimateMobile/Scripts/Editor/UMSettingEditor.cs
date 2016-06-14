using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;

[CustomEditor(typeof(UltimateMobileSettings))]
public class UMSettingEditor : Editor {
	
	
	GUIContent SdkVersion   = new GUIContent("Plugin Version [?]", "This is Plugin version.  If you have problems or compliments please include this so we know exactly what version to look out for.");

	GUIContent AutoLoadSmallImagesLoadTitle  = new GUIContent("Autoload Small Player Photo[?]:", "As soon as player info received, small player photo will be requested automatically");
	GUIContent AutoLoadBigmagesLoadTitle  = new GUIContent("Autoload Big Player Photo[?]:", "As soon as player info received, big player photo will be requested automatically");
	GUIContent AutoLoadLeaderboardsInfoTitle  = new GUIContent("Autoload Leaderboards Info[?]:", "Player connected event will not be fired until current player score for all leaderbaords received.");
	GUIContent AutoLoadAchievementsInfoTitle  = new GUIContent("Autoload Achievements Info[?]:", "Player connected event will not be fired until current player achievements info received.");

	// near the top of the script
	GUIContent AINCREMENTAL = new GUIContent("Incremental[?]:", "Does this achievement require multiple steps to unlock?");
	GUIContent ASTEPS = new GUIContent("Achievement Steps[?]:", "Total steps needed to unlock achievement.");
	
	private UltimateMobileSettings settings;
	
	
	void Awake() {
		

		
		#if !UNITY_WEBPLAYER
		UpdatePluginSettings();
		#endif
		
	}
	
	
	public override void OnInspectorGUI() {
		settings = UltimateMobileSettings.Instance;
		
		GUI.changed = false;
		
		InstallOptions();
		GUILayoutOption[] toolbarSize = new GUILayoutOption[]{GUILayout.Width(Width-5), GUILayout.Height(30)};
		
		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		
		UltimateMobileSettings.Instance.ToolbarSelectedIndex =  GUILayout.Toolbar(UltimateMobileSettings.Instance.ToolbarSelectedIndex, ToolbarImages, toolbarSize);
		GUILayout.FlexibleSpace();
		
		EditorGUILayout.EndHorizontal();
		
		switch(UltimateMobileSettings.Instance.ToolbarSelectedIndex) {
		case 0:
			Actions();
			PluginSettings();
			EditorGUILayout.Space();
			AboutGUI();
			break;
			
		case 1:
			GameServiceSettings();
			break;
			
		case 2:
			InAppSettings();
			break;
			
		case 3:
			AdSettings();
			break;
			
		case 4:
			GoogleAnalyticsSettingsEditor.DrawAnalyticsSettings();
			break;
		case 5:
			OtherSettings ();
			break;
		}
		
		
		if(GUI.changed) {
			DirtyEditor();
		}

	}
	
	
	GUIContent LID = new GUIContent("Leaderboard Id[?]:", "Uniquie Leaderboard Id");
	GUIContent IOSLID = new GUIContent("IOS Leaderboard Id[?]:", "IOS Leaderboard Id");
	GUIContent ANDROIDLID = new GUIContent("Android Leaderboard Id[?]:", "Android Leaderboard Id");
	
	
	
	GUIContent AID = new GUIContent("Achievement Id[?]:", "Uniquie Leaderboard Id");
	GUIContent ALID = new GUIContent("IOS Achievement Id[?]:", "IOS Leaderboard Id");
	GUIContent ANDROIDAID = new GUIContent("Android Achievement Id[?]:", "Android Leaderboard Id");
	
	
	GUIContent Base64KeyLabel = new GUIContent("Base64 Key[?]:", "Base64 Key app key.");
	GUIContent InAppID = new GUIContent("ProductId[?]:", "UniquieProductId");
	GUIContent IsCons = new GUIContent("Is Consumable[?]:", "Is prodcut allowed to be purchased more than once?");
	
	GUIContent IOSSKU = new GUIContent("IOS SKU[?]:", "IOS SKU");
	GUIContent AndroidSKU = new GUIContent("Android SKU[?]:", "Android SKU");
	GUIContent WP8SKU = new GUIContent("WP8 SKU[?]:", "WP8 SKU");
	GUIContent DefaultPrice = new GUIContent("Default Price[?] :", "This Price is used, when Product Templates are NOT loaded and there is NO Internet connection");	
	


	private Texture[] _ToolbarImages = null;
	
	public Texture[] ToolbarImages {
		get {
			if(_ToolbarImages == null) {
				Texture2D um_billing =  Resources.Load("um_billing") as Texture2D;
				Texture2D um_games =  Resources.Load("um_games") as Texture2D;
				Texture2D um_ad =  Resources.Load("um_ad") as Texture2D;
				Texture2D um_analytics =  Resources.Load("um_analytics") as Texture2D;
				Texture2D um_settings =  Resources.Load("um_settings") as Texture2D;
				
				Texture2D um_icon =  Resources.Load("um_icon") as Texture2D;
				
				List<Texture2D> textures =  new List<Texture2D>();
				textures.Add(um_icon);
				textures.Add(um_games);
				textures.Add(um_billing);
				textures.Add(um_ad);
				textures.Add(um_analytics);
				textures.Add(um_settings);
				
				
				_ToolbarImages = textures.ToArray();
				
			}
			return _ToolbarImages;
		}
	}

	private int _Width = 500;
	public int Width {
		get {
			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			Rect scale = GUILayoutUtility.GetLastRect();
			
			if(scale.width != 1) {
				_Width = System.Convert.ToInt32(scale.width);
			}

			
			return _Width;
		}
	}




	public static bool IsInstalled {
		get {
			return SA_VersionsManager.Is_UM_Installed;
		}
	}

	public static bool IsUpToDate {
		get {
			if(CurrentVersion == SA_VersionsManager.UM_Version) {
				return true;
			} else {
				return false;
			}
		}
	}

	public static int CurrentVersion {
		get {
			return SA_VersionsManager.ParceVersion(UltimateMobileSettings.VERSION_NUMBER);
		}
	}
	
	public static int CurrentMagorVersion {
		get {
			return SA_VersionsManager.ParceMagorVersion(UltimateMobileSettings.VERSION_NUMBER);
		}
	}
	
	public static void InstallUltimateMobile() {
		
		PluginsInstalationUtil.Android_InstallPlugin();
		PluginsInstalationUtil.IOS_InstallPlugin();
		UpdateGoogleAdIOSAPI();
		
		
		FileStaticAPI.Write(SA_VersionsManager.UM_VERSION_INFO_PATH, UltimateMobileSettings.VERSION_NUMBER);
		AndroidNativeSettingsEditor.UpdateVersionInfo();
		GoogleMobileAdSettingsEditor.UpdateVersionInfo ();
		IOSNativeSettingsEditor.UpdateVersionInfo ();
		UpdatePluginSettings();
	}


	
	private void InstallOptions() {

		
		if(!IsInstalled) {
			EditorGUILayout.BeginVertical (GUI.skin.box);
			EditorGUILayout.HelpBox("Install Required ", MessageType.Error);
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.Space();
			Color c = GUI.color;
			GUI.color = Color.cyan;
			if(GUILayout.Button("Install Plugin",  GUILayout.Width(350))) {
				InstallUltimateMobile();
			}
			
			EditorGUILayout.Space();
			GUI.color = c;
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Space();
			EditorGUILayout.EndVertical();
			EditorGUILayout.Space();

			UltimateMobileSettings.Instance.ToolbarSelectedIndex = 0;
			
		}
		
		if(IsInstalled) {
			if(!IsUpToDate) {
				EditorGUILayout.BeginVertical (GUI.skin.box);
				EditorGUILayout.HelpBox("Update Required \nResources version: " + SA_VersionsManager.UM_StringVersionId + " Plugin version: " + UltimateMobileSettings.VERSION_NUMBER, MessageType.Warning);

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.Space();
				Color c = GUI.color;
				GUI.color = Color.cyan;
				
				
				
				if(CurrentMagorVersion != SA_VersionsManager.UM_MagorVersion) {
					if(GUILayout.Button("How to update",  GUILayout.Width(350))) {
						Application.OpenURL("https://goo.gl/LNxbz6");
					}
				} else {
					if(GUILayout.Button("Upgrade Resources",  GUILayout.Width(350))) {
						InstallUltimateMobile();
					}
				}
				
				
				GUI.color = c;
				EditorGUILayout.Space();
				EditorGUILayout.EndHorizontal();
				
				EditorGUILayout.Space();
				
				EditorGUILayout.EndVertical();
				EditorGUILayout.Space();

				UltimateMobileSettings.Instance.ToolbarSelectedIndex = 0;

			} else {
				EditorGUILayout.HelpBox("Ultimate Mobile Plugin v" + UltimateMobileSettings.VERSION_NUMBER + " is installed", MessageType.Info);
			}
		}
		
		
		EditorGUILayout.Space();
		
	}
	


	private void PluginSettings() {

		EditorGUILayout.Space();
		EditorGUILayout.HelpBox("Components Settings", MessageType.None);
	
			
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.Space();
		
		if(GUILayout.Button("Android Settings ",  GUILayout.Width(140))) {
			Selection.activeObject = AndroidNativeSettings.Instance;
		}
		
		if(GUILayout.Button("IOS Settings ",  GUILayout.Width(140))) {
			Selection.activeObject = IOSNativeSettings.Instance;
		}

		if(GUILayout.Button("WP8 Settings ",  GUILayout.Width(140))) {
			Selection.activeObject = WP8NativeSettings.Instance;
		}


		EditorGUILayout.EndHorizontal();
		EditorGUILayout.Space();
		
		
		
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.Space();
		if(GUILayout.Button("Google Analytics ",  GUILayout.Width(140))) {
			Selection.activeObject = GoogleAnalyticsSettings.Instance;
		}
		
		if(GUILayout.Button("Google Mobile Ad  ",  GUILayout.Width(140))) {
			Selection.activeObject = GoogleMobileAdSettings.Instance;
		}

		if(GUILayout.Button("Player Settings",  GUILayout.Width(140))) {
			EditorApplication.ExecuteMenuItem("Edit/Project Settings/Player");
		}
		
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.Space();
		
		
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.Space();
		
		
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.Space();

	}
	
	private void Actions() {

		EditorGUILayout.Space();
		EditorGUILayout.HelpBox("More Actions", MessageType.None);

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.Space();
		
		if(GUILayout.Button("Open Manifest ",  GUILayout.Width(140))) {
			UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal("Assets" + AN_ManifestManager.MANIFEST_FILE_PATH, 1);
		}
		
		if(GUILayout.Button("Reinstall ",  GUILayout.Width(140))) {
			InstallUltimateMobile();
		}

		EditorGUILayout.EndHorizontal();
		EditorGUILayout.Space();
		
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.Space();
		
		if(GUILayout.Button("Load Example Settings",  GUILayout.Width(140))) {
			
			UltimateMobileSettings.Instance.Leaderboards.Clear();
			
			UM_Leaderboard lb = new UM_Leaderboard();
			lb.id = "LeaderBoardSample_1";
			lb.AndroidId = "CgkIipfs2qcGEAIQAA";
			UltimateMobileSettings.Instance.Leaderboards.Add(lb);
			
			
			lb = new UM_Leaderboard();
			lb.id = "LeaderBoardSample_2";
			lb.AndroidId = "CgkIipfs2qcGEAIQFQ";
			UltimateMobileSettings.Instance.Leaderboards.Add(lb);
			
			
			settings.InAppProducts.Clear();
			
			UM_InAppProduct p;
			
			p =  new UM_InAppProduct();
			p.id = "coins_bonus";
			p.IOSId = "purchase.example.coins_bonus";
			p.AndroidId = "coins_bonus";
			p.IsConsumable = false;
			
			settings.AddProduct(p);
			
			
			p =  new UM_InAppProduct();
			p.id = "coins_pack";
			p.IOSId = "purchase.example.small_coins_bag";
			p.AndroidId = "pm_coins";
			p.IsConsumable = true;
			
			settings.AddProduct(p);
			
			
			#if UNITY_IOS || UNITY_IPHONE
			
			PlayerSettings.bundleIdentifier = "com.iosnative.preview";
			
			
			#endif
			
			#if UNITY_ANDROID
			
			PlayerSettings.bundleIdentifier = "com.unionassets.android.plugin.preview";
			
			
			#endif
			
			AndroidNativeSettingsEditor.LoadExampleSettings();				
		}		

		if(GUILayout.Button("Remove",  GUILayout.Width(140))) {
			SA_RemoveTool.RemovePlugins();				
		}

		EditorGUILayout.EndHorizontal();
	}
	
	
	public static void UpdateGoogleAdIOSAPI(bool forseDisable = false) {
		
		bool IsEnabled = false; 
		
		if(!forseDisable) {
			if(UltimateMobileSettings.Instance.IOSAdEdngine == UM_IOSAdEngineOprions.GoogleMobileAd) {
				IsEnabled = true;
			}
		}
		
		
		
		
		
		string IOSADBannerContent = FileStaticAPI.Read("Extensions/GoogleMobileAd/Scripts/IOS/IOSADBanner.cs");
		string IOSAdMobControllerContent = FileStaticAPI.Read("Extensions/GoogleMobileAd/Scripts/IOS/IOSAdMobController.cs");
		string GoogleMobileAdPostProcessContent = FileStaticAPI.Read("Extensions/GoogleMobileAd/Scripts/Editor/GoogleMobileAdPostProcess.cs");
		string ScarletPostProcessorContent = FileStaticAPI.Read("Extensions/GoogleMobileAd/Scripts/Editor/ScarletPostProcessor.cs");


		
		if(IsEnabled)  {
			IOSADBannerContent = IOSADBannerContent.Replace("#define CODE_DISABLED", "//#define CODE_DISABLED");
			IOSAdMobControllerContent = IOSAdMobControllerContent.Replace("#define CODE_DISABLED", "//#define CODE_DISABLED");
			GoogleMobileAdPostProcessContent = GoogleMobileAdPostProcessContent.Replace("#define CODE_DISABLED", "//#define CODE_DISABLED");
			ScarletPostProcessorContent = ScarletPostProcessorContent.Replace("#define CODE_DISABLED", "//#define CODE_DISABLED");

			PluginsInstalationUtil.InstallGMAPart();

		} else {
			IOSADBannerContent = IOSADBannerContent.Replace("//#define CODE_DISABLED", "#define CODE_DISABLED");
			IOSAdMobControllerContent = IOSAdMobControllerContent.Replace("//#define CODE_DISABLED", "#define CODE_DISABLED");
			GoogleMobileAdPostProcessContent = GoogleMobileAdPostProcessContent.Replace("//#define CODE_DISABLED", "#define CODE_DISABLED");
			ScarletPostProcessorContent = ScarletPostProcessorContent.Replace("//#define CODE_DISABLED", "#define CODE_DISABLED");


			FileStaticAPI.DeleteFile(PluginsInstalationUtil.IOS_DESTANATION_PATH + "GMA_SA_Lib_Proxy.mm");
			FileStaticAPI.DeleteFile(PluginsInstalationUtil.IOS_DESTANATION_PATH + "GMA_SA_Lib.h");
			FileStaticAPI.DeleteFile(PluginsInstalationUtil.IOS_DESTANATION_PATH + "GMA_SA_Lib.m");
		}
		
		FileStaticAPI.Write("Extensions/GoogleMobileAd/Scripts/IOS/IOSADBanner.cs", IOSADBannerContent);
		FileStaticAPI.Write("Extensions/GoogleMobileAd/Scripts/IOS/IOSAdMobController.cs", IOSAdMobControllerContent);
		FileStaticAPI.Write("Extensions/GoogleMobileAd/Scripts/Editor/GoogleMobileAdPostProcess.cs", GoogleMobileAdPostProcessContent);
		FileStaticAPI.Write("Extensions/GoogleMobileAd/Scripts/Editor/ScarletPostProcessor.cs", ScarletPostProcessorContent);
		


	}
	
	
	private string GetKeyForValue(Dictionary<string, string> dictionary, string value) {
		foreach (KeyValuePair<string, string> pair in dictionary) {
			if (pair.Value.Equals(value)) {
				return pair.Key;
			}
		}
		return string.Empty;
	}

	GUIContent LeaderboardDescriptionLabel 	= new GUIContent("Description[?]:", "This is the description of the Leaderboard. The description cannot be longer than 255 bytes.");
	GUIContent AchievementDescriptionLabel 	= new GUIContent("Description[?]:", "This is the description of the Achievement. The description cannot be longer than 255 bytes.");

	private GK_AchievementTemplate GetIOSAchievement(string id) {
		foreach (GK_AchievementTemplate ach in IOSNativeSettings.Instance.Achievements) {
			if (ach.Id.Equals(id)) {
				return ach;
			}
		}
		GK_AchievementTemplate a = new GK_AchievementTemplate(){Id = id};
		IOSNativeSettings.Instance.Achievements.Add(a);
		return a;
	}

	private GPAchievement GetAndroidAchievement(string id) {
		foreach (GPAchievement ach in AndroidNativeSettings.Instance.Achievements) {
			if (ach.Id.Equals(id)) {
				return ach;
			}
		}
		GPAchievement a = new GPAchievement(id, string.Empty);
		AndroidNativeSettings.Instance.Achievements.Add(a);
		return a;
	}

	private GK_Leaderboard GetIOSLeaderboard(string id) {
		foreach (GK_Leaderboard lb in IOSNativeSettings.Instance.Leaderboards) {
			if (lb.Id.Equals(id)) {
				return lb;
			}
		}
		GK_Leaderboard l = new GK_Leaderboard(id);
		IOSNativeSettings.Instance.Leaderboards.Add(l);
		return l;
	}
	
	private GPLeaderBoard GetAndroidLeaderboard(string id) {
		foreach (GPLeaderBoard lb in AndroidNativeSettings.Instance.Leaderboards) {
			if (lb.Id.Equals(id)) {
				return lb;
			}
		}
		GPLeaderBoard l = new GPLeaderBoard(id, string.Empty);
		AndroidNativeSettings.Instance.Leaderboards.Add(l);
		return l;
	}

	private bool DrawSortingButtons(object currentObject, IList ObjectsList,
	                                object androidObject, IList androidObjectsList,
	                                object iosObject, IList iosObjectsList) {
		
		int ObjectIndex = ObjectsList.IndexOf(currentObject);
		if(ObjectIndex == 0) {
			GUI.enabled = false;
		} 
		
		bool up 		= GUILayout.Button("↑", EditorStyles.miniButtonLeft, GUILayout.Width(20));
		if(up) {
			object c = currentObject;
			ObjectsList[ObjectIndex]  		= ObjectsList[ObjectIndex - 1];
			ObjectsList[ObjectIndex - 1] 	=  c;

			c = androidObject;
			androidObjectsList[ObjectIndex]  		= androidObjectsList[ObjectIndex - 1];
			androidObjectsList[ObjectIndex - 1] 	=  c;

			c = iosObject;
			iosObjectsList[ObjectIndex]  		= iosObjectsList[ObjectIndex - 1];
			iosObjectsList[ObjectIndex - 1] 	=  c;
		}
		
		
		if(ObjectIndex >= ObjectsList.Count -1) {
			GUI.enabled = false;
		} else {
			GUI.enabled = true;
		}
		
		bool down 		= GUILayout.Button("↓", EditorStyles.miniButtonMid, GUILayout.Width(20));
		if(down) {
			object c = currentObject;
			ObjectsList[ObjectIndex] =  ObjectsList[ObjectIndex + 1];
			ObjectsList[ObjectIndex + 1] = c;

			c = androidObject;
			androidObjectsList[ObjectIndex] =  androidObjectsList[ObjectIndex + 1];
			androidObjectsList[ObjectIndex + 1] = c;

			c = iosObject;
			iosObjectsList[ObjectIndex] =  iosObjectsList[ObjectIndex + 1];
			iosObjectsList[ObjectIndex + 1] = c;
		}
		
		
		GUI.enabled = true;
		bool r 			= GUILayout.Button("-", EditorStyles.miniButtonRight, GUILayout.Width(20));
		if(r) {
			ObjectsList.Remove(currentObject);
			androidObjectsList.Remove(androidObject);
			iosObjectsList.Remove(iosObject);
		}
		
		return r;
	}

	private void GameServiceSettings() {
		EditorGUILayout.Space();
		EditorGUI.indentLevel = 0;

				
		Dictionary<string, string> resources = new Dictionary<string, string>();
		if (FileStaticAPI.IsFileExists ("Plugins/Android/AN_Res/res/values/ids.xml")) {
			//Parse XML file with PlayService Settings ID's
			XmlDocument doc = new XmlDocument();
			doc.Load(Application.dataPath + "/Plugins/Android/AN_Res/res/values/ids.xml");
			
			XmlNode rootResourcesNode = doc.DocumentElement;						
			foreach(XmlNode chn in rootResourcesNode.ChildNodes) {
				if (chn.Name.Equals("string")) {
					if (chn.Attributes["name"] != null) {
						if (!chn.Attributes["name"].Value.Equals("app_id")) {
							resources.Add(chn.Attributes["name"].Value, chn.InnerText);
						}
					}
				}
			}
		}

		EditorGUILayout.HelpBox("Leaderboards Info", MessageType.None);

		EditorGUI.indentLevel++; {
			EditorGUILayout.BeginVertical (GUI.skin.box);

			settings.IsLeaderBoardsOpen = EditorGUILayout.Foldout(settings.IsLeaderBoardsOpen, "Leaderboards");
			if(settings.IsLeaderBoardsOpen) {
				foreach(UM_Leaderboard leaderbaord in settings.Leaderboards) {
					GPLeaderBoard gpLb = GetAndroidLeaderboard(leaderbaord.AndroidId);
					GK_Leaderboard gkLb = GetIOSLeaderboard(leaderbaord.IOSId);

					EditorGUILayout.BeginVertical (GUI.skin.box);
					EditorGUILayout.BeginHorizontal();

					GUIStyle s =  new GUIStyle();
					s.padding =  new RectOffset();
					s.margin =  new RectOffset();
					s.border =  new RectOffset();
					
					if(leaderbaord.Texture != null) {
						GUILayout.Box(leaderbaord.Texture, s, new GUILayoutOption[]{GUILayout.Width(18), GUILayout.Height(18)});
					}

					leaderbaord.IsOpen = EditorGUILayout.Foldout(leaderbaord.IsOpen, leaderbaord.id);
					bool ItemWasRemoved = DrawSortingButtons((object) leaderbaord, settings.Leaderboards,
					                                         (object) gpLb, AndroidNativeSettings.Instance.Leaderboards,
					                                         (object) gkLb, IOSNativeSettings.Instance.Leaderboards);
					if(ItemWasRemoved) {
						return;
					}
					EditorGUILayout.EndHorizontal();

					if(leaderbaord.IsOpen) {
						EditorGUI.indentLevel++; {
							
							EditorGUILayout.BeginHorizontal();
							EditorGUILayout.LabelField(LID);
							leaderbaord.id	 	= EditorGUILayout.TextField(leaderbaord.id);
							if(leaderbaord.id.Length > 0) {
								leaderbaord.id		= leaderbaord.id.Trim();
							}
							gkLb.Info.Title = leaderbaord.id;
							gpLb.Name = leaderbaord.id;
							EditorGUILayout.EndHorizontal();
							
							EditorGUILayout.BeginHorizontal();
							EditorGUILayout.LabelField(IOSLID);
							leaderbaord.IOSId	 	= EditorGUILayout.TextField(leaderbaord.IOSId);
							if(leaderbaord.IOSId.Length > 0) {
								leaderbaord.IOSId 		= leaderbaord.IOSId.Trim();
							}
							EditorGUILayout.EndHorizontal();
							gkLb.Info.Identifier = leaderbaord.IOSId;
							
							EditorGUILayout.BeginHorizontal();
							EditorGUILayout.LabelField(ANDROIDLID);
							
							EditorGUI.BeginChangeCheck();
							
							bool doesntMatch = false;
							bool fileDoesntExists = false;
							
							string name = string.Empty;
							string[] names = new string[resources.Keys.Count + 1];
							names[0] = "[None]";
							resources.Keys.CopyTo(names, 1);
							List<string> listNames = new List<string>(names);
							if (leaderbaord.AndroidId.Equals(string.Empty)) {
								name = names[EditorGUILayout.Popup(0, names)];
							} else {
								if (FileStaticAPI.IsFileExists ("Plugins/Android/AN_Res/res/values/ids.xml")) {
									if (resources.ContainsValue(leaderbaord.AndroidId)) {
										name = names[EditorGUILayout.Popup(listNames.IndexOf(GetKeyForValue(resources, leaderbaord.AndroidId)), names)];
									} else {
										doesntMatch = true;
										name = names[EditorGUILayout.Popup(0, names)];
									}
								} else {
									fileDoesntExists = true;
								}
							}
							
							if (EditorGUI.EndChangeCheck()){
								if (!name.Equals("[None]")) {
									leaderbaord.AndroidId = resources[name];
								}
							}
							
							if(leaderbaord.AndroidId.Length > 0) {
								leaderbaord.AndroidId 		= leaderbaord.AndroidId.Trim();
							}
							gpLb.Id = leaderbaord.AndroidId;
							EditorGUILayout.EndHorizontal();
							
							EditorGUILayout.BeginHorizontal();
							if (fileDoesntExists) {
								EditorGUILayout.HelpBox("XML file with PlayService ID's DOESN'T exist", MessageType.Warning);
							}
							if (doesntMatch) {
								EditorGUILayout.HelpBox("Leaderboard ID doesn't match any PlayService ID of ids.xml file", MessageType.Warning);
							}
							EditorGUILayout.EndHorizontal();
							
							EditorGUILayout.Space();
							EditorGUILayout.Space();
							
							EditorGUILayout.BeginHorizontal();
							EditorGUILayout.LabelField(LeaderboardDescriptionLabel);
							EditorGUILayout.EndHorizontal();
							
							EditorGUILayout.BeginHorizontal();
							leaderbaord.Description	 = EditorGUILayout.TextArea(leaderbaord.Description,  new GUILayoutOption[]{GUILayout.Height(60), GUILayout.Width(200)} );
							gkLb.Info.Description = leaderbaord.Description;
							gpLb.Description = leaderbaord.Description;
							leaderbaord.Texture = (Texture2D) EditorGUILayout.ObjectField("", leaderbaord.Texture, typeof (Texture2D), false);
							gkLb.Info.Texture = leaderbaord.Texture;
							gpLb.Texture = leaderbaord.Texture;
							EditorGUILayout.EndHorizontal();
							EditorGUILayout.Space();
						} EditorGUI.indentLevel--;
					}
					EditorGUILayout.EndVertical();
				}
				
				
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.Space();
				if(GUILayout.Button("Add new", EditorStyles.miniButton, GUILayout.Width(250))) {
					UM_Leaderboard lb = new UM_Leaderboard();

					int index = 0;
					do {
						index++;
					} while (IsLeaderboardExists(lb.id + index.ToString()));
					lb.id = lb.id + index.ToString();
					lb.AndroidId = lb.id;
					lb.IOSId = lb.id;

					settings.AddLeaderboard(lb);
					AndroidNativeSettings.Instance.Leaderboards.Add(new GPLeaderBoard(lb.AndroidId, lb.id));
					GK_Leaderboard iOSLb = new GK_Leaderboard(lb.IOSId);
					iOSLb.Info.Title = lb.id;
					IOSNativeSettings.Instance.Leaderboards.Add(iOSLb);
				}
				EditorGUILayout.Space();
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.Space();
			}

			EditorGUILayout.EndVertical();

		}EditorGUI.indentLevel--; 


		EditorGUILayout.Space();
		EditorGUILayout.HelpBox("Achievements Info", MessageType.None);
	
		EditorGUI.indentLevel++; {
			
			EditorGUILayout.BeginVertical (GUI.skin.box);
		
			settings.IsAchievementsOpen = EditorGUILayout.Foldout(settings.IsAchievementsOpen, "Achievements");
			if(settings.IsAchievementsOpen) {

				
				foreach(UM_Achievement achievement in settings.Achievements) {
					GPAchievement gpAch = GetAndroidAchievement(achievement.AndroidId);
					GK_AchievementTemplate gkAch = GetIOSAchievement(achievement.IOSId);

					EditorGUILayout.BeginVertical (GUI.skin.box);

					EditorGUILayout.BeginHorizontal();
					
					GUIStyle s =  new GUIStyle();
					s.padding =  new RectOffset();
					s.margin =  new RectOffset();
					s.border =  new RectOffset();
					
					if(achievement.Texture != null) {
						GUILayout.Box(achievement.Texture, s, new GUILayoutOption[]{GUILayout.Width(18), GUILayout.Height(18)});
					}

					achievement.IsOpen = EditorGUILayout.Foldout(achievement.IsOpen, achievement.id);
					bool ItemWasRemoved = DrawSortingButtons((object) achievement, settings.Achievements,
					                                         (object) gpAch, AndroidNativeSettings.Instance.Achievements,
					                                         (object) gkAch, IOSNativeSettings.Instance.Achievements);
					if(ItemWasRemoved) {
						return;
					}
					EditorGUILayout.EndHorizontal();

					if(achievement.IsOpen) {
						EditorGUI.indentLevel++; {
							
							EditorGUILayout.BeginHorizontal();
							EditorGUILayout.LabelField(AID);
							achievement.id	 	= EditorGUILayout.TextField(achievement.id);
							if(achievement.id.Length > 0) {
								achievement.id 		= achievement.id.Trim();
							}
							gkAch.Title = achievement.id;
							gpAch.Name = achievement.id;
							EditorGUILayout.EndHorizontal();
							
							
							EditorGUILayout.BeginHorizontal();
							EditorGUILayout.LabelField(ALID);
							achievement.IOSId	 	= EditorGUILayout.TextField(achievement.IOSId);
							if(achievement.IOSId.Length > 0) {
								achievement.IOSId 		= achievement.IOSId.Trim();
							}
							gkAch.Id = achievement.IOSId;
							EditorGUILayout.EndHorizontal();								
							
							EditorGUILayout.BeginHorizontal();
							EditorGUILayout.LabelField(ANDROIDAID);
							
							EditorGUI.BeginChangeCheck();
							
							bool doesntMatch = false;
							bool fileDoesntExists = false;
							
							string name = string.Empty;
							string[] names = new string[resources.Keys.Count + 1];
							names[0] = "[None]";
							resources.Keys.CopyTo(names, 1);
							List<string> listNames = new List<string>(names);
							if (achievement.AndroidId.Equals(string.Empty)) {
								name = names[EditorGUILayout.Popup(0, names)];
							} else {
								if (FileStaticAPI.IsFileExists ("Plugins/Android/AN_Res/res/values/ids.xml")) {
									if (resources.ContainsValue(achievement.AndroidId)) {
										name = names[EditorGUILayout.Popup(listNames.IndexOf(GetKeyForValue(resources, achievement.AndroidId)), names)];
									} else {
										doesntMatch = true;
										name = names[EditorGUILayout.Popup(0, names)];
									}
								} else {
									fileDoesntExists = true;
								}
							}
							
							if (EditorGUI.EndChangeCheck()){
								if (!name.Equals("[None]")) {
									achievement.AndroidId = resources[name];
								}
							}
							
							if(achievement.AndroidId.Length > 0) {
								achievement.AndroidId 		= achievement.AndroidId.Trim();
							}
							gpAch.Id = achievement.AndroidId;
							EditorGUILayout.EndHorizontal();

							EditorGUILayout.BeginHorizontal();
							if (fileDoesntExists) {
								EditorGUILayout.HelpBox("XML file with PlayService ID's DOESN'T exist", MessageType.Warning);
							}
							if (doesntMatch) {
								EditorGUILayout.HelpBox("Achievement ID doesn't match any PlayService ID of ids.xml file", MessageType.Warning);
							}
							EditorGUILayout.EndHorizontal();

							EditorGUILayout.Space();
							EditorGUILayout.Space();

							// in the appropriate place within the GameServiceSettings() function.
							EditorGUILayout.BeginHorizontal();
							EditorGUILayout.LabelField(AINCREMENTAL);
							achievement.IsIncremental = EditorGUILayout.Toggle(achievement.IsIncremental);
							EditorGUILayout.EndHorizontal();
							
							if (achievement.IsIncremental) {
								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField(ASTEPS);
								achievement.Steps = EditorGUILayout.IntField(achievement.Steps);
								EditorGUILayout.EndHorizontal();
								
								if (achievement.Steps < 2) {
									EditorGUILayout.BeginHorizontal();
									EditorGUILayout.HelpBox("Number of steps must be a value between 2 and 10,000 (inclusive).", MessageType.Warning);
									EditorGUILayout.EndHorizontal();
								}
							}

							EditorGUILayout.Space();
							EditorGUILayout.Space();
							
							EditorGUILayout.BeginHorizontal();
							EditorGUILayout.LabelField(AchievementDescriptionLabel);
							EditorGUILayout.EndHorizontal();
							
							EditorGUILayout.BeginHorizontal();
							achievement.Description	 = EditorGUILayout.TextArea(achievement.Description,  new GUILayoutOption[]{GUILayout.Height(60), GUILayout.Width(200)} );
							gkAch.Description = achievement.Description;
							gpAch.Description = achievement.Description;
							achievement.Texture = (Texture2D) EditorGUILayout.ObjectField("", achievement.Texture, typeof (Texture2D), false);
							gkAch.Texture = achievement.Texture;
							gpAch.Texture = achievement.Texture;
							EditorGUILayout.EndHorizontal();
							EditorGUILayout.Space();
							
						} EditorGUI.indentLevel--;
					}
					EditorGUILayout.EndVertical();
				}
				
				
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.Space();
				if(GUILayout.Button("Add new", EditorStyles.miniButton, GUILayout.Width(250))) {
					UM_Achievement ac = new UM_Achievement();

					int index = 0;
					do {
						index++;
					} while (IsAchievementExists(ac.id + index.ToString()));
					ac.id = ac.id + index.ToString();
					ac.AndroidId = ac.id;
					ac.IOSId = ac.id;

					settings.AddAchievement(ac);
					AndroidNativeSettings.Instance.Achievements.Add(new GPAchievement(ac.AndroidId, ac.id));
					IOSNativeSettings.Instance.Achievements.Add(new GK_AchievementTemplate(){Id = ac.IOSId, Title = ac.id});
				}
				EditorGUILayout.Space();
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.Space();					
			}

			EditorGUILayout.EndVertical();
			
			
			
			
		}EditorGUI.indentLevel--;


		EditorGUILayout.Space();
		EditorGUILayout.HelpBox("Game Services API Settings", MessageType.None);


		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField(AutoLoadBigmagesLoadTitle);
		UltimateMobileSettings.Instance.AutoLoadUsersBigImages = EditorGUILayout.Toggle(UltimateMobileSettings.Instance.AutoLoadUsersBigImages);

		IOSNativeSettings.Instance.AutoLoadUsersBigImages = UltimateMobileSettings.Instance.AutoLoadUsersBigImages;
		AndroidNativeSettings.Instance.LoadProfileImages = UltimateMobileSettings.Instance.AutoLoadUsersBigImages;

		EditorGUILayout.EndHorizontal();
		
		
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField(AutoLoadSmallImagesLoadTitle);
		UltimateMobileSettings.Instance.AutoLoadUsersSmallImages = EditorGUILayout.Toggle(UltimateMobileSettings.Instance.AutoLoadUsersSmallImages);

		IOSNativeSettings.Instance.AutoLoadUsersSmallImages = UltimateMobileSettings.Instance.AutoLoadUsersSmallImages;
		AndroidNativeSettings.Instance.LoadProfileIcons = UltimateMobileSettings.Instance.AutoLoadUsersSmallImages;

		EditorGUILayout.EndHorizontal();


		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField(AutoLoadLeaderboardsInfoTitle);
		UltimateMobileSettings.Instance.AutoLoadLeaderboardsInfo = EditorGUILayout.Toggle(UltimateMobileSettings.Instance.AutoLoadLeaderboardsInfo);
		
		EditorGUILayout.EndHorizontal();


		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField(AutoLoadAchievementsInfoTitle);
		UltimateMobileSettings.Instance.AutoLoadAchievementsInfo = EditorGUILayout.Toggle(UltimateMobileSettings.Instance.AutoLoadAchievementsInfo);
		
		EditorGUILayout.EndHorizontal();
		
	}

	private bool IsLeaderboardExists(string id) {
		foreach (UM_Leaderboard lb in settings.Leaderboards) {
			if (lb.id.Equals(id)) {
				return true;
			}
		}
		return false;
	}

	private bool IsAchievementExists(string id) {
		foreach(UM_Achievement ac in settings.Achievements) {
			if (ac.id.Equals(id)) {
				return true;
			}
		}
		return false;
	}
	
	GUIContent IOS_UnitAdId  	 = new GUIContent("Banners Ad Unit Id [?]:",  "IOS Banners Ad Unit Id ");
	GUIContent IOS_InterstAdId   = new GUIContent("Interstitials Ad Unit Id [?]:", "IOS Interstitials Ad Unit Id");
	
	GUIContent Android_UnitAdId  	 = new GUIContent("Banners Ad Unit Id [?]:",  "Android Banners Ad Unit Id ");
	GUIContent Android_InterstAdId   = new GUIContent("Interstitials Ad Unit Id [?]:", "Android Interstitials Ad Unit Id");
	
	GUIContent WP8_UnitAdId  	 = new GUIContent("Banners Ad Unit Id [?]:",  "WP8 Banners Ad Unit Id ");
	GUIContent WP8_InterstAdId   = new GUIContent("Interstitials Ad Unit Id [?]:", "WP8 Interstitials Ad Unit Id");
	
	GUIContent Ad_Endgine   = new GUIContent("Ad Engine [?]:", "Ad Engine for seleceted platform");
	
	private void AdSettings() {



		EditorGUILayout.Space();
		EditorGUILayout.HelpBox("Advertisement", MessageType.None);
		EditorGUI.indentLevel = 0;



		EditorGUILayout.LabelField("IOS", EditorStyles.boldLabel);
		EditorGUI.indentLevel++; {


			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(Ad_Endgine);
			
			EditorGUI.BeginChangeCheck();
			settings.IOSAdEdngine = (UM_IOSAdEngineOprions) EditorGUILayout.EnumPopup(settings.IOSAdEdngine);
			
			if(EditorGUI.EndChangeCheck()) {
				UpdateGoogleAdIOSAPI();
			}
			
			
			EditorGUILayout.EndHorizontal();
			
			
			if(settings.IOSAdEdngine == UM_IOSAdEngineOprions.GoogleMobileAd) {
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField(IOS_UnitAdId);
				GoogleMobileAdSettings.Instance.IOS_BannersUnitId	 	= EditorGUILayout.TextField(GoogleMobileAdSettings.Instance.IOS_BannersUnitId);
				
				if(GoogleMobileAdSettings.Instance.IOS_BannersUnitId.Length > 0) {
					GoogleMobileAdSettings.Instance.IOS_BannersUnitId		= GoogleMobileAdSettings.Instance.IOS_BannersUnitId.Trim();
				}
				
				EditorGUILayout.EndHorizontal();
				
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField(IOS_InterstAdId);
				GoogleMobileAdSettings.Instance.IOS_InterstisialsUnitId	 	= EditorGUILayout.TextField(GoogleMobileAdSettings.Instance.IOS_InterstisialsUnitId);
				if(GoogleMobileAdSettings.Instance.IOS_InterstisialsUnitId.Length > 0) {
					GoogleMobileAdSettings.Instance.IOS_InterstisialsUnitId		= GoogleMobileAdSettings.Instance.IOS_InterstisialsUnitId.Trim();
				}
				
				EditorGUILayout.EndHorizontal();
			}
		} EditorGUI.indentLevel--;


		EditorGUILayout.LabelField("Android", EditorStyles.boldLabel);
		EditorGUI.indentLevel++; {

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(Android_UnitAdId);
			GoogleMobileAdSettings.Instance.Android_BannersUnitId	 	= EditorGUILayout.TextField(GoogleMobileAdSettings.Instance.Android_BannersUnitId);
			if(GoogleMobileAdSettings.Instance.Android_BannersUnitId.Length > 0) {
				GoogleMobileAdSettings.Instance.Android_BannersUnitId		= GoogleMobileAdSettings.Instance.Android_BannersUnitId.Trim();
			}
			
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(Android_InterstAdId);
			GoogleMobileAdSettings.Instance.Android_InterstisialsUnitId	 	= EditorGUILayout.TextField(GoogleMobileAdSettings.Instance.Android_InterstisialsUnitId);
			if(GoogleMobileAdSettings.Instance.Android_InterstisialsUnitId.Length > 0) {
				GoogleMobileAdSettings.Instance.Android_InterstisialsUnitId		= GoogleMobileAdSettings.Instance.Android_InterstisialsUnitId.Trim();
			}
			
			EditorGUILayout.EndHorizontal();

		} EditorGUI.indentLevel--;
			
			
		EditorGUILayout.LabelField("WP8", EditorStyles.boldLabel);
		EditorGUI.indentLevel++; {

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(Ad_Endgine);
			settings.WP8AdEdngine = (UM_WP8AdEngineOprions) EditorGUILayout.EnumPopup(settings.WP8AdEdngine);
			
			
			EditorGUILayout.EndHorizontal();
			
			
			if(settings.WP8AdEdngine == UM_WP8AdEngineOprions.GoogleMobileAd) {
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField(WP8_UnitAdId);
				GoogleMobileAdSettings.Instance.WP8_BannersUnitId	 	= EditorGUILayout.TextField(GoogleMobileAdSettings.Instance.WP8_BannersUnitId);
				if(GoogleMobileAdSettings.Instance.WP8_BannersUnitId.Length > 0) {
					GoogleMobileAdSettings.Instance.WP8_BannersUnitId		= GoogleMobileAdSettings.Instance.WP8_BannersUnitId.Trim();
				}
				
				EditorGUILayout.EndHorizontal();
				
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField(WP8_InterstAdId);
				GoogleMobileAdSettings.Instance.WP8_InterstisialsUnitId	 	= EditorGUILayout.TextField(GoogleMobileAdSettings.Instance.WP8_InterstisialsUnitId);
				if(GoogleMobileAdSettings.Instance.WP8_InterstisialsUnitId.Length > 0) {
					GoogleMobileAdSettings.Instance.WP8_InterstisialsUnitId		= GoogleMobileAdSettings.Instance.WP8_InterstisialsUnitId.Trim();
				}
				EditorGUILayout.EndHorizontal();
			}

		}EditorGUI.indentLevel--;
	}
	
	
	private void InAppSettings() {
		
		EditorGUI.indentLevel = 0;
		EditorGUILayout.Space();
		EditorGUILayout.HelpBox("In-App Purchases", MessageType.None);
		
		
		EditorGUI.indentLevel++; {
			
			EditorGUILayout.BeginVertical (GUI.skin.box);
			
			
			EditorGUI.indentLevel = 1;
			settings.IsInAppSettingsProductsOpen = EditorGUILayout.Foldout(settings.IsInAppSettingsProductsOpen, "Products");
			if(settings.IsInAppSettingsProductsOpen) {

				foreach(UM_InAppProduct p in settings.InAppProducts) {
					EditorGUILayout.BeginVertical (GUI.skin.box);
					
					
					p.IsOpen = EditorGUILayout.Foldout(p.IsOpen, p.id);
					if(p.IsOpen) {
						EditorGUI.indentLevel++;
						
						
						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.LabelField(InAppID);
						p.id	 	= EditorGUILayout.TextField(p.id);
						if(p.id.Length > 0) {
							p.id 		= p.id.Trim();
						}
						EditorGUILayout.EndHorizontal();
						
						
						
						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.LabelField(IsCons);
						p.IsConsumable	 	= EditorGUILayout.Toggle(p.IsConsumable);
						EditorGUILayout.EndHorizontal();
						EditorGUILayout.Space();
						
						
						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.LabelField(IOSSKU);
						p.IOSId	 	= EditorGUILayout.TextField(p.IOSId);
						
						if(p.IOSId.Length > 0) {
							p.IOSId 		= p.IOSId.Trim();
						}
						EditorGUILayout.EndHorizontal();
						
						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.LabelField(AndroidSKU);
						p.AndroidId	 	= EditorGUILayout.TextField(p.AndroidId);
						if(p.AndroidId.Length > 0) {
							p.AndroidId 		= p.AndroidId.Trim();
						}
						
						EditorGUILayout.EndHorizontal();
						
						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.LabelField(WP8SKU);
						p.WP8Id	 	= EditorGUILayout.TextField(p.WP8Id);
						
						if(p.WP8Id.Length > 0) {
							p.WP8Id 		= p.WP8Id.Trim();
						}
						EditorGUILayout.EndHorizontal();
						
						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.LabelField(DefaultPrice);
						p.ActualPrice	 	= EditorGUILayout.TextField(p.ActualPrice);
						
						if(p.ActualPrice.Length > 0) {
							p.ActualPrice 		= p.ActualPrice.Trim();
						}
						EditorGUILayout.EndHorizontal();
						
						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.Space();
						if(GUILayout.Button("Remove",  GUILayout.Width(80))) {
							settings.RemoveProduct(p);
							break;
						}
						EditorGUILayout.EndHorizontal();
						
						EditorGUILayout.Space();
						EditorGUI.indentLevel--;
					}
					
					
					
					
					
					EditorGUILayout.EndVertical();
				}
				
				
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.Space();
				if(GUILayout.Button("Add new", EditorStyles.miniButton, GUILayout.Width(250))) {
					settings.AddProduct(new UM_InAppProduct());
				}
				EditorGUILayout.Space();
				EditorGUILayout.EndHorizontal();
				
				
				EditorGUILayout.Space();
				
				
			}
			
			



			EditorGUILayout.EndVertical();
		}EditorGUI.indentLevel--;
		

		EditorGUILayout.Space();
		EditorGUILayout.HelpBox("Platfroms Settings", MessageType.None);

			
			
		EditorGUILayout.LabelField("Android:", EditorStyles.boldLabel);
		EditorGUI.indentLevel++ ; {
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(Base64KeyLabel);
			AndroidNativeSettings.Instance.base64EncodedPublicKey	 	= EditorGUILayout.TextField(AndroidNativeSettings.Instance.base64EncodedPublicKey);
			
			if(AndroidNativeSettings.Instance.base64EncodedPublicKey.ToString().Length > 0) {
				AndroidNativeSettings.Instance.base64EncodedPublicKey		= AndroidNativeSettings.Instance.base64EncodedPublicKey.ToString().Trim();
			}

			EditorGUILayout.EndHorizontal();
		}EditorGUI.indentLevel--;

			

	}
	
	
	public static void UpdatePluginSettings() {
		AndroidNativeSettingsEditor.UpdatePluginDefines();
		IOSNativeSettingsEditor.UpdatePluginSettings();
		
		if(IsInstalled && IsUpToDate) {
			AndroidNativeSettingsEditor.UpdateManifest();
		}


		string UM_InAppPurchaseManagerPath = "Extensions/UltimateMobile/Scripts/InApps/Manage/UM_InAppPurchaseManager.cs";
		SA_EditorTool.ChnageDefineState(UM_InAppPurchaseManagerPath, "ATC_SUPPORT_ENABLED", AndroidNativeSettings.Instance.EnableATCSupport);

	}
	
	private void OtherSettings() {

		EditorGUILayout.Space();
		EditorGUILayout.HelpBox("Camera And Gallery", MessageType.None);

		EditorGUI.indentLevel++; {
			UltimateMobileSettings.Instance.IsCameraAndGalleryIOSSettingsOpen = EditorGUILayout.Foldout(settings.IsCameraAndGalleryIOSSettingsOpen, "IOS");
			if(UltimateMobileSettings.Instance.IsCameraAndGalleryIOSSettingsOpen) {
				IOSNativeSettingsEditor.CameraAndGallery(false);
			}
			
			UltimateMobileSettings.Instance.IsCameraAndGalleryAndroidSettingsOpen = EditorGUILayout.Foldout(settings.IsCameraAndGalleryAndroidSettingsOpen, "Android");
			if(UltimateMobileSettings.Instance.IsCameraAndGalleryAndroidSettingsOpen) {
				AndroidNativeSettingsEditor.CameraAndGalleryParams(false);
			}
		}EditorGUI.indentLevel--; 
			
		
		
		EditorGUILayout.Space();
		EditorGUILayout.HelpBox("Local And Push Notifications", MessageType.None);

		EditorGUI.indentLevel++; {

			UltimateMobileSettings.Instance.IsLP_IOS_SettingsOpen = EditorGUILayout.Foldout(settings.IsLP_IOS_SettingsOpen, "IOS");
			if(UltimateMobileSettings.Instance.IsLP_IOS_SettingsOpen) {
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.Space();
				if(GUILayout.Button("IOS Push Notifications Guide",  GUILayout.Width(300))) {
					Application.OpenURL("https://goo.gl/xmel8O");
				}
				EditorGUILayout.Space();
				EditorGUILayout.EndHorizontal();
			}
			
			UltimateMobileSettings.Instance.IsLP_Android_SettingsOpen = EditorGUILayout.Foldout(settings.IsLP_Android_SettingsOpen, "Android");
			if(UltimateMobileSettings.Instance.IsLP_Android_SettingsOpen) {

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.Space();
				if(GUILayout.Button("Open Android Push Notifications Settings",  GUILayout.Width(300))) {
					AndroidNativeSettings.Instance.ToolbarSelectedIndex = 3;
					Selection.activeObject = AndroidNativeSettings.Instance;
				}
				EditorGUILayout.Space();
				EditorGUILayout.EndHorizontal();
			}

		}EditorGUI.indentLevel--; 
		
		EditorGUILayout.Space ();
		EditorGUILayout.HelpBox("Third Party Settings", MessageType.None);
		EditorGUI.indentLevel++; {

			UltimateMobileSettings.Instance.TP_IOS_SettingsOpen = EditorGUILayout.Foldout(settings.TP_IOS_SettingsOpen, "IOS");
			if(UltimateMobileSettings.Instance.TP_IOS_SettingsOpen) {
				IOSNativeSettingsEditor.ThirdPartySettings(false);
			}
			
			UltimateMobileSettings.Instance.TP_Android_SettingsOpen = EditorGUILayout.Foldout(settings.TP_Android_SettingsOpen, "Android");
			if(UltimateMobileSettings.Instance.TP_Android_SettingsOpen) {
				AndroidNativeSettingsEditor.ThirdPartyParams(false);
			}
		}EditorGUI.indentLevel--; 
		
		
	}
	
	
	
	private void AboutGUI() {
		
		EditorGUILayout.HelpBox("About Ultimate Mobile", MessageType.None);
		EditorGUILayout.Space();

		SA_EditorTool.SelectableLabelField(SdkVersion,   UltimateMobileSettings.VERSION_NUMBER);
		SA_EditorTool.SupportMail();
		EditorGUILayout.Space();


		EditorGUILayout.BeginHorizontal();
		SA_EditorTool.SelectableLabelField(new GUIContent("Android Native") ,   AndroidNativeSettings.VERSION_NUMBER);
		SA_EditorTool.SelectableLabelField(new GUIContent("Google Play Lib"),  AndroidNativeSettings.GOOGLE_PLAY_SDK_VERSION_NUMBER);

		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		SA_EditorTool.SelectableLabelField(new GUIContent("IOS Native") ,   IOSNativeSettings.VERSION_NUMBER);
		SA_EditorTool.SelectableLabelField(new GUIContent("WP8 Native") ,   WP8NativeSettings.VERSION_NUMBER);
		
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		SA_EditorTool.SelectableLabelField(new GUIContent("Google Mobile Ad") ,   GoogleMobileAdSettings.VERSION_NUMBER);
		SA_EditorTool.SelectableLabelField(new GUIContent("Google Analytics") ,   GoogleAnalyticsSettings.VERSION_NUMBER);
		
		EditorGUILayout.EndHorizontal();

	

		SA_EditorTool.DrawSALogo();

	}
	

	
	
	
	private static void DirtyEditor() {
		#if UNITY_EDITOR
		EditorUtility.SetDirty(UltimateMobileSettings.Instance);
		EditorUtility.SetDirty(AndroidNativeSettings.Instance);
		EditorUtility.SetDirty(GoogleMobileAdSettings.Instance);
		EditorUtility.SetDirty(IOSNativeSettings.Instance);
		#endif
	}
	
	
}
