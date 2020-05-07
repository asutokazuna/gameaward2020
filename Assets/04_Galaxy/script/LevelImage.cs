/**
 * @file    LevelImage.cs
 * @brief   レベルの画像表記
 * @author  Risa Ito
 * @date    2020/05/07(木)  作成
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * @class LevelImage
 * @brief レベルの画像表記
 */
public class LevelImage : MonoBehaviour
{
    // 表示する数値管理用
    private const int       MAX_LEVEL = 10;     //!< 最大10
    private int             _value;             //!< 表示する数
    private int             _planetID;          //!< 表示する数
    [SerializeField] int    _turnBackNum;       //!< 折り返す位置(個数)

    // 画像管理用
    public Sprite[]         _levelImage;        //!< image画像対応

    // タグ指定用
    [SerializeField] string _levelTag;          //!< タグ名保存用

    // オブジェクト指定用
    [SerializeField] string _levelObject;       //!< オブジェクト名保存用

    // Start is called before the first frame update
    void Start()
    {
        // 初期化
        _value = 0;
    }

    /**
    * @brief        画像のセット
    * @param[in]    int 表示したい画像の種類(惑星番号) int 表示したい個数
    * @return       なし
    * @details      表示したい画像と個数をセットする関数です
    */
    public void SetLevel(int planetID, int num)
    {
        // 同じ個数同じ画像なら処理しない
        if (num != _value || planetID != _planetID)
        {
            // 対象となるオブジェクトを探す
            var objs = GameObject.FindGameObjectsWithTag(_levelTag);

            foreach (var obj in objs)
            {
                // いままで表示されてたオブジェクト削除
                if (0 <= obj.name.LastIndexOf("Clone"))
                {
                    Destroy(obj);
                }
            }

            _value = num;           // 個数セット
            _planetID = planetID;   // 画像指定

            // 上限下限チェック
            if (_value > MAX_LEVEL)
            {
                _value = MAX_LEVEL;
            }
            else if(_value < 1)
            {
                _value = 1;
            }

            View();       // 描画
        }
    }

    /**
    * @brief        画像の描画
    * @return       なし
    * @details      表示したい個数分描画する関数です
    */
    void View()
    {
        // 画像の指定
        Sprite _image = _levelImage[_planetID];

        // 一つ目
        GameObject.Find(_levelObject).GetComponent<Image>().sprite = _image;

        // 指定個数分描画
        for (int i = 1; i < _value; i++)
        {
            RectTransform _setImage = (RectTransform)Instantiate(GameObject.Find(_levelObject)).transform;      // 初期ポジション
            _setImage.SetParent(this.transform, false);                                                         // 親の選択
            _setImage.localPosition = new Vector2(                                                              // ポジション指定
                _setImage.localPosition.x + _setImage.sizeDelta.x * (i % _turnBackNum) / 2,                     // X
                _setImage.localPosition.y - _setImage.sizeDelta.y * (i / _turnBackNum) / 2);                    // Y
            _setImage.GetComponent<Image>().sprite = _image;                                                    // 対応数値の選択
        }
    }
}
