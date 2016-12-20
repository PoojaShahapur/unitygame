namespace SDK.Lib
{
    public enum MEventType
    {
        World_3D,   // Perform a Physics.Raycast and sort by distance to the point that was hit.
        UI_3D,      // Perform a Physics.Raycast and sort by widget depth.
        World_2D,   // Perform a Physics2D.OverlapPoint
        UI_2D,      // Physics2D.OverlapPoint then sort by widget depth
    }
}