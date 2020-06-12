/**
 * @file    TrampolineAnim.cs
 * @brief   トランポリンアニメーションの管理
 * @author  Risa Ito
 * @date    2020/04/26(日)  作成
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampolineAnim : MonoBehaviour
{
    // アニメーション管理用
    private Animator    _TPAnimator;        //!< アニメーター取得用
    private bool        _startTPLoop;       //!< アニメーション開始フラグ
    private int         _countShrink;       //!< 最後に揺れる回数カウント
    public  int         _maxShrink;         //!< 最後に揺れる回数

    // Start is called before the first frame update
    void Start()
    {
        // 初期化処理
        _TPAnimator = GetComponent<Animator>();
        _countShrink = 0;
    }

    /**
    * @brief        トランポリン待機のアニメーションの開始をセット
    * @return       なし
    * @details      トランポリン待機の開始をセットする関数です
    */
    public void StartWaitTP()
    {
        Invoke("StartWaitTPAnim", 0.3f);

    }

    void StartWaitTPAnim()
    {
        _TPAnimator.SetBool("FinishTP", false);         // アニメータにセット
        _TPAnimator.SetBool("StartWaitTP", true);
        _TPAnimator.SetBool("Jump", false);
        _countShrink = 0;
        _TPAnimator.SetInteger("Count", _countShrink);
        _TPAnimator.speed = 1.1f;
    }

    /**
    * @brief        トランポリン待機のアニメーションの終了をセット
    * @return       なし
    * @details      トランポリン待機の終了をセットする関数です
    */
    public void StartJumpTP()
    {
        _TPAnimator.SetBool("StartWaitTP", false);     // アニメータにセット
        _TPAnimator.SetBool("StartJumpTP", true);      // アニメータにセット
        _TPAnimator.SetBool("Jump", true);             // アニメータにセット
    }

    /**
    * @brief        トランポリン待機のアニメーションの終了をセット
    * @return       なし
    * @details      トランポリンアニメーションの終了をセットするアニメーション関数です
    */
    void CountTP()
    {
        if (_TPAnimator.GetBool("Jump"))
        {
            _countShrink++;
            _TPAnimator.speed /= 1.3f;
            _TPAnimator.SetInteger("Count", _countShrink);
        }
    }
}
