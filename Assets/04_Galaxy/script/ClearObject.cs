/**
 * @file    ClearObject.cs
 * @brief   クリア後のオブジェクト表示
 * @author  Risa Ito
 * @date    2020/05/25(月)   作成
 * @date    2020/06/06(土)   自動生成用に修正
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @class ClearObject
 * @brief クリア後のオブジェクト表示
 */
public class ClearObject : MonoBehaviour
{
    private SceneMgr                _sceneMgr;                  //!< フラグ取得用
    static  private bool[]          _isClearFlg = new bool[60]; //!< クリア演出管理フラグ
    static  private bool            _isInit = true;             //!< クリア演出管理初期化フラグ
            public  bool            _isChange;                  //!< クリア演出管理フラグ

    // Start is called before the first frame update
    void Start()
    {
        // 初期化
        if(_isInit)
        {
            Init(); // ゲーム全体で一度だけ行う初期化処理
        }

        _isChange = false;
        _sceneMgr = GameObject.Find("SceneManager").GetComponent<SceneMgr>();

        for (int i = (int)E_SCENE._1_1; i < (int)E_SCENE.MAX; i++) 
        {
            // 配列外参照しないように
            if (_isClearFlg.Length - 1 < i - (int)E_SCENE._1_1)
            {
                break;
            }
            // はじめてのクリアかどうかチェック
            else if (_sceneMgr.GetStageClear((E_SCENE)i) && !_isClearFlg[i - (int)E_SCENE._1_1])
            {
                _isChange = true;   // 演出必要
                _isClearFlg[i - (int)E_SCENE._1_1] = true;
            }
        }
    }

    /**
    * @brief        初期化処理
    * @return       なし
    * @details      ゲーム内で一回だけ初期化する関数です
    */
    void Init()
    {
        _isInit = false;

        // クリアフラグをすべてfalseに
        for (int i = 0; i < _isClearFlg.Length; i++)
        {
            if(_isClearFlg.Length <= i)
            {
                break;
            }
            _isClearFlg[i] = false;
        }
    }
}
