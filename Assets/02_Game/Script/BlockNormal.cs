/*
 * @file	BlockNormal.cs
 * @brief   プレイヤーの管理
 *
 * @author	Kota Nakagami
 * @date1	2020/02/21(金)
 * @data2   2020/03/06(金)
 *
 * @version	1.00
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * @class BlockNormal
 * @brief 通常ブロッククラス
 */
public class BlockNormal : BaseObject
{

    /*
     * @brief 初期化処理
     * @return なし
     */
    public void Awake()
    {
        _myObject = E_FIELD_OBJECT.BLOCK_NORMAL;
    }


    /*
     * @brief 初期化
     * @return なし
     */
    override public void Start()
    {
        Init();
    }


    /*
     * @brief 更新処理
     * @return なし
     */
    override public void Update()
    {
        
    }


}

// EOF