/*
 * @file	FieldController.cs
 * @brief   ステージマップの管理
 *
 * @author	Kota Nakagami
 * @date1	2020/02/20(木)
 *
 * @version	1.00
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @enum E_FIELD_MODE
 * @brief ゲームの状態
 */
public enum E_FIELD_MODE
{
    E_FIELD_NONE,   //!< 処理なし
    E_FIELD_MOVE,   //!< 移動中
}


/*
 * @class FieldController
 * @brief フィールドの全体管理
 */
public class FieldController : MonoBehaviour {

    //! 定数定義
    public const    int     MAX_FIELD_OBJECT    = 10;       // マップ1辺あたりに置けるオブジェクトの最大値
    [SerializeField]int     VAL_FIELD_MOVE      = 1;        // 一マス当たりの移動値
    //private         int     VAL_FIELD_ROTATE    = 90;       // カメラの回転角度
    private const   int     MAX_FIELD_RIDE      = 1;        // プレイヤーやその他の上りの上限値
    private const   int     MAX_FIELD_FALL      = 1;        // プレイヤーやその他の下りの上限値

    //! 変数宣言
    //[SerializeField] private int        _FieldAngle;        //!< マップ角度
    [SerializeField] private Vector3Int _direct;            //!< 方向
    public BaseObject[ , , ] _aField = new BaseObject[MAX_FIELD_OBJECT + 1, MAX_FIELD_OBJECT + 1, MAX_FIELD_OBJECT + 1];   //! マップ配列


    /*
     * @brief 初期化
     * @return なし
     */
    void Awake()
    {
        // フィールドの初期化
        for (int y = 0; y < MAX_FIELD_OBJECT; y++)
        {
            for (int z = 0; z < MAX_FIELD_OBJECT; z++)
            {
                for (int x = 0; x < MAX_FIELD_OBJECT; x++)
                {
                    _aField[x, y, z] = new BaseObject();
                    //_aField[x, y, z].GetComponent<Kota.BaseObject>().Init();
                }
            }
        }
    }


    /*
     * @brief 初期化
     * @return なし
     */
    void Start()
    {
        //_FieldAngle = 10;
    }


    /*
     * @brief 更新処理
     * @return なし
     */
    void Update()
    {
        ObjectMovement();
        HandAction();
    }


    /*
     * @brief 配列情報の更新
     * @param1 オブジェクト情報
     * @return なし
     */
    public void UpdateField(BaseObject _object)
    {
        _aField[_object._oldPosition.x, _object._oldPosition.y, _object._oldPosition.z] = new BaseObject();
        _aField[_object._position.x, _object._position.y, _object._position.z]          = _object;
    }


    /*
     * @brief オブジェクトの移動操作
     * @return なし
     */
    private void ObjectMovement()
    {
        bool isUpdate       = false;            //!< 更新フラグ

        // 移動キーを何も押してなかったら
        if (!Input.GetKeyDown(KeyCode.D) && !Input.GetKeyDown(KeyCode.A) && !Input.GetKeyDown(KeyCode.W) && !Input.GetKeyDown(KeyCode.S))
        {
            return;
        }
        else
        {
            _direct = new Vector3Int();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            _direct.x = VAL_FIELD_MOVE;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            _direct.x = -VAL_FIELD_MOVE;
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            _direct.z = VAL_FIELD_MOVE;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            _direct.z = -VAL_FIELD_MOVE;
        }
        for (int x = 0; x <= MAX_FIELD_OBJECT; x++)
        {
            for (int y = 0; y <= MAX_FIELD_OBJECT; y++)
            {
                foreach (BaseObject obj in _aField)
                {
                    if (obj == null) continue;
                    isUpdate = this.isMoveObject(_direct, obj, x, y); // オブジェクトのソート
                    if (isUpdate)
                    { // 移動可能なら実行
                        obj.Move(_direct);
                    }
                }
            }
        }
    }


    /*
     * @brief 持つ、話すの行動
     * @return なし
     */
    public void HandAction()
    {
        if (!Input.GetKeyDown(KeyCode.Space)) return;

        bool isUpdate       = false;            //!< 更新フラグ
        
        for (int i = 0; i <= MAX_FIELD_OBJECT; i++)
        {
            for (int j = 0; j <= MAX_FIELD_OBJECT; j++)
            {
                foreach (BaseObject obj in _aField)
                {
                    if (obj == null) continue;
                    isUpdate = this.isMoveObject(_direct, obj, i, j); // オブジェクトのソート
                    if (isUpdate)
                    { // 移動可能なら実行
                        obj.HandAction();
                    }
                }
            }
        }
    }


    /*
     * @brief 対象のオブジェクトを持ち上げる
     * @param1 自身のフィールド座標
     * @param2 対象のフィールド座標
     * @return 自信が持ち上げたオブジェクト情報
     */
    public E_FIELD_OBJECT LiftObject(Vector3Int myPosition, Vector3Int targetPos)
    {
        E_FIELD_OBJECT var = _aField[targetPos.x, targetPos.y, targetPos.z]._myObject;  // 変数の保持
        _aField[targetPos.x, targetPos.y, targetPos.z].Lifted(
                new Vector3Int(myPosition.x, myPosition.y + 1, myPosition.z));  // 持ち上げる処理
        return var;
    }


    /*
     * @brief 落下
     * @param1 移動先座標
     * @return フィールド外にでるなら true
     */
    public bool isFall(Vector3Int target)
    {
        for (int y = target.y; y >= 0; y--)
        {
            if (isUse(new Vector3Int(target.x, y, target.z)))
                return false;
        }
        return true;
    }


    /*
     * @brief 置けるかの判定
     * @param1 移動先座標
     * @return オブジェクトを置ける = true
     */
    public bool isPut(Vector3Int pos)
    {
        if (isLimitField(pos))
        {// フィールド外
            return true;
        }
        if (!isUse(pos) && !isUse(new Vector3Int(pos.x, pos.y - 1, pos.z)) && !isUse(new Vector3Int(pos.x, pos.y - 2, pos.z)))
        {// 1マス以上の落下
            return true;
        }
        if (isUse(new Vector3Int(pos.x, pos.y + 1, pos.z)) && isUse(new Vector3Int(pos.x, pos.y, pos.z)))
        {// 二段以上の積み上げ
            return true;
        }
        return false;
    }


    /*
     * @brief 移動可能かの探索
     * @param1 移動量(移動方向)
     * @param2 対象オブジェクト
     * @param3 配列の添え字番号
     * @return 移動出来るなら true
     */
    private bool isMoveObject(Vector3Int movement, BaseObject obj, int x, int y)
    {
        if ((movement.x > 0 && obj.transform.position.x == MAX_FIELD_OBJECT - x - MAX_FIELD_OBJECT * 0.5f) && obj._position.y == y)
        {// 右進行
            return true;
        }
        if ((movement.x < 0 && obj.transform.position.x + MAX_FIELD_OBJECT * 0.5f == x) && obj._position.y == y)
        {// 左進行
            return true;
        }
        if ((movement.z > 0 && obj.transform.position.z == MAX_FIELD_OBJECT - x - MAX_FIELD_OBJECT * 0.5f) && obj._position.y == y)
        {// 奥進行
            return true;
        }
        if ((movement.z < 0 && obj.transform.position.z + MAX_FIELD_OBJECT * 0.5f == x) && obj._position.y == y)
        {// 手前進行
            return true;
        }
        return false;
    }



    /*
     * @brief プレイヤーの移動制限
     * @param1 ターゲット座標
     * @return プレイヤーが動けなければtrue
     */
    public bool isDontMovePlayer(Vector3Int targetPos, Vector3Int pos)
    {
        // 二段以上積み上げている時
        if (isUse(new Vector3Int(pos.x, pos.y + 1, pos.z)) && isUse(new Vector3Int(pos.x, pos.y + 2, pos.z)))
        {
            return true;
        }
        // 何かに持ち上げられている時
        if (!isLimitField(new Vector3Int(pos.x, pos.y, pos.z)) && _aField[pos.x, pos.y, pos.z]._bLifted.Equals(true))
        {
            return true;
        }
        // 二段以上で登れない
        if (isUse(targetPos) && isUse(new Vector3Int(targetPos.x, targetPos.y + 1, targetPos.z)))
        {
            return true;
        }
        // 二段以上で降りれない
        if (!isUse(targetPos) && !isUse(new Vector3Int(targetPos.x, targetPos.y - 1, targetPos.z)) &&
            !isUse(new Vector3Int(targetPos.x, targetPos.y - 2, targetPos.z)))
        {
            return true;
        }
        return false;
    }


    /*
     * @brief 使用中かどうかの判定
     * @param1 フィールド座標
     * @return 指定のフィールド座標にオブジェクトがあれば true
     */
    public bool isUse(Vector3Int pos)
    {
        if (!isLimitField(pos) && _aField[pos.x, pos.y, pos.z] == null)
            return false;
        return true;
    }


    /*
     * @brief フィールド上でのオブジェクト同士の衝突を検出
     * @param1 目的座標
     * @return 当たってたら true を返す
     */
    public bool isCollisionToObject(Vector3Int targetPos)
    {
        if (!isLimitField(targetPos) && isUse(targetPos) &&
            _aField[targetPos.x, targetPos.y, targetPos.z]._myObject != E_FIELD_OBJECT.NONE)
        {
            return true;
        }
        return false;
    }


    /*
     * @brief フィールド上で当たったオブジェクトを検出
     * @param1 目的座標
     * @param2 衝突オブジェクト
     * @return 当たってたら true を返す
     */
    public bool isCollisionToObject(Vector3Int targetPos, E_FIELD_OBJECT obj)
    {
        if (!isUse(targetPos)) return false;
        return _aField[targetPos.x, targetPos.y, targetPos.z]._myObject.Equals(obj);
    }


    /*
     * @brief フィールド上で当たったオブジェクトを検出
     * @param1 目的座標
     * @return 当たってたら true を返す
     */
    public bool isGetoff(Vector3Int targetPos)
    {
        if (targetPos.y - MAX_FIELD_FALL - 1 >= 0 &&
            !isUse(new Vector3Int(targetPos.x, targetPos.y - 1, targetPos.z)) &&
            isUse(new Vector3Int(targetPos.x, targetPos.y - 2, targetPos.z)))
            return true;
        return false;
    }


    /*
     * @brief ステージフィールドの限界値
     * @param1 配列座標
     * @return ステージの限界値(外)に出る場合、true
     */
    public bool isLimitField(Vector3Int targetPos)
    {
        if (targetPos.x > MAX_FIELD_OBJECT || targetPos.x < 0 ||
            targetPos.y > MAX_FIELD_OBJECT || targetPos.y < 0 ||
            targetPos.z > MAX_FIELD_OBJECT || targetPos.z < 0)
            return true;
        return false;
    }


    /*
     * @brief ワールド座標の補正
     *        とりあえずの処理
     *        更新処理の最後に呼び出す
     * @param1 自身の
     * @return ワールド座標
     */
    public Vector3 offsetPos(Vector3Int pos)
    {
        return new Vector3(
            pos.x - MAX_FIELD_OBJECT * 0.5f,
            pos.y,
            pos.z - MAX_FIELD_OBJECT * 0.5f
            );
    }


    /*
     * @brief オブジェクトが置けるかの判定
     * @param1 目的座標
     * @return 置けないなら true
     */
    public bool DontPut(Vector3Int target)
    {
        if (isUse(new Vector3Int(target.x, target.y + 1, target.z)) && isUse(new Vector3Int(target.x, target.y, target.z)))
        {
            return true;
        }
        return false;
    }


    /*
     * @brief オブジェクトが置かれる座標の取得
     * @param1 最初の目的座標
     * @return オブジェクトが置かれる
     */
    public Vector3Int GetPutPos(Vector3Int target)
    {
        for (; target.y >= 0; target.y--)
        {
            if (isUse(target))
            {
                return new Vector3Int(target.x, target.y + 1, target.z);
            }
        }
        return new Vector3Int();
    }
    

    // なし
    // /*
    //  * @brief カメラの回転
    //  *        回転するのはカメラ
    //  *        プレイヤーの移動などの為にフィールドの角度を算出
    //  */
    // private void FieldRotate()
    // {
    //     // 左回転
    //     if (Input.GetKeyDown(KeyCode.Q))
    //     {
    //         if (_FieldAngle - VAL_FIELD_ROTATE < 0)
    //         {
    //             _FieldAngle += VAL_FIELD_ROTATE * (360 / VAL_FIELD_ROTATE - 1);
    //         }
    //         else
    //         {
    //             _FieldAngle -= VAL_FIELD_ROTATE;
    //         }
    //     }
    //     // 右回転
    //     if (Input.GetKeyDown(KeyCode.E))
    //     {
    //         if (_FieldAngle + VAL_FIELD_ROTATE > 360)
    //         {
    //             _FieldAngle -= VAL_FIELD_ROTATE * (360 / VAL_FIELD_ROTATE - 1);
    //         }
    //         else
    //         {
    //             _FieldAngle += VAL_FIELD_ROTATE;
    //         }
    //     }
    // }
    // 
    // /*
    //  * return マップを見る角度
    //  */
    // public int GetFieldAngle()
    // {
    //     return _FieldAngle;
    // }
}

// EOF