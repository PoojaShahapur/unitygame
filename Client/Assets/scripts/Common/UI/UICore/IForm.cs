namespace SDK.Common
{
	/**
	 * @brief UI ½Ó¿Ú
	 */
	public interface IForm 
	{
        void init();
        void show();
        void exit();
        UIFormID getFormID();
	}
}