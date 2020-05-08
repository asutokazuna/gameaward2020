/**
 * @file    StageSelectUI.cs
 * @brief   ステージ番号管理用
 * @author  Risa Ito
 * @date    2020/05/05(火)  作成
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
    [SerializeField] int _stageNum;         //!< 1惑星当たりのステージ数
    NumberImage _stageNoImage;
    LevelImage _planetName;
    int _stageNo;
    int _planetID;

    // Start is called before the first frame update
    void Start()
    {
        // 初期化処理
        _stageNoImage = GetComponent<NumberImage>();
        _planetName = GetComponent<LevelImage>();
        _stageNo = (int)GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneMgr>().NowScene - 1;
        _planetID = 0;

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
        _stageNoImage.SetValue(_stageNo);
    }
}
