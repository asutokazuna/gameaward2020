/**
 * @file    CountBoxUI.cs
 * @brief   箱の全体数や水の満たされた箱の個数を表示するUI管理
 * @author  Risa Ito
 * @date    2020/03/30(月)  作成
 * @date    2020/04/05(日)  画像表示に対応
 * @date    2020/05/05(火)  NumberImage.csの仕様変更に合わせて一部修正
 * @date    2020/05/09(土)  カラーの変更対応
 * @date    2020/05/30(土)　アイコンのアニメーションを追加
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * @class CountBoxUI
 * @brief 箱の全体数や水の満たされた箱の個数を表示するUI管理
 */
public class CountBoxUI : MonoBehaviour
{
    //! 箱の全体数
    int _countTatalBox;

    //! タグ指定用
    public string _countBoxTag;

    // 画像表示用
    public NumberImage          _tatalBoxImage;                 //!< 箱全体数画像表示用
    public FullBoxImage         _fullBoxImage;                  //!< 箱全体数画像表示用
    [SerializeField] Color[]    _countNumColor = default;       //!< 画像のカラー保持
    StageNameUI                 _stageNameUI;                   //!< 現在のステージ取得用
    WaterAnim                   _waterAnim;                     //!< アイコンアニメーション
    int                         _waterAnimCount;                //!< アイコンアニメーション管理用
    float                       _waterAnimQuater;               //!< 1/4の個数を管理

    // Start is called before the first frame update
    void Start()
    {
        // 初期化処理
        _stageNameUI = GameObject.Find("StageName").GetComponent<StageNameUI>();

        // 指定されたタグ付きオブジェクトの個数を数える
        GameObject[] _boxObjects = GameObject.FindGameObjectsWithTag(_countBoxTag);
        _countTatalBox = _boxObjects.Length;

        // 画像表示
        GameObject.Find("Slash").GetComponent<Image>().color = _countNumColor[_stageNameUI.PlanetID];
        _tatalBoxImage.SetNumberColor(_countNumColor[_stageNameUI.PlanetID]);
        _tatalBoxImage.SetValue(_countTatalBox, false);
        _fullBoxImage.SetFullBoxColor(_countNumColor[_stageNameUI.PlanetID]);
        _fullBoxImage.AddFullBox(0);

        _waterAnim = GameObject.Find("WaterBlockIconMask").GetComponent<WaterAnim>();
        _waterAnimQuater = (float)_countTatalBox / 4;
        _waterAnimCount = 0;
    }

    /**
    * @brief        満たされた箱の数の追加
    * @param[in]    int 満たされた箱の数の増減量
    * @return       なし
    * @details      満たされた箱の数の増減をセットする関数です
    */
    public void AddFullBox(int fullBoxNum)
    {
        int _waterCount;

        _fullBoxImage.SetFullBoxColor(_countNumColor[_stageNameUI.PlanetID]);
        _fullBoxImage.AddFullBox(fullBoxNum);

        if (fullBoxNum > 0)
        {
            _waterAnimCount++;
            _waterCount = _waterAnimCount;

            // 水のアニメーションセット
            if (_waterCount >= _countTatalBox)              // すべての箱に水が入った時
            {
                _waterAnim.SetWaterAnim(5);
            }
            else if (_waterCount >= _waterAnimQuater * 3)   // 3/4以上の箱に水が入った時
            {
                _waterAnim.SetWaterAnim(4);
            }
            else if (_waterCount >= _waterAnimQuater * 2)   // 2/4以上の箱に水が入った時
            {
                _waterAnim.SetWaterAnim(3);
            }
            else if (_waterCount >= _waterAnimQuater)       // 1/4以上の箱に水が入った時
            {
                _waterAnim.SetWaterAnim(2);
            }
            else if (_waterCount == 1)                      // 1つの箱に水が入った時
            {
                _waterAnim.SetWaterAnim(1);
            }
            else if (_waterCount == 0)                      // 水の入った箱がひとつもないとき
            {
                _waterAnim.SetWaterAnim(0);
            }
        }
        else
        {
            _waterCount = _waterAnimCount;
            _waterAnimCount--;

            // 水のアニメーションセット
            if (_waterCount == 0)                               // 水の入った箱がひとつもないとき
            {
                _waterAnim.SetWaterAnim(0);
            }
            else if (_waterCount == 1)                          // 1つの箱に水が入った時
            {
                _waterAnim.SetWaterAnim(1);
            }
            else if (_waterCount >= _waterAnimQuater)           // 1/4以上の箱に水が入った時
            {
                _waterAnim.SetWaterAnim(2);
            }
            else if (_waterCount >= _waterAnimQuater * 2)       // 2/4以上の箱に水が入った時
            {
                _waterAnim.SetWaterAnim(3);
            }
            else if (_waterCount >= _waterAnimQuater * 3)       // 3/4以上の箱に水が入った時
            {
                _waterAnim.SetWaterAnim(4);
            }
        }
    }
}
