/*
 * @file	BaseObject.cs
 * @brief   フィールドに置かれるオブジェクトの基底クラス
 *
 * @author	Kota Nakagami
 * @date1	2020/03/05(木)
 * @data2   2020/04/10(金)   マップ配列の参照を FieldController.cs から Map.cs に変更した
 * @data3   2020/04/16(木)   コンストラクタの削除
 *
 * @version	1.00
 */


#define MODE_MAP    // 扱うスクリプト


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


/*
 * @enum オブジェクト情報
 */
public enum E_OBJECT_MODE
{
    WAIT,       // 待機
    ROTATE,     // 回転
    MOVE,       // 移動
    GET_UP,     // 上に登る
    GET_OFF,    // 下に降りる
    LIFT,       // 持ち上げる
    LIFTED,     // 持ち上げられる
    PUT,        // 置く
    PUTED,      // 置かれる
    FALL,       // 落下
}


/*
 * @class BaseObject
 * @brief フィールドに置かれるオブジェクトの抽象クラス
 */
public class BaseObject : MonoBehaviour
{
    //! 変数宣言
    [SerializeField] public E_FIELD_OBJECT      _myObject;      //!< 自身のオブジェクト情報
    [SerializeField] public int                 _myNumber;      //!< 自身のオブジェクト番号
    [SerializeField] public Vector3Int          _position;      //!< 現在フィールド座標
    [SerializeField] public Vector3Int          _oldPosition;   //!< 過去フィールド座標
    [SerializeField] protected Vector3          _nextPos;       //!< 次の座標
    [SerializeField] public Vector3Int          _direct;        //!< 向いてる方向
    [SerializeField] public bool                _lifted;        //!< 何かに持ち上げられいる時 = true
    [SerializeField] protected E_OBJECT_MODE    _mode;          //!< オブジェクトの状態
#if !MODE_MAP
    [SerializeField] public E_FIELD_OBJECT  _haveObj;       //!< 持っているオブジェクト
    [SerializeField] public Vector3         _addPos;        //!< 加算量
                     public bool            _nowMove;       //!< 移動フラグ
    [SerializeField] public Vector3         _nextPos;       //!< 移動先の座標
#endif

#if MODE_MAP
    /*
     * @brief オブジェクト情報の初期化
     * @param1 オブジェクト番号
     * @return なし
     */
    virtual public void Init(int number)
    {
        _myObject   = E_FIELD_OBJECT.NONE;
        _myNumber   = number;

        // 座標の補正
        _position = _oldPosition = new Vector3Int(0, 0, 0);

        _lifted     = false;
        _mode       = E_OBJECT_MODE.WAIT;
        _direct     = new Vector3Int(0, 0, 1);  // 取り合えずの処理
    }


    /*
     * @brief 初期化
     * @return なし
     */
    virtual public void Start()
    {

    }


    /*
     * @brief 更新処理
     * @return なし
     */
    virtual public void Update()
    {

    }


    /*
     * @brief オブジェクトの移動
     * @return なし
     */
    virtual public void Move()
    {

    }


    /*
     * @brief オブジェクトの追従
     * @param1 目的座標
     * @return なし
     */
    virtual public void Follow(Vector3Int pos)
    {
        _oldPosition    = _position;
        _position       = pos;
    }


    virtual public void Fall(Vector3Int pos)
    {
        _oldPosition    = _position;
        _position       = pos;
        _lifted         = false;
        _mode           = E_OBJECT_MODE.FALL;
        offSetTransform();
        JumpMode();
    }


    /*
     * @brief  持ち上げられる
     * @param1 ターゲット座標
     * @return なし
     */
    virtual public void Lifted(Vector3Int pos)
    {
        _oldPosition    = _position;
        _position       = pos;
        _lifted         = true;
        _mode           = E_OBJECT_MODE.LIFTED;
        offSetTransform();
        JumpMode();   // 持ち上げられる
    }


    /*
     * @brief  置かれる
     * @param1 ターゲット座標
     * @return なし
     */
    virtual public void Puted(Vector3Int pos)
    {
        _oldPosition    = _position;
        _position       = pos;
        _lifted         = false;
        _mode           = E_OBJECT_MODE.PUTED;
        offSetTransform();
        JumpMode();
    }


    /*
     * @brief 待機モード
     * @return なし
     */
    virtual protected void WaitMode()
    {
        _mode = E_OBJECT_MODE.WAIT;
    }


    /*
     * @brief ジャンプモード
     * @return なし
     */
    virtual protected void JumpMode()
    {
        float power;
        if (_mode == E_OBJECT_MODE.LIFTED)
        {// 持ち上げられる時
            power = _position.y - _oldPosition.y;
            if (power == 0) power = 0.5f;
            transform.DOJump(
                new Vector3(_nextPos.x, _nextPos.y, _nextPos.z),   // 目的座標
                power, // ジャンプパワー
                1,  // ジャンプ回数
                GameObject.FindGameObjectWithTag("PlayerManager").GetComponent<PlayerManager>().MoveTime,   // 時間
                false
            ).OnComplete(() =>
            {
                WaitMode();
            });
        }
        else if (_mode == E_OBJECT_MODE.PUTED)
        {// 置かれる時
            power = _oldPosition.y - _position.y;
            if (power == 0) power = 0.5f;
            transform.DOJump(
                new Vector3(_nextPos.x, _nextPos.y, _nextPos.z),   // 目的座標
                power, // ジャンプパワー
                1,  // ジャンプ回数
                GameObject.FindGameObjectWithTag("PlayerManager").GetComponent<PlayerManager>().MoveTime,   // 時間
                false
            ).OnComplete(() =>
            {
                WaitMode();
            });
        }
        else if (_mode == E_OBJECT_MODE.FALL)
        {// 落ちる時
            transform.DOJump(
                new Vector3(_nextPos.x, _nextPos.y, _nextPos.z),   // 目的座標
                (_oldPosition.y - _position.y), // ジャンプパワー
                1,  // ジャンプ回数
                GameObject.FindGameObjectWithTag("PlayerManager").GetComponent<PlayerManager>().MoveTime,   // 時間
                false
                ).OnComplete(() =>
            {
                WaitMode();
                GameObject.FindGameObjectWithTag("Map").GetComponent<Map>()._gameOver = true;  // ゲームオーバーやで
            });
        }
    }


    /*
     * @brief 移動後の座標の調整
     * @return なし
     */
    virtual public void offSetTransform()
    {
        Map map = GameObject.FindGameObjectWithTag("Map").GetComponent<Map>(); // コンポーネントの取得
        _nextPos = new Vector3(
            (float)(_position.x + map._offsetPos.x),
            (float)(_position.y + map._offsetPos.y),
            (float)(_position.z + map._offsetPos.z)
            );
    }


#else
    /*
     * @brief 初期化
     * @return なし
     */
    public void Init()
    {
        offSetArrayPos();
        InitDirect();
        GameObject.FindGameObjectWithTag("FieldController").GetComponent<FieldController>().UpdateField(this);
        // 新しいBase
        //GameObject.FindGameObjectWithTag("Map").GetComponent<Map>().SetObject(this);
    }


    /*
     * @brief 初期化
     * @return なし
     */
    virtual public void Start()
    {

    }


    /*
     * @brief 更新処理
     * @return なし
     */
    virtual public void Update()
    {

    }


    /*
     * @brief ゲームオーバー処理
     * @return なし
     */
    public void GameOevr(GameObject gameObject)
    {
        // フラグ管理に変更する可能性があります
        SceneManager.LoadScene("SampleScene");
        Debug.Log("落ちたよ " + name + "今のフィールド座標" + _position);
        GameObject.FindGameObjectWithTag("FieldController").GetComponent<FieldController>()
            ._field[_oldPosition.x, _oldPosition.y, _oldPosition.z] = new BaseObject();
        Destroy(gameObject);
    }


    /*
     * @brief オブジェクトが何かしらの行動をとれるか
     * @return 行動できるのなら true
     */
    protected bool isAction()
    {
        if (_animCnt > 0)
        {// 移動中
            Debug.Log("移動できひんで");
            return false;
        }
        _animCnt = MAX_ANIM_WALK;
        _nowMove = true;
        return true;
    }


    /*
     * @brief オブジェクトを動かす
     * @return なし
     */
    virtual public void Move(Vector3Int vector3Int)
    {

    }


    /*
     * @brief 移動量の算出
     * @return なし
     */
    protected void Move()
    {
        _nextPos = GameObject.FindGameObjectWithTag("FieldController").GetComponent<FieldController>()
            .offsetPos(_myObject, _position);
        _addPos = new Vector3(_nextPos.x - transform.position.x, _nextPos.y - transform.position.y, _nextPos.z - transform.position.z);
        _addPos = new Vector3(_addPos.x / _animCnt, _addPos.y / _animCnt, _addPos.z / _animCnt);
    }


    /*
     * @brief 物を持ち上げる、下す
     * @return なし
     */
    virtual public void HandAction()
    {
        if (_animCnt > 0)
        {
            return;
        }
        if (_haveObj.Equals(E_FIELD_OBJECT.NONE))
        {// 物を持ち上げる
            Lift();
        }
        else
        {// 物を下す
            LetDown();
        }
    }


    /*
     * @brief 物を持ち上げる
     * @return なし
     */
    virtual public void Lift()
    {
    
    }


    /*
     * @brief  持ち上げられる
     * @param1 ターゲット座標
     * @return なし
     */
    virtual public void Lifted(Vector3Int pos)
    {
        FieldController fieldCtrl = GameObject.FindGameObjectWithTag("FieldController")
            .GetComponent<FieldController>();   //!< メインのフィールド保持
        _oldPosition        = _position;
        _position           = pos;
        fieldCtrl.UpdateField(this);
    
        _animCnt = MAX_ANIM_WALK;   // 後で直す
        Move();
        _nowMove = true;
    
        _lifted  = true;
    }


    /*
     * @brief 物を下す
     * @return なし
     */
    virtual public void LetDown()
    {
        
    }


    /*
     * @brief 置く
     * @param1 ターゲット座標
     * @return なし
     */
    public void Put(Vector3Int pos)
    {
        FieldController fieldCtrl = GameObject.FindGameObjectWithTag("FieldController")
            .GetComponent<FieldController>();   //!< メインのフィールド保持
        _oldPosition = _position;
        _position = pos;
        fieldCtrl.UpdateField(this);

        _animCnt = MAX_ANIM_WALK;   // 後で直す
        Move();
        _nowMove = true;

        _lifted = false;
    }


    /*
     * @brief オブジェクトの追従
     * @return なし
     */
    virtual public Vector3Int Follow(Vector3Int pos, Vector3Int direct)
    {
        _oldPosition = _position;
        _position = pos;
        
        GameObject.FindGameObjectWithTag("FieldController").GetComponent<FieldController>().UpdateField(this);
        return _position;
        //if (!_haveObj.Equals(E_FIELD_OBJECT.NONE))
        //{
        //    Follow(new Vector3Int(_position))
        //}
    }


    /*
     * @brief 配列座標の補正
     * @return なし
     */
    virtual protected void offSetArrayPos()
    {
        _oldPosition = _position = new Vector3Int(
            (int)(transform.position.x - GameObject.FindGameObjectWithTag("FieldController").transform.position.x),
            (int)(transform.position.y),
            (int)(transform.position.z - GameObject.FindGameObjectWithTag("FieldController").transform.position.z)
            );
    }

#endif


    /*
     * @brief 向いてる方向の設定
     * @param1 目的座標
     * @param2 過去座標
     * @return なし
     */
    public void offsetDirect()
    {
        // 脳死方向の設定プログラム
        if (_position.x - _oldPosition.x > 0)
        {// 右
            _direct = new Vector3Int(1, 0, 0);
            transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        }
        if (_position.x - _oldPosition.x < 0)
        {// 左
            _direct = new Vector3Int(-1, 0, 0);
            transform.rotation = Quaternion.Euler(0f, -90f, 0f);
        }
        if (_position.z - _oldPosition.z > 0)
        {// 奥
            _direct = new Vector3Int(0, 0, 1);
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        if (_position.z - _oldPosition.z < 0)
        {// 手前
            _direct = new Vector3Int(0, 0, -1);
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
    }


    /*
     * @brief 向いてる方向の初期化
     * @param1 方向
     * @return なし
     */
    public void InitDirect()
    {
        // 後で変更
        // 取り合えず全部正面を向いておく
        _direct = new Vector3Int(0, 0, 1);
    }


    //public void Rotate(Vector3Int direct)
    //{
    //    if (direct.x > 0)
    //    {// 右に回転
    //
    //    }
    //}
}

// EOF