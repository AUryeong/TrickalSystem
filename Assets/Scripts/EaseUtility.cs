using UnityEngine;

public static class EaseUtility
{
    public static float GetEaseOutElastic(float x)
    {
        const float c4 = (2 * Mathf.PI) / 3;

        if (x <= 0) return 0;
        if (x >= 1) return 1;

        return Mathf.Pow(2, -10 * x) * Mathf.Sin((x * 10f - 0.75f) * c4) + 1f;
    }
}