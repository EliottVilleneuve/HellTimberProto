using UnityEngine;

public static class V
{
    public static Vector3 SetX(Vector3 vector, float x) { return new Vector3(x, vector.y, vector.z); }
    public static Vector3 SetY(Vector3 vector, float y) { return new Vector3(vector.x, y, vector.z); }
    public static Vector3 SetZ(Vector3 vector, float z) { return new Vector3(vector.x, vector.y, z); }
}
