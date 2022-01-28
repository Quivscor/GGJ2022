using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorCast
{
    public static Vector2 CastVector3ToVector2(Vector3 vector)
    {
        return new Vector2(vector.x, vector.z);
    }
}
