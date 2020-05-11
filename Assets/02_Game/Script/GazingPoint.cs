using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazingPoint : MonoBehaviour
{
    // Start is called before the first frame update
    MeshRenderer _meshRenderer;
    Transform myTransform;
    void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshRenderer.material.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        myTransform = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
     
    Transform GetTransform()
    {
        return myTransform;
    }
}
