namespace SDK.Lib
{
	public class fNewMaterialEvent : IDispatchObject
	{
		public string id;
		public float width;
		public float height;

		public fNewMaterialEvent(string id, float width = 1, float height = -1)
		{
			this.id = id;
			this.width = width;
			this.height = height;
		}
	}
}