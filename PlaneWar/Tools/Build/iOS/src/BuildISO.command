# cer ��ʽ�� SHA1
MY_CER_SHA1="6F367D58817CB3D953F629801AA7809A9E0495CF"
#cer ����·��
MY_SER_FULLPATH="/Users/zt-2010879/File/MacCer/myseabed/������֤��/ios_distribution.cer"
#Կ�״���ִ��Ŀ¼
MY_KEYCHAIN="/Users/zt-2010879/Library/Keychains/login.keychain"
#֤������
MY_CER_PASSWORD="morefuntek"
#�����ļ�Ŀ¼
MY_PROVISION_FULLPATH="/Users/zt-2010879/File/MacCer/myseabed/������֤��/My_Seabed_dis.mobileprovision"
#�����ļ�Ŀ¼������
MY_PROJECT_PATH="/Users/zt-2010879/File/snowball/Proj/IOS/Unity-iPhone.xcodeproj"
#ģʽ����
MY_SCHEME="Unity-iPhone"
#archive Ŀ¼
MY_ARCHIVE_PATH="/Users/zt-2010879/File/snowball/Output/fish.xcarchive"
#��� ipa Ŀ¼������
MY_IPA_PATH="/Users/zt-2010879/File/snowball/Output/fish.ipa"
#��������
MY_BUILD_CONFIG="Release"
#Bundle Id
MY_PRODUCT_BUNDLE_IDENTIFIER="com.ztgame.ztmyseabed"
#֤���ʶ��
MY_CODE_SIGN_IDENTITY="iPhone Distribution: Giant Mobile Technology Co., LTD"
#��������
MY_PROVISIONING_PROFILE="My Seabed_dis"
#ƽ̨����
MY_PLATFORM_NAME="iphoneos"
#���� ipa ��ʱ����Ҫ�������ļ�����
MY_EXPORT_IPA_PROVISIONING_PROFILE="My Seabed_dis"
#����ʱʹ�õ� plist �ļ�
MY_EXPORT_PLIST="/Users/zt-2010879/File/snowball/Tools/Build/iOS/Info.plist"
#�����ʾ
MY_CC_FLAGS=-arch "armv7"


#ɾ��֤��
security delete-certificate -Z "$MY_CER_SHA1"
#����֤��
security import "$MY_SER_FULLPATH" -k "$MY_KEYCHAIN" -P "$MY_CER_PASSWORD" -A
#���������ļ��� XCode �����У������Ҫ XCode ��ȷ��
#sudo open $MY_PROVISION_FULLPATH -P "ztgame@123"
sudo open $MY_PROVISION_FULLPATH
#����֤����Ϣ
xcodebuild -project "$MY_PROJECT_PATH" PRODUCT_BUNDLE_IDENTIFIER="$MY_PRODUCT_BUNDLE_IDENTIFIER" CODE_SIGN_IDENTITY="$MY_CODE_SIGN_IDENTITY" PROVISIONING_PROFILE="$MY_PROVISIONING_PROFILE"
#���� xcarchive �ļ�
xcodebuild -project "$MY_PROJECT_PATH" -scheme "$MY_SCHEME" archive -archivePath "$MY_ARCHIVE_PATH" -configuration "$MY_BUILD_CONFIG" PLATFORM_NAME="$MY_PLATFORM_NAME" "$MY_CC_FLAGS"
#���� ipa
xcodebuild -exportArchive -exportFormat ipa -archivePath "$MY_ARCHIVE_PATH" -exportPath "$MY_IPA_PATH" -exportProvisioningProfile "$MY_EXPORT_IPA_PROVISIONING_PROFILE"
#xcodebuild -exportArchive -archivePath "$MY_ARCHIVE_PATH" -exportPath "$MY_IPA_PATH" -exportOptionsPlist "$MY_EXPORT_PLIST"