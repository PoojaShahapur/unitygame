package org.ffilmation.engine.renderEngines.flash9RenderEngine
{
	import org.ffilmation.engine.core.fElement;
	
	/**
	 * This stores a render message
	 * @private
	 */
	public class fRenderMessage
	{
		public var message:int;
		public var target:fElement;
		public var target2:fElement;
		
		// Constructor
		public function fRenderMessage(message:int, target:fElement, target2:fElement = null):void
		{
			this.message = message;
			this.target = target;
			this.target2 = target2;
		}
		
		public function dispose():void
		{
			this.target = null;
			this.target2 = null;
		}
	}
}