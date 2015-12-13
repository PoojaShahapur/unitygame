package org.ffilmation.engine.renderEngines.flash9RenderEngine
{
	import com.pblabs.engine.entity.BeingEntity;
	import com.pblabs.engine.entity.EntityCValue;
	import com.util.DebugBox;
	import com.util.PBUtil;
	//import com.util.UtilFilter;
	
	//import flash.display.Bitmap;
	//import flash.display.BitmapData;
	//import flash.display.Sprite;
	//import flash.filters.BitmapFilterQuality;
	//import flash.filters.BitmapFilterType;
	
	//import flash.filters.GlowFilter;
	import flash.filters.GradientGlowFilter;
	import flash.geom.Point;
	
	import org.ffilmation.engine.core.fElementContainer;
	import org.ffilmation.engine.elements.fObject;
	import org.ffilmation.engine.elements.fSceneObject;
	//import org.ffilmation.utils.objectPool;
	
	/**
	 * ...
	 * @author zouzhiqiang
	 */
	public class fFlash9SceneObjectSeqRenderer extends fFlash9ObjectSeqRenderer
	{
		//private var _needDrawTextField:Boolean = false;
		
		// KBEN: 掉血数字    
		
		//protected var m_picFilter:GlowFilter;			// 显示的图片的滤镜
		//protected var m_picFilter:GradientGlowFilter; // 显示的图片的滤镜		
		
		public function fFlash9SceneObjectSeqRenderer(rEngine:fFlash9RenderEngine, container:fElementContainer, element:fObject):void
		{
			super(rEngine, container, element);
		}
		
		//override public function set rawTextField(b:Boolean):void
		//{
		//_needDrawTextField = b;
		//}
		
		public override function createAssets():void
		{
			super.createAssets();
			
			//this.m_picFilter = null;
			
			var ele:fSceneObject = this.element as fSceneObject;
			if (ele.scene.m_sceneConfig.bshowCharCenter)
			{
				super.addSimpleShadow();
			}
			
			/*if (ele is BeingEntity)
			{
				(ele as BeingEntity).onCreateAssets();
			}*/
			
			//super.addImageShadow();
		}
		
		override public function changeInfoByActDir(act:uint, dir:uint):void
		{
			var el:fSceneObject = this.element as fSceneObject;
			super.changeInfoByActDir(act, dir);
			
			if (_baseObj && el.scene.m_sceneConfig.m_background)
			{
				this._baseObj.graphics.clear();
				this._baseObj.graphics.beginFill(0xff0000, 0.5)
				this._baseObj.graphics.drawRect(element.bounds2d.x, element.bounds2d.y, element.bounds2d.width, element.bounds2d.height);
				this._baseObj.graphics.endFill();
			}
		}
		
		// 这个就是占位资源设置的时候需要设置的参数
		public function defaultPlaceInfo():void
		{
			var el:fSceneObject = this.element as fSceneObject;
			el.defaultPlaceInfo();
			
			if (_textField)
			{
				//var tWidth:uint = 150;
				var tWidth:uint = _textField.width;
				//_textField.x = element.bounds2d.x + (element.bounds2d.width - tWidth) / 2;
				//_textField.y = element.bounds2d.y - 40;
				_textField.x = element.m_tagBounds2d.x + (element.m_tagBounds2d.width - tWidth) / 2;
				_textField.y = element.m_tagBounds2d.y - 20;
			}
			
			if (_currentBitMap)
			{
				_currentBitMap.x = element.bounds2d.x;
				_currentBitMap.y = element.bounds2d.y;
			}
			
			if (_baseObj && el.scene.m_sceneConfig.m_background)
			{
				this._baseObj.graphics.clear();
				this._baseObj.graphics.beginFill(0xff0000, 0.5)
				this._baseObj.graphics.drawRect(element.bounds2d.x, element.bounds2d.y, element.bounds2d.width, element.bounds2d.height);
				this._baseObj.graphics.endFill();
			}
		}
		
		override public function onTick(deltaTime:Number):void
		{
			super.onTick(deltaTime);
			var act:uint = (this.element as fObject).getAction();
			
			if (this.screenVisible)
			{
				var el:fObject = this.element as fObject;
				
				if (!isLoaded(act, this._currentSpriteIndex) && (this.element as fSceneObject).scene)
				{
					if (EntityCValue.PHBEING == el.m_resType)
					{
						//_currentBitMap.bitmapData = (this.element as fSceneObject).scene.engine.m_context.m_playerPlace;
						_currentBitMap.bitmapData = (this.element as fSceneObject).scene.engine.m_context.m_replaceResSys.playerPlace;
					}
					else if (EntityCValue.PHFOBJ == el.m_resType)
					{
						//_currentBitMap.bitmapData = (this.element as fSceneObject).scene.engine.m_context.m_thingPlace;
						_currentBitMap.bitmapData = (this.element as fSceneObject).scene.engine.m_context.m_replaceResSys.thingPlace;
					}
					
					// 需要设置位置
					defaultPlaceInfo();
				}
			}
		}
		
		override public function hitTest(globalx:Number, globaly:Number):Boolean
		{
			// 直接图片判断
			/*
			if (_frames && currentFrame < _frames.length)
			{
				if(_frames[currentFrame])
				{
					if (_frames[currentFrame].hitTest(new Point(0, 0), 100, _currentBitMap.globalToLocal(new Point(globalx, globaly))))
					{
						return true;
					}
				}
			}
			*/
			
			if (_currentBitMap && _currentBitMap.bitmapData)
			{
				try
				{
					if (_currentBitMap.bitmapData.hitTest(new Point(0, 0), 100, _currentBitMap.globalToLocal(new Point(globalx, globaly))))
					{
						return true;
					}
					//else if ((this.element as fObject).curHorseData)	// 坐骑判断
					else if(this.hasMountsRenderData)
					{
						if (_currentHorseBitMap.bitmapData.hitTest(new Point(0, 0), 100, _currentHorseBitMap.globalToLocal(new Point(globalx, globaly))))
						{
							return true;
						}
					}
				}
				catch (e:Error)
				{
					var str:String = "发现这个框后，要告诉我！！fFlash9SceneObjectSeqRenderer::hitTest;" + e.message;
					DebugBox.addLog(str);
					DebugBox.info(str);	
				}
			}
			
			return false;
		}
		
		override public function onMouseEnter():void
		{
			if (!m_picFilter)
			{
				//m_picFilter = new GlowFilter(0xFF0000, 0.5, 6, 6, 32);
				//var m_picFilter:GradientGlowFilter = new GradientGlowFilter();
				//m_picFilter.distance = 0;
				//m_picFilter.angle = 45;
				//m_picFilter.colors = [0x000000, 0xFF0000];
				//m_picFilter.alphas = [0, 1];
				//m_picFilter.ratios = [0, 255];
				//m_picFilter.blurX = 10;
				//m_picFilter.blurY = 10;
				//m_picFilter.strength = 2;
				//m_picFilter.quality = BitmapFilterQuality.HIGH;
				//m_picFilter.type = BitmapFilterType.OUTER;
				m_picFilter = PBUtil.buildGradientGlowFilter();
			}
			
			// 判断鼠标移动上去的颜色
			if ((this.element as BeingEntity).canAttacked)
			{
				m_picFilter.colors = [0x000000, 0xFF0000];
			}
			else
			{
				m_picFilter.colors = [0x000000, 0x00FF00];
			}
			
			this._currentBitMap.filters = [m_picFilter];
			
			if (this.hasMountsRenderData)
			{
				this._currentHorseBitMap.filters = [m_picFilter];
			}
		}
		
		public function set filters(ar:Array):void
		{
			if (this._currentBitMap)
			{
				this._currentBitMap.filters = ar;
			}
			
			//if (this.hasMountsRenderData)
			if(this._currentHorseBitMap)
			{
				this._currentHorseBitMap.filters = ar;
			}
		}
		
		override public function onMouseLeave():void
		{
			this._currentBitMap.filters = null;
			
			//if (this.hasMountsRenderData)
			//{
				this._currentHorseBitMap.filters = null;
			//}
		}	
	}
}