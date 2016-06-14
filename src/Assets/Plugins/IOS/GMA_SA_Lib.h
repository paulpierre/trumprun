//
//  GMA_SA_Lib.h
//  Unity-iPhone
//
//  Created by lacost on 10/4/15.
//
//

#ifndef GMA_SA_Lib_h
#define GMA_SA_Lib_h


#endif /* GMA_SA_Lib_h */





@interface GoogleMobileAdController : NSObject //<GADInterstitialDelegate, GADInAppPurchaseDelegate>

//@property(nonatomic, strong) GADInAppPurchase *purchase;

+ (id) sharedInstance;
- (void) initAd:(NSString*) unit_id;
- (void) ChangeBannersUnitID:(NSString*) unit_id;
- (void) ChangeInterstisialsUnitID:(NSString*) unit_id;
- (void) addKeyword:(NSString*) keyword;
- (void) AddTestDevice:(NSString*) uid;
- (void) AddTestDevices:(NSString*) uids;
- (void) SetGender:(int) gender;
- (void) setBirthday:(int)day month: (int)month year: (int) year;
- (void) tagForChildDirectedTreatment:(BOOL) val;
- (void) Refresh: (int) bannerId;
- (void) CreateBannerAd:(int) gravity size: (int) size bannerId: (int) bannerId;
- (void) CreateBannerAd:(int) x y: (int) y size: (int) size bannerId: (int) bannerId;
- (void) ShowAd: (int) bannerId;
- (void) HideAd: (int) bannerId;
- (void) SetPosition:(int)gravity bannerId: (int) bannerId;
- (void) SetPosition:(int)x y:(int)y bannerId: (int) bannerId;
- (void) DestroyBanner: (int) bannerId;

- (void) StartInterstitialAd;
- (void) LoadInterstitialAd;
- (void) ShowInterstitialAd;


- (void) reportPurchaseStatus: (int) value;


- (NSString*) GetUnitId;


+ (NSString *)   charToNSString:(char *)value;
+ (const char *) NSIntToChar:(NSInteger)value;
+ (const char *) NSStringToChar:(NSString *)value;

//- (GADRequest*) GetAdRequest;

@end

