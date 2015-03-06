﻿/**
 * @brief 各种宏定义
 */
// Release 自己使用的定义
//NET_MULTHREAD;MSG_ENCRIPT;MSG_COMPRESS

// Debug 自己使用的定义
//NET_MULTHREAD;MSG_ENCRIPT;MSG_COMPRESS;THREAD_CALLCHECK;ENABLE_WINLOG;ENABLE_NETLOG;UNIT_TEST_SRC

// 网络处理多线程，主要是调试的时候使用单线程，方便调试，运行的时候使用多线程
//#define NET_MULTHREAD

// 是否检查函数接口调用线程
//#define THREAD_CALLCHECK

// 消息加密
//#define MSG_ENCRIPT

// 消息压缩
//#define MSG_COMPRESS

// 开启窗口日志
//#define ENABLE_WINLOG

// 开启网络日志
//#define ENABLE_NETLOG

// 单元测试
//#define UNIT_TEST_SRC