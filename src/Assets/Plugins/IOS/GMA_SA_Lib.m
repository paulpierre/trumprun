//
//  GoogleMobileAdController.m
//  Unity-iPhone
//
//  Created by lacost on 1/16/14.
//
//

#import <Foundation/Foundation.h>
@import GoogleMobileAds;

#include "GMA_SA_Lib.h"
#if UNITY_VERSION < 450
#include "iPhone_View.h"
#endif



@interface GoogleMobileAdBanner : NSObject<GADBannerViewDelegate>

@property (strong)  GADBannerView *bannerView;
@property (strong)  NSNumber *bid;

- (void) InitBanner:(int) size bannerId: (int) bannerId;
- (void) CreateBanner:(int) gravity size: (int) size bannerId: (int) bannerId;
- (void) CreateBannerAdPos:(int) x y: (int) y size: (int) size bannerId: (int) bannerId;

- (void) Refresh;
- (void) ShowAd;
- (void) HideAd;
- (void) SetPosition:(int)gravity;
- (void) SetPosition:(int)x y:(int)y;
@end


@implementation GoogleMobileAdController

static GADInAppPurchase *purchase;

static NSString * _ad_unit_id;
static NSString * _interstitial_ad_unit_id;
static GoogleMobileAdController *_sharedInstance;

static GADInterstitial *interstitial_ = NULL;
static bool showInterstitialOnLoad = false;

static GADRequest *adRequest;

static NSMutableDictionary* _banners;

+ (id)sharedInstance {
    
    if (_sharedInstance == nil)  {
        _banners = [[NSMutableDictionary alloc] init];
        _sharedInstance = [[self alloc] init];
    }
    
    return _sharedInstance;
}

- (void) initAd:(NSString *)unit_id {
    
    NSLog(@"google ad Inited");
    
    _ad_unit_id = unit_id;
#if UNITY_VERSION < 500
    [_ad_unit_id retain];
#endif
    
    
    _interstitial_ad_unit_id = unit_id;
    
#if UNITY_VERSION < 500
    [_interstitial_ad_unit_id retain];
#endif
    
    
    adRequest =[GADRequest request];
    
    
#if UNITY_VERSION < 500
    [adRequest retain];
#endif
    
    
    
    NSLog(@"ad unity id: %@",_ad_unit_id);
}

-(void) ChangeBannersUnitID:(NSString *)unit_id {
    NSLog(@"ChangeBannersUnitID");
    _ad_unit_id = unit_id;
    
#if UNITY_VERSION < 500
    [_ad_unit_id retain];
#endif
    
    
    NSLog(@"ad unity id: %@",_ad_unit_id);
}

-(void) ChangeInterstisialsUnitID:(NSString *)unit_id {
    NSLog(@"ChangeInterstisialsUnitID");
    _interstitial_ad_unit_id = unit_id;
    
#if UNITY_VERSION < 500
    [_interstitial_ad_unit_id retain];
#endif
    
    
    NSLog(@"interstitial ad unity id: %@",_interstitial_ad_unit_id);
}



- (void) CreateBannerAd:(int)gravity size:(int)size bannerId:(int)bannerId {
    
    GoogleMobileAdBanner* banner;
    banner = [[GoogleMobileAdBanner alloc] init];
    
    [banner CreateBanner:gravity size:size bannerId:bannerId];
    [_banners setObject:banner forKey:[NSNumber numberWithInt:bannerId]];
    
}

-(void) CreateBannerAd:(int)x y:(int)y size:(int)size bannerId:(int)bannerId {
    GoogleMobileAdBanner* banner;
    banner = [[GoogleMobileAdBanner alloc] init];
    
    
    [banner CreateBannerAdPos:x y:y size:size bannerId:bannerId];
    [_banners setObject:banner forKey:[NSNumber numberWithInt:bannerId]];
    
}


- (void) addKeyword:(NSString *)keyword {
    
    //[adRequest addKeyword:keyword];
}

- (void) AddTestDevice:(NSString *)uid {
    
    NSLog(@"Adding TestDevice ID: %@", uid);
    
    NSMutableArray *ids = [[NSMutableArray alloc] init];
    [ids addObject:uid];
    for (NSString *string in [adRequest testDevices]) {
        [ids addObject:string];
    }
    
    adRequest.testDevices = [ids copy];
    
}

- (void) AddTestDevices:(NSString *)uids {
    
    NSArray *items = [uids componentsSeparatedByString:@","];
    
    for(NSString* s in items) {
        [self AddTestDevice:s];
    }
    
}



- (void) SetGender:(int)gender {
    if(gender == 0) {
        adRequest.gender = kGADGenderFemale;
    }
    
    if(gender == 1) {
        adRequest.gender = kGADGenderMale;
    }
    
    if(gender == 2) {
        adRequest.gender = kGADGenderUnknown;
    }
    
}

- (void) setBirthday:(int)day month:(int)month year:(int)year {
    // [adRequest setBirthdayWithMonth:month day:day year:year];
}

- (void) tagForChildDirectedTreatment:(BOOL)val {
    [adRequest tagForChildDirectedTreatment:val];
}

- (void) Refresh:(int)bannerId {
    GoogleMobileAdBanner *banner = [_banners objectForKey:[NSNumber numberWithInt:bannerId]];
    if(banner != nil) {
        [banner Refresh];
        
    }
    
    
}

-(void) HideAd:(int)bannerId {
    GoogleMobileAdBanner *banner = [_banners objectForKey:[NSNumber numberWithInt:bannerId]];
    if(banner != nil) {
        [banner HideAd];
        
    }
    
}

-(void) ShowAd:(int)bannerId {
    GoogleMobileAdBanner *banner = [_banners objectForKey:[NSNumber numberWithInt:bannerId]];
    if(banner != nil) {
        [banner ShowAd];
        
    }
    
}

- (void) SetPosition:(int)gravity bannerId:(int)bannerId {
    GoogleMobileAdBanner *banner = [_banners objectForKey:[NSNumber numberWithInt:bannerId]];
    if(banner != nil) {
        [banner SetPosition:gravity];
    }
}

-(void) SetPosition:(int)x y:(int)y bannerId:(int)bannerId {
    GoogleMobileAdBanner *banner = [_banners objectForKey:[NSNumber numberWithInt:bannerId]];
    if(banner != nil) {
        [banner SetPosition:x y:y];
    }
}

- (void) DestroyBanner:(int)bannerId {
    GoogleMobileAdBanner *banner = [_banners objectForKey:[NSNumber numberWithInt:bannerId]];
    if(banner != nil) {
        [banner HideAd];
        // [[banner bannerView] dealloc];
        
#if UNITY_VERSION < 500
        [banner dealloc];
#endif
        
        
        
    }
    
}


- (void) reportPurchaseStatus:(int)value {
    if(purchase != nil) {
        
        switch (value) {
            case 0:
                [purchase reportPurchaseStatus:kGADInAppPurchaseStatusSuccessful];
                break;
            case 1:
                [purchase reportPurchaseStatus:kGADInAppPurchaseStatusError];
                break;
            case 2:
                [purchase reportPurchaseStatus:kGADInAppPurchaseStatusInvalidProduct];
                break;
            case 3:
                [purchase reportPurchaseStatus:kGADInAppPurchaseStatusCancel];
                break;
                
            default:
                break;
        }
        purchase = nil;
    }
}

#pragma mark getters

- (NSString*) GetUnitId {
    return  _ad_unit_id;
}
- (GADRequest*) GetAdRequest {
    return adRequest;
}


#pragma mark Interstitial

-(void) StartInterstitialAd {
    NSLog(@"StartInterstitialAd");
    [self LoadInterstitialAd];
    showInterstitialOnLoad = true;
    
}

-(void) LoadInterstitialAd {
    interstitial_ = [[GADInterstitial alloc] init];
    interstitial_.adUnitID = _interstitial_ad_unit_id;
    interstitial_.delegate = self;
    interstitial_.inAppPurchaseDelegate = self;
    [interstitial_ loadRequest:adRequest];
    
    showInterstitialOnLoad = false;
}

-(void) ShowInterstitialAd {
    if(interstitial_ != NULL) {
        UIViewController *vc =  UnityGetGLViewController();
        [interstitial_ presentFromRootViewController:vc];
    }
    
}

#pragma mark GADInterstitialDelegate implementation

- (void)interstitialDidReceiveAd:(GADInterstitial *)interstitial {
    NSLog(@"interstitial interstitialDidReceiveAd ");
    
    if(showInterstitialOnLoad) {
        [self ShowInterstitialAd];
    }
    
    UnitySendMessage("IOSAdMobController", "OnInterstitialAdLoaded", "");
    
    
}
- (void)interstitial:(GADInterstitial *)interstitial didFailToReceiveAdWithError:(GADRequestError *)error {
    NSLog(@"interstitial didFailToReceiveAdWithError: %@", error.description);
    
    UnitySendMessage("IOSAdMobController", "OnInterstitialAdFailedToLoad", "");
    
}

- (void)interstitialWillPresentScreen:(GADInterstitial *)interstitial {
    NSLog(@"interstitial interstitialWillPresentScreen ");
    UnitySendMessage("IOSAdMobController", "OnInterstitialAdOpened", "");
    
}


- (void)interstitialDidDismissScreen:(GADInterstitial *)interstitial {
    NSLog(@"interstitial interstitialDidDismissScreen ");
    UnitySendMessage("IOSAdMobController", "OnInterstitialAdClosed", "");
    
}

- (void)interstitialWillLeaveApplication:(GADInterstitial *)interstitial {
    NSLog(@"interstitial interstitialWillLeaveApplication ");
    UnitySendMessage("IOSAdMobController", "OnInterstitialAdLeftApplication", "");
    
}

#pragma mark GADInAppPurchaseDelegate implementation

- (void)didReceiveInAppPurchase:(GADInAppPurchase *)p{
    purchase = p;
    UnitySendMessage("IOSAdMobController", "OnInAppPurchaseRequested",  [purchase.productID UTF8String]);
}





#pragma mark Unity data parce implementation

+(NSString *) charToNSString:(char *)value {
    if (value != NULL) {
        return [NSString stringWithUTF8String: value];
    } else {
        return [NSString stringWithUTF8String: ""];
    }
}

+(const char *)NSIntToChar:(NSInteger)value {
    NSString *tmp = [NSString stringWithFormat:@"%d", value];
    return [tmp UTF8String];
}

+ (const char *)NSStringToChar:(NSString *)value {
    return [value UTF8String];
}
@end


@implementation GoogleMobileAdBanner


-(void) InitBanner:(int)size bannerId:(int)bannerId {
    NSLog(@"GAD: InitBanner");
    
    
    NSNumber *n = [NSNumber numberWithInt:bannerId];
    [self setBid:n];
    
    
    bool IsLandscape;
    UIInterfaceOrientation orientation = [UIApplication sharedApplication].statusBarOrientation;
    if(orientation == UIInterfaceOrientationLandscapeLeft || orientation == UIInterfaceOrientationLandscapeRight) {
        IsLandscape = true;
    } else {
        IsLandscape = false;
    }
    
    
    if(size == 1) {
        [self setBannerView:[[GADBannerView alloc] initWithAdSize:kGADAdSizeBanner]];
    }
    
    
    if(size == 2) {
        
        if(IsLandscape) {
            [self setBannerView:[[GADBannerView alloc] initWithAdSize:kGADAdSizeSmartBannerLandscape]];
        } else {
            [self setBannerView:[[GADBannerView alloc] initWithAdSize:kGADAdSizeSmartBannerPortrait]];
        }
        
    }
    
    
    if(size == 3) {
        [self setBannerView:[[GADBannerView alloc] initWithAdSize:kGADAdSizeFullBanner]];
    }
    
    if(size == 4) {
        [self setBannerView:[[GADBannerView alloc] initWithAdSize:kGADAdSizeLeaderboard]];
    }
    
    
    if(size == 5) {
        [self setBannerView:[[GADBannerView alloc] initWithAdSize:kGADAdSizeMediumRectangle]];
    }
}

-(void) StartBannerRequest {
    NSLog(@"GAD: StartBannerRequest");
    
    UIViewController *vc =  UnityGetGLViewController();
    
    [self bannerView].adUnitID =  [[GoogleMobileAdController sharedInstance] GetUnitId];
    
    [self bannerView].rootViewController = vc;
    [[vc view] addSubview:[self bannerView]];
    [self bannerView].delegate = self;
    [self bannerView].inAppPurchaseDelegate = [GoogleMobileAdController sharedInstance];
    
    [[self bannerView] loadRequest:[[GoogleMobileAdController sharedInstance] GetAdRequest]];
    
    [self HideAd];
    
}



- (void) CreateBannerAdPos:(int)x y:(int)y size:(int)size bannerId:(int)bannerId {
    NSLog(@"GAD: CreateBannerAdPos");
    
    [self InitBanner:size bannerId:bannerId];
    
    [self bannerView].frame = CGRectMake(x,
                                         y,
                                         [self bannerView].frame.size.width,
                                         [self bannerView].frame.size.height);
    
    
    [self StartBannerRequest];
    
}




-(void) CreateBanner:(int)gravity size:(int)size bannerId:(int)bannerId {
    NSLog(@"GAD: CreateBanner");
    
    [self InitBanner:size bannerId:bannerId];
    
    
    float x = 0.0;
    float y = 0.0;
    
    if(gravity == 83) {
        y = [self GetH: 2];
    }
    
    if(gravity == 81) {
        x = [self GetW:1];
        y = [self GetH: 2];
        
    }
    
    if(gravity == 85) {
        x = [self GetW:2];
        y = [self GetH: 2];
        
    }
    
    
    if(gravity == 51) {
        //ziros
    }
    
    if(gravity == 49) {
        x = [self GetW:1];
        
    }
    
    if(gravity == 53) {
        x = [self GetW:2];
    }
    
    if(gravity == 19) {
        y = [self GetH: 1];
    }
    
    if(gravity == 17) {
        x = [self GetW:1];
        y = [self GetH: 1];
        
    }
    
    if(gravity == 21) {
        x = [self GetW:2];
        y = [self GetH: 1];
    }
    
    
    
    [self bannerView].frame = CGRectMake(x,y,
                                         [self bannerView].frame.size.width,
                                         [self bannerView].frame.size.height);
    
    
    
    [self StartBannerRequest];
}

- (float) GetW: (int) p {
    UIViewController *vc =  UnityGetGLViewController();
    
    bool IsLandscape;
    UIInterfaceOrientation orientation = [UIApplication sharedApplication].statusBarOrientation;
    if(orientation == UIInterfaceOrientationLandscapeLeft || orientation == UIInterfaceOrientationLandscapeRight) {
        IsLandscape = true;
    } else {
        IsLandscape = false;
    }
    
    CGFloat w;
    if(IsLandscape) {
        w = vc.view.frame.size.height;
    } else {
        w = vc.view.frame.size.width;
    }
    
    NSArray *vComp = [[UIDevice currentDevice].systemVersion componentsSeparatedByString:@"."];
    if ([[vComp objectAtIndex:0] intValue] >= 8) {
        NSLog(@"IOS 8 detected");
        w = vc.view.frame.size.width;
    }
    
    
    if(p == 1) {
        return  (w - [self bannerView].frame.size.width) / 2;
    }
    
    if(p == 2) {
        return  w - [self bannerView].frame.size.width;
    }
    
    return 0.0;
    
    
}

- (float) GetH: (int) p {
    UIViewController *vc =  UnityGetGLViewController();
    
    bool IsLandscape;
    UIInterfaceOrientation orientation = [UIApplication sharedApplication].statusBarOrientation;
    if(orientation == UIInterfaceOrientationLandscapeLeft || orientation == UIInterfaceOrientationLandscapeRight) {
        IsLandscape = true;
    } else {
        IsLandscape = false;
    }
    
    CGFloat h;
    if(IsLandscape) {
        h = vc.view.frame.size.width;
    } else {
        h = vc.view.frame.size.height;
    }
    
    NSArray *vComp = [[UIDevice currentDevice].systemVersion componentsSeparatedByString:@"."];
    if ([[vComp objectAtIndex:0] intValue] >= 8) {
        NSLog(@"IOS 8 detected");
        h = vc.view.frame.size.height;
    }
    
    
    if(p == 1) {
        return  (h - [self bannerView].frame.size.height) / 2;
    }
    
    if(p == 2) {
        return  h - [self bannerView].frame.size.height;
    }
    
    return 0.0;
    
    
    
}


- (void) Refresh {
    [[self bannerView] loadRequest:[[GoogleMobileAdController sharedInstance] GetAdRequest]];}

-(void) HideAd {
    [[self bannerView] removeFromSuperview];
    
}

-(void) ShowAd {
    UIViewController *vc =  UnityGetGLViewController();
    [[vc view] addSubview:[self bannerView]];
    
}




-(void) SetPosition:(int)gravity {
    float x = 0.0;
    float y = 0.0;
    
    if(gravity == 83) {
        y = [self GetH: 2];
    }
    
    if(gravity == 81) {
        x = [self GetW:1];
        y = [self GetH: 2];
        
    }
    
    if(gravity == 85) {
        x = [self GetW:2];
        y = [self GetH: 2];
        
    }
    
    
    if(gravity == 51) {
        //ziros
    }
    
    if(gravity == 49) {
        x = [self GetW:1];
        
    }
    
    if(gravity == 53) {
        x = [self GetW:2];
    }
    
    if(gravity == 19) {
        y = [self GetH: 1];
    }
    
    if(gravity == 17) {
        x = [self GetW:1];
        y = [self GetH: 1];
        
    }
    
    if(gravity == 21) {
        x = [self GetW:2];
        y = [self GetH: 1];
    }
    
    
    
    [self bannerView].frame = CGRectMake(x,y,
                                         [self bannerView].frame.size.width,
                                         [self bannerView].frame.size.height);
    
}



-(void) SetPosition:(int)x y:(int)y {
    [self bannerView].frame = CGRectMake(x,
                                         y,
                                         [self bannerView].frame.size.width,
                                         [self bannerView].frame.size.height);
}




#pragma mark GADBannerViewDelegate implementation



// We've received an ad successfully.
- (void)adViewDidReceiveAd:(GADBannerView *)adView {
    NSLog(@"GAD: Received ad successfully");
    
    NSMutableString * data = [[NSMutableString alloc] init];
    NSNumber *w = [NSNumber numberWithInt: (int)[self bannerView].frame.size.width];
    NSNumber *h = [NSNumber numberWithInt: (int)[self bannerView].frame.size.height];
    
    
    
    [data appendString:[[self bid] stringValue]];
    [data appendString:@"|"];
    [data appendString:  [w stringValue]];
    [data appendString:@"|"];
    [data appendString:  [h stringValue]];
    
    
    NSString *str = [data copy];
#if UNITY_VERSION < 500
    [data autorelease];
#endif
    
    UnitySendMessage("IOSAdMobController", "OnBannerAdLoaded", [str UTF8String]);
    
    
    
}

- (void)adView:(GADBannerView *)view didFailToReceiveAdWithError:(GADRequestError *)error {
    NSLog(@"GAD: Failed to receive ad with error: %@", [error localizedFailureReason]);
    UnitySendMessage("IOSAdMobController", "OnBannerAdFailedToLoad", [[[self bid] stringValue] UTF8String]);
    
}

- (void)adViewWillPresentScreen:(GADBannerView *)adView {
    NSLog(@"GAD: OnBannerAdOpened");
    UnitySendMessage("IOSAdMobController", "OnBannerAdOpened", [[[self bid] stringValue] UTF8String]);
}


- (void)adViewDidDismissScreen:(GADBannerView *)adView {
    NSLog(@"GAD: OnBannerAdClosed");
    UnitySendMessage("IOSAdMobController", "OnBannerAdClosed", [[[self bid] stringValue] UTF8String]);
}


- (void)adViewWillLeaveApplication:(GADBannerView *)adView {
    NSLog(@"GAD: OnBannerAdLeftApplication");
    UnitySendMessage("IOSAdMobController", "OnBannerAdLeftApplication", [[[self bid] stringValue] UTF8String]);
}





@end

