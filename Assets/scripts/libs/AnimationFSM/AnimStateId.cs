using AIEngine;

public class AnimStateId : StateId
{
    public static readonly AnimStateId ASIDLE = new AnimStateId("ASIDLE");
    public static readonly AnimStateId ASIWALK = new AnimStateId("ASIWALK");
    public static readonly AnimStateId ASRUN = new AnimStateId("ASRUN");

    public AnimStateId(string id)
        : base(id)
    {
    }
}