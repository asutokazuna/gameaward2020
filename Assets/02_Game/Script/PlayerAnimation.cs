/**
 * @file    PlayerAnimation.cs
 * @brief   プレイヤーアニメーションの管理
 * @author  Risa Ito
 * @date    2020/03/30(月)  作成
 * @date    2020/04/10(金)  ジャンプアニメーション追加・アニメーションのセットの仕方を変更
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
    };

    // アニメーション管理用
    Animator                        _playerAnimator;    //!< アニメーター取得用
    [SerializeField] PlayerState    _playerState;       //!< プレイヤーの状態管理用
    PlayerState                     _playerNextState;   //!< プレイヤーの状態管理用
    bool                            _changeState;       //!< アニメーション変更フラグ

    // Start is called before the first frame update
    void Start()
    {
        // 初期化
        _playerAnimator = GetComponent<Animator>();
        _playerState = PlayerState.E_WAIT;
        _changeState = false;
    }

    // Update is called once per frame
    void Update()
    {
        // 状態の変更
        if (_changeState)
        {
            // ひとつ前の状態をログで確認
            Debug.Log("Old : " + _playerState);

            // 次の状態をセット
            _playerState = _playerNextState;

            // アニメータにセット
            _playerAnimator.SetInteger("PlayerState", (int)_playerState);
            _changeState = false;

            // 現在の状態をログで確認
            Debug.Log(_playerState);
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
        _playerNextState = state;
        _changeState = true;
    }
}
