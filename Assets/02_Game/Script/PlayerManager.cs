/**
 * @file	PlayerManager.cs
 * @brief   プレイヤーの共通機能の統括
 *
 * @author	Kota Nakagami
 * @data    2020/04/15(水)   クラス作成
 *                           移動時間の統一化   
 *
 * @version	1.00
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerManager : MonoBehaviour
{
    //! 変数宣言
    [SerializeField] float _moveTime;               //!< 移動時間
    [SerializeField] float _liftTime;               //!< 持ち上げる時間
    [SerializeField] float _putTime;                //!< 置く時間
    [SerializeField] float _jumpTime;               //!< ジャンプ時間
    [SerializeField] float _outsideTheAreaTime;     //!< エリア外落下時間
    [SerializeField] bool _debug = default;


    // Start is called before the first frame update
    void Start()
    {
        if (_moveTime <= 0)
        {// 移動時間が0を下回った状態でセットされていたら
            _moveTime = 1;  // デフォルトタイムにセット(アニメーションに合わせる)
        }
        if (_liftTime <= 0)
        {// 移動時間が0を下回った状態でセットされていたら
            _liftTime = 1;  // デフォルトタイムにセット(アニメーションに合わせる)
        }
        if (_putTime <= 0)
        {// 移動時間が0を下回った状態でセットされていたら
            _putTime = 1;  // デフォルトタイムにセット(アニメーションに合わせる)
        }
        if (_jumpTime <= 0)
        {// 移動時間が0を下回った状態でセットされていたら
            _jumpTime = 1;  // デフォルトタイムにセット(アニメーションに合わせる)
        }
        if (_outsideTheAreaTime <= 0)
        {// 移動時間が0を下回った状態でセットされていたら
            _outsideTheAreaTime = 1;
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }


    /**
     * @brief 移動時間変数の取得
     */
    public float MoveTime
    {
        get { return _moveTime; }
    }

    /**
     * @brief 移動時間変数の取得
     */
    public float LiftTime
    {
        get { return _liftTime; }
    }


    /**
     * @brief 移動時間変数の取得
     */
    public float PutTime
    {
        get { return _putTime; }
    }

    /**
     * @brief 移動時間変数の取得
     */
    public float JumpTime
    {
        get { return _jumpTime; }
    }


    /**
     * @brief 移動時間変数の取得
     */
    public float OutsideTheEreaTime
    {
        get { return _outsideTheAreaTime; }
    }


    /**
     * @brief 移動中かどうかの判定
     */
    public bool Debug //!< 移動フラグ
    {
        get { return _debug; }  // ゲッター
    }
}


// EOF