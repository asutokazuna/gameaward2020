/**
 * @file    Clear.cs
 * @brief   クリアアニメーションの管理
 * @author  Risa Ito
 * @date    2020/04/22(水)  作成
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @class Clear
 * @brief クリアアニメーションの管理
 */
public class Clear : MonoBehaviour
{
    // アニメーション管理用
    Animator        _clearAnimator;                     //!< アニメーター取得用
    public bool     _finishClear { get; private set; }  //!< アニメーション終了フラグ

    // Start is called before the first frame update
    void Start()
    {
        // 初期化処理
        _clearAnimator = GetComponent<Animator>();
        _finishClear = false;
    }

    /**
    * @brief        アニメーションの終了をセット
    * @return       なし
    * @details      アニメーションの終了をセットするアニメーションイベントの関数です
    */
    void SetFinishClearAnim()
    {
        _finishClear = true;
    }

    /**
    * @brief        クリアアニメーションの開始をセット
    * @return       なし
    * @details      クリアアニメーションの開始をセットする関数です
    */
    public void StartClear()
    {
        _clearAnimator.SetBool("FinishClear", false);       // アニメータにセット
        _clearAnimator.SetBool("StartClear", true);         // アニメータにセット
    }

    /**
    * @brief        クリアアニメーションの開始をセット
    * @return       なし
    * @details      クリアアニメーションの開始をセットする関数です
    */
    public void SetClearLoop()
    {
        _clearAnimator.SetBool("StartClear", false);        // アニメータにセット
        _clearAnimator.SetBool("StartClearLoop", true);     // アニメータにセット
    }

    /**
    * @brief        クリアアニメーションの終了をセット
    * @return       なし
    * @details      クリアアニメーションの終了をセットする関数です
    */
    public void FinishClear()
    {
        _finishClear = false;
        _clearAnimator.SetBool("StartClearLoop", false);    // アニメータにセット
        _clearAnimator.SetBool("FinishClear", true);        // アニメータにセット
    }
}
