using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazingPoint : MonoBehaviour
{
    // Start is called before the first frame update
    MeshRenderer _meshRenderer;
    void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshRenderer.material.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
    }

}
