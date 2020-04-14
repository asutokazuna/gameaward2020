/*
 * @file	WaterSourceBlock.cs
 * @brief   水源のブロック
 *
 * @author	Kota Nakagami
 * @date1	2020/03/30(月)
 * @data2   2020/04/10(金)   マップ配列の参照を FieldController.cs から Map.cs に変更した
 *
 * @version	1.00
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * @class 水源ブロック
 * @brief 水が流れ出るブロックの制御
 */
public class WaterSourceBlock : BaseObject
{

    /*
     * @brief 初期化
     * @return なし
     */
    override public void Init(int number)
    {
        Map map     = GameObject.FindGameObjectWithTag("Map").GetComponent<Map>(); // コンポーネントの取得
        _myObject   = E_FIELD_OBJECT.BLOCK_WATER_SOURCE;
        _myNumber   = number;

        // 座標の補正
        _position = _oldPosition = new Vector3Int(
            (int)(transform.position.x - map._offsetPos.x),
            (int)(transform.position.y - map._offsetPos.y),
            (int)(transform.position.z - map._offsetPos.z)
            );

        _lifted     = false;
        _fullWater  = false;
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
