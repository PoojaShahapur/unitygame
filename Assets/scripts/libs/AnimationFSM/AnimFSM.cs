using UnityEngine;
using System.Collections;
using AIEngine;
using SDK.Lib;

public class AnimFSM : FSM 
{
    private BeingEntity m_beingEntity;

    public BeingEntity beingEntity
    {
        get
        {
            return m_beingEntity;
        }
        set
        {
            m_beingEntity = value;
        }
    }

    public override void InitFSM()
    {
        base.InitFSM();
    }

    public override void UpdateFSM()
    {
        base.UpdateFSM();
    }

    protected override FSMState CreateState(StateId state)
    {
        switch (state.GetId())
        {
            case "ASIDLE":
                return new AnimIdleFS(this, m_beingEntity);
            case "ASIWALK":
                return new AnimWalkFS(this, m_beingEntity);
            case "ASRUN":
                return new AnimRunFS(this, m_beingEntity);
            default:
                return null;
        }
    }
    
    public override void StopFSM()
    {
        base.StopFSM();
    }

    void OnDrawGizmos()
    {
        if(currentState != null)
        {
            currentState.OnDrawGizmos();
        }
    }
}