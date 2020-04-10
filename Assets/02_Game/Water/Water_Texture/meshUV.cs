using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meshUV : MonoBehaviour
{
    private Mesh _myMesh;       //UV操作用
    private MeshRenderer _mr;       //カラー操作用
    private GameObject _parent;     //親情報取得用
    private BlockTank _myScript;        //コンポーネント取得用
    private Color _color;        //色保存用

    // Start is called before the first frame update
    void Start()
    {
        _myMesh = gameObject.GetComponent<MeshFilter>().mesh;            //UV操作の為にmeshの情報を取得
        _mr = GetComponent<MeshRenderer>();                                         //色情報の取得
        _parent = transform.parent.gameObject;                                          //親(ブロック)の情報を取得
        _myScript = _parent.GetComponent<BlockTank>();                          //ブロックの情報を取得
        _color = _mr.material.color;                                                            //色を保存
        _mr.material.color = new Color(_color.r, _color.g, _color.b, 0);          //最初に透明にする
    }

    // Update is called once per frame
    void Update()
    {
        Vector2[] nUV = _myMesh.uv;
        nUV = _myMesh.uv;

        for (int i = 0; i < _myMesh.uv.Length; i++)
        {
            nUV[i].x += 0.0001f;                                //ここの数値を変えるとテクスチャスクロールの速さが変わる
            //nUV[i].y += 0.0001f;
        }
        _myMesh.uv = nUV;

        if (_myScript._numWater >= _myScript._maxWater && _mr.material.color.a < _color.a)
        {
            _mr.material.color += new Color(0, 0, 0, 0.0001f);                  //段々と透明度を下げる、最後の数字を増やすと早く表示される
        }
        else if (_myScript._numWater <= 0 && _mr.material.color.a >= _color.a)
        {
            _mr.material.color = new Color(_color.r, _color.g, _color.b, 0);    //水が抜けたら透明に戻す(表示を切る)
        }
    }
}