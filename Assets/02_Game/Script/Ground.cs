/*
 * @file	Ground.cs
 * @brief   プレイヤーの管理
 *
 * @author	Kota Nakagami
 * @date1	2020/02/21(金)
 * @data2   2020/03/06(金)
 * @data2   2020/04/10(金)   マップ配列の参照を FieldController.cs から Map.cs に変更した
 *
 * @version	1.00
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * @class Ground
 * @brief 地面
 */
public class Ground : BaseObject
{

    /*
     * @brief 初期化処理
     * @return なし
     */
    public void Awake()
    {
        
    }


    /*
     * @brief 初期化
     * @return なし
     */
    override public void Init(int number)
    {
        Map map     = GameObject.FindGameObjectWithTag("Map").GetComponent<Map>(); // コンポーネントの取得
        _myObject   = E_FIELD_OBJECT.BLOCK_GROUND;
        _myNumber   = number;

        // 座標の補正
        _position = _oldPosition = new Vector3Int(
            (int)(transform.position.x - map._offsetPos.x),
            (int)((transform.position.y + 0.5f) - map._offsetPos.y),
            (int)(transform.position.z - map._offsetPos.z)
            );

        _lifted     = false;
        _fullWater  = false;
        _animCnt    = 0;
        _direct     = new Vector3Int(0, 0, 1);  // 取り合えずの処理
    }


    /*
     * @brief 初期化
     * @return なし
     */
    override public void Start()
    {

    }


    /*
     * @brief 更新処理
     * @return なし
     */
    override public void Update()
    {
        
    }


    /*
     * @brief 通常ブロックの動き
     * @param1 ベクトル
     * @return なし
     */
    override public void Move(Vector3Int movement)
    {
        
    }
}

// EOF