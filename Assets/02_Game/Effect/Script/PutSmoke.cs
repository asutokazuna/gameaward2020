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
    E_HANDS_ACTION a;

    /**
     * 初期化
     */
    void Start()
    {
        a = this.transform.parent.GetComponent<BlockTank>()._lifted;
    }

    /**
     * 更新
     */
    void Update()
    {
        if(a == E_HANDS_ACTION.NOW_PLAY && this.transform.parent.GetComponent<BlockTank>()._lifted == E_HANDS_ACTION.NONE)
        {// 箱が置かれると
            _putSmoke.Play();     // 再生
        }
        a = this.transform.parent.GetComponent<BlockTank>()._lifted;
    }
}
