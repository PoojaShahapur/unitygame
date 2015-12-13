package org.ffilmation.engine.renderEngines.flash9RenderEngine
{
	//import com.pblabs.engine.entity.EntityCValue;

	//import flash.display.Bitmap;
	//import flash.display.BitmapData;
	import flash.display.Sprite;
	import flash.filters.GlowFilter;
	import flash.text.TextField;
	import flash.text.TextFieldAutoSize;
	
	import org.ffilmation.engine.core.fElementContainer;
	import org.ffilmation.engine.elements.fEmptySprite;
	//import org.ffilmation.engine.elements.fObject;
	import org.ffilmation.utils.movieClipUtils;
	import org.ffilmation.utils.objectPool;
	/**
	 * ...
	 * @author 
	 */
	public class fFlash9EmptySpriteRenderer extends fFlash9ElementRenderer
	{
		// Private properties
		protected var baseObj:Sprite;		
		//protected var _needDrawTextField:Boolean = false;
		protected var _shadowSp:Sprite;
		
		protected var m_uiLayer:Sprite;
		
		function fFlash9EmptySpriteRenderer(rEngine:fFlash9RenderEngine, container:fElementContainer, element:fEmptySprite):void
		{
			// Previous
			super(rEngine, element, null, container);
		}
		
		public override function createAssets():void
		{
			// Attach base clip
			this.baseObj = objectPool.getInstanceOf(Sprite) as Sprite;
			container.addChild(this.baseObj);
			this.baseObj.mouseEnabled = false;
			
			
			// test: 
			var element:fEmptySprite = this.element as fEmptySprite;
			if(element.scene.m_sceneConfig.bshowEmptyCenter)
			{
				addSimpleShadow();
			}
			
			this.m_uiLayer = new Sprite();
			this.container.addChild(this.m_uiLayer);
		}		
				
		override public function onTick(deltaTime:Number):void
		{
			
		}
		
		private function addSimpleShadow():void
		{
			_shadowSp ||= new Sprite();
			_shadowSp.mouseEnabled = false;
			var clip:Sprite = new Sprite();
			if (!_shadowSp.numChildren)
			{
				clip = objectPool.getInstanceOf(Sprite) as Sprite;
				movieClipUtils.circle(clip.graphics, 0, 0, 30, 20, 0x00FF00, 100);
				_shadowSp.addChild(clip);
				container.addChild(this._shadowSp);
			}
		}
		
		//override public function set rawTextField(b:Boolean):void
		//{
			//_needDrawTextField = b;
		//}
		public function removeUI():void
		{
			while (m_uiLayer.numChildren > 0)
			{
				m_uiLayer.removeChildAt(0);
			}
		}
		
		public function get uiLayer():Sprite
		{
			return m_uiLayer;
		}		
		
	}
}