/**
 * @file    PutSmoke.cs
 * @brief   置いたときにパーティクルを発生させる
 * @author  Kaiki Mori
 * @date    2020/04/26(日)  作成
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PutSmoke : MonoBehaviour
{
    // 変数宣言
    [SerializeField] private ParticleSystem _putSmoke = default;     //!< 置いた時のパーティクルシステム
    private Map                              _map;                    //!< マップ情報
    E_HANDS_ACTION _oldState;                                        //!< 箱の前回の状態取得用

    /**
     * 初期化
     */
    void Start()
    {
        _map = GameObject.FindGameObjectWithTag("Map").GetComponent<Map>();                      // コンポーネントの取得
        _oldState = this.transform.parent.GetComponent<BlockTank>()._lifted;
    }

    /**
     * 更新
     */
    void Update()
    {
        if(_map._gameOver)
        {// 箱が落ちた・割れたなら再生しない
            return;
        }

        if(_oldState == E_HANDS_ACTION.NOW_PLAY && this.transform.parent.GetComponent<BlockTank>()._lifted == E_HANDS_ACTION.NONE)
        {// 箱が置かれると
            _putSmoke.Play();     // 再生
        }
        _oldState = this.transform.parent.GetComponent<BlockTank>()._lifted;
    }
}
