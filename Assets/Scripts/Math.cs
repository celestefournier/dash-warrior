using UnityEngine;

class Math
{
    public static Vector2 AngleToVector(float angle)
    {
        return new Vector2(
            Mathf.Cos(Mathf.Deg2Rad * angle),
            Mathf.Sin(Mathf.Deg2Rad * angle)
        );
    }

    public static float VectorToAngle(Vector2 vector)
    {
        return Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
    }

    public static float VectorToAngle(float x, float y)
    {
        return Mathf.Atan2(y, x) * Mathf.Rad2Deg;
    }
}
