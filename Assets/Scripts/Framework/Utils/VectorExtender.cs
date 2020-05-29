using UnityEngine;

namespace Framework.Utils
{
    public class VectorExtender : MonoBehaviour
    {
        public static float GetMagnitudeFromVectorDifference(Vector3 vectorA, Vector3 vectorB)
        {
            return (vectorA - vectorB).magnitude;
        }

        public static bool InputIsLeft(Vector2 vectorA, Vector2 vectorB)
        {
            return Vector2.Angle(vectorA, vectorB) < 180f;
        }
    }
}