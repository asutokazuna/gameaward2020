/**
 * @file    NumberImage.cs
 * @brief   与えられた数値を画像で表示
 * @author  Risa Ito
 * @date    2020/04/03(金)  作成
 * @date    2020/04/05(日)  0の時0桁になってしまうバグを修正
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * @class NumberImage
 * @brief 与えられた数値を画像で表示
 */
public class NumberImage : MonoBehaviour
{
    // 表示する数値管理用
    private const int MAX_NUM = 999999;     //!< 最大6桁
    private int _value;					    //!< 表示する値

    // 画像管理用
    public Sprite[] _numImage;                          //!< image画像対応
    public List<int> _digitList = new List<int>();      //!< 値の各桁保存用

    // タグ指定用
    public string _numberTag;               //!< タグ名保存用

    // オブジェクト指定用
    public string _numberObject;            //!< オブジェクト名保存用

    // Start is called before the first frame update
    void Start()
    {
        // 初期化
        _value = 0;
    }

    /**
    * @brief        値の加算
    * @param[in]    int
    * @return       なし
    * @details      表示したい値の増減をセットする関数です
    */
    public void AddValue(int num)
    {
        // 対象となるオブジェクトを探す
        var objs = GameObject.FindGameObjectsWithTag(_numberTag);

        foreach (var obj in objs)
        {
            // いままで表示されてたスコアオブジェクト削除(ScoreImageの削除,更新画像の削除)
            if (0 <= obj.name.LastIndexOf("Clone"))
            {
                Destroy(obj);
            }
        }

        _value += num;      // 値の加算

        // 上限チェック
        if (_value > MAX_NUM)
        {
            _value = MAX_NUM;
        }

        View();       // 描画
    }

    /**
    * @brief        値の描画
    * @param[in]    なし
    * @return       なし
    * @details      表示したい値を描画する関数です
    */
    void View()
    {
        // 計算用  
        int _num = _value;              //!< 値保存用
        var _digit = _num;              //!< 桁管理用
        _digitList = new List<int>();

        if (_num != 0)
        {
            // 各桁の確認
            while (_digit != 0)
            {
                _num = _digit % 10;
                _digit = _digit / 10;
                _digitList.Add(_num);
            }
        }
        else
        {
            _digitList.Add(_num);
        }

        // 1桁目
        GameObject.Find(_numberObject).GetComponent<Image>().sprite = _numImage[_digitList[0]];

        // 2桁目以降
        for (int i = 1; i < _digitList.Count; i++)
        {
            RectTransform _numberImage = (RectTransform)Instantiate(GameObject.Find(_numberObject)).transform;      // 初期ポジション
            _numberImage.SetParent(this.transform, false);                                                          // 親の選択
            _numberImage.localPosition = new Vector2(                                                               // ポジション指定
                _numberImage.localPosition.x - _numberImage.sizeDelta.x * i / 2,                                    // X
                _numberImage.localPosition.y);                                                                      // Y
            _numberImage.GetComponent<Image>().sprite = _numImage[_digitList[i]];                                   // 対応数値の選択
        }
    }
}
