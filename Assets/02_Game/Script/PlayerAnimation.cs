/**
 * @file    PlayerAnimation.cs
 * @brief   プレイヤーアニメーションの管理
 * @author  Risa Ito
 * @date    2020/03/30(月)  作成
 * @date    2020/04/10(金)  ジャンプアニメーション追加・アニメーションのセットの仕方を変更
 * @date    2020/04/18(土)  アニメーションの終了検知用の関数追加・アニメーションの遷移の仕様を変更
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @class PlayerAnimation
 * @brief プレイヤーのアニメーションの管理
 */
public class PlayerAnimation : MonoBehaviour
{
    // プレイヤーの状態
    public enum PlayerState
    {
        E_WAIT,
        E_WALK,
        E_LIFT_CHARA,
        E_WAIT_CHARA,
        E_WALK_CHARA,
        E_PUT_CHARA,
        E_LIFT_BOX,
        E_WAIT_BOX,
        E_WALK_BOX,
        E_PUT_BOX,
        E_JUMP,
        E_JUMP_CHARA,
        E_JUMP_BOX,

        E_NONE
    };

    // アニメーション管理用
    Animator                        _playerAnimator;    //!< アニメーター取得用
    [SerializeField] PlayerState    _playerState;       //!< プレイヤーの状態管理用
    PlayerState                     _playerNextState;   //!< プレイヤーの状態管理用
    bool _animFinish;               //!< アニメーション変更フラグ

    //!< アニメーション変更フラグ参照用
    public bool AnimFinish
    {
        get { return _animFinish; }
    }

    // Start is called before the first frame update
    void Start()
    {
        // 初期化
        _playerAnimator = GetComponent<Animator>();
        _playerState = PlayerState.E_WAIT;
        _playerNextState = PlayerState.E_NONE;
        _animFinish = true;
    }

    // Update is called once per frame
    void Update()
    {
        // アニメーションの変更を受け付けたか確認
        if (_playerNextState != PlayerState.E_NONE)
        {
            // ひとつ前のアニメーションが終了していればアニメーションを変更
            if (_animFinish)
            {
                Debug.Log("Old : " + _playerState);     // ひとつ前の状態をログで確認

                // アニメーションを更新
                _playerState = _playerNextState;                                // 次のアニメーションをセット
                _playerAnimator.SetInteger("PlayerState", (int)_playerState);   // アニメータにセット
                _animFinish = false;                                            // アニメーション開始

                Debug.Log(_playerState);    // 現在の状態をログで確認

                // アニメーションをリセット
                _playerNextState = PlayerState.E_NONE;
            }
        }
    }

    /**
    * @brief        プレイヤーの状態のセット
    * @param[in]    PlayerState プレイヤーの状態
    * @return       なし
    * @details      プレイヤーの状態をセットする関数です
    */
    public void SetPlayerState(PlayerState state)
    {
        _playerNextState = state;       // 次のアニメーションをセット
    }

    /**
    * @brief        アニメーションの終了をセット
    * @return       なし
    * @details      アニメーションの終了をセットするアニメーションイベントの関数です
    */
    public void SetAnimFinish()
    {
        _animFinish = true;        // アニメーション終了
    }
}
