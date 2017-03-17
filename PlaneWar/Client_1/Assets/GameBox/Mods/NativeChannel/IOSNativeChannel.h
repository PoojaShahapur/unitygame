//
//  IOSNativeChannel.h
//  IOSNativeChannel
//
//  Created by mouguangyi on 06/02/2017.
//  Copyright © 2017 Giant. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import "UnityAppController.h"

// ------------------------------------------------
// - IManagedProxy
//  IOS层调用C#层对象方法接口。
// ------------------------------------------------
@protocol IManagedProxy
// 根据方法名和参数调用C#层对象的对应方法。
- (void)callWithMethod:(NSString*)method, ... NS_REQUIRES_NIL_TERMINATION;

@end

// ------------------------------------------------
// - INativeProxy
//  IOS层需要实现的协议接口。
// ------------------------------------------------
@protocol INativeProxy
// Native层主动连接NativeProxy，并将IManagedProxy接口传入，供Native层触发Managed层相应方法调用。
- (void)connect:(id<IManagedProxy>) proxy;

@end

// ------------------------------------------------
// - IOSManifest
//  读取IOSManifest.xml数据工具。
// ------------------------------------------------
@interface IOSManifest : NSObject<NSXMLParserDelegate>

- (NSString*)receiverForAction:(NSString*)action;
- (NSArray<NSString*>*)receiversForAction:(NSString*)action;
- (id)metaDataForName:(NSString*)name;

+ (instancetype)sharedInstance;

@end

// - NativeChannelController
@interface NativeChannelController : UnityAppController
@end

IMPL_APP_CONTROLLER_SUBCLASS(NativeChannelController)

