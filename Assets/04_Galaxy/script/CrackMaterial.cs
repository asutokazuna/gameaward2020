using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackMaterial : MonoBehaviour
{
    [SerializeField] Color[]        _setColor = default;            //!< 変化させる色  
    [SerializeField] AnimationCurve _setColorCurve = default;       //!< 変化させ方
    private MeshRenderer            _material;                      //!< マテリアル取得用
    private Color                   _color;                         //!< 色保存用

    // Start is called before the first frame update
    void Start()
    {
        _material = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // 色を計算
        _color = Color.Lerp(_setColor[0], _setColor[1], _setColorCurve.Evaluate(Time.time));
        _material.material.SetColor("_BaseColor", _color);
    }
}
