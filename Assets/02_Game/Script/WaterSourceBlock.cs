/*
 * @file	WaterSourceBlock.cs
 * @brief   水源のブロック
 *
 * @author	Kota Nakagami
 * @date1	2020/03/30(月)
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
    //! 変数宣言
    //[SerializeField] private Vector3Int _waterDirect;   //!< 水の流れ出る方向
    // 今は全方位に水が流れ出る


    /*
     * @brief 初期化処理
     * @return なし
     */
    public void Awake()
    {
        _myObject = E_FIELD_OBJECT.BLOCK_WATER_SOURCE;
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


    /*
     * @brief 通常ブロックの動き
     * @param1 ベクトル
     * @return なし
     */
    override public void Move(Vector3Int movement)
    {
        //_oldPosition = _position;       //!< 座標の保持
        //_position = new Vector3Int(_position.x + movement.x, _position.y + movement.y, _position.z + movement.z);
        //// 向いてる方向の補正
        //offsetDirect();
        //Debug.Log("ブロックが動いた");
    }
}
