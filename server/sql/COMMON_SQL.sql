
# Host: 172.17.104.22
# Database: tx
# Table: 'CHARBASE'
# 
CREATE TABLE `CHARBASE` (
    `CHARID` int(10) unsigned NOT NULL auto_increment,
    `ACCID` int(10) unsigned NOT NULL default '0',
    `NAME` varchar(33) NOT NULL default '',
    `TYPE` smallint(5) unsigned NOT NULL default '0',
    `LEVEL` smallint(5) unsigned NOT NULL default '1',
    `FACE` int(10) unsigned NOT NULL default '0',
    `HAIR` int(10) unsigned NOT NULL default '0',
    `BODYCOLOR` int(10) unsigned NOT NULL default '0',
    `GOODNESS` int(10) unsigned NOT NULL default '0',
    `MAPID` int(10) unsigned NOT NULL default '0',
    `MAPNAME` varchar(33) default '',
    `X` int(10) unsigned NOT NULL default '0',
    `Y` int(10) unsigned NOT NULL default '0',
    `UNIONID` int(10) unsigned NOT NULL default '0',
    `SCHOOLID` int(10) unsigned NOT NULL default '0',
    `SEPTID` int(10) unsigned NOT NULL default '0',
    `HP` int(10) unsigned NOT NULL default '0',
    `MP` int(10) unsigned NOT NULL default '0',
    `SP` int(10) unsigned NOT NULL default '0',
    `EXP` bigint(20) unsigned NOT NULL default '0',
    `SKILLPOINTS` smallint(5) unsigned NOT NULL default '0',
    `POINTS` smallint(5) unsigned NOT NULL default '0',
    `COUNTRY` int(10) unsigned NOT NULL default '0',
    `CONSORT` int(10) unsigned NOT NULL default '0',
    `FORBIDTALK` bigint(20) unsigned NOT NULL default '0',
    `BITMASK` int(10) unsigned NOT NULL default '0',
    `ONLINETIME` int(10) unsigned NOT NULL default '0',
    `AVAILABLE` tinyint(3) unsigned NOT NULL default '1',
    `CON` smallint(5) unsigned NOT NULL default '0',
    `MEN` smallint(5) unsigned NOT NULL default '0',
    `INT` smallint(5) unsigned NOT NULL default '0',
    `DEX` smallint(5) unsigned NOT NULL default '0',
    `STR` smallint(5) unsigned NOT NULL default '0',
    `GRACE` int(10) unsigned NOT NULL default '0',
    `RELIVEWEAKTIME` smallint(5) unsigned NOT NULL default '0',
    `EXPLOIT` int(10) unsigned NOT NULL default '0',
    `TIRETIME` tinyblob NOT NULL,
    `OFFLINETIME` int(10) unsigned NOT NULL default '0',
    `FIVETYPE` int(10) unsigned NOT NULL default '0',
    `FIVELEVEL` int(10) unsigned NOT NULL default '0',
    `PKADDITION` int(10) unsigned NOT NULL default '0',
    `MONEY` int(10) unsigned NOT NULL default '0',
    `ANSWERCOUNT` int(10) unsigned NOT NULL default '0',
    `HONOR` int(10) unsigned NOT NULL default '0',
    `GOMAPTYPE` int(10) unsigned NOT NULL default '0',
    `MAXHONOR` int(10) unsigned NOT NULL default '0',
    `MSGTIME` int(10) unsigned NOT NULL default '0',
    `GOLD` int(10) unsigned NOT NULL default '0',
    `TICKET` int(10) unsigned NOT NULL default '0',
    `ACCPRIV` int(10) unsigned NOT NULL default '0',
    `CREATEIP` int(10) unsigned NOT NULL default '0',
    `CREATETIME` int(10) unsigned NOT NULL default '0',
    `GOLDGIVE` int(10) unsigned NOT NULL default '0',
    `PETPACK` smallint(5) unsigned NOT NULL default '0',
    `LEVELSEPT` int(10) unsigned NOT NULL default '0',
    `PETPOINT` int(10) unsigned NOT NULL default '0',
    `PUNISHTIME` int(10) unsigned NOT NULL default '0',
    `TRAINTIME` int(10) unsigned NOT NULL default '0',
    `ZS` int(10) unsigned NOT NULL default '0',
    `DOUBLETIME` int(10) unsigned NOT NULL default '0',
    `ZONE_STATE` int(10) unsigned NOT NULL default '0',
    `SOURCE_ID` int(10) unsigned NOT NULL default '0',
    `SOURCE_ZONE` int(10) unsigned NOT NULL default '0',
    `TARGET_ZONE` int(10) unsigned NOT NULL default '0',
    `ALLBINARY` blob,
    PRIMARY KEY  (`CHARID`),
    UNIQUE KEY `NAME` (`NAME`),
    KEY `ACCID` (`ACCID`),
    KEY `COUNTRYID` (`COUNTRY`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

# Host: 172.17.104.22
# Database: tx
# Table: 'SAMPLERELATION'
# 
CREATE TABLE `SAMPLERELATION` (
    `CHARID` int(10) unsigned NOT NULL default '0',
    `RELATIONID` int(10) unsigned NOT NULL default '0',
    `RELATIONNAME` varchar(33) NOT NULL default '',
    `TYPE` tinyint(3) unsigned NOT NULL default '0',
    `LASTTIME` int(10) unsigned NOT NULL default '0',
    `OCCUPATION` smallint(5) unsigned NOT NULL default '0',
    `DEGREE` smallint(5) unsigned NOT NULL default '0',
    `TODAY_ADD` int(10) unsigned NOT NULL default '0',
    `LAST_REDUCE` int(10) unsigned NOT NULL default '0',
    `REVENGE_TIME` int(10) unsigned NOT NULL default '0',
    `GROUP` int(10) unsigned NOT NULL default '0',
    PRIMARY KEY  (`CHARID`,`RELATIONID`),
    KEY `id` (`CHARID`),
    KEY `relationid` (`RELATIONID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;


# Host: 172.17.104.22
# Database: tx
# Table: 'SORTLIST'
# 
CREATE TABLE `SORTLIST` (
  `CHARID` int(10) unsigned NOT NULL default '0',
  `LEVEL` smallint(5) unsigned NOT NULL default '0',
  `EXP` bigint(20) unsigned NOT NULL default '0',
  `COUNTRY` int(10) unsigned NOT NULL default '0',
  `JOB` int(11) NOT NULL default '0',
  `NAME` varchar(33) NOT NULL default '0',
  PRIMARY KEY  (`CHARID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1; 

# Host: 172.17.104.22
# Database: tx
# Table: 'MAIL'
# 
CREATE TABLE `MAIL` (
  `ID` int(10) unsigned NOT NULL auto_increment,
  `STATE` tinyint(3) unsigned NOT NULL default '0',
  `FROMNAME` varchar(32) NOT NULL default '',
  `TONAME` varchar(32) NOT NULL default '',
  `TITLE` varchar(32) NOT NULL default '',
  `TYPE` tinyint(3) unsigned NOT NULL default '0',
  `CREATETIME` int(10) unsigned NOT NULL default '0',
  `DELTIME` int(10) unsigned NOT NULL default '0',
  `ACCESSORY` tinyint(3) unsigned NOT NULL default '0',
  `ITEMGOT` tinyint(3) unsigned NOT NULL default '0',
  `TEXT` varchar(255) NOT NULL default '',
  `SENDMONEY` int(10) unsigned NOT NULL default '0',
  `RECVMONEY` int(10) unsigned NOT NULL default '0',
  `SENDGOLD` int(10) unsigned NOT NULL default '0',
  `RECVGOLD` int(10) unsigned NOT NULL default '0',
  `BIN` blob,
  `TOID` int(10) unsigned NOT NULL default '0',
  `FROMID` int(10) unsigned NOT NULL default '0',
  `ITEMID` int(10) unsigned NOT NULL default '0',
  `DANGERVALUE` int(10) unsigned NOT NULL default '0',
  `BIN2` blob,
  `BIN3` blob,
  `ITEMID2` int(10) unsigned NOT NULL default '0',
  `ITEMID3` int(10) unsigned NOT NULL default '0',
  PRIMARY KEY  (`ID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1; 
