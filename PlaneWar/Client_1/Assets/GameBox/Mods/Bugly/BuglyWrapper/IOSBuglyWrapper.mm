//
//  IOSBuglyWrapper.m
//  IOSBuglyWrapper
//
//  Created by mouguangyi on 08/02/2017.
//  Copyright © 2017 Giant. All rights reserved.
//

#import "IOSBuglyWrapper.h"
#import "BuglyBridge.h"

@implementation BuglyWrapper
    
- (void)connect:(id<IManagedProxy>)proxy
{
}

- (void)init:(NSString*)appId
{
    _BuglyInit([appId cStringUsingEncoding:NSUTF8StringEncoding], false, 0);
}
    
- (void)setGameType:(NSNumber*)type
{
}

- (void)setSdkPackageName:(NSString*)packageName
{
    _BuglyConfigCrashReporterType(1);
}

- (void)postException:(NSNumber *)type arg1:(NSString *)name arg2:(NSString *)reason arg3:(NSString *)stackTrace arg4:(BOOL)quit
{
    _BuglyReportException(
                          [type intValue],
                          [name cStringUsingEncoding:NSUTF8StringEncoding],
                          [reason cStringUsingEncoding:NSUTF8StringEncoding],
                          [stackTrace cStringUsingEncoding:NSUTF8StringEncoding],
                          "",
                          quit);
}
    
- (void)setUnityVersion:(NSString *)version
{
    _BuglySetKeyValue("UnityVersion", [version cStringUsingEncoding:NSUTF8StringEncoding]);
}

- (void)didFinishLaunchingWithOptions
{
    NSLog(@"BuglyWrapper didFinishLaunchingWithOptions");
    
    NSString* appId = [[IOSManifest sharedInstance] metaDataForName:@"BUGLY_APPID"];
    if (nil != appId) {
        _BuglyInit([appId cStringUsingEncoding:NSUTF8StringEncoding], false, 0);
    }
}

@end
