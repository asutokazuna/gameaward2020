/**
 * @file    FootSmoke.cs
 * @brief   プレイヤー移動時にパーティクルを発生させる
 * @author  Kaiki Mori
 * @date    2020/04/23(木)  作成
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSmoke : MonoBehaviour
{
    // 変数宣言
    [SerializeField] private ParticleSystem _footSmoke = default;     //!< 移動時のパーティクルシステム
    public PlayerAnimation                  _animation = default;     //!< プレイヤーのアニメーション

    /**
     * 初期化
     * なし
     */
    void Start()
    {
        
    }

    /**
     * 更新
     */
    void Update()
    {
        //if(_animation.GetPlayerState() == PlayerAnimation.PlayerState.E_WALK) //|| 
        //   //_animation.GetPlayerState() == PlayerAnimation.PlayerState.E_WALK_BOX ||
        //   //_animation.GetPlayerState() == PlayerAnimation.PlayerState.E_WALK_CHARA)
        //{// プレイヤーが移動したら
        //    if(!_footSmoke.isEmitting)
        //    {// 再生
        //        _footSmoke.Play();
        //    }
        //}
        //else
        //{// プレイヤーが止まったら
        //    if(_footSmoke.isEmitting)
        //    {// 停止
        //        _footSmoke.Stop();
        //    }
        //}
    }
}
