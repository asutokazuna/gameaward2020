/*
 * @file	PlayerManager.cs
 * @brief   プレイヤーの共通機能の統括
 *
 * @author	Kota Nakagami
 * @data1   2020/04/15(水)   クラス作成
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
    [SerializeField] float _moveTime;   //!< 移動時間

    // Start is called before the first frame update
    void Start()
    {
        if (_moveTime <= 0)
        {// 移動時間が0を下回った状態でセットされていたら
            _moveTime = 1;  // デフォルトタイムにセット
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
     * @brief 移動時間変数の取得
     */
    public float MoveTime
    {
        get { return _moveTime; }
    }
}


// EOF