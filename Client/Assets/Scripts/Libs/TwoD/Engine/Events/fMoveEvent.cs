namespace SDK.Lib
{
	public class fMoveEvent : IDispatchObject
	{
		public float dx;
		public float dy;
		public float dz;
		
		public fMoveEvent(float dx, float dy, float dz)
		{
			this.dx = dx;
			this.dy = dy;
			this.dz = dz;
		}
	}
}