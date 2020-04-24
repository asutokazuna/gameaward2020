using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TramplineBlock : BaseObject
{
    override public void Init(int number)
    {
        Map map = GameObject.FindGameObjectWithTag("Map").GetComponent<Map>(); // コンポーネントの取得
        _myObject = E_FIELD_OBJECT.BLOCK_TRAMPLINE;
        _myNumber = number;

        // 座標の補正
        _position = _oldPosition = new Vector3Int(
            (int)(transform.position.x - map._offsetPos.x),
            (int)(transform.position.y - map._offsetPos.y),
            (int)(transform.position.z - map._offsetPos.z)
            );

        _lifted = false;
        _mode = E_OBJECT_MODE.WAIT;
        _direct = new Vector3Int(0, 0, 0);  // 取り合えずの処理
    }

    // Start is called before the first frame update
    override public void Start()
    {
        
    }

    // Update is called once per frame
    override public void Update()
    {
        
    }

}
