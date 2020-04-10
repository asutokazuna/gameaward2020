using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meshUV : MonoBehaviour
{
    private Mesh myMesh;
    private Mesh Imesh;
    private MeshRenderer mr;
    public bool fadeSwitch = false;
    public float TargetTime;
    private Color color;
    private float fTime;

    // Start is called before the first frame update
    void Start()
    {
        myMesh = gameObject.GetComponent<MeshFilter>().mesh;
        Imesh = Instantiate(myMesh);
        gameObject.GetComponent<MeshFilter>().mesh = Imesh;
        mr = GetComponent<MeshRenderer>();
        color = mr.material.color;
        mr.material.color = new Color(color.r, color.g, color.b, 0);
        fTime = TargetTime;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2[] nUV = Imesh.uv;
        for (int i = 0; i < Imesh.uv.Length; i++)
        {
            nUV[i].x += 0.0001f;
            //nUV[i].y += 0.0001f;
        }
        Imesh.uv = nUV;

        fTime -= Time.deltaTime;

        if(fTime <= 0.0f)
        {
            mr.material.color += new Color(color.r, color.g, color.b, 0.0f);
        }
    }
}
