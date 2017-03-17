//
//  IOSGetuiWrapper.h
//  IOSGetuiWrapper
//
//  Created by mouguangyi on 14/02/2017.
//  Copyright © 2017 Giant. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "IOSNativeChannel.h"
#import "GeTuiSdk.h"

#if __IPHONE_OS_VERSION_MAX_ALLOWED >= __IPHONE_10_0
#import <UserNotifications/UserNotifications.h>
#endif

@interface GetuiWrapper : NSObject<INativeProxy, GeTuiSdkDelegate, UNUserNotificationCenterDelegate>

- (void)connect:(id<IManagedProxy>)proxy;

- (void)init:(NSString*)appId arg1:(NSString*)appKey arg2:(NSString*)appSecret;

- (void)didFinishLaunchingWithOptions;
- (void)didRegisterForRemoteNotificationsWithDeviceToken:(NSData*)deviceToken;
- (void)performFetchWithCompletionHandler;
- (void)didReceiveRemoteNotification:(NSDictionary*)userInfo;

@end
