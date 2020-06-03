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
    public PlayerAnim                       _animation = default;     //!< プレイヤーのアニメーション
    private Map                             _map;                     //!< マップ情報

    /**
     * 初期化
     */
    void Start()
    {
        _map = GameObject.FindGameObjectWithTag("Map").GetComponent<Map>();     // コンポーネントの取得
    }

    /**
     * 更新
     */
    void Update()
    {
        if (_animation.GetPlayerState() == PlayerAnim.PlayerState.E_WALK) 
        {// プレイヤーが移動したら
            if (!_footSmoke.isEmitting)
            {// 再生
                _footSmoke.Play();
            }
        }
        else
        {// プレイヤーが止まったら
            if (_footSmoke.isEmitting)
            {// 停止
                _footSmoke.Stop();
            }
        }

        if (_map._gameOver)
        {// おならジェット封印
            _footSmoke.Stop();
        }

    }
}
