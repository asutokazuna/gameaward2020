/**
 * @file    PlayerAnimation.cs
 * @brief   プレイヤーアニメーションの管理
 * @author  Risa Ito
 * @date    2020/03/30(月)  作成
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
        E_PUT_BOX
    };

    // アニメーション管理用
    Animator _playerAnimator;       //!< アニメーター取得用
    PlayerState _playerState;       //!< プレイヤーの状態管理用
    PlayerState _playerNextState;   //!< プレイヤーの状態管理用
    bool _changeState;              //!< アニメーション変更フラグ
    int _stateNo;

    // Start is called before the first frame update
    void Start()
    {
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
            Debug.Log("Old : " + _playerState);
            _playerState = _playerNextState;

            switch (_playerState)
            {
                case PlayerState.E_WAIT:
                    _stateNo = 0;
                    break;
                case PlayerState.E_WALK:
                    _stateNo = 1;
                    break;
                case PlayerState.E_LIFT_CHARA:
                    _stateNo = 2;
                    break;
                case PlayerState.E_WAIT_CHARA:
                    _stateNo = 3;
                    break;
                case PlayerState.E_WALK_CHARA:
                    _stateNo = 4;
                    break;
                case PlayerState.E_PUT_CHARA:
                    _stateNo = 5;
                    break;
                case PlayerState.E_LIFT_BOX:
                    _stateNo = 6;
                    break;
                case PlayerState.E_WAIT_BOX:
                    _stateNo = 7;
                    break;
                case PlayerState.E_WALK_BOX:
                    _stateNo = 8;
                    break;
                case PlayerState.E_PUT_BOX:
                    _stateNo = 9;
                    break;
            }

            // アニメータにセット
            _playerAnimator.SetInteger("PlayerState", _stateNo);
            _changeState = false;
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
