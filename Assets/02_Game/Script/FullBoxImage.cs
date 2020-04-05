/**
 * @file    FullBoxImage.cs
 * @brief   与えられた数値を画像で表示
 * @author  Risa Ito
 * @date    2020/04/05(日)  作成
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * @class FullBoxImage
 * @brief 与えられた数値を画像で表示
 */
public class FullBoxImage : MonoBehaviour
{
    // 表示する数値管理用
    private const int MAX_FULL_BOX = 999999;             //!< 最大6桁
    private int _fullBoxNum;					         //!< 表示する値

    // 画像管理用
    public Sprite[] _fullBoxImage;                                     //!< image画像対応
    public List<int> _fullBoxDigitList = new List<int>();              //!< 値の各桁保存用

    // タグ指定用
    public string _fullBoxImageTag;                      //!< タグ名保存用

    // オブジェクト指定用
    public string _fullBoxImageObject;                   //!< オブジェクト名保存用

    // Start is called before the first frame update
    void Start()
    {
        // 初期化
        _fullBoxNum = 0;
    }

    /**
    * @brief        値の加算
    * @param[in]    int
    * @return       なし
    * @details      表示したい値の増減をセットする関数です
    */
    public void AddFullBox(int num)
    {
        // 対象となるオブジェクトを探す
        var objs = GameObject.FindGameObjectsWithTag(_fullBoxImageTag);

        foreach (var obj in objs)
        {
            // いままで表示されてたスコアオブジェクト削除(ScoreImageの削除,更新画像の削除)
            if (0 <= obj.name.LastIndexOf("Clone"))
            {
                Destroy(obj);
            }
        }

        _fullBoxNum += num;      // 値の加算

        // 上限チェック
        if (_fullBoxNum > MAX_FULL_BOX)
        {
            _fullBoxNum = MAX_FULL_BOX;
        }

        ViewFullBox();       // 描画
    }

    /**
    * @brief    値の描画
    * @return   なし
    * @details  表示したい値を描画する関数です
    */
    void ViewFullBox()
    {
        // 計算用  
        int _fullBoxCount = _fullBoxNum;                //!< 値保存用
        var _fullBoxDigit = _fullBoxCount;              //!< 桁管理用
        _fullBoxDigitList = new List<int>();

        if (_fullBoxCount != 0)
        {
            // 各桁の確認
            while (_fullBoxDigit != 0)
            {
                _fullBoxCount = _fullBoxDigit % 10;
                _fullBoxDigit = _fullBoxDigit / 10;
                _fullBoxDigitList.Add(_fullBoxCount);
            }
        }
        else
        {
            _fullBoxDigitList.Add(_fullBoxCount);
        }

        // 1桁目
        GameObject.Find(_fullBoxImageObject).GetComponent<Image>().sprite = _fullBoxImage[_fullBoxDigitList[0]];

        // 2桁目以降
        for (int i = 1; i < _fullBoxDigitList.Count; i++)
        {
            RectTransform _fullImage = (RectTransform)Instantiate(GameObject.Find(_fullBoxImageObject)).transform;      // 初期ポジション
            _fullImage.SetParent(this.transform, false);                                                                // 親の選択
            _fullImage.localPosition = new Vector2(                                                                     // ポジション指定
                _fullImage.localPosition.x - _fullImage.sizeDelta.x * i / 2,                                            // X
                _fullImage.localPosition.y);                                                                            // Y
            _fullImage.GetComponent<Image>().sprite = _fullBoxImage[_fullBoxDigitList[i]];                              // 対応数値の選択
        }
    }
}
