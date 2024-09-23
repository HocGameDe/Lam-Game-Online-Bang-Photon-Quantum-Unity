using UnityEngine;

namespace Quantum
{
    public class SpriteOrderFixer : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            spriteRenderer.sortingOrder = -(int)(transform.position.y * 100);
        }
    }
}