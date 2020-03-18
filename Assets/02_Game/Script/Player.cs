/*
 * @file	Player.cs
 * @brief   プレイヤーの管理
 *
 * @author	Kota Nakagami
 * @date1	2020/02/21(金)
 *
 * @version	1.00
 */


 using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * @class Player
 * @brief プレイヤーの動き
 */
public class Player : BaseObject {

    [SerializeField] Vector3Int _havePos;   //!< 持ってるオブジェクトの座標の保持


    /*
     * @brief 初期化処理
     * @return なし
     */
    public void Awake()
    {// プレイヤーの設定を後で変更しなきゃ
        _myObject = E_FIELD_OBJECT.PLAYER_01;   // とりあえず
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
        FieldController fieldCtrl = GameObject.FindGameObjectWithTag("FieldController")
            .GetComponent<FieldController>();   //!< メインのフィールド保持
        _oldPosition    = _position;       //!< 座標の保持
        _position       = new Vector3Int(_position.x + movement.x, _position.y + movement.y, _position.z + movement.z);

        // 向いてる方向の補正
        offsetDirect();

        // プレイヤー01の移動
        if (_myObject.Equals(E_FIELD_OBJECT.PLAYER_01))
        {
            // フィールドから落ちる場合
            if (fieldCtrl.isFall(_position) || fieldCtrl.isLimitField(_position))
            {
                GameOevr(gameObject);
                return;
            }
            // 移動出来ない場合
            if (fieldCtrl.isDontMovePlayer(_position,_oldPosition))
            {
                _position = _oldPosition;
                return;
            }

            // 衝突イベント
            if (fieldCtrl.isCollisionToObject(_position))
            {
                // ブロックと衝突
                if (fieldCtrl.isCollisionToObject(_position, E_FIELD_OBJECT.BLOCK_NORMAL))
                {
                    // 上がる
                    _position = new Vector3Int(_position.x, _position.y + 1, _position.z);
                }
            }
            // 何とも衝突しない
            else
            {
                // 下に降りる処理
                if (fieldCtrl.isGetoff(_position))
                {
                    _position = new Vector3Int(_position.x, _position.y - 1, _position.z);
                }
            }

            // フィールドアップデート
            fieldCtrl.UpdateField(this);
            if (!_eHaveObj.Equals(E_FIELD_OBJECT.NONE))
            {
                //Vector3Int pos = new Vector3Int(_oldPosition.x, _oldPosition.y + 1, _oldPosition.z);
                Debug.Log("通ってる");
            }
            
            // 瞬間移動やんけ
            transform.position = fieldCtrl.offsetPos(_position);    // ワールド座標の補正

            Debug.Log(name + " が処理されたよ");

            return;
        }

        // プレイヤー02の移動
        if (_myObject.Equals(E_FIELD_OBJECT.PLAYER_02))
        {

        }

        // プレイヤー03の移動
        if (_myObject.Equals(E_FIELD_OBJECT.PLAYER_03))
        {

        }
    }


    /*
     * @brief 物を持ち上げる
     * @return なし
     */
    override public void Lift()
    {
        FieldController fieldCtrl = GameObject.FindGameObjectWithTag("FieldController")
            .GetComponent<FieldController>();   //!< メインのフィールド保持
        Vector3Int targetPos = new Vector3Int(_position.x + _direct.x, _position.y + _direct.y, _position.z + _direct.z);

        if (fieldCtrl.isCollisionToObject(targetPos) &&
            !fieldCtrl.isCollisionToObject(new Vector3Int(targetPos.x, targetPos.y + 1, targetPos.z)))
        {// 前方に何かオブジェクトがあったら
            Debug.Log(name + " が" + fieldCtrl._aField[targetPos.x, targetPos.y, targetPos.z].name + " を持ち上げました");
            _eHaveObj = fieldCtrl.LiftObject(_position, targetPos);    // 持ち上げる
            GameObject.Find(fieldCtrl._aField[_position.x, _position.y + 1, _position.z].name).transform.parent = transform;
            _havePos = new Vector3Int(_position.x, _position.y + 1, _position.z);

            if (!fieldCtrl._aField[_position.x, _position.y + 1, _position.z]._eHaveObj.Equals(E_FIELD_OBJECT.NONE))
            {// もし既に何かを持っていたら
                fieldCtrl._aField[_position.x, _position.y + 1, _position.z].Lift();   // もう一度持ち上げる
            }
        }
    }


    /*
     * @brief 物を下す
     * @return なし
     */
    override public void LetDown()
    {
        FieldController fieldCtrl = GameObject.FindGameObjectWithTag("FieldController")
            .GetComponent<FieldController>();   //!< メインのフィールド保持
        Vector3Int targetPos = new Vector3Int(_position.x + _direct.x, _position.y + _direct.y, _position.z + _direct.z);

        if (!fieldCtrl.isPut(targetPos))
        {
            // 親子関係を解除
            GameObject.Find(fieldCtrl._aField[_havePos.x, _havePos.y, _havePos.z].name).transform.parent = null;
            // オブジェクトを手放す
            _eHaveObj = E_FIELD_OBJECT.NONE;

            // 置いたものがフィールド外の場合
            if (fieldCtrl.isFall(targetPos))
            {
                GameOevr(fieldCtrl._aField[_havePos.x, _havePos.y, _havePos.z].gameObject);
            }
            else
            {
                targetPos = fieldCtrl.GetPutPos(targetPos);
            }

            // 置く処理
            fieldCtrl._aField[_havePos.x, _havePos.y, _havePos.z].
                Put(new Vector3Int(targetPos.x, targetPos.y, targetPos.z));

            if (!fieldCtrl._aField[targetPos.x, targetPos.y, targetPos.z]._eHaveObj.Equals(E_FIELD_OBJECT.NONE))
            {// もし既に何かを持っていたら
                fieldCtrl._aField[targetPos.x, targetPos.y, targetPos.z].LetDown();   // もう一度置く
            }
        }
    }
}

// EOF