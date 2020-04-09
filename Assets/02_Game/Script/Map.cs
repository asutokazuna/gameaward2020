/*
 * @file	Map.cs
 * @brief   フィールドのマップ情報
 *
 * @author	Kota Nakagami
 * @date1	2020/04/06(月)
 *
 * @version	1.00
 */


 //#define MODE_MAP


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * @enum オブジェクト情報
 */
public enum E_FIELD_OBJECT
{
    NONE,                   // 無
    PLAYER_01,              // プレイヤー01
    BLOCK_NORMAL,           // 通常ブロック
    BLOCK_WATER_SOURCE,     // 水源ブロック
    BLOCK_TANK,             // 水槽
}


/*
 * @brief 一マスあたりの情報
 */
public struct SquareInfo {
    public E_FIELD_OBJECT  _myObject;  //!< マスが持つオブジェクト情報
    public int             _number;    //!< オブジェクトナンバー
    public bool            _isUpdate;  //!< 更新flag
}


/*
 * @class Map
 */
public class Map : MonoBehaviour
{
    // 定数定義
    [SerializeField] int MAX_FIELD_OBJECT   = 10;       // マップ1辺あたりに置けるオブジェクトの最大値
    [SerializeField] int VAL_FIELD_MOVE     = 1;        // 一マス当たりの移動値
    [SerializeField] int VAL_FALL           = 2;        // 落下死判定になるマス数

    //! 変数宣言
    public SquareInfo[,,]       _map;           //!< マップ情報
    public Player[]             _player;        //!< プレイヤーオブジェクト
    public BlockTank[]          _box;           //!< 箱オブジェクト
    public BlockNormal[]        _ground;        //!< 地面ブロック
    public WaterSourceBlock[]   _waterblock;    //!< 水源ブロック
    [SerializeField] int        _playerCnt;     //!< プレイヤーカウント
    [SerializeField] int        _boxCnt;        //!< 箱カウント
    [SerializeField] int        _groundCnt;     //!< 地面カウント
    [SerializeField] int        _waterblockCnt; //!< 水源カウント
    [SerializeField] Vector3Int _direct;        //!< 全プレイヤーが向いてる方向
    bool                        _start;         //!< 最初の一回だけ関数を呼ぶため(後で消す)
    [SerializeField] bool       _gameOver;      //!< ゲームオーバーフラグ


    /*
     * @brief Awake
     * @return なし
     */
    void Awake()
    {
        _map     = new SquareInfo[MAX_FIELD_OBJECT, MAX_FIELD_OBJECT, MAX_FIELD_OBJECT];
        _start      = true;
        _gameOver   = false;
    }


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
#if MODE_MAP
        if (_start)
        {
            InitObject();
            _start ^= _start;
        }
        MoveObject();
        HandAction();
#endif
    }


    /*
     * @brief プレイヤーの移動
     * @return なし
     */
    private void MoveObject()
    {
        // 移動キーを何も押してなかったら
        if (!Input.GetKeyDown(KeyCode.D) && !Input.GetKeyDown(KeyCode.A) &&
            !Input.GetKeyDown(KeyCode.W) && !Input.GetKeyDown(KeyCode.S))
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
        for (int n = 0; n < _playerCnt; n++)
        {// 取り合えずソートはなし
            //PlayerMove(_player[n], _direct);
            _player[n].Move(_direct);
            SetObject(_player[n], n);
            DeleteObject(_player[n]._oldPosition);
        }
        CallDebug();
    }


    /*
     * @brief 持ち上げる、又は話す
     * @return なし
     */
    public void HandAction()
    {
        if (!Input.GetKeyDown(KeyCode.Space)) return;

        for (int n = 0; n < _playerCnt; n++)
        {// 取り合えずソートはなし
            _player[n].HandAction();
        }
    }


    /*
     * @brief ゲームオーバー判定
     * @param1 移動先座標
     * @return ゲームオーバーなら true
     */
    public bool isGameOver(Vector3Int pos)
    {
        for (int y = pos.y; y >= 0 && y >= pos.y - VAL_FALL; y--)
        {// 2マス以上落下した場合
            if (isUse(new Vector3Int(pos.x, y, pos.z)))
                return false;
        }
        return true;
    }


    /*
     * @brief プレイヤーが移動できない判定
     * @param1 移動先
     * @return 移動できない時 true
     */
    public bool isDontMove(Vector3Int targetPos, Vector3Int oldPos)
    {
        // 二段以上積み上げている時
        if (isUse(new Vector3Int(oldPos.x, oldPos.y + 1, oldPos.z)) && isUse(new Vector3Int(oldPos.x, oldPos.y + 2, oldPos.z)))
        {
            return true;
        }
        // 二段以上で登れない
        if (isUse(targetPos) && isUse(new Vector3Int(targetPos.x, targetPos.y + 1, targetPos.z)))
        {
            return true;
        }
        return false;
    }


    /*
     * @brief 上に登れるか
     * @param1 目的座標
     * @return 登れるなら true を返す
     */
    public bool isGetup(Vector3Int targetPos)
    {
        if (_map[targetPos.x, targetPos.y, targetPos.z]._myObject == E_FIELD_OBJECT.NONE ||
            _map[targetPos.x, targetPos.y, targetPos.z]._myObject == E_FIELD_OBJECT.PLAYER_01)
        {// 登れるオブジェクトが無い場合
            return false;
        }
        return true;
    }
    

    /*
     * @brief 下に降りれるか
     * @param1 目的座標
     * @return 降りれるなら true を返す
     */
    public bool isGetoff(Vector3Int targetPos)
    {
        if (_map[targetPos.x, targetPos.y - 1, targetPos.z]._myObject == E_FIELD_OBJECT.NONE)
        {// 下に何もない
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
        if (!isLimitField(pos) && _map[pos.x, pos.y, pos.z]._myObject != E_FIELD_OBJECT.NONE)
            return true;
        return false;
    }


    /*
     * @brief ステージフィールドの限界値
     * @param1 配列座標
     * @return ステージの限界値(外)に出る場合、true
     */
    public bool isLimitField(Vector3Int pos)
    {
        if (pos.x >= MAX_FIELD_OBJECT || pos.x < 0 ||
            pos.y >= MAX_FIELD_OBJECT || pos.y < 0 ||
            pos.z >= MAX_FIELD_OBJECT || pos.z < 0)
            return true;
        return false;
    }


    /*
     * @brief オブジェクト情報のセット
     * @param1 BaseObject obj, int number
     * @return なし
     */
    private void SetObject(BaseObject obj, int number)
    {
        _map[obj._position.x, obj._position.y, obj._position.z]._myObject    = obj._myObject;
        _map[obj._position.x, obj._position.y, obj._position.z]._number      = number;
        _map[obj._position.x, obj._position.y, obj._position.z]._isUpdate    = false;
    }


    /*
     * @brief オブジェクト情報の破棄
     * @param1 座標
     */
    private void DeleteObject(Vector3Int pos)
    {
        _map[pos.x, pos.y, pos.z]._myObject   = E_FIELD_OBJECT.NONE;
        _map[pos.x, pos.y, pos.z]._number     = 0;
        _map[pos.x, pos.y, pos.z]._isUpdate   = false;
    }


    /*
     * @brief マップ情報と各オブジェクト情報の初期化
     * @return なし
     */
    private void InitObject()
    {
        InitPlayerObj();        // プレイヤーの初期化
        InitBlockTankObj();     // 箱の初期化
        InitGroundObj();        // 地面の初期化
        InitWaterblockObj();    // 水源ブロックの初期化
    }


    /*
     * @brief プレイヤー情報の初期化
     * @return なし
     */
    private void InitPlayerObj()
    {
        GameObject[] player = GameObject.FindGameObjectsWithTag("Player");
        _playerCnt = player.Length;
        _player = new Player[_playerCnt];
        for (int n = 0; n < _playerCnt; n++)
        {
            _player[n] = new Player();
            _player[n] = player[n].GetComponent<Player>();
            //_player[n].Init();    // ここで初期化をしたいです
            SetObject(_player[n], n);
        }
    }


    /*
     * @brief 箱情報の初期化
     * @return なし
     */
    private void InitBlockTankObj()
    {
        GameObject[] box = GameObject.FindGameObjectsWithTag("Box");
        _boxCnt = box.Length;
        _box = new BlockTank[_boxCnt];
        for (int n = 0; n < _boxCnt; n++)
        {
            _box[n] = new BlockTank();
            _box[n] = box[n].GetComponent<BlockTank>();
            SetObject(_box[n], n);
        }
    }


    /*
     * @brief 箱情報の初期化
     * @return なし
     */
    private void InitGroundObj()
    {
        GameObject[] ground = GameObject.FindGameObjectsWithTag("Ground");
        _groundCnt = ground.Length;
        _ground = new BlockNormal[_groundCnt];
        for (int n = 0; n < _groundCnt; n++)
        {
            _ground[n] = new BlockNormal();
            _ground[n] = ground[n].GetComponent<BlockNormal>();
            SetObject(_ground[n], n);
        }
    }


    /*
     * @brief 箱情報の初期化
     * @return なし
     */
    private void InitWaterblockObj()
    {
        GameObject[] waterblock = GameObject.FindGameObjectsWithTag("WaterBlock");
        _waterblockCnt = waterblock.Length;
        _waterblock = new WaterSourceBlock[_waterblockCnt];
        for (int n = 0; n < _waterblockCnt; n++)
        {
            _waterblock[n] = new WaterSourceBlock();
            _waterblock[n] = waterblock[n].GetComponent<WaterSourceBlock>();
            SetObject(_waterblock[n], n);
        }
    }


    /*
     * @brief デバッグ用関数
     * @return なし
     */
    private void CallDebug()
    {
        for (int y = 0; y < MAX_FIELD_OBJECT; y++)
        {
            for (int z = 0; z < MAX_FIELD_OBJECT; z++)
            {
                for (int x = 0; x < MAX_FIELD_OBJECT; x++)
                {
                    if (_map[x, y, z]._myObject.Equals(E_FIELD_OBJECT.PLAYER_01))
                    {
                        Debug.Log("座標 x =" + x + " y =" + y + " z =" + z + " にプレイヤーがいます");
                    }
                    if (_map[x, y, z]._myObject.Equals(E_FIELD_OBJECT.BLOCK_TANK))
                    {
                        Debug.Log("座標 x =" + x + " y =" + y + " z =" + z + " に水槽があります");
                    }
                }
            }
        }
    }


    /*
     * @brief ゲームオーバーフラグのセット
     * return ゲームオーバーなら true
     */
    public void SetGameOver()
    {
        _gameOver = true;
    }
}

// EOF