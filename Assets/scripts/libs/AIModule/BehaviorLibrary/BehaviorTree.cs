using System;
using System.Collections.Generic;
using BehaviorLibrary.Components;
using BehaviorLibrary.Components.Composites;
using SDK.Common;

namespace BehaviorLibrary
{
    public enum BehaviorReturnCode
    {
        Failure,
        Success,
        Running
    }

    public delegate BehaviorReturnCode BehaviorReturn();

    /// <summary>
    /// @brief 行为树只处理结构，没有实例数据，共享同一个行为树
    /// </summary>
    public class BehaviorTree : IBehaviorTree
    {
		private BehaviorComponent _Root;
        private BehaviorReturnCode _ReturnCode;

        protected string m_name;            // 行为树名字
        protected InsParam m_inputParam;    // 输入数据

        public BehaviorReturnCode ReturnCode
        {
            get { return _ReturnCode; }
            set { _ReturnCode = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="root"></param>
        public BehaviorTree(IndexSelector root)
        {
            _Root = root;
            m_inputParam = new InsParam();
        }

        public string name
        {
            get
            {
                return m_name;
            }
            set
            {
                m_name = value;
            }
        }

        public InsParam inputParam
        {
            get
            {
                return m_inputParam;
            }
            set
            {
                m_inputParam = value;
            }
        }

        public BehaviorTree(BehaviorComponent root)
        {
			_Root = root;
            _Root.behaviorTree = this;
            m_inputParam = new InsParam();
		}

        public BehaviorComponent root
        {
            get
            {
                return _Root;
            }
        }

        /// <summary>
        /// perform the behavior
        /// </summary>
        public BehaviorReturnCode Behave()
        {
            try
            {
                switch (_Root.Behave())
                {
                    case BehaviorReturnCode.Failure:
                        ReturnCode = BehaviorReturnCode.Failure;
                        return ReturnCode;
                    case BehaviorReturnCode.Success:
                        ReturnCode = BehaviorReturnCode.Success;
                        return ReturnCode;
                    case BehaviorReturnCode.Running:
                        ReturnCode = BehaviorReturnCode.Running;
                        return ReturnCode;
                    default:
                        ReturnCode = BehaviorReturnCode.Running;
                        return ReturnCode;
                }
            }
            catch (Exception e)
            {
#if DEBUG
                Console.Error.WriteLine(e.ToString());
#endif
                ReturnCode = BehaviorReturnCode.Failure;
                return ReturnCode;
            }
        }
    }
}