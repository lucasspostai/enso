using UnityEngine;

[CreateAssetMenu(fileName = "CameraProperties", menuName = "Enso/Camera")]
public class CameraProperties : ScriptableObject
{
    public float VerticalOffset;
    public float LookAheadDistance;
    public float LookAheadSmoothTime;
    public Vector2 FocusAreaSize;
}
