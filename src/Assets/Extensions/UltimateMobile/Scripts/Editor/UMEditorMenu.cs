////////////////////////////////////////////////////////////////////////////////
//  
// @module V2D
// @author Osipov Stanislav lacost.st@gmail.com
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using UnityEditor;
using System.Collections;

public class UMEditorMenu : EditorWindow {
	
	//--------------------------------------
    //  GENERAL
    //--------------------------------------

	[MenuItem("Window/Stan's Assets/Ultimate Mobile/Edit Settings", false, 0)]
    public static void Edit() {
        Selection.activeObject = UltimateMobileSettings.Instance;
    }



    //--------------------------------------
    //  GETTING STARTED
    //--------------------------------------

    [MenuItem("Window/Stan's Assets/Ultimate Mobile/Documentation/Getting Started/Overview")]
    public static void UMGSOverview() {
        string url = "https://unionassets.com/ultimate-mobile/overview-218";
        Application.OpenURL(url);
    }

    [MenuItem("Window/Stan's Assets/Ultimate Mobile/Documentation/Getting Started/Setup and Update")]
    public static void UMGSSetupAndUpdate() {
        string url = "https://unionassets.com/ultimate-mobile/setup-and-update-219";
        Application.OpenURL(url);
    }

    //--------------------------------------
    //  IN-APP PURCHACES
    //--------------------------------------

    [MenuItem("Window/Stan's Assets/Ultimate Mobile/Documentation/In-App Purchases/Setup")]
    public static void UMIASetup() {
        string url = "https://unionassets.com/ultimate-mobile/setup-225";
        Application.OpenURL(url);
    }

    [MenuItem("Window/Stan's Assets/Ultimate Mobile/Documentation/In-App Purchases/Coding Guidelines")]
    public static void UMIACodingGuidelines() {
        string url = "https://unionassets.com/ultimate-mobile/coding-guidelines-226";
        Application.OpenURL(url);
    }

    [MenuItem("Window/Stan's Assets/Ultimate Mobile/Documentation/In-App Purchases/Validation")]
    public static void UMIAValidation() {
        string url = "https://unionassets.com/ultimate-mobile/validation-434";
        Application.OpenURL(url);
    }

    //--------------------------------------
    //  GAME SERVICES
    //--------------------------------------

    [MenuItem("Window/Stan's Assets/Ultimate Mobile/Documentation/Game Services/Setup")]
    public static void UMGSSetup() {
        string url = "https://unionassets.com/ultimate-mobile/setup-227";
        Application.OpenURL(url);
    }

    [MenuItem("Window/Stan's Assets/Ultimate Mobile/Documentation/Game Services/Sing in")]
    public static void UMGSSingIn() {
        string url = "https://unionassets.com/ultimate-mobile/sing-in-251";
        Application.OpenURL(url);
    }

    [MenuItem("Window/Stan's Assets/Ultimate Mobile/Documentation/Game Services/Achievements")]
    public static void UMAGSchievements() {
        string url = "https://unionassets.com/ultimate-mobile/achievements-253";
        Application.OpenURL(url);
    }

    [MenuItem("Window/Stan's Assets/Ultimate Mobile/Documentation/Game Services/Leaderboards")]
    public static void UMGSLeaderboards() {
        string url = "https://unionassets.com/ultimate-mobile/leaderboards-252";
        Application.OpenURL(url);
    }

    //--------------------------------------
    //  ADVERTISEMENT
    //--------------------------------------

    [MenuItem("Window/Stan's Assets/Ultimate Mobile/Documentation/Advertisement/Setup")]
    public static void UMADSetup() {
        string url = "https://unionassets.com/ultimate-mobile/setup-228";
        Application.OpenURL(url);
    }

    [MenuItem("Window/Stan's Assets/Ultimate Mobile/Documentation/Advertisement/Coding Guidelines")]
    public static void UMADCodingGuidelines() {
        string url = "https://unionassets.com/ultimate-mobile/coding-guidelines-229";
        Application.OpenURL(url);
    }

    //--------------------------------------
    //  MORE UNIFIED API
    //--------------------------------------

    [MenuItem("Window/Stan's Assets/Ultimate Mobile/Documentation/More Unified API/Social Sharing")]
    public static void UMMUSocialSharing() {
        string url = "https://unionassets.com/ultimate-mobile/social-sharing-230";
        Application.OpenURL(url);
    }

    [MenuItem("Window/Stan's Assets/Ultimate Mobile/Documentation/More Unified API/Camera and Gallery")]
    public static void UMMUCameraAndGallery() {
        string url = "https://unionassets.com/ultimate-mobile/camera-and-gallery-231";
        Application.OpenURL(url);
    }

    [MenuItem("Window/Stan's Assets/Ultimate Mobile/Documentation/More Unified API/Local And Push Notifications")]
    public static void UMMULocalAndPushNotifications() {
        string url = "https://unionassets.com/ultimate-mobile/local-and-push-notifications-232";
        Application.OpenURL(url);
    }

    [MenuItem("Window/Stan's Assets/Ultimate Mobile/Documentation/More Unified API/Pop Ups and Pre-loaders")]
    public static void UMMUPopUpsAndPreLoaders() {
        string url = "https://unionassets.com/ultimate-mobile/pop-ups-and-pre-loaders-235";
        Application.OpenURL(url);
    }

    [MenuItem("Window/Stan's Assets/Ultimate Mobile/Documentation/More Unified API/Analytics Tracking")]
    public static void UMMUAnalyticsTracking() {
        string url = "https://unionassets.com/ultimate-mobile/analytics-tracking-234";
        Application.OpenURL(url);
    }

    //--------------------------------------
    //  PLAYMAKER
    //--------------------------------------

    [MenuItem("Window/Stan's Assets/Ultimate Mobile/Documentation/Playmaker/Actions List")]
    public static void UMPActionsList() {
        string url = "https://unionassets.com/ultimate-mobile/playmaker-277";
        Application.OpenURL(url);
    }

    [MenuItem("Window/Stan's Assets/Ultimate Mobile/Documentation/Playmaker/InApp Purchasing with Playmaker")]
    public static void UMPInAppPurchasingWithPlaymaker() {
        string url = "https://unionassets.com/ultimate-mobile/inapp-purchasing-with-playmaker-466";
        Application.OpenURL(url);
    }

    //--------------------------------------
    //  THIRD-PARTY PLUG-INS SUPPORT
    //--------------------------------------

    [MenuItem("Window/Stan's Assets/Ultimate Mobile/Documentation/Third-Party Plug-ins Support/Anti-Cheat Toolkit")]
    public static void UMTPAntiCheatToolkit() {
        string url = "https://unionassets.com/ultimate-mobile/anti-cheat-toolkit-421";
        Application.OpenURL(url);
    }

    //--------------------------------------
    //  FAQ
    //--------------------------------------
    //--------------------------------------

    [MenuItem("Window/Stan's Assets/Ultimate Mobile/Documentation/FAQ")]
    public static void UMFAQ() {
        string url = "https://unionassets.com/ultimate-mobile/manual#faq";
        Application.OpenURL(url);
    }
    
}
