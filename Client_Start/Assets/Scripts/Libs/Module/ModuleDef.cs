namespace SDK.Lib
{
    public class ModulePath
    {
        public const string LOGINMN = "RootLayer/Login";
        public const string GAMEMN = NotDestroyPath.ND_CV_Game;
        public const string AUTOUPDATEMN = "RootLayer/AutoUpdate";
    }

    public class ModuleName
    {
        public const string LOGINMN = "Login";
        public const string GAMEMN = "Game";
        public const string AUTOUPDATEMN = "AutoUpdate";
    }

    public enum ModuleID
    {
        LOGINMN,
        GAMEMN,
        AUTOUPDATEMN,
    }
}