namespace KBEngine
{
  	using UnityEngine; 
	using System; 
	using System.Collections; 
	using System.Collections.Generic;
	
    public class SnowBlock : KBEngine.GameObject   
    {
    	public Combat combat = null;
    	
    	public static SkillBox skillbox = new SkillBox();
    	
		public SnowBlock()
		{
            mEntity_SDK = new SDK.Lib.SnowBlock();
            mEntity_SDK.setEntity_KBE(this);
        }
		
		public override void __init__()
		{
            mEntity_SDK.init();
        }

		public override void onDestroy ()
		{
			if(isPlayer())
			{
				KBEngine.Event.deregisterIn(this);
			}

            mEntity_SDK.dispose();
        }

        public override void set_position(object old)
        {
            base.set_position(old);
            this.mEntity_SDK.setOriginal(this.position);
        }

        public override void set_direction(object old)
        {
            base.set_direction(old);
            this.mEntity_SDK.setRotateEulerAngle(this.direction);
        }

        public virtual void updatePlayer(float x, float y, float z, float yaw)
		{
	    	position.x = x;
	    	position.y = y;
	    	position.z = z;
			
	    	direction.z = yaw;
		}
		
		public override void onEnterWorld()
		{
			base.onEnterWorld();
		}

        public override void onLeaveWorld()
        {
            base.onLeaveWorld();
            this.mEntity_SDK.dispose();
        }
    }
} 
