//
//  GoogleMobileAdController.m
//  Unity-iPhone
//
//  Created by lacost on 1/16/14.
//
//
#include "GMA_SA_Lib.h"



extern "C" {
    
    void _initGoogleAd (char* unit_id)  {
        [[GoogleMobileAdController sharedInstance] initAd:[GoogleMobileAdController charToNSString:unit_id ]];
    }
    
    void _GADChangeBannersUnitID (char* unit_id)  {
        [[GoogleMobileAdController sharedInstance] ChangeBannersUnitID:[GoogleMobileAdController charToNSString:unit_id ]];
    }
    
    void _GADChangeInterstisialsUnitID (char* unit_id)  {
        [[GoogleMobileAdController sharedInstance] ChangeInterstisialsUnitID:[GoogleMobileAdController charToNSString:unit_id ]];
    }
    
    void _GADAddKeyWord (char* keyword)  {
        [[GoogleMobileAdController sharedInstance] addKeyword:[GoogleMobileAdController charToNSString:keyword ]];
    }
    
    
    void _GADAddTestDevice (char* uid)  {
        [[GoogleMobileAdController sharedInstance] AddTestDevice:[GoogleMobileAdController charToNSString:uid ]];
    }
    
    void _GADAddTestDevices (char* uids)  {
        [[GoogleMobileAdController sharedInstance] AddTestDevices:[GoogleMobileAdController charToNSString:uids ]];
    }
    
    
    void _GADSetGender (int gender)  {
        [[GoogleMobileAdController sharedInstance] SetGender:gender];
    }
    
    void _GADSetBirthday (int day, int month, int year)  {
        [[GoogleMobileAdController sharedInstance] setBirthday:day month:month year:year];
    }
    
    void _GADTagForChildDirectedTreatment (BOOL val)  {
        [[GoogleMobileAdController sharedInstance] tagForChildDirectedTreatment:val];
        
    }
    
    
    void _GADCreateBannerAd (int gravity, int size, int bannerId)  {
        [[GoogleMobileAdController sharedInstance] CreateBannerAd:gravity size:size bannerId:bannerId];
    }
    
    void _GADCreateBannerAdPos(int x, int y, int size, int bannerId) {
       [[GoogleMobileAdController sharedInstance] CreateBannerAd:x y:y size:size bannerId:bannerId];
    }
    
    
    void _GADRefresh (int bannerId)  {
        [[GoogleMobileAdController sharedInstance] Refresh:bannerId];
    }
    
    
    void _GADShowAd(int bannerId) {
        [[GoogleMobileAdController sharedInstance] ShowAd:bannerId];
    }
    
    void _GADHideAd(int bannerId) {
        [[GoogleMobileAdController sharedInstance] HideAd:bannerId];
    }
    
    void _GADSetPosition(int gravity, int bannerId) {
        [[GoogleMobileAdController sharedInstance] SetPosition:gravity bannerId:bannerId];
    }
    
    void _GADSetPositionXY(int x, int y, int bannerId) {
        [[GoogleMobileAdController sharedInstance] SetPosition:x y:y bannerId:bannerId];
    }
    
    
    void _GADDestroyBanner(int bannerId) {
        [[GoogleMobileAdController sharedInstance] DestroyBanner:bannerId];
    }
    
    
    void _GADStartInterstitialAd() {
        [[GoogleMobileAdController sharedInstance] StartInterstitialAd];
    }
    
    void _GADLoadInterstitialAd() {
        [[GoogleMobileAdController sharedInstance] LoadInterstitialAd];
    }
    
    void _GADShowInterstitialAd() {
        [[GoogleMobileAdController sharedInstance] ShowInterstitialAd];
    }
    
    void _GADReportPurchaseStatus(int status) {
         [[GoogleMobileAdController sharedInstance] reportPurchaseStatus:status];
    }
    
    
    
}


