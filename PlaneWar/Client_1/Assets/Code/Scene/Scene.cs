using GameBox.Framework;

namespace Giant
{
    public class Scene : C0
    {
        public virtual void OnEnter()
        {
            // 场景初始化
        }

        public virtual void OnUpdate(float delta)
        {
            // 场景帧刷新
        }

        public virtual void OnLeave()
        {
            // 场景销毁
        }
    }
}

