/**
 * @file    Gameover.cs
 * @brief   ゲームオーバーアニメーションの管理
 * @author  Risa Ito
 * @date    2020/04/20(月)  作成
 * @date    2020/04/22(水)  背景の出し方などを変更
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @class Gameover
 * @brief ゲームオーバーアニメーションの管理
 */
public class Gameover : MonoBehaviour
{
    // アニメーション管理用
    private Animator    _gameoverAnimator;                      //!< アニメーター取得用
    public  bool        _finishGameover{ get; private set; }    //!< アニメーション終了フラグ
    private Map         _map;                                   //!< ゲームオーバーフラグ取得用

    // Start is called before the first frame update
    void Start()
    {
        // 初期化処理
        _gameoverAnimator = GetComponent<Animator>();
        _finishGameover = false;
        _map = GameObject.FindWithTag("Map").GetComponent<Map>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_map._gameOver)
        {
            _gameoverAnimator.SetBool("StartGameover", true);       // アニメータにセット
        }
    }
}
