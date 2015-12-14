namespace SDK.Lib
{
	public class fCollideEvent : IDispatchObject
	{
		public fRenderableElement victim;

		public fCollideEvent(fRenderableElement victim)
		{
			this.victim = victim;
		}
	}
}