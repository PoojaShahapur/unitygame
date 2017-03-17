//
//  IOSBuglyWrapper.h
//  IOSBuglyWrapper
//
//  Created by mouguangyi on 08/02/2017.
//  Copyright © 2017 Giant. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "IOSNativeChannel.h"

@interface BuglyWrapper : NSObject<INativeProxy>

- (void)connect:(id<IManagedProxy>)proxy;
    
- (void)init:(NSString*)appId;
- (void)setGameType:(NSNumber*)type;
- (void)setSdkPackageName:(NSString*)packageName;
- (void)postException:(NSNumber*)type arg1:(NSString*)name arg2:(NSString*)reason arg3:(NSString*)stackTrace arg4:(BOOL)quit;
- (void)setUnityVersion:(NSString*)version;

- (void)didFinishLaunchingWithOptions;

@end
