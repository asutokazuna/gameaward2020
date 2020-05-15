/**
 * @file    CountBoxUI.cs
 * @brief   箱の全体数や水の満たされた箱の個数を表示するUI管理
 * @author  Risa Ito
 * @date    2020/03/30(月)  作成
 * @date    2020/04/05(日)  画像表示に対応
 * @date    2020/05/05(火)  NumberImage.csの仕様変更に合わせて一部修正
 * @date    2020/05/09(土)  カラーの変更対応
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
    public NumberImage          _tatalBoxImage;     //!< 箱全体数画像表示用
    public FullBoxImage         _fullBoxImage;      //!< 箱全体数画像表示用
    [SerializeField] Color[]    _countNumColor = default;     //!< 画像のカラー保持
    StageNameUI                 _stageNameUI;       //!< 現在のステージ取得用

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
        _tatalBoxImage.SetValue(_countTatalBox, true);
        _fullBoxImage.SetFullBoxColor(_countNumColor[_stageNameUI.PlanetID]);
        _fullBoxImage.AddFullBox(0);
    }

    /**
    * @brief        満たされた箱の数の追加
    * @param[in]    int 満たされた箱の数の増減量
    * @return       なし
    * @details      満たされた箱の数の増減をセットする関数です
    */
    public void AddFullBox(int fullBoxNum)
    {
        _fullBoxImage.SetFullBoxColor(_countNumColor[_stageNameUI.PlanetID]);
        _fullBoxImage.AddFullBox(fullBoxNum);
    }
}
