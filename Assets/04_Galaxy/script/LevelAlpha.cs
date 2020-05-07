/**
 * @file    LevelAlpha.cs
 * @brief   レベル表記クローン管理
 * @author  Risa Ito
 * @date    2020/05/07(木)  作成
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * @class LevelAlpha
 * @brief レベル表記クローン管理用
 */
public class LevelAlpha : MonoBehaviour
{
    [SerializeField] string _parentName;    //!< 参考にするオブジェクト
    float _alphaColor;                      //!< アルファ値管理用
    Image _levelImage;                      //!< 参考にする画像
    Image _levelClone;                      //!< アルファ値セット用

    // Start is called before the first frame update
    void Start()
    {
        // 初期化
        _levelImage = GameObject.Find(_parentName).GetComponent<Image>();
        _levelClone = GetComponent<Image>();
        
    }

    // Update is called once per frame
    void Update()
    {
        // アルファ値の取得とセット
        _alphaColor = _levelImage.color.a;
        _levelClone.color = new Color(1.0f,1.0f,1.0f,_alphaColor);
    }
}
