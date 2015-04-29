﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Game.UI;
using SDK.Common;

namespace SDK.Lib
{
    public struct set
    {
        public int id;
        public string name;
        public EnPlayerCareer classs;
        public string cards;
    }

    public struct playerInfo
    {
        public string id;
        public string nickname;
        public int expack;
        public int gold;
        public float rmb;
        public string cards;
    }

    public enum shopType
    {
        Pay,
        buy
    }

    public enum moneyType
    {
        gold,
        rmb
    }
}