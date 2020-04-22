using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meshUV : MonoBehaviour
{
    private Mesh _myMesh;       //UV操作用
    private MeshRenderer _mr;       //カラー操作用
    private GameObject _parent;     //親情報取得用
    private WaterFlow _myScript;        //コンポーネント取得用
    [SerializeField] private Color _color;        //色保存用

    // Start is called before the first frame update
    void Start()
    {
        _myMesh = gameObject.GetComponent<MeshFilter>().mesh;
        _mr = GetComponent<MeshRenderer>();
        _parent = transform.parent.gameObject;
        _myScript = _parent.GetComponent<WaterFlow>();
        _color = _parent.GetComponent<ParticleSystem>().colorOverLifetime.color.gradient.colorKeys[0].color;    //パーティクルシステムの色を取得
        _mr.material.color = new Color(_color.r, _color.g, _color.b, 0);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2[] nUV = _myMesh.uv;
        nUV = _myMesh.uv;
        for (int i = 0; i < _myMesh.uv.Length; i++)
        {
            nUV[i].x += 0.0001f;
            //nUV[i].y += 0.0001f;
        }
        _myMesh.uv = nUV;

        if (_myScript._isFullWater)
        {
            _mr.material.color = _color;
        }
        else if (_myScript._currentWater >= 0)
        {
            _mr.material.color = new Color(_color.r, _color.g, _color.b, 0);
        }
    }
}
