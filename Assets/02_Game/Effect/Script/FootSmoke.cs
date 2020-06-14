/**
 * @file    FootSmoke.cs
 * @brief   プレイヤー移動時にパーティクルを発生させる
 * @author  Kaiki Mori
 * @date    2020/04/23(木)  作成
 * @data    2020/06/03(水)　おならジェット封印
 * @data    2020/06/05(金)　ジャンプの着地時に砂煙発生追加等
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
    E_OBJECT_MODE                           _oldState;                //!< プレイヤーの前回の情報取得用

    /**
     * 初期化
     */
    void Start()
    {
        _map = GameObject.FindGameObjectWithTag("Map").GetComponent<Map>();     // コンポーネントの取得
        _oldState = this.transform.parent.parent.parent.GetComponent<Player>()._mode;   // プレイヤーの前情報取得
    }

    /**
     * 更新
     */
    void Update()
    {
        if (_animation.GetPlayerState() == PlayerAnim.PlayerState.E_WALK ||
            this.transform.parent.parent.parent.GetComponent<Player>()._mode == E_OBJECT_MODE.DONT_MOVE) 
        {// プレイヤーが足踏み
            if (!_footSmoke.isEmitting)
            {// 再生
                _footSmoke.Play();
            }
        }
        else if((_oldState == E_OBJECT_MODE.GET_OFF || _oldState == E_OBJECT_MODE.GET_UP)&&
                this.transform.parent.parent.parent.GetComponent<Player>()._mode == E_OBJECT_MODE.WAIT)
        {// プレイヤーがジャンプ→着地したら
            if (!_footSmoke.isEmitting)
            {// 再生
                _footSmoke.Play();
            }
        }
        else if(_animation.GetPlayerState() == PlayerAnim.PlayerState.E_WAIT)
        {// プレイヤーが待機アニメーションになったら
            if (_footSmoke.isEmitting)
            {// 停止
                _footSmoke.Stop();
            }
        }

        if (_map._gameOver || this.transform.parent.parent.parent.GetComponent<Player>()._lifted != E_HANDS_ACTION.NONE)
        {// おならジェット封印 と 持たれてるプレイヤーが壁にぶつかった時
            _footSmoke.Stop();
        }
        // プレイヤーの前情報取得
        _oldState = this.transform.parent.parent.parent.GetComponent<Player>()._mode;
    }
}
