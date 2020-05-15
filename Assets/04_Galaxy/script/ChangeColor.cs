/**
 * @file    ChangeColor.cs
 * @brief   画像のカラー管理
 * @author  Risa Ito
 * @date    2020/05/07(木)  作成
 * @date    2020/05/09(土)  色全てを変更できるように仕様変更
 * @date    2020/05/10(日)  変化の仕方にパターンを追加
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
    // 色の変化パターン
    enum ChangeColorType
    {
        E_REFERENCE = 0,
        E_BETWEEN_TWO_COLOR
    }

    [SerializeField] ChangeColorType    _changeColorType = default;   //!< 変化の仕方  
    [SerializeField] Color[]            _setColor = default;          //!< 変化させる色  
    [SerializeField] AnimationCurve     _setColorCurve = default;     //!< 変化させ方  
    [SerializeField] string             _parentName = default;        //!< 参考にするオブジェクト
    Color _color;                                           //!< アルファ値管理用
    Color _oldColor;                                        //!< アルファ値管理用
    Image _parentImage;                                     //!< 参考にする画像
    Image _cloneImage;                                      //!< アルファ値セット用

    // Start is called before the first frame update
    void Start()
    {
        // 初期化
        _cloneImage = GetComponent<Image>();
        Init();
        _oldColor = _color;
    }

    // Update is called once per frame
    void Update()
    {
        switch(_changeColorType)
        {
            case ChangeColorType.E_REFERENCE:
                ChangeReferenceColor();
                break;
            case ChangeColorType.E_BETWEEN_TWO_COLOR:
                BetweenTwoColor();
                break;
        }

        // 色が変化していなければ処理しない
        if (_color != _oldColor)
        {
            _cloneImage.color = new Color(_color.r, _color.g, _color.b, _color.a);
            _oldColor = _color;
        }
    }

    /**
    * @brief        初期化処理
    * @return       なし
    * @details      パターンごとの初期化処理をする関数です
    */
    void Init()
    {
        // パターン別の初期化処理
        switch(_changeColorType)
        {
            case ChangeColorType.E_REFERENCE:
                _parentImage = GameObject.Find(_parentName).GetComponent<Image>();
                _color = _parentImage.color;
                break;
            case ChangeColorType.E_BETWEEN_TWO_COLOR:
                _color = Color.Lerp(_setColor[0], _setColor[1], _setColorCurve.Evaluate(Time.time));
                break;
        }
    }

    /**
    * @brief        対象となる画像に合わせた色の変化
    * @return       なし
    * @details      対象となる画像の色変化に合わせる関数です
    */
    void ChangeReferenceColor()
    {
        // カラーの取得
        _color = _parentImage.color;
    }

    /**
    * @brief        指定した2色間での色の変化
    * @return       なし
    * @details      指定した2色間で色を変化させる関数です
    */
    void BetweenTwoColor()
    {
        // 色を計算
        _color = Color.Lerp(_setColor[0], _setColor[1], _setColorCurve.Evaluate(Time.time));
    }
}
