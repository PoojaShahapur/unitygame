using System.Collections.Generic;
namespace CreateAnimatorController
{
    public class AnimatorControllerCreate
    {
        protected string m_inPath;
        protected string m_outPath;
        protected string m_outName;
        protected string m_outExtName;

        protected Params m_param;
        protected Layers m_layers;
    }

    public class Params
    {
        protected List<Param> m_paramList;
    }

    public class Param
    {
        protected string m_name;
        protected string m_type;
    }

    public class Layers
    {
        protected List<Layer> m_layerList;
    }

    public class Layer
    {
        protected List<StateMachine> m_stateMachineList;
    }

    public class StateMachine
    {
        protected List<State> m_stateList;
    }

    public class State
    {
        protected string m_motion;
        protected Condition m_condition;
    }

    public class Condition
    {
        protected string m_name;
        protected string m_value;
    }
}