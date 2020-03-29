/*
 * @file	BaseObject.cs
 * @brief   フィールドに置かれるオブジェクトの基底クラス
 *
 * @author	Kota Nakagami
 * @date1	2020/03/05(木)
 *
 * @version	1.00
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/*
 * @enum オブジェクト情報
 */
public enum E_FIELD_OBJECT
{
    NONE,           // 無
    PLAYER_01,      // プレイヤー01
    PLAYER_02,      // プレイヤー02
    PLAYER_03,      // プレイヤー03
    BLOCK_NORMAL,   // 通常ブロック
}

/*
 * @class BaseObject
 * @brief フィールドに置かれるオブジェクトの抽象クラス
 */
public class BaseObject : MonoBehaviour
{
    //! 変数宣言
    [SerializeField] public E_FIELD_OBJECT  _myObject;      //!< 自身のオブジェクト情報
    [SerializeField] public Vector3Int      _position;      //!< 現在フィールド座標
    [SerializeField] public Vector3Int      _oldPosition;   //!< 過去フィールド座標
    [SerializeField] public Vector3Int      _direct;        //!< 向いてる方向
    [SerializeField] public E_FIELD_OBJECT  _eHaveObj;      //!< 持っているオブジェクト
    [SerializeField] public bool            _bLifted;       //!< 何かに持ち上げられいる時 = true


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
        _eHaveObj       = E_FIELD_OBJECT.NONE;
        _bLifted        = false;
    }


    /*
     * @brief 初期化
     */
    public void Init()
    {
        offSetArrayPos();
        InitDirect();
        GameObject.FindGameObjectWithTag("FieldController").GetComponent<FieldController>().UpdateField(this);
    }


    /*
     * @brief 初期化
     */
    virtual public void Start()
    {

    }


    /*
     * @brief 更新処理
     */
    virtual public void Update()
    {

    }


    /*
     * @brief 配列座標の補正
     * @param1 FieldControllerのワールド座標
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
            ._aField[_oldPosition.x, _oldPosition.y, _oldPosition.z] = new BaseObject();
        Destroy(gameObject);
    }


    /*
     * @brief オブジェクトを動かす
     * @return なし
     */
    virtual public void Move(Vector3Int vector3Int = new Vector3Int())
    {

    }


    /*
     * @brief 物を持ち上げる、下す
     * @return なし
     */
    virtual public void HandAction()
    {
        if (_eHaveObj.Equals(E_FIELD_OBJECT.NONE))
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
        transform.position  = fieldCtrl.offsetPos(_myObject, _position);
        _bLifted            = true;
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
        transform.position = fieldCtrl.offsetPos(_myObject, _position);
        _bLifted = false;
    }

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
        // 脳死方向の設定プログラム
        if (_direct.x > 0)
        {// 右
            transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        }
        if (_direct.x < 0)
        {// 左
            transform.rotation = Quaternion.Euler(0f, -90f, 0f);
        }
        if (_direct.z > 0)
        {// 奥
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        if (_direct.z < 0)
        {// 手前
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
    }
}

// EOF