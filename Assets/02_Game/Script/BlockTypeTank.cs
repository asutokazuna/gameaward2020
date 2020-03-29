/*
 * @file    BlockTypeTank.cs
 * @brief   箱にどんだけ水が入っているかの確認
 * 
 * @author  Kohdai Fukuda
 * @date1   2020/03/25(水)
 * 
 * @version 1.00
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockTypeTank : BaseObject
{
    public int MaxWater;
    private int numWater;
    private bool bFullTank = false;
    GameObject FieldController;
    FieldController fieldctl;
    private Vector3Int Oldpos;

    public void Awake()
    {
        _myObject = E_FIELD_OBJECT.BLOCK_TANK;
    }
    // Start is called before the first frame update
    override public void Start()
    {
        Init();
        FieldController = GameObject.Find("FieldController");
        fieldctl = FieldController.GetComponent<FieldController>();
    }

    // Update is called once per frame
    override public void Update()
    {
        AddWater();
    }

    public void AddWater()
    {
        if (isCollisionToNormalBox())
        {
            if (Oldpos.y == _position.y)
            {
                if (!bFullTank && numWater <= MaxWater)
                {
                    numWater += 1;
                    Debug.Log("現在の水の量" + numWater);
                }
                else
                {
                    Debug.Log("水満タンになった" + name);
                    bFullTank = true;
                    return;
                }
            }
        }
        else
        {
            if (numWater > 0)
            {
                numWater = 0;
                bFullTank = false;
                Debug.Log(name + "水ザバー");
            }
            else
            {
                return;
            }
        }
    }

    private bool isCollisionToNormalBox()
    {
        Vector3Int targetPos = new Vector3Int(_position.x, _position.y, _position.z);
        if(fieldctl.isCollisionToObject(new Vector3Int(targetPos.x + 1,targetPos.y,targetPos.z),E_FIELD_OBJECT.BLOCK_NORMAL)
        || fieldctl.isCollisionToObject(new Vector3Int(targetPos.x - 1,targetPos.y,targetPos.z),E_FIELD_OBJECT.BLOCK_NORMAL)
        || fieldctl.isCollisionToObject(new Vector3Int(targetPos.x,targetPos.y,targetPos.z + 1),E_FIELD_OBJECT.BLOCK_NORMAL)
        || fieldctl.isCollisionToObject(new Vector3Int(targetPos.x,targetPos.y,targetPos.z - 1),E_FIELD_OBJECT.BLOCK_NORMAL))
        {
            return true;
        }

        return false;
    }
}
