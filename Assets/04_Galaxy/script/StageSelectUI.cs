/**
 * @file    StageSelectUI.cs
 * @brief   ステージ番号管理用
 * @author  Risa Ito
 * @date    2020/05/05(火)  作成
 * @date    2020/05/07(木)  レベル表示対応
 * @date    2020/05/08(金)  カラーの変更対応
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * @class StageSelectUI
 * @brief ステージ番号管理用
 */
public class StageSelectUI : MonoBehaviour
{
    [SerializeField] Color[] _stageColor = default;     //!< ステージ表記のカラー
    [SerializeField] int     _stageNum = 0;             //!< 1惑星当たりのステージ数
    NumberImage _stageSelectImage;          //!< ステージNoを画像
    E_SCENE     _oldStageId;                //!< 選ばれてるステージID管理用
    E_SCENE     _stageId;                   //!< 選ばれてるステージID管理用
    Animator    _stageSelectAnim;           //!< アニメーター取得用
    RaySystem   _selectStage;               //!< 選択情報取得用
    int         _planetID;                  //!< 惑星管理用
    int         _level;                     //!< レベル管理用
    LevelImage  _levelImage;                //!< レベル画像セット用
    LevelImage  _backDecoImage;             //!< ステージ表示の飾り画像セット用
    PlayMovie   _playMovie;                 //!< 動画再生用

    // Start is called before the first frame update
    void Start()
    {
        // 初期化処理
        _stageSelectAnim = GetComponent<Animator>();
        _stageSelectImage = GetComponent<NumberImage>();
        _levelImage = GetComponent<LevelImage>();
        _backDecoImage = GameObject.Find("StageBackUI").GetComponent<LevelImage>();
        _selectStage = GameObject.Find("CameraObj").GetComponent<RaySystem>();
        _stageId = 0;
        _oldStageId = 0;
        _planetID = 0;
        _playMovie = GameObject.Find("PlayVideo").GetComponent<PlayMovie>();
    }

    // Update is called once per frame
    void Update()
    {
        // 選択されているIDの取得
        _stageId = _selectStage.GetID();

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
        _planetID = 0;

        // 星の数分ステージ番号を補正
        while (stageNo > _stageNum)
        {
            stageNo -= _stageNum;
            _planetID++;
        }

        if (stageNo > 0)
        {
            // 島が選択された場合
            if (_stageSelectAnim.GetBool("Change"))
            {
                _oldStageId = _stageId;
                _level = _selectStage.GetLevel();
                GameObject.Find("Stage").GetComponent<Image>().color = _stageColor[_planetID];
                _stageSelectImage.SetNumberColor(_stageColor[_planetID]);
                _stageSelectImage.SetValue(stageNo, false);
                _levelImage.SetImageColor(new Color(1.0f,1.0f,1.0f,0.0f));
                _levelImage.SetImage(_planetID,_level);
                _backDecoImage.SetImageColor(_stageColor[_planetID]);
                _backDecoImage.SetImage(_planetID, 1);
                _stageSelectAnim.SetBool("Select", true);
                _playMovie.SetVideo((int)_stageId - 2);       //動画再生
            }
        }
        else
        {
            // 島の選択が外れた場合
            SetSelectFinish();
        }
    }

    /**
    * @brief        ステージセレクトのUI表示管理
    * @return       なし
    * @details      ステージが選択されて無い場合に表示をOffにする関数です
    */
    public void SetSelectFinish()
    {
        _oldStageId = _stageId;
        _stageSelectAnim.SetBool("Select", false);
    }
}
