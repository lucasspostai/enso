using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public static Vector2 Movement;

    private void Update()
    {
        Movement.x = Input.GetAxis("Horizontal");
        Movement.y = Input.GetAxis("Vertical");
    }
}
