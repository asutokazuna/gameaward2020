/*
 * @file	BaseObject.cs
 * @brief   フィールドに置かれるオブジェクトの基底クラス
 *
 * @author	Kota Nakagami
 * @date1	2020/03/05(木)
 *
 * @version	1.00
 */


//#define MODE_MAP    // 扱うスクリプト


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/*
 * @class BaseObject
 * @brief フィールドに置かれるオブジェクトの抽象クラス
 */
public class BaseObject : MonoBehaviour
{
    // 定数定義
    [SerializeField] public int MAX_ANIM_WALK = 60;

    //! 変数宣言
    [SerializeField] public E_FIELD_OBJECT  _myObject;      //!< 自身のオブジェクト情報
    [SerializeField] public int             _myNumber;      //!< 自身のオブジェクト番号
    [SerializeField] public Vector3Int      _position;      //!< 現在フィールド座標
    [SerializeField] public Vector3Int      _oldPosition;   //!< 過去フィールド座標
    [SerializeField] public Vector3Int      _direct;        //!< 向いてる方向
#if !MODE_MAP
    [SerializeField] public E_FIELD_OBJECT  _haveObj;       //!< 持っているオブジェクト
    [SerializeField] public Vector3         _addPos;        //!< 加算量
                     public bool            _nowMove;       //!< 移動フラグ
    [SerializeField] public Vector3         _nextPos;       //!< 移動先の座標
#endif
    [SerializeField] public bool            _lifted;        //!< 何かに持ち上げられいる時 = true
    [SerializeField] public bool            _fullWater;     //!< たまってるかのフラグ
                     public int             _animCnt;       //!< アニメーションカウント


    /*
     * @brief コンストラクタ
     * @return なし
     */
    public BaseObject()
    {
        _myObject       = E_FIELD_OBJECT.NONE;
        _position       = new Vector3Int();
        _oldPosition    = new Vector3Int();
        _direct         = new Vector3Int();
#if !MODE_MAP
        _haveObj        = E_FIELD_OBJECT.NONE;
#endif
        _lifted         = false;
        _fullWater      = false;
        _animCnt        = 0;
    }

#if MODE_MAP
    /*
     * @brief オブジェクト情報の初期化
     * @param1 オブジェクト情報
     * @return なし
     */
    virtual public void Init()
    {
#if !MODE_MAP
        offSetArrayPos();
        InitDirect();
        GameObject.FindGameObjectWithTag("FieldController").GetComponent<FieldController>().UpdateField(this);
#endif
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
     * @brief オブジェクトを動かす
     * @param1 目的座標
     * @return なし
     */
    virtual public void Move(Vector3Int pos)
    {
        _oldPosition    = _position;
        _position       = pos;
        //GameObject.FindGameObjectWithTag("Map").GetComponent<Map>().
    }


    /*
     * @brief  持ち上げられる
     * @param1 ターゲット座標
     * @return なし
     */
    public void Lifted(Vector3Int pos)
    {
        _oldPosition    = _position;
        _position       = pos;

        // 後で変更
        GameObject.FindGameObjectWithTag("FieldController")
            .GetComponent<FieldController>().UpdateField(this);   //!< メインのフィールド保持
        transform.position = GameObject.FindGameObjectWithTag("FieldController")
            .GetComponent<FieldController>().offsetPos(_myObject, _position);

        _lifted = true;
    }


    /*
     * @brief  置かれる
     * @param1 ターゲット座標
     * @return なし
     */
    public void Puted(Vector3Int pos)
    {
        _oldPosition    = _position;
        _position       = pos;

        // 後で変更
        GameObject.FindGameObjectWithTag("FieldController")
            .GetComponent<FieldController>().UpdateField(this);   //!< メインのフィールド保持
        transform.position = GameObject.FindGameObjectWithTag("FieldController")
            .GetComponent<FieldController>().offsetPos(_myObject, _position);

        _lifted = false;
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


    /*
     * @brief 満タンフラグの取得
     * @return 満タンなら true
     */
    public bool GetFullWater()
    {
        return _fullWater;
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