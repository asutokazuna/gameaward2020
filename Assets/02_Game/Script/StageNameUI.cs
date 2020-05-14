/**
 * @file    StageSelectUI.cs
 * @brief   ステージ番号管理用
 * @author  Risa Ito
 * @date    2020/05/05(火)  作成
 * @date    2020/05/09(土)  現在の惑星を外部から取得できるように変更、フェードアウトするように変更
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * @class StageSelectUI
 * @brief ステージ番号管理用
 */
public class StageNameUI : MonoBehaviour
{
    [SerializeField] Color[] _stageColor;   //!< ステージごと画像のカラー
    [SerializeField] int     _stageNum;     //!< 1惑星当たりのステージ数
    NumberImage _stageNoImage;              //!< ステージ番号画像管理
    LevelImage  _planetName;                //!< 惑星名管理
    int         _stageNo;                   //!< ステージ番号
    int         _planetID;                  //!< 現在の惑星
    Animator    _stageNameAnim;             //!< アニメーション取得

    // Start is called before the first frame update
    void Start()
    {
        // 初期化処理
        _stageNoImage = GetComponent<NumberImage>();
        _planetName = GetComponent<LevelImage>();
        _stageNo = (int)GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneMgr>().NowScene - 1;
        _planetID = 0;
        _stageNameAnim = GetComponent<Animator>();

        // 星の数分ステージ番号を補正
        while (_stageNo > _stageNum)
        {
            _stageNo -= _stageNum;
            _planetID++;
        }

        GameObject.Find("Stage").GetComponent<Image>().color = _stageColor[_planetID];

        _planetName.SetImageColor(new Color(1.0f, 1.0f, 1.0f, 1.0f));
        _planetName.SetImage(_planetID, 1);
        _stageNoImage.SetNumberColor(_stageColor[_planetID]);
        _stageNoImage.SetValue(_stageNo, false);
    }

    /**
    * @brief        ステージ名のフェードアウト開始
    * @return       なし
    * @details      ステージ名のフェードアウトをセットする関数です
    */
    public void FadeOutStageName()
    {
        _stageNameAnim.SetBool("StartFade", true);
    }

    // 現在の惑星
    public int PlanetID
    {
        get { return _planetID; }
    }
}
