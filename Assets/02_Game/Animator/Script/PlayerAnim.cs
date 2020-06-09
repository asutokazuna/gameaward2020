using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    // プレイヤーの状態
    public enum PlayerState
    {
        E_WAIT = 0,
        E_WALK,
        E_JUMP,
        E_BUMP,
        E_FALL,
        E_FALL_FAINT,
        E_HIT_FAINT,
        E_HAPPY,
        E_LIFT,
        E_PUT,

        E_STATE_MAX
    };

    // プレイヤーに関する情報
    public enum PlayerInfo
    {
        E_TP,
        E_BOX,
        E_CHARA,
        E_OVER,
        E_FALL,
        E_FAINT,
        E_LIFTED,

        E_TP_FALSE,
        E_OVER_FALSE,
        E_LIFTED_FALSE,
    }

    // アニメーション管理用
    Animator    _playerAnimator;        //!< アニメーター取得用
    PlayerState _playerState;           //!< プレイヤーの状態管理用
    WaitState[] _waitState;
    TrampolineAnim _trampolineAnim;
    public bool _faintFlag;

    // Start is called before the first frame update
    void Start()
    {
        // 初期化処理
        _playerAnimator = GetComponent<Animator>();
        _playerState = PlayerState.E_WAIT;
        _playerAnimator.SetBool("Finish", true);
        _waitState = _playerAnimator.GetBehaviours<WaitState>();
        _faintFlag = false;
    }

    // Update is called once per frame
    void Update()
    {
        // 待機ステートに入ったかチェック
        for (int i = 0; i < _waitState.Length; i++)
        {
            if(_waitState[i]._isWait)
            {
                _waitState[i]._isWait = false;
                _playerState = PlayerState.E_WAIT;
                Invoke("SetAnimFinish", 0.2f);
                break;
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
        // アニメーションの変更を受け付けたか確認
        if (_playerState != state)
        {
            Debug.Log("Old : " + _playerState);     // ひとつ前の状態をログで確認

            // アニメーションを更新
            _playerState = state;                                           // 次のアニメーションをセット
            _playerAnimator.SetInteger("PlayerState", (int)_playerState);   // アニメータにセット

            Debug.Log(_playerState);    // 現在の状態をログで確認
        }
    }

    /**
    * @brief        プレイヤーの状態のセット
    * @param[in]    PlayerInfo プレイヤーの情報
    * @return       なし
    * @details      プレイヤーの情報をセットする関数です
    */
    public void SetPlayerInfo(PlayerInfo info)
    {
        switch (info)
        {
            case PlayerInfo.E_TP:
                _playerAnimator.SetBool("TP", true);
                break;
            case PlayerInfo.E_BOX:
                _playerAnimator.SetBool("withBox", true);
                break;
            case PlayerInfo.E_CHARA:
                _playerAnimator.SetBool("withChara", true);
                break;
            case PlayerInfo.E_OVER:
                _playerAnimator.SetBool("Over", true);
                break;
            case PlayerInfo.E_FALL:
                _playerAnimator.SetBool("Fall", true);
                break;
            case PlayerInfo.E_FAINT:
                _playerAnimator.SetBool("Faint", true);
                break;
            case PlayerInfo.E_LIFTED:
                _playerAnimator.SetBool("Lifted", true);
                break;
            case PlayerInfo.E_TP_FALSE:
                _playerAnimator.SetBool("TP", false);
                break;
            case PlayerInfo.E_OVER_FALSE:
                _playerAnimator.SetBool("Over", false);
                break;
            case PlayerInfo.E_LIFTED_FALSE:
                _playerAnimator.SetBool("Lifted", false);
                break;
        }
    }

    /**
    * @brief        高さのセット
    * @param[in]    int 持つものの高さ
    * @return       なし
    * @details      プレイヤーの情報をセットする関数です
    */
    public void SetPlayerInfo(int height)
    {
        _playerAnimator.SetInteger("Height", height);
    }

    /**
    * @brief        アニメーション終了取得
    * @return       bool アニメーション終了フラグ
    * @details      アニメーションの終了を取得する関数です
    */
    public bool GetAnimFinish()
    {
        return _playerAnimator.GetBool("Finish");
    }

    /**
    * @brief        アニメーション終了セット
    * @return       なし
    * @details      アニメーションの終了をセットする関数です
    */
    void SetAnimFinish()
    {
        _playerAnimator.SetBool("Finish", true);
    }

    
    /**
    * @brief        トランポリンのアニメーションをセット
    * @return       なし
    * @details      トランポリンのアニメーションをセットするアニメーションイベントの関数です
    */
    public void SetTrampolineAnim(GameObject trampoline)
    {
        if(trampoline == null)
        {
            return;
        }
        _trampolineAnim = trampoline.GetComponent<TrampolineAnim>();
        _trampolineAnim.StartWaitTP();        // トランポリンアニメーションを開始
    }

    /**
    * @brief        ジャンプした際のトランポリンのアニメーションをセット
    * @return       なし
    * @details      トランポリンのアニメーションをセットするアニメーションイベントの関数です
    */
    public void SetJumpTrampolineAnim()
    {
        _trampolineAnim.StartJumpTP();        // トランポリンアニメーションを開始
    }

    /**
    * @brief        アニメーション終了取得
    * @return       bool アニメーション終了フラグ
    * @details      アニメーションの終了を取得する関数です
    */
    public bool GetTPFlag()
    {
        return _playerAnimator.GetBool("TP");
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
}
