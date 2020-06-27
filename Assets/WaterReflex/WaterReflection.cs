// 2016 - Damien Mayance (@Valryon)
// Source: https://github.com/valryon/water2d-unity/

using UnityEngine;

namespace WaterReflex
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class WaterReflection : MonoBehaviour
    {
        #region Members

        private SpriteRenderer spriteSource;
        private SpriteRenderer spriteRenderer;

        [Header("Reflection Properties")] 
        [SerializeField] private Vector3 LocalPosition = new Vector3(0, -0.25f, 0);
        [SerializeField] private Vector3 LocalRotation = new Vector3(0, 0, -180);
        [SerializeField] private Vector3 LocalScale = Vector3.one;

        [Tooltip("Optional: force the reflected sprite. If null it will be a copy of the source.")]
        [SerializeField] private Sprite Sprite;
        [SerializeField] private string SpriteLayer = "Default";
        [SerializeField] private int SpriteLayerOrder = -5;
        private bool isSpriteNull;
        private bool isSpriteSourceNotNull;

        #endregion

        #region Timeline

        private void Awake()
        {
            isSpriteSourceNotNull = spriteSource != null;
            isSpriteNull = Sprite == null;
            
            var reflectGameObject = new GameObject("Water Reflect");
            reflectGameObject.transform.parent = transform;
            reflectGameObject.transform.localPosition = LocalPosition;
            reflectGameObject.transform.localRotation = Quaternion.Euler(LocalRotation);
            reflectGameObject.transform.localScale = LocalScale;

            spriteRenderer = reflectGameObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sortingLayerName = SpriteLayer;
            spriteRenderer.sortingOrder = SpriteLayerOrder;

            spriteSource = GetComponent<SpriteRenderer>();
        }

        private void OnDestroy()
        {
            if (spriteRenderer != null)
            {
                Destroy(spriteRenderer.gameObject);
            }
        }

        private void LateUpdate()
        {
            if (!isSpriteSourceNotNull) return;
            spriteRenderer.sprite = isSpriteNull ? spriteSource.sprite : Sprite;

            spriteRenderer.flipX = spriteSource.flipX;
            spriteRenderer.flipY = spriteSource.flipY;
            spriteRenderer.color = spriteSource.color;
        }

        #endregion
    }
}