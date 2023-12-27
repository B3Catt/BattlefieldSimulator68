using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisualSingle : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    public void show()
    {
        meshRenderer.enabled = true;
    }

    public void hide()
    {
        meshRenderer.enabled = false;
    }
}
