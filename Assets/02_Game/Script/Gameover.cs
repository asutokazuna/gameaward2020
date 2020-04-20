/**
 * @file    Gameover.cs
 * @brief   ゲームオーバーアニメーションの管理
 * @author  Risa Ito
 * @date    2020/04/20(月)  作成
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @class PlayerAnimation
 * @brief ゲームオーバーアニメーションの管理
 */
public class Gameover : MonoBehaviour
{
    // アニメーション管理用
    Animator            _gameoverAnimator;          //!< アニメーター取得用
    bool                _startGameover;             //!< アニメーション開始フラグ
    public GameObject   _backObj;                   //!< 背景を暗くする画像用

    public bool _finishGameover{ get; private set; }    //!< アニメーション終了フラグ

    // Start is called before the first frame update
    void Start()
    {
        // 初期化処理
        _gameoverAnimator = GetComponent<Animator>();
        _finishGameover = true;
        _backObj.SetActive(false);
    }

    /**
    * @brief        アニメーションの終了をセット
    * @return       なし
    * @details      アニメーションの終了をセットするアニメーションイベントの関数です
    */
    void FinishGameoverAnim()
    {
        _finishGameover = true;
    }

    /**
    * @brief        ゲームオーバーアニメーションの開始をセット
    * @return       なし
    * @details      ゲームオーバーアニメーションの開始をセットする関数です
    */
    public void StartGameover()
    {
        _backObj.SetActive(true);
        _gameoverAnimator.SetBool("FinishGameover", false);     // アニメータにセット
        _gameoverAnimator.SetBool("StartGameover", true);       // アニメータにセット
    }

    /**
    * @brief        ゲームオーバーアニメーションの終了をセット
    * @return       なし
    * @details      ゲームオーバーアニメーションの終了をセットする関数です
    */
    public void FinishGameover()
    {
        _finishGameover = false;
        _backObj.SetActive(false);
        _gameoverAnimator.SetBool("StartGameover", false);      // アニメータにセット
        _gameoverAnimator.SetBool("FinishGameover", true);     // アニメータにセット
    }
}
