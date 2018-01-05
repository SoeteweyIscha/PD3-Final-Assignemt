using UnityEngine;

public class StaticFunctions
{
    //Calculates the squqred distance between 2 points
    public static float SqrDistance(Vector3 start, Vector3 end)
    {
        float sqrDist = 0;

        Vector3 offset = start - end;
        sqrDist = offset.sqrMagnitude;

        return sqrDist;

    }
}
