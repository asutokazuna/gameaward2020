/**
 * @file    PlayerAnimation.cs
 * @brief   プレイヤーアニメーションの管理
 * @author  Risa Ito
 * @date    2020/03/30(月)  作成
 * @date    2020/04/10(金)  ジャンプアニメーション追加・アニメーションのセットの仕方を変更
 * @date    2020/04/18(土)  アニメーションの終了検知用の関数追加・アニメーションの遷移の仕様を変更
 * @data    2020/04/23(木)  プレイヤーの状態を取得する関数の追加　(Kaiki Mori)
 * @data    2020/04/24(金)  プレイヤーの状態を追加
 * @data    2020/04/26(金)  トランポリンのアニメーション制御機能を追加
 */

//#define TEST_ANIM   // アニメーター切り替え

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @class PlayerAnimation
 * @brief プレイヤーのアニメーションの管理
 */
public class PlayerAnimation : MonoBehaviour
{
#if !TEST_ANIM

    // プレイヤーの状態
    public enum PlayerState
    {
        E_WAIT,
        E_WALK,
        E_JUMP,
        E_WAIT_TP,
        E_JUMP_TP,
        E_BUMP,
        E_FALL,
        E_FALL_FAINT,
        E_HIT_FAINT,
        E_HAPPY,
        E_LIFT_UP_BOX,
        E_LIFT_BOX,
        E_LIFT_LOW_BOX,
        E_PUT_UP_BOX,
        E_PUT_BOX,
        E_PUT_LOW_BOX,
        E_WAIT_BOX,
        E_WALK_BOX,
        E_WAIT_OVER_BOX,
        E_JUMP_BOX,
        E_WAIT_TP_BOX,
        E_JUMP_TP_BOX,
        E_BUMP_BOX,
        E_FALL_BOX,
        E_LIFT_UP_CHARA,
        E_LIFT_CHARA,
        E_LIFT_LOW_CHARA,
        E_PUT_UP_CHARA,
        E_PUT_CHARA,
        E_PUT_LOW_CHARA,
        E_WAIT_CHARA,
        E_WALK_CHARA,
        E_WAIT_OVER_CHARA,
        E_JUMP_CHARA,
        E_WAIT_TP_CHARA,
        E_JUMP_TP_CHARA,
        E_BUMP_CHARA,
        E_FALL_CHARA,
        E_HAPPY_CHARA,

        E_NONE,
    };

#else
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
        E_WAIT_TP,
        E_WAIT_OVER_BOX,
        E_WAIT_TP_BOX,
        E_WAIT_OVER_CHARA,
        E_WAIT_TP_CHARA,

        E_NONE
    };
#endif

    // アニメーション管理用
    Animator                        _playerAnimator;    //!< アニメーター取得用
    [SerializeField] PlayerState    _playerState;       //!< プレイヤーの状態管理用
    PlayerState                     _playerNextState;   //!< プレイヤーの状態管理用
    bool                            _animFinish;        //!< アニメーション変更フラグ
    public TrampolineAnim           _trampolineAnim;   //!< トランポリンアニメーションのセット

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
    * @brief        プレイヤーの状態の取得
    * @return       _playerState　プレイヤーの状態
    * @details      プレイヤーの状態を取得する関数です
    */
    public PlayerState GetPlayerState()
    {
        return _playerState;
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

    /**
    * @brief        トランポリンのアニメーションをセット
    * @return       なし
    * @details      トランポリンのアニメーションをセットするアニメーションイベントの関数です
    */
    //void SetTrampolineAnim()
    //{
    //    _trampolineAnim.StartWaitTP();        // トランポリンアニメーションを開始
    //}

    /**
    * @brief        ジャンプした際のトランポリンのアニメーションをセット
    * @return       なし
    * @details      トランポリンのアニメーションをセットするアニメーションイベントの関数です
    */
    void SetJumpTrampolineAnim()
    {
        _trampolineAnim.StartJumpTP();        // トランポリンアニメーションを開始
    }
}
