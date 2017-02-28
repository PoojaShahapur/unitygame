CurrentPath=$(pwd)
BasePath=$(cd `dirname $0`; pwd)

# cer 正式的 SHA1
MY_CER_SHA1="6F367D58817CB3D953F629801AA7809A9E0495CF"
#cer 完整路径
MY_SER_FULLPATH="/Users/zt-2010879/File/MacCer/myseabed/发布者证书/ios_distribution.cer"
#钥匙串可执行目录
MY_KEYCHAIN="/Users/zt-2010879/Library/Keychains/login.keychain"
#证书密码
MY_CER_PASSWORD="morefuntek"
#描述文件目录
MY_PROVISION_FULLPATH="/Users/zt-2010879/File/MacCer/myseabed/发布者证书/My_Seabed_dis.mobileprovision"
#工程文件目录和名字
MY_PROJECT_PATH="/Users/zt-2010879/File/snowball/Proj/IOS/Unity-iPhone.xcodeproj"
#模式名字
MY_SCHEME="Unity-iPhone"
#archive 目录
MY_ARCHIVE_PATH="/Users/zt-2010879/File/snowball/Output/fish.xcarchive"
#输出 ipa 目录和名字
MY_IPA_PATH="/Users/zt-2010879/File/snowball/Output/fish.ipa"
#编译配置
MY_BUILD_CONFIG="Release"
#Bundle Id
MY_PRODUCT_BUNDLE_IDENTIFIER="com.ztgame.ztmyseabed"
#证书标识符
MY_CODE_SIGN_IDENTITY="iPhone Distribution: Giant Mobile Technology Co., LTD"
#描述名字
MY_PROVISIONING_PROFILE="My Seabed_dis"
#平台名字
MY_PLATFORM_NAME="iphoneos"
#导出 ipa 的时候需要的描述文件名字
MY_EXPORT_IPA_PROVISIONING_PROFILE="My Seabed_dis"
#导出时使用的 plist 文件
MY_EXPORT_PLIST="/Users/zt-2010879/File/snowball/Tools/Build/iOS/Info.plist"
#编译标示
MY_CC_FLAGS=-arch "armv7"


#删除证书
security delete-certificate -Z "$MY_CER_SHA1"
#导入证书
security import "$MY_SER_FULLPATH" -k "$MY_KEYCHAIN" -P "$MY_CER_PASSWORD" -A
#导入描述文件到 XCode 缓存中，这个需要 XCode 打开确认
#sudo open $MY_PROVISION_FULLPATH -P "ztgame@123"
sudo open $MY_PROVISION_FULLPATH
#导入证书信息
xcodebuild -project "$MY_PROJECT_PATH" PRODUCT_BUNDLE_IDENTIFIER="$MY_PRODUCT_BUNDLE_IDENTIFIER" CODE_SIGN_IDENTITY="$MY_CODE_SIGN_IDENTITY" PROVISIONING_PROFILE="$MY_PROVISIONING_PROFILE"
#生成 xcarchive 文件
xcodebuild -project "$MY_PROJECT_PATH" -scheme "$MY_SCHEME" archive -archivePath "$MY_ARCHIVE_PATH" -configuration "$MY_BUILD_CONFIG" PLATFORM_NAME="$MY_PLATFORM_NAME" "$MY_CC_FLAGS"
#导出 ipa
xcodebuild -exportArchive -exportFormat ipa -archivePath "$MY_ARCHIVE_PATH" -exportPath "$MY_IPA_PATH" -exportProvisioningProfile "$MY_EXPORT_IPA_PROVISIONING_PROFILE"
#xcodebuild -exportArchive -archivePath "$MY_ARCHIVE_PATH" -exportPath "$MY_IPA_PATH" -exportOptionsPlist "$MY_EXPORT_PLIST"