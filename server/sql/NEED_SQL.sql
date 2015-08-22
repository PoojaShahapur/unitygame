
CREATE TABLE `SERVERLIST` (
  `ID` int(10) unsigned NOT NULL auto_increment,
    `TYPE` int(10) unsigned NOT NULL default '0',
      `IP` varchar(16) NOT NULL default '127.0.0.1',
      `PORT` int(10) unsigned NOT NULL default '0',
      PRIMARY KEY  (`ID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

CREATE TABLE `GAMETIME` (
  `GAMETIME` bigint(20) unsigned NOT NULL default '0'
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

################################## LoginServer(FLServer) ##########################################################
CREATE TABLE `LOGIN`(
    `ACCID` int(10) unsigned NOT NULL default '0',
    `PASSWORD` varchar(33) NOT NULL default '',
    `LOGINNAME` varchar(33) NOT NULL default '',
    `ISUSED` smallint(5) unsigned NOT NULL default '0',
    `ISFORBID` smallint(5) unsigned NOT NULL default '0',
    PRIMARY KEY  (`ACCID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

################################# roleRegServer ########################################################################
DROP TABLE IF EXISTS `ROLEREG0000`;
CREATE TABLE `ROLEREG0000` ( 
    `CHARID` int(11) unsigned NOT NULL auto_increment, 
    `NAME` char(33) NOT NULL default '', 
    `GAME` int(11) default '0',
    `ACCID` int(11) default '0', 
    `ZONE` int(11) default '0',
    PRIMARY KEY  (`CHARID`),  UNIQUE KEY `index` (`NAME`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1 AUTO_INCREMENT=10000000 MAX_ROWS=10000000 ROW_FORMAT=FIXED;

DROP TABLE IF EXISTS `ROLEREG0001`;
CREATE TABLE `ROLEREG0001` ( 
    `CHARID` int(11) unsigned NOT NULL auto_increment, 
    `NAME` char(33) NOT NULL default '', 
    `GAME` int(11) default '0',
    `ACCID` int(11) default '0', 
    `ZONE` int(11) default '0',
    PRIMARY KEY  (`CHARID`),  UNIQUE KEY `index` (`NAME`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1 AUTO_INCREMENT=20000000 MAX_ROWS=10000000 ROW_FORMAT=FIXED;

DROP TABLE IF EXISTS `ROLEREG0002`;
CREATE TABLE `ROLEREG0002` ( 
    `CHARID` int(11) unsigned NOT NULL auto_increment, 
    `NAME` char(33) NOT NULL default '', 
    `GAME` int(11) default '0',
    `ACCID` int(11) default '0', 
    `ZONE` int(11) default '0',
    PRIMARY KEY  (`CHARID`),  UNIQUE KEY `index` (`NAME`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1 AUTO_INCREMENT=30000000 MAX_ROWS=10000000 ROW_FORMAT=FIXED;

DROP TABLE IF EXISTS `ROLEREG0003`;
CREATE TABLE `ROLEREG0003` ( 
    `CHARID` int(11) unsigned NOT NULL auto_increment, 
    `NAME` char(33) NOT NULL default '', 
    `GAME` int(11) default '0',
    `ACCID` int(11) default '0', 
    `ZONE` int(11) default '0',
    PRIMARY KEY  (`CHARID`),  UNIQUE KEY `index` (`NAME`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1 AUTO_INCREMENT=40000000 MAX_ROWS=10000000 ROW_FORMAT=FIXED;

DROP TABLE IF EXISTS `ROLEREG0004`;
CREATE TABLE `ROLEREG0004` ( 
    `CHARID` int(11) unsigned NOT NULL auto_increment, 
    `NAME` char(33) NOT NULL default '', 
    `GAME` int(11) default '0',
    `ACCID` int(11) default '0', 
    `ZONE` int(11) default '0',
    PRIMARY KEY  (`CHARID`),  UNIQUE KEY `index` (`NAME`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1 AUTO_INCREMENT=50000000 MAX_ROWS=10000000 ROW_FORMAT=FIXED;

DROP TABLE IF EXISTS `ROLEREG0005`;
CREATE TABLE `ROLEREG0005` ( 
    `CHARID` int(11) unsigned NOT NULL auto_increment, 
    `NAME` char(33) NOT NULL default '', 
    `GAME` int(11) default '0',
    `ACCID` int(11) default '0', 
    `ZONE` int(11) default '0',
    PRIMARY KEY  (`CHARID`),  UNIQUE KEY `index` (`NAME`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1 AUTO_INCREMENT=60000000 MAX_ROWS=10000000 ROW_FORMAT=FIXED;

DROP TABLE IF EXISTS `ROLEREG0006`;
CREATE TABLE `ROLEREG0006` ( 
    `CHARID` int(11) unsigned NOT NULL auto_increment, 
    `NAME` char(33) NOT NULL default '', 
    `GAME` int(11) default '0',
    `ACCID` int(11) default '0', 
    `ZONE` int(11) default '0',
    PRIMARY KEY  (`CHARID`),  UNIQUE KEY `index` (`NAME`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1 AUTO_INCREMENT=70000000 MAX_ROWS=10000000 ROW_FORMAT=FIXED;

DROP TABLE IF EXISTS `ROLEREG0007`;
CREATE TABLE `ROLEREG0007` ( 
    `CHARID` int(11) unsigned NOT NULL auto_increment, 
    `NAME` char(33) NOT NULL default '', 
    `GAME` int(11) default '0',
    `ACCID` int(11) default '0', 
    `ZONE` int(11) default '0',
    PRIMARY KEY  (`CHARID`),  UNIQUE KEY `index` (`NAME`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1 AUTO_INCREMENT=80000000 MAX_ROWS=10000000 ROW_FORMAT=FIXED;

DROP TABLE IF EXISTS `ROLEREG0008`;
CREATE TABLE `ROLEREG0008` ( 
    `CHARID` int(11) unsigned NOT NULL auto_increment, 
    `NAME` char(33) NOT NULL default '', 
    `GAME` int(11) default '0',
    `ACCID` int(11) default '0', 
    `ZONE` int(11) default '0',
    PRIMARY KEY  (`CHARID`),  UNIQUE KEY `index` (`NAME`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1 AUTO_INCREMENT=90000000 MAX_ROWS=10000000 ROW_FORMAT=FIXED;

DROP TABLE IF EXISTS `ROLEREG0009`;
CREATE TABLE `ROLEREG0009` ( 
    `CHARID` int(11) unsigned NOT NULL auto_increment, 
    `NAME` char(33) NOT NULL default '', 
    `GAME` int(11) default '0',
    `ACCID` int(11) default '0', 
    `ZONE` int(11) default '0',
    PRIMARY KEY  (`CHARID`),  UNIQUE KEY `index` (`NAME`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1 AUTO_INCREMENT=100000000 MAX_ROWS=10000000 ROW_FORMAT=FIXED;

DROP TABLE IF EXISTS `ROLEREG0010`;
CREATE TABLE `ROLEREG0010` ( 
    `CHARID` int(11) unsigned NOT NULL auto_increment, 
    `NAME` char(33) NOT NULL default '', 
    `GAME` int(11) default '0',
    `ACCID` int(11) default '0', 
    `ZONE` int(11) default '0',
    PRIMARY KEY  (`CHARID`),  UNIQUE KEY `index` (`NAME`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1 AUTO_INCREMENT=110000000 MAX_ROWS=10000000 ROW_FORMAT=FIXED;

DROP TABLE IF EXISTS `ROLEREG0011`;
CREATE TABLE `ROLEREG0011` ( 
    `CHARID` int(11) unsigned NOT NULL auto_increment, 
    `NAME` char(33) NOT NULL default '', 
    `GAME` int(11) default '0',
    `ACCID` int(11) default '0', 
    `ZONE` int(11) default '0',
    PRIMARY KEY  (`CHARID`),  UNIQUE KEY `index` (`NAME`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1 AUTO_INCREMENT=120000000 MAX_ROWS=10000000 ROW_FORMAT=FIXED;

DROP TABLE IF EXISTS `ROLEREG0012`;
CREATE TABLE `ROLEREG0012` ( 
    `CHARID` int(11) unsigned NOT NULL auto_increment, 
    `NAME` char(33) NOT NULL default '', 
    `GAME` int(11) default '0',
    `ACCID` int(11) default '0', 
    `ZONE` int(11) default '0',
    PRIMARY KEY  (`CHARID`),  UNIQUE KEY `index` (`NAME`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1 AUTO_INCREMENT=130000000 MAX_ROWS=10000000 ROW_FORMAT=FIXED;

DROP TABLE IF EXISTS `ROLEREG0013`;
CREATE TABLE `ROLEREG0013` ( 
    `CHARID` int(11) unsigned NOT NULL auto_increment, 
    `NAME` char(33) NOT NULL default '', 
    `GAME` int(11) default '0',
    `ACCID` int(11) default '0', 
    `ZONE` int(11) default '0',
    PRIMARY KEY  (`CHARID`),  UNIQUE KEY `index` (`NAME`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1 AUTO_INCREMENT=140000000 MAX_ROWS=10000000 ROW_FORMAT=FIXED;

DROP TABLE IF EXISTS `ROLEREG0014`;
CREATE TABLE `ROLEREG0014` ( 
    `CHARID` int(11) unsigned NOT NULL auto_increment, 
    `NAME` char(33) NOT NULL default '', 
    `GAME` int(11) default '0',
    `ACCID` int(11) default '0', 
    `ZONE` int(11) default '0',
    PRIMARY KEY  (`CHARID`),  UNIQUE KEY `index` (`NAME`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1 AUTO_INCREMENT=150000000 MAX_ROWS=10000000 ROW_FORMAT=FIXED;

DROP TABLE IF EXISTS `ROLEREG0015`;
CREATE TABLE `ROLEREG0015` ( 
    `CHARID` int(11) unsigned NOT NULL auto_increment, 
    `NAME` char(33) NOT NULL default '', 
    `GAME` int(11) default '0',
    `ACCID` int(11) default '0', 
    `ZONE` int(11) default '0',
    PRIMARY KEY  (`CHARID`),  UNIQUE KEY `index` (`NAME`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1 AUTO_INCREMENT=160000000 MAX_ROWS=10000000 ROW_FORMAT=FIXED;

DROP TABLE IF EXISTS `ROLEREG0016`;
CREATE TABLE `ROLEREG0016` ( 
    `CHARID` int(11) unsigned NOT NULL auto_increment, 
    `NAME` char(33) NOT NULL default '', 
    `GAME` int(11) default '0',
    `ACCID` int(11) default '0', 
    `ZONE` int(11) default '0',
    PRIMARY KEY  (`CHARID`),  UNIQUE KEY `index` (`NAME`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1 AUTO_INCREMENT=170000000 MAX_ROWS=10000000 ROW_FORMAT=FIXED;

DROP TABLE IF EXISTS `ROLEREG0017`;
CREATE TABLE `ROLEREG0017` ( 
    `CHARID` int(11) unsigned NOT NULL auto_increment, 
    `NAME` char(33) NOT NULL default '', 
    `GAME` int(11) default '0',
    `ACCID` int(11) default '0', 
    `ZONE` int(11) default '0',
    PRIMARY KEY  (`CHARID`),  UNIQUE KEY `index` (`NAME`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1 AUTO_INCREMENT=180000000 MAX_ROWS=10000000 ROW_FORMAT=FIXED;

DROP TABLE IF EXISTS `ROLEREG0018`;
CREATE TABLE `ROLEREG0018` ( 
    `CHARID` int(11) unsigned NOT NULL auto_increment, 
    `NAME` char(33) NOT NULL default '', 
    `GAME` int(11) default '0',
    `ACCID` int(11) default '0', 
    `ZONE` int(11) default '0',
    PRIMARY KEY  (`CHARID`),  UNIQUE KEY `index` (`NAME`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1 AUTO_INCREMENT=190000000 MAX_ROWS=10000000 ROW_FORMAT=FIXED;

DROP TABLE IF EXISTS `ROLEREG0019`;
CREATE TABLE `ROLEREG0019` ( 
    `CHARID` int(11) unsigned NOT NULL auto_increment, 
    `NAME` char(33) NOT NULL default '', 
    `GAME` int(11) default '0',
    `ACCID` int(11) default '0', 
    `ZONE` int(11) default '0',
    PRIMARY KEY  (`CHARID`),  UNIQUE KEY `index` (`NAME`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1 AUTO_INCREMENT=200000000 MAX_ROWS=10000000 ROW_FORMAT=FIXED;

DROP TABLE IF EXISTS `ROLEREG0020`;
CREATE TABLE `ROLEREG0020` ( 
    `CHARID` int(11) unsigned NOT NULL auto_increment, 
    `NAME` char(33) NOT NULL default '', 
    `GAME` int(11) default '0',
    `ACCID` int(11) default '0', 
    `ZONE` int(11) default '0',
    PRIMARY KEY  (`CHARID`),  UNIQUE KEY `index` (`NAME`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1 AUTO_INCREMENT=210000000 MAX_ROWS=10000000 ROW_FORMAT=FIXED;

DROP TABLE IF EXISTS `ROLEREG0021`;
CREATE TABLE `ROLEREG0021` ( 
    `CHARID` int(11) unsigned NOT NULL auto_increment, 
    `NAME` char(33) NOT NULL default '', 
    `GAME` int(11) default '0',
    `ACCID` int(11) default '0', 
    `ZONE` int(11) default '0',
    PRIMARY KEY  (`CHARID`),  UNIQUE KEY `index` (`NAME`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1 AUTO_INCREMENT=220000000 MAX_ROWS=10000000 ROW_FORMAT=FIXED;

DROP TABLE IF EXISTS `ROLEREG0022`;
CREATE TABLE `ROLEREG0022` ( 
    `CHARID` int(11) unsigned NOT NULL auto_increment, 
    `NAME` char(33) NOT NULL default '', 
    `GAME` int(11) default '0',
    `ACCID` int(11) default '0', 
    `ZONE` int(11) default '0',
    PRIMARY KEY  (`CHARID`),  UNIQUE KEY `index` (`NAME`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1 AUTO_INCREMENT=230000000 MAX_ROWS=10000000 ROW_FORMAT=FIXED;

DROP TABLE IF EXISTS `ROLEREG0023`;
CREATE TABLE `ROLEREG0023` ( 
    `CHARID` int(11) unsigned NOT NULL auto_increment, 
    `NAME` char(33) NOT NULL default '', 
    `GAME` int(11) default '0',
    `ACCID` int(11) default '0', 
    `ZONE` int(11) default '0',
    PRIMARY KEY  (`CHARID`),  UNIQUE KEY `index` (`NAME`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1 AUTO_INCREMENT=240000000 MAX_ROWS=10000000 ROW_FORMAT=FIXED;

DROP TABLE IF EXISTS `ROLEREG0024`;
CREATE TABLE `ROLEREG0024` ( 
    `CHARID` int(11) unsigned NOT NULL auto_increment, 
    `NAME` char(33) NOT NULL default '', 
    `GAME` int(11) default '0',
    `ACCID` int(11) default '0', 
    `ZONE` int(11) default '0',
    PRIMARY KEY  (`CHARID`),  UNIQUE KEY `index` (`NAME`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1 AUTO_INCREMENT=250000000 MAX_ROWS=10000000 ROW_FORMAT=FIXED;

DROP TABLE IF EXISTS `ROLEREG0025`;
CREATE TABLE `ROLEREG0025` ( 
    `CHARID` int(11) unsigned NOT NULL auto_increment, 
    `NAME` char(33) NOT NULL default '', 
    `GAME` int(11) default '0',
    `ACCID` int(11) default '0', 
    `ZONE` int(11) default '0',
    PRIMARY KEY  (`CHARID`),  UNIQUE KEY `index` (`NAME`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1 AUTO_INCREMENT=260000000 MAX_ROWS=10000000 ROW_FORMAT=FIXED;

DROP TABLE IF EXISTS `ROLEREG0026`;
CREATE TABLE `ROLEREG0026` ( 
    `CHARID` int(11) unsigned NOT NULL auto_increment, 
    `NAME` char(33) NOT NULL default '', 
    `GAME` int(11) default '0',
    `ACCID` int(11) default '0', 
    `ZONE` int(11) default '0',
    PRIMARY KEY  (`CHARID`),  UNIQUE KEY `index` (`NAME`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1 AUTO_INCREMENT=270000000 MAX_ROWS=10000000 ROW_FORMAT=FIXED;

DROP TABLE IF EXISTS `ROLEREG0027`;
CREATE TABLE `ROLEREG0027` ( 
    `CHARID` int(11) unsigned NOT NULL auto_increment, 
    `NAME` char(33) NOT NULL default '', 
    `GAME` int(11) default '0',
    `ACCID` int(11) default '0', 
    `ZONE` int(11) default '0',
    PRIMARY KEY  (`CHARID`),  UNIQUE KEY `index` (`NAME`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1 AUTO_INCREMENT=280000000 MAX_ROWS=10000000 ROW_FORMAT=FIXED;

DROP TABLE IF EXISTS `ROLEREG0028`;
CREATE TABLE `ROLEREG0028` ( 
    `CHARID` int(11) unsigned NOT NULL auto_increment, 
    `NAME` char(33) NOT NULL default '', 
    `GAME` int(11) default '0',
    `ACCID` int(11) default '0', 
    `ZONE` int(11) default '0',
    PRIMARY KEY  (`CHARID`),  UNIQUE KEY `index` (`NAME`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1 AUTO_INCREMENT=290000000 MAX_ROWS=10000000 ROW_FORMAT=FIXED;

DROP TABLE IF EXISTS `ROLEREG0029`;
CREATE TABLE `ROLEREG0029` ( 
    `CHARID` int(11) unsigned NOT NULL auto_increment, 
    `NAME` char(33) NOT NULL default '', 
    `GAME` int(11) default '0',
    `ACCID` int(11) default '0', 
    `ZONE` int(11) default '0',
    PRIMARY KEY  (`CHARID`),  UNIQUE KEY `index` (`NAME`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1 AUTO_INCREMENT=300000000 MAX_ROWS=10000000 ROW_FORMAT=FIXED;

DROP TABLE IF EXISTS `ROLEREG0030`;
CREATE TABLE `ROLEREG0030` ( 
    `CHARID` int(11) unsigned NOT NULL auto_increment, 
    `NAME` char(33) NOT NULL default '', 
    `GAME` int(11) default '0',
    `ACCID` int(11) default '0', 
    `ZONE` int(11) default '0',
    PRIMARY KEY  (`CHARID`),  UNIQUE KEY `index` (`NAME`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1 AUTO_INCREMENT=310000000 MAX_ROWS=10000000 ROW_FORMAT=FIXED;

DROP TABLE IF EXISTS `ROLEREG0031`;
CREATE TABLE `ROLEREG0031` ( 
    `CHARID` int(11) unsigned NOT NULL auto_increment, 
    `NAME` char(33) NOT NULL default '', 
    `GAME` int(11) default '0',
    `ACCID` int(11) default '0', 
    `ZONE` int(11) default '0',
    PRIMARY KEY  (`CHARID`),  UNIQUE KEY `index` (`NAME`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1 AUTO_INCREMENT=320000000 MAX_ROWS=10000000 ROW_FORMAT=FIXED;
################################# end of roleRegServer ########################################################################


################################# roleChangeServer ########################################################################
CREATE TABLE `SENDERROR`(
    `NO` bigint(20) unsigned NOT NULL auto_increment,
    `rTimestamp` datetime default '0000-00-00 00:00:00',	    #插入的时间
    `fromGameZone` int(10) unsigned default '0',		    #来自得游戏区
    `toGameZone` int(10) unsigned default '0',			    #角色要去的游戏区
    `SendSign` int(10) unsigned default '0',			    #是否已经重发过
    `MESSAGE` blob,						    #角色数据
    `nCmdLen` int(10) unsigned default '0',			    #MESSAGE长度
    PRIMARY KEY  (`NO`)
)ENGINE=MyISAM DEFAULT CHARSET=latin1;

CREATE TABLE `ROLECHANGE20150213`(		    #成功发送角色信息的表(按天存放)
    `NO` bigint(20) unsigned NOT NULL auto_increment,
    `rTimestamp` datetime default '0000-00-00 00:00:00',	    #插入的时间
    `fromGameZone` int(10) unsigned default '0',		    #来自得游戏区
    `toGameZone` int(10) unsigned default '0',			    #角色要去的游戏区
    `SendSign` int(10) unsigned default '0',			    #是否已经重发过
    `MESSAGE` blob,						    #角色数据
    `nCmdLen` int(10) unsigned default '0',			    #MESSAGE长度
    PRIMARY KEY  (`NO`)
)ENGINE=MyISAM DEFAULT CHARSET=latin1;

DROP TABLE IF EXISTS `changeZoneLogin`;
CREATE TABLE `changeZoneLogin` (
    `NO` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
    `accid` int(11) unsigned DEFAULT '0',
    `secretkey` int(11) unsigned DEFAULT '0', 
    `game` int(10) unsigned NOT NULL DEFAULT '0', 
    `zone` int(10) unsigned NOT NULL DEFAULT '0',
    `macAddr` char(16) NOT NULL DEFAULT '',
    `rTimestamp` bigint(20) unsigned DEFAULT '0',
    PRIMARY KEY (`NO`)
) ENGINE=MEMORY AUTO_INCREMENT=6572 DEFAULT CHARSET=latin1;
################################# end of roleChangeServer ########################################################################
