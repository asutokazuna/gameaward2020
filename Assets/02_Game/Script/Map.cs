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
    BLOCK_GROUND,           // 地面
    BLOCK_WATER_SOURCE,     // 水源ブロック
    BLOCK_TANK,             // 水槽
    MAX,                    // 最大値
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
#if MODE_MAP
    // 定数定義
    [SerializeField] int MAX_FIELD_OBJECT   = 10;       // マップ1辺あたりに置けるオブジェクトの最大値
    [SerializeField] int VAL_FIELD_MOVE     = 1;        // 一マス当たりの移動値
    [SerializeField] int VAL_FALL           = 2;        // 落下死判定になるマス数

    //! 変数宣言
    [NamedArrayAttribute(new string[] {
        "オブジェクトなし",
        "プレイヤー01",
        "通常ブロック",
        "地面ブロック",
        "水源ブロック",
        "水槽ブロック",
        })]  // オブジェクトが増えたら随時追加
    [SerializeField] string[] _objectTag = new string[(int)E_FIELD_OBJECT.MAX];
    public SquareInfo[,,]       _map;               //!< マップ情報
    public Player[]             _player;            //!< プレイヤーオブジェクト
    public BlockTank[]          _waterBlock;        //!< 水槽オブジェクト
    public BlockNormal[]        _ground;            //!< 地面ブロック
    public WaterSourceBlock[]   _waterSource;       //!< 水源ブロック
    [SerializeField] int        _playerCnt;         //!< プレイヤーカウント
    [SerializeField] int        _waterBlockCnt;     //!< 水槽カウント
    [SerializeField] int        _groundCnt;         //!< 地面カウント
    [SerializeField] int        _waterSourceCnt;    //!< 水源カウント
    [SerializeField] Vector3Int _direct;            //!< 全プレイヤーが向いてる方向
    public Vector3Int           _offsetPos;         //!< 配列座標補正用変数
    bool                        _start;             //!< 最初の一回だけ関数を呼ぶため(後で消す)
    [SerializeField] bool       _gameOver;          //!< ゲームオーバーフラグ


    /*
     * @brief Awake
     * @return なし
     */
    void Awake()
    {
        _map        = new SquareInfo[MAX_FIELD_OBJECT, MAX_FIELD_OBJECT, MAX_FIELD_OBJECT];
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
            UpdateMap(_player[n]);
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
     * @brief オブジェクトを持ち上げる
     * @param1 自身の座標
     * @param2 持ち上げるオブジェクトの座標
     * @return オブジェクト情報
     */
    public BaseObject LiftToObject(Vector3Int pos, Vector3Int target)
    {
        if (_map[target.x, target.y, target.z]._myObject == E_FIELD_OBJECT.BLOCK_TANK)
        {// これから持つオブジェクトが水槽だった場合
            _waterBlock[_map[target.x, target.y, target.z]._number].Lifted(new Vector3Int(pos.x, pos.y + 1, pos.z));

            SetObject(_waterBlock[_map[target.x, target.y, target.z]._number]);
            DeleteObject(_waterBlock[_map[target.x, target.y, target.z]._number]._oldPosition);

            return _waterBlock[_map[target.x, target.y, target.z]._number];
        }
        return null;
    }


    /*
     * @brief オブジェクトを置く
     * @param1 持っているオブジェクト情報
     * @param2 置かれる座標
     * @return なし
     */
    public void PutToObject(SquareInfo haveObj, Vector3Int targetPos)
    {
        if (haveObj._myObject == E_FIELD_OBJECT.BLOCK_TANK)
        {// 水槽の場合
            _waterBlock[haveObj._number].transform.parent = null;   // 親子関係を話す
            _waterBlock[haveObj._number].Puted(targetPos);
            UpdateMap(_waterBlock[haveObj._number]);
        }
    }


    /*
     * @brief プレイヤーへ追従
     * @param1 プレイヤーが所持しているオブジェクト情報
     * @param2 プレイヤーのオブジェクト座標
     * @return なし
     */
    public void Follow(SquareInfo haveObj, Vector3Int playerPos)
    {
        if (haveObj._myObject == E_FIELD_OBJECT.BLOCK_TANK)
        {// 水槽の場合
            _waterBlock[haveObj._number].Move(new Vector3Int(playerPos.x, playerPos.y + 1, playerPos.z));
            UpdateMap(_waterBlock[haveObj._number]);
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
     * @brief プレイヤーがオブジェクトを持てるかの判定
     * @param1 これから持つオブジェクト座標
     * @return オブジェクトを持てるなら true を返す
     */
    public bool isLift(Vector3Int pos)
    {
        if (_map[pos.x, pos.y, pos.z]._myObject == E_FIELD_OBJECT.BLOCK_TANK &&     // 水槽ブロックの場合
            !_waterBlock[_map[pos.x, pos.y, pos.z]._number]._lifted)                // 何かに持たれてない
        {
            return true;
        }
        return false;
    }


    /*
     * @brief プレイヤーがオブジェクトを置けるかの判定
     * @param1 これから置くオブジェクト座標
     * @return オブジェクトを置けるなら true を返す
     */
    public bool isPut(Vector3Int pos)
    {
        if (isLimitField(new Vector3Int(pos.x, pos.y - 1, pos.z)))
        {// ここのif分入ったら多分ゲームオーバー
            return false;
        }
        if (_map[pos.x, pos.y, pos.z]._myObject == E_FIELD_OBJECT.NONE &&
            isRideon(new Vector3Int(pos.x, pos.y - 1, pos.z)))
        {// 一個下に置ける
            return true;
        }
        return false;
    }


    /*
     * @brief オブジェクトの上判定
     * @param1 座標
     * @return 上に行けるなら true を返す
     * @details 与えられた座標にオブジェクトがあり、
     *          かつその上に乗せれるかの判定
     */
    private bool isRideon(Vector3Int pos)
    {
        if (_map[pos.x, pos.y, pos.z]._myObject == E_FIELD_OBJECT.NONE      ||
            _map[pos.x, pos.y, pos.z]._myObject == E_FIELD_OBJECT.MAX       ||
            _map[pos.x, pos.y, pos.z]._myObject == E_FIELD_OBJECT.PLAYER_01
            )
        {
            return false;
        }
        return true;
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
     * @brief マップの更新
     * @param1 BaseObject obj
     * return なし
     */
    public void UpdateMap(BaseObject obj)
    {
        SetObject(obj);
        DeleteObject(obj._oldPosition);
    }


    /*
     * @brief オブジェクト情報のセット
     * @param1 BaseObject obj, int number
     * @return なし
     */
    private void SetObject(BaseObject obj)
    {
        _map[obj._position.x, obj._position.y, obj._position.z]._myObject    = obj._myObject;
        _map[obj._position.x, obj._position.y, obj._position.z]._number      = obj._myNumber;
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
        SetOffsetPos();         // 配列座標補正値の初期化
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
        GameObject[] player = GameObject.FindGameObjectsWithTag(_objectTag[(int)E_FIELD_OBJECT.PLAYER_01]);
        _playerCnt          = player.Length;
        _player             = new Player[_playerCnt];
        for (int n = 0; n < _playerCnt; n++)
        {
            _player[n] = new Player();
            _player[n] = player[n].GetComponent<Player>();
            _player[n].Init(n);         // プレイヤーコンポーネントの初期化
            SetObject(_player[n]);      // マップ情報にプレイヤー情報をセット
        }
    }


    /*
     * @brief 箱情報の初期化
     * @return なし
     */
    private void InitBlockTankObj()
    {
        GameObject[] box = GameObject.FindGameObjectsWithTag(_objectTag[(int)E_FIELD_OBJECT.BLOCK_TANK]);
        _waterBlockCnt = box.Length;
        _waterBlock = new BlockTank[_waterBlockCnt];
        for (int n = 0; n < _waterBlockCnt; n++)
        {
            _waterBlock[n] = new BlockTank();
            _waterBlock[n] = box[n].GetComponent<BlockTank>();
            SetObject(_waterBlock[n]);
        }
    }


    /*
     * @brief 地面情報の初期化
     * @return なし
     */
    private void InitGroundObj()
    {
        GameObject[] ground = GameObject.FindGameObjectsWithTag(_objectTag[(int)E_FIELD_OBJECT.BLOCK_GROUND]);
        _groundCnt = ground.Length;
        _ground = new BlockNormal[_groundCnt];
        for (int n = 0; n < _groundCnt; n++)
        {
            _ground[n] = new BlockNormal();
            _ground[n] = ground[n].GetComponent<BlockNormal>();
            SetObject(_ground[n]);
        }
    }


    /*
     * @brief 水源情報の初期化
     * @return なし
     */
    private void InitWaterblockObj()
    {
        GameObject[] waterblock = GameObject.FindGameObjectsWithTag(_objectTag[(int)E_FIELD_OBJECT.BLOCK_WATER_SOURCE]);
        _waterSourceCnt = waterblock.Length;
        _waterSource = new WaterSourceBlock[_waterSourceCnt];
        for (int n = 0; n < _waterSourceCnt; n++)
        {
            _waterSource[n] = new WaterSourceBlock();
            _waterSource[n] = waterblock[n].GetComponent<WaterSourceBlock>();
            SetObject(_waterSource[n]);
        }
    }


    private void SetOffsetPos()
    {
        // 取り合えずの処理
        // 一番左下のオブジェクト取得で補正したい
        _offsetPos = new Vector3Int(-5, -1, -5);
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
#endif
}

// EOF