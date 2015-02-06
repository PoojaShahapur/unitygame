namespace Game.UI
{
    public enum SceneWDSCBtnEnum
    {
        eBtnBack,

        eBtnTotal
    }

    public enum SCCardNumPerPage            // 每一页卡牌的数量
    {
        eNum = 8,
    }

    public enum SCTPNumPerPage            // 总共套牌数量
    {
        eNum = 10,
    }

    public class SceneSCPath
    {
        public const string BtnClose = "wdscjm/wdscfh/btn";                         // 关闭按钮
        public const string BtnAddTaoPai = "wdscjm/setbtn/newSetBtn";               // 新增套牌
        public const string BtnPrePage = "wdscjm/SceneUI/Canvas/PrePageBtn";               // 前一页
        public const string BtnNextPage = "wdscjm/SceneUI/Canvas/NextPageBtn";               // 后一页
        public const string TextPageNum = "wdscjm/page/pagenum";               // 第一页
    }
}