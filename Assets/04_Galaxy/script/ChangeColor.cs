/**
 * @file    ChangeColor.cs
 * @brief   画像のカラー管理
 * @author  Risa Ito
 * @date    2020/05/07(木)  作成
 * @date    2020/05/09(土)  色全てを変更できるように仕様変更
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * @class ChangeColor
 * @brief 画像のカラー管理
 */
public class ChangeColor : MonoBehaviour
{
    [SerializeField] string _parentName;    //!< 参考にするオブジェクト
    Color _color;                           //!< アルファ値管理用
    Color _oldColor;                        //!< アルファ値管理用
    Image _parentImage;                     //!< 参考にする画像
    Image _cloneImage;                      //!< アルファ値セット用

    // Start is called before the first frame update
    void Start()
    {
        // 初期化
        _parentImage = GameObject.Find(_parentName).GetComponent<Image>();
        _cloneImage = GetComponent<Image>();
        _color = _parentImage.color;
        _oldColor = _color;
    }

    // Update is called once per frame
    void Update()
    {
        // アルファ値の取得とセット
        _color = _parentImage.color;

        if (_color != _oldColor)
        {
            _cloneImage.color = new Color(_color.r, _color.g, _color.b, _color.a);
            _oldColor = _color;
        }
    }
}
