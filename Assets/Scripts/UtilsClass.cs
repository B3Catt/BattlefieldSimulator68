using UnityEngine;

public static class UtilsClass
{
    static public Vector3 ApplyRotation2VectorXZ(this Vector3 source, Vector3 axis, float angle)
    {
        Quaternion q = Quaternion.AngleAxis(angle, axis);
        return q * source;
    }
}
