using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Quantum
{
    public class MeshOrderFixer : MonoBehaviour
    {
        private MeshRenderer meshRenderer;

        private void Awake()
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }

        private void Update()
        {
            meshRenderer.sortingOrder = -(int)(transform.position.y * 100);
        }
    }
}