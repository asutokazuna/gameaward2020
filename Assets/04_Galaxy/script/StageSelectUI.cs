/**
 * @file    StageSelectUI.cs
 * @brief   ステージ番号管理用
 * @author  Risa Ito
 * @date    2020/03/30(月)  作成
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @class StageSelectUI
 * @brief ステージ番号管理用
 */
public class StageSelectUI : MonoBehaviour
{
    [SerializeField] int _stageNum;     //!< 1惑星当たりのステージ数
    NumberImage _stageSelectImage;      //!< ステージNoを画像
    E_SCENE     _oldStageId;            //!< 選ばれてるステージID管理用
    E_SCENE     _stageId;               //!< 選ばれてるステージID管理用
    Animator    _stageSelectAnim;       //!< アニメーター取得用

    // Start is called before the first frame update
    void Start()
    {
        // 初期化処理
        _stageSelectAnim = GetComponent<Animator>();
        _stageSelectImage = GameObject.Find("CanvasUI").GetComponent<NumberImage>();
        _stageId = GameObject.Find("CameraObj").GetComponent<RaySystem>().GetID();
    }

    // Update is called once per frame
    void Update()
    {
        // 選択されているIDの取得
        _stageId = GameObject.Find("CameraObj").GetComponent<RaySystem>().GetID();

        // 選択対象が変更されたらUI変更
        if (_stageId != _oldStageId)
        {
            SetStageNo();
        }
    }

    /**
    * @brief        ステージ番号のセット
    * @return       なし
    * @details      ステージ番号の変更をセットする関数です
    */
    void SetStageNo()
    {
        // ステージIDをステージ番号に計算
        int stageNo = (int)_stageId - 1;

        // 星の数分ステージ番号を補正
        while(stageNo > _stageNum)
        {
            stageNo -= _stageNum;
        }

        if (stageNo > 0)
        {
            // 島が選択された場合
            if (_stageSelectAnim.GetBool("Change"))
            {
                _oldStageId = _stageId;
                _stageSelectImage.SetValue(stageNo);
                _stageSelectAnim.SetBool("Select", true);
            }
        }
        else
        {
            // 島の選択が外れた場合
            _oldStageId = _stageId;
            _stageSelectAnim.SetBool("Select", false);
        }
    }
}
