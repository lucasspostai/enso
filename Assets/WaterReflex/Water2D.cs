// 2016 - Damien Mayance (@Valryon)
// Source: https://github.com/valryon/water2d-unity/

using UnityEngine;

namespace WaterReflex
{
    public class Water2D : MonoBehaviour
    {
        [SerializeField] private Vector2 Speed = new Vector2(0.01f, 0f);

        private Renderer waterRenderer;
        private Material waterMaterial;

        private void Awake()
        {
            waterRenderer = GetComponent<Renderer>();
            waterMaterial = waterRenderer.material;
        }

        private void LateUpdate()
        {
            var scroll = Time.deltaTime * Speed;

            waterMaterial.mainTextureOffset += scroll;
        }
    }
}