// JavaScript Document
function is_weixn(){
	var ua = navigator.userAgent.toLowerCase();
	if(ua.match(/MicroMessenger/i)=="micromessenger") {
		return true;
	} else {
		return false;
	}
}
var wx = is_weixn();
if(!wx==true){
	$('.dl_android').attr('href','http://gif5.club/Pak/fish.apk');	
	$('.dl_iphone').attr('href','itms-services://?action=download-manifest&url=https://gif5.club/plist/mysea.plist')
	//$('.dl_iphone,.dl_android').on('click',function(){
		//$('.tip_wrap,.wxtip').show()
	//})
}


var browser = {
versions: function() {
var u = navigator.userAgent, app = navigator.appVersion;
return {//移动终端浏览器版本信息 
trident: u.indexOf('Trident') > -1, //IE内核
presto: u.indexOf('Presto') > -1, //opera内核
webKit: u.indexOf('AppleWebKit') > -1, //苹果、谷歌内核
gecko: u.indexOf('Gecko') > -1 && u.indexOf('KHTML') == -1, //火狐内核
mobile: !!u.match(/AppleWebKit.*Mobile.*/) || !!u.match(/AppleWebKit/), //是否为移动终端
ios: !!u.match(/\(i[^;]+;( U;)? CPU.+Mac OS X/), //ios终端
android: u.indexOf('Android') > -1 || u.indexOf('Linux') > -1, //android终端或者uc浏览器
iPhone: u.indexOf('iPhone') > -1 || u.indexOf('Mac') > -1, //是否为iPhone或者QQHD浏览器
iPad: u.indexOf('iPad') > -1, //是否iPad
webApp: u.indexOf('Safari') == -1 //是否web应该程序，没有头部与底部
};
}(),
language: (navigator.browserLanguage || navigator.language).toLowerCase()
}
 
if (browser.versions.ios || browser.versions.iPhone || browser.versions.iPad) {
	$('.dl_iphone').show();
	$('.btn_popTips').css('display','block');
	$('.dl_android').hide();
	if(wx==true){
		$('.dl_iphone').on('click',function(){
			
			$('.tip_wrap,.tip_ios').show()
			$('.tip_android').hide()
		})
	}

}
else if (browser.versions.android) {
	$('.dl_iphone').hide();
	$('.dl_android').show();
	if(wx==true){
		$('.dl_android').on('click',function(){
			
			$('.tip_wrap,.tip_android').show()
			$('.tip_ios').hide()
			//$('.tip_android').show()
		})
	}
}
