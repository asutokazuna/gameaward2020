/*
 * @file	Player01.cs
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
 * @class Player01
 * @brief プレイヤーの動き
 */
public class Player : BaseObject {

    // 定数定義
    [SerializeField] int MAX_ANIM_WALK = 200;

    //! 変数宣言
    [SerializeField] Vector3Int _havePos;   //!< 持ってるオブジェクトの座標の保持
    [SerializeField] Vector3    _nextPos;
    [SerializeField] Vector3    _addPos;    //!< 加算量
                     bool       _isUpdate;  //!< 更新flag
                     bool       _nowMove;   //!< 移動フラグ
    FieldController _fieldCtrl;
    public PlayerAnimation _playerAnimation;


    /*
     * @brief 初期化処理
     * @return なし
     */
    public void Awake()
    {// プレイヤーの設定を後で変更しなきゃ
        _myObject = E_FIELD_OBJECT.PLAYER_01;   // とりあえず
        _playerAnimation = GameObject.Find(name).GetComponent<PlayerAnimation>();
    }


    /*
     * @brief 初期化
     * @return なし
     */
    override public void Start()
    {
        _fieldCtrl = GameObject.FindGameObjectWithTag("FieldController")
            .GetComponent<FieldController>();   //!< メインのフィールド保持
        Init();

        //_playerAnimation.SetPlayerState(PlayerAnimation.PlayerState.E_WALK);
    }


    /*
     * @brief 更新処理
     * @return なし
     */
    override public void Update()
    {
        _isUpdate = false;

        //playerAnimation.SetPlayerState(PlayerAnimation.PlayerState.E_WAIT);

        if (_animCnt > 0)
        {
            if (_nowMove)
            {
                transform.position =
                    new Vector3(transform.position.x + _addPos.x, transform.position.y + _addPos.y, transform.position.z + _addPos.z);
            }
            _animCnt--;
        }
        else if (_animCnt == 0)
        {// 移動していないとき

            if (!_haveObj.Equals(E_FIELD_OBJECT.NONE))
            {// 何かを持っている時
                _playerAnimation.SetPlayerState(PlayerAnimation.PlayerState.E_WAIT_BOX);
                Debug.Log("物をもった状態でのアニメーション");
            }
            else
            {// 何も持ってない時
                _playerAnimation.SetPlayerState(PlayerAnimation.PlayerState.E_WAIT);
            }

            // 座標の補正
            transform.position = _fieldCtrl.offsetPos(_myObject, _position);
            _animCnt = -1;
            _nowMove = false;
        }
    }


    /*
     * @brief 通常ブロックの動き
     * @param1 ベクトル
     * @return なし
     */
    override public void Move(Vector3Int movement)
    {
        if (_animCnt > 0)
        {// 移動中
            Debug.Log("移動できひんで");
            return;
        }

        _animCnt = MAX_ANIM_WALK;

        _nowMove = true;

        _oldPosition = _position;       //!< 座標の保持
        _position = new Vector3Int(_position.x + movement.x, _position.y + movement.y, _position.z + movement.z);

        // 向いてる方向の補正
        offsetDirect();

        // フィールドから落ちる場合
        if (_fieldCtrl.isFall(_position) || _fieldCtrl.isLimitField(_position))
        {
            GameOevr(gameObject);   // ゲームオーバー
            return;
        }
        // 移動出来ない場合
        if (_fieldCtrl.isDontMovePlayer(_position, _oldPosition))
        {
            _position = _oldPosition;

            // 移動しない
            this.Move();

            return;
        }

        // 衝突イベント
        if (_fieldCtrl.isCollisionToObject(_position))
        {
            // 登れるアイテムと衝突
            if (_fieldCtrl.isGetup(_position))
            {
                // 上がる
                _position = new Vector3Int(_position.x, _position.y + 1, _position.z);
            }
        }
        // 何とも衝突しない
        else
        {
            // 下に降りる処理
            if (_fieldCtrl.isGetoff(_position))
            {
                _position = new Vector3Int(_position.x, _position.y - 1, _position.z);
            }
            // 直進
            else
            {
                if (!_haveObj.Equals(E_FIELD_OBJECT.NONE))
                {// 何かを持っているとき
                    _playerAnimation.SetPlayerState(PlayerAnimation.PlayerState.E_WALK_BOX);
                }
                else
                {
                    _playerAnimation.SetPlayerState(PlayerAnimation.PlayerState.E_WALK);
                }
            }
        }

        // フィールドアップデート
        _fieldCtrl.UpdateField(this);
        if (!_haveObj.Equals(E_FIELD_OBJECT.NONE))
        {
            _havePos = _fieldCtrl._field[_havePos.x, _havePos.y, _havePos.z].Follow(
               new Vector3Int(_position.x, _position.y + 1, _position.z), _direct);
            Debug.Log("通ってる");
        }

        // 移動
        this.Move();

        Debug.Log(name + " が処理されたよ");

        return;
    }


    /*
     * @brief 物を持ち上げる
     * @return なし
     */
    override public void Lift()
    {
        if (_isUpdate) return;

        if (_animCnt > 0)
        {// 移動中
            Debug.Log("移動できひんで");
            return;
        }

        Vector3Int targetPos = new Vector3Int(_position.x + _direct.x, _position.y + _direct.y + 1, _position.z + _direct.z);

        // 前方に何かオブジェクトがあったら
        if (_fieldCtrl.isCollisionToObject(targetPos) &&
            !_fieldCtrl.isCollisionToObject(new Vector3Int(targetPos.x, targetPos.y, targetPos.z), E_FIELD_OBJECT.BLOCK_WATER_SOURCE))
        {
            if (_fieldCtrl.isCollisionToObject(new Vector3Int(targetPos.x, targetPos.y, targetPos.z), E_FIELD_OBJECT.BLOCK_TANK) &&
                _fieldCtrl.isCollisionToObject(new Vector3Int(targetPos.x, targetPos.y + 1, targetPos.z)))
            {// 上に物が置いてあったら
                return;
            }

            // 持ち上げる
            Debug.Log(name + " が" + _fieldCtrl._field[targetPos.x, targetPos.y, targetPos.z].name + " を持ち上げました");
            _haveObj = _fieldCtrl.LiftObject(_position, targetPos);

            // 追従
            GameObject.Find(_fieldCtrl._field[_position.x, _position.y + 1, _position.z].name).transform.parent = transform;
            _havePos = new Vector3Int(_position.x, _position.y + 1, _position.z);

            // もし既に何かを持っていたら
            if (!_fieldCtrl._field[_position.x, _position.y + 1, _position.z]._haveObj.Equals(E_FIELD_OBJECT.NONE))
            {
                // もう一度持ち上げる
                _fieldCtrl._field[_position.x, _position.y + 1, _position.z].Lift();
                Debug.Log("もう一度持ち上げるドン！");
            }

            _animCnt = MAX_ANIM_WALK;
            _playerAnimation.SetPlayerState(PlayerAnimation.PlayerState.E_LIFT_BOX);
        }


        else if (_fieldCtrl.isCollisionToObject(targetPos
            = new Vector3Int(_position.x + _direct.x, _position.y + _direct.y, _position.z + _direct.z)) &&
            !_fieldCtrl.isCollisionToObject(new Vector3Int(targetPos.x, targetPos.y, targetPos.z), E_FIELD_OBJECT.BLOCK_WATER_SOURCE))
        {

            if (_fieldCtrl.isCollisionToObject(new Vector3Int(targetPos.x, targetPos.y, targetPos.z), E_FIELD_OBJECT.BLOCK_TANK) &&
                _fieldCtrl.isCollisionToObject(new Vector3Int(targetPos.x, targetPos.y + 1, targetPos.z)))
            {// 上に物が置いてあったら
                return;
            }

            // 持ち上げる
            Debug.Log(name + " が" + _fieldCtrl._field[targetPos.x, targetPos.y, targetPos.z].name + " を持ち上げました");
            _haveObj = _fieldCtrl.LiftObject(_position, targetPos);

            // 追従
            GameObject.Find(_fieldCtrl._field[_position.x, _position.y + 1, _position.z].name).transform.parent = transform;
            _havePos = new Vector3Int(_position.x, _position.y + 1, _position.z);

            // もし既に何かを持っていたら
            if (!_fieldCtrl._field[_position.x, _position.y + 1, _position.z]._haveObj.Equals(E_FIELD_OBJECT.NONE))
            {
                // もう一度持ち上げる
                _fieldCtrl._field[_position.x, _position.y + 1, _position.z].Lift();
                Debug.Log("もう一度持ち上げるドン！");
            }

            _animCnt = MAX_ANIM_WALK;
            _playerAnimation.SetPlayerState(PlayerAnimation.PlayerState.E_LIFT_BOX);
        }

        _isUpdate = true;
    }


    /*
     * @brief 物を下す
     * @return なし
     */
    override public void LetDown()
    {
        if (_isUpdate) return;
        //return;

        if (_animCnt > 0)
        {// 移動中
            Debug.Log("移動できひんで");
            return;
        }

        Vector3Int targetPos = new Vector3Int(_position.x + _direct.x, _position.y + _direct.y, _position.z + _direct.z);

        if (!_fieldCtrl.isPut(targetPos))
        {
            Debug.Log(name + "離した");
            // 親子関係を解除
            GameObject.Find(_fieldCtrl._field[_havePos.x, _havePos.y, _havePos.z].name).transform.parent = null;
            // オブジェクトを手放す
            _haveObj = E_FIELD_OBJECT.NONE;

            // 置いたものがフィールド外の場合
            if (_fieldCtrl.isFall(targetPos))
            {
                GameOevr(_fieldCtrl._field[_havePos.x, _havePos.y, _havePos.z].gameObject);
            }
            else
            {
                targetPos = _fieldCtrl.GetPutPos(targetPos);
            }

            // 置く処理
            _fieldCtrl._field[_havePos.x, _havePos.y, _havePos.z].
                Put(new Vector3Int(targetPos.x, targetPos.y, targetPos.z));

            _animCnt = MAX_ANIM_WALK;
            _playerAnimation.SetPlayerState(PlayerAnimation.PlayerState.E_PUT_BOX);

            // もし既に何かを持っていたら
            if (!_fieldCtrl._field[targetPos.x, targetPos.y, targetPos.z]._haveObj.Equals(E_FIELD_OBJECT.NONE))
            {
                _fieldCtrl._field[targetPos.x, targetPos.y, targetPos.z].LetDown();   // もう一度置く
            }
        }
        _isUpdate = true;
    }



    private void Move()
    {
        _nextPos = _fieldCtrl.offsetPos(_myObject, _position);
        _addPos = new Vector3(_nextPos.x - transform.position.x, _nextPos.y - transform.position.y, _nextPos.z - transform.position.z);
        _addPos = new Vector3(_addPos.x / _animCnt, _addPos.y / _animCnt, _addPos.z / _animCnt);
    }


    /*
     * @brief 配列座標の補正
     * @param1 FieldControllerのワールド座標
     * @return なし
     */
    override protected void offSetArrayPos()
    {
        _oldPosition = _position = new Vector3Int(
            (int)(transform.position.x - GameObject.FindGameObjectWithTag("FieldController").transform.position.x),
            (int)(transform.position.y + 0.5f),
            (int)(transform.position.z - GameObject.FindGameObjectWithTag("FieldController").transform.position.z)
            );
    }
}

// EOF