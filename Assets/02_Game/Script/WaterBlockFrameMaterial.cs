using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBlockFrameMaterial : MonoBehaviour
{
    [SerializeField] Color[]        _setColor = default;            //!< 変化させる色  
    [SerializeField] AnimationCurve _setColorCurve = default;       //!< 変化させ方
    [SerializeField] int            _materialNo = 0;                //!< マテリアルの格納されている場所
    private Material[]              _material;                      //!< マテリアル取得用
    private Color                   _color;                         //!< 色保存用

    // Start is called before the first frame update
    void Start()
    {
        _material = GetComponent<MeshRenderer>().materials;
    }

    // Update is called once per frame
    void Update()
    {
        // 色を計算
        _color = Color.Lerp(_setColor[0], _setColor[1], _setColorCurve.Evaluate(Time.time));
        _material[_materialNo].color = new Color(_color.r, _color.g, _color.b);
    }
}
