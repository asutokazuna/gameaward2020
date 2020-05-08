/**
 * @file	Map.cs
 * @brief   フィールドのマップ情報
 *
 * @author	Kota Nakagami
 * @date	2020/04/06(月)   クラスの作成
 * @data    2020/04/15(水)   プレイヤーの操作ソート
 * @data    2020/04/16(木)   警告文の解決
 * @data    2020/04/18(土)   カメラの向きに合わせたプレイヤーのソート
 * @date	2020/04/21(火)   クリア処理の追加    加藤
 *
 * @version	1.00
 */


#define MODE_MAP


using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/**
 * @enum オブジェクト情報
 */
public enum E_OBJECT
{
    NONE,                   // 無
    PLAYER_01,              // プレイヤー01
    BLOCK_NORMAL,           // 通常ブロック
    BLOCK_GROUND,           // 地面ブロック
    BLOCK_WATER_SOURCE,     // 水源ブロック
    BLOCK_TANK,             // 水槽ブロック
    BLOCK_TRAMPLINE,        // トランポリンブロック
    MAX,                    // 最大値
}


/**
 * @enum 操作受付処理
 */
public enum E_TURN
{
    WAIT,   // 操作受付
    MOVE,   // 操作実行中
    END,    // 終わり
}



/**
 * @brief 一マスあたりの情報
 */
public struct SquareInfo {
    public E_OBJECT         _myObject;  //!< マスが持つオブジェクト情報
    public int              _number;    //!< オブジェクトナンバー
}


/**
 * @class Map
 */
public class Map : MonoBehaviour
{
#if MODE_MAP
    // 定数定義
    [SerializeField] int MAX_OBJECT         = 10;       // マップ1辺あたりに置けるオブジェクトの最大値
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
        "跳ねブロック",
        })]  // オブジェクトが増えたら随時追加
    [SerializeField] string[] _objectTag = new string[(int)E_OBJECT.MAX];

    public SquareInfo[,,]           _map            = default;      //!< マップ情報
    public Player[]                 _player         = default;      //!< プレイヤー
    public Ground[]                 _ground         = default;      //!< 地面
    public WaterSourceBlock[]       _waterSource    = default;      //!< 水源
    public BlockTank[]              _tankBlock      = default;      //!< 水槽
    public TramplineBlock[]         _trampoline     = default;      //!< トランポリン

    private int _playerCnt      = 0;    //!< プレイヤーの総数
    private int _tankBlockCnt   = 0;    //!< 水槽の総数

    [SerializeField] Vector3Int     _direct;            //!< 全プレイヤーが向いてる方向
    public Vector3Int               _offsetPos;         //!< 配列座標補正用変数
    Controller                      _input;             //!< 入力

    public bool                 _gameOver { get; set; } //!< ゲームオーバーフラグ
    public bool                 _gameClear { get; set; }//!< ゲームクリアフラグ
    public int                  _fullWaterBlockCnt;
    public E_TURN               _turn;                  //!< ターン制

    private AudioSource _audioSource;
    public AudioClip _SEGameclear;
    public AudioClip _SEGameover;
    public AudioClip _SEBoxBreak;


    /**
     * @brief Awake
     * @return なし
     */
    void Awake()
    {
        _map        = new SquareInfo[MAX_OBJECT, MAX_OBJECT, MAX_OBJECT];
        _gameOver   = false;
        _gameClear  = false;
        _fullWaterBlockCnt = 0; 
        //↑とりあえずここにいれたけど、他にいい場所あったら移動させてください。 4/21 加藤
        //_input      = GameObject.FindGameObjectWithTag("Input").GetComponent<Controller>();               // コンポーネントの取得
    }


    // Start is called before the first frame update
    void Start()
    {
        // マップとオブジェクト情報の初期化
        InitObject();
        _direct = _player[0]._direct;
        PlayerSort();   // ソートと更新
        _turn = E_TURN.WAIT;
        if (_input == null)
            _input = GameObject.FindGameObjectWithTag("Input").GetComponent<Controller>();
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
#if MODE_MAP
        if(isGameClear())
        {
            _gameClear = true;
        }
        if (_gameOver && _turn != E_TURN.END)
        {// 取り合えずここでゲームオーバーの実装
            _turn = E_TURN.END;
            _audioSource.PlayOneShot(_SEGameover);
            foreach (GameObject n in GetGameOverObjects())
            {// ゲームオーバーオブジェクトの確認
                Debug.Log(n.name);
            }
        }
        if (_gameClear && _turn != E_TURN.END)
        {// ゲームクリア時の処理を追加する場所
            _turn = E_TURN.END;
            _audioSource.PlayOneShot(_SEGameclear);
            foreach (Player obj in _player)
            {
                obj.GameClear();
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            CallDebug(E_OBJECT.PLAYER_01);
        }
        if (GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MainCamera>()._startMove)
        {// カメラ移動中やで動くな(スタート時だけ)
            return;
        }
        foreach (Player obj in _player)
        {
            if (obj._isMove || !obj.isAnim())
            {// まだ移動中のプレイヤーがいれば、操作を受け付けない
                return;
            }
        }
        foreach (Player obj in _player)
        {
            if (obj._putUpdate)
            {// まだ移動中のプレイヤーがいれば、操作を受け付けない
                HandAction(true);
            }
        }
        if (_turn == E_TURN.MOVE)
        {
            _turn = E_TURN.WAIT;
            return;
        }

        if (_turn == E_TURN.WAIT)
        {
            MoveObject();
            HandAction();
            RotateObject();
        }
#endif
    }


    /**
     * @brief プレイヤーの移動
     * @return なし
     */
    private void MoveObject()
    {
        // 移動キーを何も押してなかったら
        if (!_input.isInput(E_INPUT_MODE.TRIGGER, E_INPUT.L_STICK_RIGHT) &&
            !_input.isInput(E_INPUT_MODE.TRIGGER, E_INPUT.L_STICK_LEFT) &&
            !_input.isInput(E_INPUT_MODE.TRIGGER, E_INPUT.L_STICK_UP) &&
            !_input.isInput(E_INPUT_MODE.TRIGGER, E_INPUT.L_STICK_DOWN) ||
            _input.isInput(E_INPUT_MODE.BUTTON, E_INPUT.LB))
        {
            return;
        }
        _direct = new Vector3Int();

        offsetDirect();
        PlayerSort();   // ソートと更新
        foreach (Player obj in _player)
        {
            obj.Move();
            UpdateMap(obj);
        }
        _turn = E_TURN.MOVE;
    }

    

    /**
     * @brief プレイヤーの回転
     * @return なし
     */
    private void RotateObject()
    {
        if (!_input.isInput(E_INPUT_MODE.BUTTON, E_INPUT.LB)) return;

        foreach (Player obj in _player)
        {
            obj.GetComponent<Player>().Rotate();
        }
        offsetDirect();
        PlayerSort();   // ソートと更新
        _turn = E_TURN.MOVE;
    }


    /**
     * @brief 持ち上げる、又は話す
     * @return なし
     */
    public void HandAction(bool flag = false)
    {
        if (!_input.isInput(E_INPUT_MODE.TRIGGER, E_INPUT.A) && !flag) return;
        foreach (Player obj in _player)
        {
            if (obj.isCenter())
            {
                obj._putUpdate = true;
            }
        }
        foreach (Player obj in _player)
        {
            obj.HandAction(flag);
        }
        foreach (Player obj in _player)
        {
            UpdateMap(obj);
        }

        _turn = E_TURN.MOVE;
    }


    #region MapLift

    /**
     * @brief プレイヤーがオブジェクトを持てるかの判定
     * @param1 これから持つオブジェクト座標
     * @return オブジェクトを持てるなら オブジェクト情報 を返す
     */
    public SquareInfo isLift(Vector3Int pos)
    {
        if (_map[pos.x, pos.y, pos.z]._myObject == E_OBJECT.BLOCK_TANK &&                           // 水槽ブロックの場合
            pos.y + 1 < MAX_OBJECT && _map[pos.x, pos.y + 1, pos.z]._myObject == E_OBJECT.NONE &&   // 上に何も積まれてない
            _tankBlock[_map[pos.x, pos.y, pos.z]._number]._lifted != E_HANDS_ACTION.DO)             // 何かに持たれてない
        {
            return _map[pos.x, pos.y, pos.z];
        }
        if (_map[pos.x, pos.y, pos.z]._myObject == E_OBJECT.PLAYER_01)
        {// プレイヤーの場合
            if (pos.y + 1 < MAX_OBJECT && _map[pos.x, pos.y + 1, pos.z]._myObject == E_OBJECT.NONE && // 上に何も積まれてない
                _player[_map[pos.x, pos.y, pos.z]._number]._lifted != E_HANDS_ACTION.DO
                )
            {
                return _map[pos.x, pos.y, pos.z];
            }
        }
        return new SquareInfo();
    }


    /**
     * @brief 持ち上げるオブジェクトの取得
     * @param 持ち上げるオブジェクト情報
     * @return GameObject型で返す
     */
    public GameObject GetLiftObject(SquareInfo obj)
    {
        if (obj._myObject == E_OBJECT.BLOCK_TANK)
        {
            return _tankBlock[obj._number].gameObject;
        }
        if (obj._myObject == E_OBJECT.PLAYER_01)
        {
            return _player[obj._number].gameObject;
        }
        return null;
    }


    /**
     * @brief オブジェクトを持ち上げる
     * @param1 持ち上げるオブジェクト情報
     * @param2 目的座標
     * @return なし
     */
    public void LiftToObject(SquareInfo obj, Vector3Int pos)
    {
        if (obj._myObject == E_OBJECT.BLOCK_TANK)
        {// これから持つオブジェクトが水槽だった場合
            _tankBlock[obj._number].Lifted(pos);
            DeleteObject(_tankBlock[obj._number]._oldPosition);
        }
        if (obj._myObject == E_OBJECT.PLAYER_01)
        {// これから持つオブジェクトがプレイヤーだった場合
            _player[obj._number].Lifted(pos);
            DeleteObject(_player[obj._number]._oldPosition);
        }
    }

    public void Poop(SquareInfo obj, Vector3Int pos)
    {// 糞関数、絶対後で直す
        if (obj._myObject == E_OBJECT.BLOCK_TANK)
        {
            _tankBlock[obj._number]._position = pos;
            DeleteObject(_tankBlock[obj._number]._oldPosition);
        }
        if (obj._myObject == E_OBJECT.PLAYER_01)
        {
            _player[obj._number]._position = pos;
            DeleteObject(_player[obj._number]._oldPosition);
        }
    }

    #endregion


    /**
     * @brief オブジェクトを置く
     * @param1 持っているオブジェクト情報
     * @param2 置かれる座標
     * @return なし
     */
    public void PutToObject(SquareInfo haveObj, Vector3Int targetPos)
    {
        if (haveObj._myObject == E_OBJECT.BLOCK_TANK)
        {// 水槽の場合
            _tankBlock[haveObj._number].transform.parent = null;   // 親子関係を話す
            _tankBlock[haveObj._number].Puted(targetPos);
            UpdateMap(_tankBlock[haveObj._number]);
        }
        else if (haveObj._myObject == E_OBJECT.PLAYER_01)
        {// プレイヤーの場合
            _player[haveObj._number].transform.parent = null;   // 親子関係を話す
            _player[haveObj._number].Puted(targetPos);
            UpdateMap(_player[haveObj._number]);
            Debug.Log(_player[haveObj._number].name + " が降ろされた");
        }
    }


    /**
     * @brief オブジェクトを落とす
     * @param1 持っているオブジェクト情報
     * @param2 落下座標
     * @return なし
     */
    public void FallToObject(SquareInfo haveObj, Vector3Int targetPos)
    {
        if (haveObj._myObject == E_OBJECT.BLOCK_TANK)
        {// 水槽の場合
            _tankBlock[haveObj._number].transform.parent = null;   // 親子関係を話す
            _tankBlock[haveObj._number].Fall(targetPos);
            _audioSource.PlayOneShot(_SEBoxBreak);//SE
            UpdateMap(_tankBlock[haveObj._number]);
        }
        else if (haveObj._myObject == E_OBJECT.PLAYER_01)
        {// プレイヤーの場合
            _player[haveObj._number].transform.parent = null;   // 親子関係を話す
            _player[haveObj._number].Fall(targetPos);
            UpdateMap(_player[haveObj._number]);
        }
    }


    /**
     * @brief プレイヤーへ追従
     * @param1 プレイヤーが所持しているオブジェクト情報
     * @param2 プレイヤーのオブジェクト座標
     * @return なし
     */
    public void Follow(SquareInfo haveObj, Vector3Int playerPos, Vector3Int direct)
    {
        if (haveObj._myObject == E_OBJECT.BLOCK_TANK)
        {// 水槽の場合
            _tankBlock[haveObj._number].Follow(new Vector3Int(playerPos.x, playerPos.y + 1, playerPos.z), direct);
            UpdateMap(_tankBlock[haveObj._number]);
        }
        else if (haveObj._myObject == E_OBJECT.PLAYER_01)
        {// 水槽の場合
            _player[haveObj._number].Follow(new Vector3Int(playerPos.x, playerPos.y + 1, playerPos.z), direct);
            UpdateMap(_player[haveObj._number]);
        }
    }


    /**
     * @brief 落下ゲームオーバー時のプレイヤーが落ちるまでの座標
     * @param1 座標
     * @return 落下地点
     */
    public Vector3Int GetFallPos(Vector3Int pos)
    {
        for (; pos.y > 0; pos.y--)
        {
            if (isUse(pos))
            {// 落下地点の一個上
                return new Vector3Int(pos.x, pos.y + 1, pos.z);
            }
        }
        return pos;    // エリア外への落下
    }


    /**
     * @brief ゲームオーバー判定
     * @param1 移動先座標
     * @param2 プレイヤーの状態
     * @return ゲームオーバーなら true
     */
    public bool isGameOver(Vector3Int pos, E_OBJECT_MODE mode)
    {

        if (mode == E_OBJECT_MODE.MOVE)
        {// 移動時
            for (int n = 0; n <= MAX_OBJECT; n++, pos.y--)
            {// 2マス以上落下した場合
                if (isTrampline(pos))
                {
                    return false;
                }
                if (isUse(pos) && n <= VAL_FALL)
                {
                    return false;
                }
            }
        }
        if (mode == E_OBJECT_MODE.PUT)
        {// 物を置くとき
            for (int n = 0; n <= VAL_FALL + 1; n++, pos.y--)
            {// 2マス以上落下した場合
                if (isUse(pos))
                    return false;
            }
        }
        // ゲームオーバー
        return true;
    }


    /**
     * @brief エリア外判定
     * @param1 プレイヤーの座標
     * @return エリア外なら true
     */
    public bool isOutsideTheArea(Vector3Int pos)
    {
        for (; pos.y >= 0; pos.y--)
        {
            if (isUse(pos))
            {
                return false;   // マップへの落下
            }
        }
        return true;    // エリア外
    }


    /**
     * @brief ゲームクリア判定
     * @return ゲームクリアなら true
     */
    private bool isGameClear()
    {
        if(_tankBlockCnt == _fullWaterBlockCnt)
        {//満水の箱数が、箱の総数と同じだったら
            return true;
        }
        return false;
    }


    /**
     * @brief プレイヤーが移動できない判定
     * @param1 移動先
     * @return 移動できない時 true
     */
    public bool isDontMove(Vector3Int targetPos, Vector3Int oldPos)
    {
        if (isUse(new Vector3Int(oldPos.x, oldPos.y + 1, oldPos.z)) && isUse(new Vector3Int(oldPos.x, oldPos.y + 2, oldPos.z)))
        {// 二段以上積み上げている時(取り合えず)
            return true;
        }
        if (isUse(targetPos) && isUse(new Vector3Int(targetPos.x, targetPos.y + 1, targetPos.z)))
        {// 二段以上で登れない
            return true;
        }
        if (_map[targetPos.x, targetPos.y, targetPos.z]._myObject == E_OBJECT.PLAYER_01 ||
            (targetPos.y - 1 >= 0 && _map[targetPos.x, targetPos.y - 1, targetPos.z]._myObject == E_OBJECT.PLAYER_01))
        {// 移動先にプレイヤーがいる場合
            return true;
        }
        return false;
    }


    /**
     * @brief 上に登れるか
     * @param1 目的座標
     * @return 登れるなら true を返す
     */
    public bool isGetup(Vector3Int targetPos)
    {
        if (_map[targetPos.x, targetPos.y, targetPos.z]._myObject == E_OBJECT.NONE ||
            _map[targetPos.x, targetPos.y, targetPos.z]._myObject == E_OBJECT.PLAYER_01)
        {// 登れるオブジェクトが無い場合
            return false;
        }
        
        return true;
    }
    

    /**
     * @brief 下に降りれるか
     * @param1 目的座標
     * @return 降りれるなら true を返す
     */
    public bool isGetoff(Vector3Int targetPos)
    {
        if (_map[targetPos.x, targetPos.y - 1, targetPos.z]._myObject == E_OBJECT.NONE &&
            _map[targetPos.x, targetPos.y - 1, targetPos.z]._myObject != E_OBJECT.PLAYER_01)
        {// 下に何もない
            return true;
        }
        return false;
    }


    /**
     * @brief トランポリン処理
     * @param 移動先座標
     * @return 移動先がトランポリンなら true
     */
    public bool isTrampline(Vector3Int targetPos)
    {
        if (isUse(targetPos) && _map[targetPos.x, targetPos.y, targetPos.z]._myObject == E_OBJECT.BLOCK_TRAMPLINE)
        {
            return true;
        }

        return false;
    }


    /**
     * @brief トランポリンからの移動先座標の取得
     * @param1 プレイヤーの座標
     * @param2 ベクトル
     * @return 移動先座標
     */
    public Vector3Int GetTramplinepPos(Vector3Int playerPos,Vector3Int vec = new Vector3Int())
    {
        Vector3Int pos = new Vector3Int(playerPos.x + vec.x, playerPos.y + vec.y, playerPos.z + vec.x );
        for(int n = MAX_OBJECT - 1;n >= 0;n--)
        {
            if (isUse(new Vector3Int(pos.x, n, pos.z)))
            {
                return new Vector3Int(pos.x, n + 1, pos.z);
            }
        }
        return new Vector3Int();
    }


    /**
     * @brief 降りる先の座標取得
     * @param 探索開始座標
     * return 移動先座標
     */
    public Vector3Int GetoffPos(Vector3Int pos)
    {
        for (int n = MAX_OBJECT - 1; n >= 0; n-- ,pos.y--)
        {
            if (isUse(new Vector3Int(pos.x, pos.y, pos.z)))
            {
                return new Vector3Int(pos.x, pos.y + 1, pos.z);
            }
        }
        return new Vector3Int();
    }


    /**
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
        if (_map[pos.x, pos.y, pos.z]._myObject == E_OBJECT.NONE &&
            isRideon(new Vector3Int(pos.x, pos.y - 1, pos.z)))
        {// 一個下に置ける
            return true;
        }
        return false;
    }


    /**
     * @brief オブジェクトの上判定
     * @param1 座標
     * @return 上に行けるなら true を返す
     * @details 与えられた座標にオブジェクトがあり、
     *          かつその上に乗せれるかの判定
     */
    private bool isRideon(Vector3Int pos)
    {
        if (_map[pos.x, pos.y, pos.z]._myObject == E_OBJECT.NONE      ||
            _map[pos.x, pos.y, pos.z]._myObject == E_OBJECT.MAX       ||
            _map[pos.x, pos.y, pos.z]._myObject == E_OBJECT.PLAYER_01
            )
        {
            return false;
        }
        return true;
    }


    /**
     * @brief 使用中かどうかの判定
     * @param1 フィールド座標
     * @return 指定のフィールド座標にオブジェクトがあれば true
     */
    public bool isUse(Vector3Int pos)
    {
        if (!isLimitField(pos) && _map[pos.x, pos.y, pos.z]._myObject != E_OBJECT.NONE)
            return true;
        return false;
    }


    /**
     * @brief ステージフィールドの限界値
     * @param1 配列座標
     * @return ステージの限界値(外)に出る場合、true
     */
    public bool isLimitField(Vector3Int pos)
    {
        if (pos.x >= MAX_OBJECT || pos.x < 0 ||
            pos.y >= MAX_OBJECT || pos.y < 0 ||
            pos.z >= MAX_OBJECT || pos.z < 0)
            return true;
        return false;
    }


    /**
     * @brief マップの更新
     * @param1 BaseObject obj
     * return なし
     */
    public void UpdateMap(BaseObject obj)
    {
        DeleteObject(obj._oldPosition);
        SetObject(obj);
    }


    /**
     * @brief オブジェクト情報のセット
     * @param1 BaseObject obj, int number
     * @return なし
     */
    private void SetObject(BaseObject obj)
    {
        _map[obj._position.x, obj._position.y, obj._position.z]._myObject    = obj._myObject;
        _map[obj._position.x, obj._position.y, obj._position.z]._number      = obj._myNumber;
    }


    /**
     * @brief オブジェクト情報の破棄
     * @param1 座標
     */
    private void DeleteObject(Vector3Int pos)
    {
        _map[pos.x, pos.y, pos.z]._myObject   = E_OBJECT.NONE;
        _map[pos.x, pos.y, pos.z]._number     = 0;
    }


    /**
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
        InitTramplineblockObj();// トランポリンの初期化
    }


    /**
     * @brief プレイヤー情報の初期化
     * @return なし
     */
    private void InitPlayerObj()
    {
        GameObject[] player = GameObject.FindGameObjectsWithTag(_objectTag[(int)E_OBJECT.PLAYER_01]);
        _playerCnt = player.Length;
        _player = new Player[_playerCnt];
        for (int n = 0; n < _playerCnt; n++)
        {
            _player[n] = player[n].GetComponent<Player>();
            _player[n].Init(n);         // プレイヤーコンポーネントの初期化
            SetObject(_player[n]);      // マップ情報にプレイヤー情報をセット
        }
    }


    /**
     * @brief 箱情報の初期化
     * @return なし
     */
    private void InitBlockTankObj()
    {
        GameObject[] box = GameObject.FindGameObjectsWithTag(_objectTag[(int)E_OBJECT.BLOCK_TANK]);
        _tankBlockCnt = box.Length;
        _tankBlock = new BlockTank[_tankBlockCnt];
        for (int n = 0; n < _tankBlockCnt; n++)
        {
            _tankBlock[n] = box[n].GetComponent<BlockTank>();
            _tankBlock[n].Init(n);
            SetObject(_tankBlock[n]);
        }
    }


    /**
     * @brief 地面情報の初期化
     * @return なし
     */
    private void InitGroundObj()
    {
        GameObject[] ground = GameObject.FindGameObjectsWithTag(_objectTag[(int)E_OBJECT.BLOCK_GROUND]);
        _ground = new Ground[ground.Length];
        for (int n = 0; n < ground.Length; n++)
        {
            _ground[n] = ground[n].GetComponent<Ground>();
            _ground[n].Init(n);
            SetObject(_ground[n]);
        }
    }


    /**
     * @brief 水源情報の初期化
     * @return なし
     */
    private void InitWaterblockObj()
    {
        GameObject[] waterblock = GameObject.FindGameObjectsWithTag(_objectTag[(int)E_OBJECT.BLOCK_WATER_SOURCE]);
        _waterSource = new WaterSourceBlock[waterblock.Length];
        for (int n = 0; n < waterblock.Length; n++)
        {
            _waterSource[n] = waterblock[n].GetComponent<WaterSourceBlock>();
            _waterSource[n].Init(n);
            SetObject(_waterSource[n]);
        }
    }
    
    
    /**
     * @brief トランポリンの初期化
     * @return なし
     */
    private void InitTramplineblockObj()
    {
        GameObject[] tramplineblock = GameObject.FindGameObjectsWithTag(_objectTag[(int)E_OBJECT.BLOCK_TRAMPLINE]);
        _trampoline = new TramplineBlock[tramplineblock.Length];
        for (int n = 0; n < tramplineblock.Length; n++)
        {
            _trampoline[n] = tramplineblock[n].GetComponent<TramplineBlock>();
            _trampoline[n].Init(n);
            SetObject(_trampoline[n]);
        }
    }


    /**
     * @brief ゲームオーバーの要因となったオブジェクトの取得
     * @return GameObject[]、なければnullを返す
     */
    public List<GameObject> GetGameOverObjects()
    {
        if (!_gameOver)
        {// まだゲームオーバーじゃあらへんで
            return null;
        }
        var obj = new List<GameObject>();
        for (int y = 0; y < MAX_OBJECT; y++)
        {
            for (int z = 0; z < MAX_OBJECT; z++)
            {
                for (int x = 0; x < MAX_OBJECT; x++)
                {
                    if (_map[x, y, z]._myObject.Equals(E_OBJECT.PLAYER_01) &&
                        _player[_map[x, y, z]._number]._gameOver)
                    {
                        obj.Add(_player[_map[x, y, z]._number].gameObject);
                    }
                    if (_map[x, y, z]._myObject.Equals(E_OBJECT.BLOCK_TANK) &&
                        _tankBlock[_map[x, y, z]._number]._gameOver)
                    {
                        obj.Add(_tankBlock[_map[x, y, z]._number].gameObject);
                    }
                    if (_map[x, y, z]._myObject.Equals(E_OBJECT.BLOCK_GROUND) &&
                        _ground[_map[x, y, z]._number]._gameOver)
                    {
                        obj.Add(_ground[_map[x, y, z]._number].gameObject);
                    }
                }
            }
        }
        return obj;
    }


    /**
     * @brief プレイヤーのソート
     * @return なし
     */
    private void PlayerSort()
    {
        if (_playerCnt <= 1)
        {// プレイヤーが一体しかいない場合
            return;
        }
        Player work;
        for (int i = _playerCnt - 1; i > 0; i--)
        {
            for (int j = 0; j < i; j++)
            {
                if (isSort(_player[j], _player[j + 1]))
                {
                    work            = _player[j];
                    _player[j]      = _player[j + 1];
                    _player[j + 1]  = work;
                }
            }
        }
        for (int n = 0; n < _playerCnt; n++)
        {// 識別番号の更新
            _player[n]._myNumber = n;
            UpdateMap(_player[n]);
        }
        for (int n = 0; n < _playerCnt; n++)
        {
            if (_player[n]._haveObject._myObject == E_OBJECT.PLAYER_01)
            {// 持っているオブジェクトの更新
                _player[n]._haveObject = _map[_player[n]._position.x, _player[n]._position.y + 1, _player[n]._position.z];
            }
        }
    }



    /**
     * @brief 入れ替えができるかの判定
     * @return 入れ替えが行われるなら true
     */
    private bool isSort(Player i, Player j)
    {
        if (_direct.x > 0 &&
            (i._position.x < j._position.x))
        {// 右方
            return true;
        }
        else if (_direct.x < 0 &&
            (i._position.x > j._position.x))
        {// 左方
            return true;
        }
        else if (_direct.z > 0 &&
            (i._position.z < j._position.z))
        {// 前方
            return true;
        }
        else if (_direct.z < 0 &&
            (i._position.z > j._position.z))
        {// 後方
            return true;
        }
        return false;
    }


    /**
     * @brief 方向の修正
     * @return なし
     */
    private void offsetDirect()
    {
        float y = GameObject.FindGameObjectWithTag("MainCamera").transform.localEulerAngles.y;

        if (_input.isInput(E_INPUT_MODE.TRIGGER, E_INPUT.L_STICK_RIGHT))
        {// 右方
            if (y > -30 && y < 30 || y > 330 && y < 390)    _direct = new Vector3Int(VAL_FIELD_MOVE, 0,  0);
            else if (y > 240 && y < 300)                    _direct = new Vector3Int(  0, 0, VAL_FIELD_MOVE);
            else if (y > 150 && y < 210)                    _direct = new Vector3Int(-VAL_FIELD_MOVE, 0,  0);
            else if (y > 60 && y < 120)                     _direct = new Vector3Int(  0, 0, -VAL_FIELD_MOVE);
        }
        else if (_input.isInput(E_INPUT_MODE.TRIGGER, E_INPUT.L_STICK_LEFT))
        {// 左方
            if (y > -30 && y < 30 || y > 330 && y < 390)    _direct = new Vector3Int(-VAL_FIELD_MOVE, 0,  0);
            else if (y > 240 && y < 300)                    _direct = new Vector3Int(  0, 0, -VAL_FIELD_MOVE);
            else if (y > 150 && y < 210)                    _direct = new Vector3Int(VAL_FIELD_MOVE, 0,  0);
            else if (y > 60 && y < 120)                     _direct = new Vector3Int(  0, 0, VAL_FIELD_MOVE);
        }
        else if (_input.isInput(E_INPUT_MODE.TRIGGER, E_INPUT.L_STICK_UP))
        {// 前方
            if (y > -30 && y < 30 || y > 330 && y < 390)    _direct = new Vector3Int(  0, 0, VAL_FIELD_MOVE);
            else if (y > 240 && y < 300)                    _direct = new Vector3Int( -VAL_FIELD_MOVE, 0,  0);
            else if (y > 150 && y < 210)                    _direct = new Vector3Int(  0, 0, -VAL_FIELD_MOVE);
            else if (y > 60 && y < 120)                     _direct = new Vector3Int(VAL_FIELD_MOVE, 0,  0);
        }
        else if (_input.isInput(E_INPUT_MODE.TRIGGER, E_INPUT.L_STICK_DOWN))
        {// 後方
            if (y > -30 && y < 30 || y > 330 && y < 390)    _direct = new Vector3Int(  0, 0, -VAL_FIELD_MOVE);
            else if (y > 240 && y < 300)                    _direct = new Vector3Int(VAL_FIELD_MOVE, 0,  0);
            else if (y > 150 && y < 210)                    _direct = new Vector3Int(  0, 0, VAL_FIELD_MOVE);
            else if (y > 60 && y < 120)                     _direct = new Vector3Int( -VAL_FIELD_MOVE, 0,  0);
        }
    }


    /**
     * @brief 座補補正用
     * @return なし
     */
    private void SetOffsetPos()
    {
        // 取り合えずの処理
        // 一番左下のオブジェクト取得で補正したい
        _offsetPos = new Vector3Int(-5, -1, -5);
    }


    /**
     * @brief デバッグ用関数
     * @param1 オブジェクト種
     * @return なし
     */
    private void CallDebug(E_OBJECT obj = E_OBJECT.MAX)
    {
        int cnt = 0;
        if (obj == E_OBJECT.MAX)
        {// 全オブジェクトのデバッグ表記
            for (int y = 0; y < MAX_OBJECT; y++)
            {
                for (int z = 0; z < MAX_OBJECT; z++)
                {
                    for (int x = 0; x < MAX_OBJECT; x++)
                    {
                        if (_map[x, y, z]._myObject.Equals(E_OBJECT.PLAYER_01))
                        {
                            Debug.Log("座標 x =" + x + " y =" + y + " z =" + z + " にプレイヤーがいます" + _map[x, y, z]._number);
                        }
                        if (_map[x, y, z]._myObject.Equals(E_OBJECT.BLOCK_TANK))
                        {
                            Debug.Log("座標 x =" + x + " y =" + y + " z =" + z + " に水槽があります" + _map[x, y, z]._number);
                        }
                        if (_map[x, y, z]._myObject.Equals(E_OBJECT.BLOCK_GROUND))
                        {
                            Debug.Log("座標 x =" + x + " y =" + y + " z =" + z + " に地面があります" + _map[x, y, z]._number);
                        }
                        if(_map[x, y, z]._myObject.Equals(E_OBJECT.BLOCK_TRAMPLINE))
                        { 
                            Debug.Log("座標 x =" + x + " y =" + y + " z =" + z + " にトランポリンがあります" + _map[x, y, z]._number);
                        }
                    }
                }
            }
        }
        else if (obj == E_OBJECT.PLAYER_01)
        {// プレイヤーのデバッグ表記
            for (int y = 0; y < MAX_OBJECT; y++)
            {
                for (int z = 0; z < MAX_OBJECT; z++)
                {
                    for (int x = 0; x < MAX_OBJECT; x++)
                    {
                        if (_map[x, y, z]._myObject.Equals(E_OBJECT.PLAYER_01))
                        {
                            Debug.Log("座標 x =" + x + " y =" + y + " z =" + z + " にプレイヤーがいます" + _map[x, y, z]._number);
                        }
                    }
                }
            }
        }
        else if (obj == E_OBJECT.BLOCK_TANK)
        {// 水槽のデバッグ表記
            for (int y = 0; y < MAX_OBJECT; y++)
            {
                for (int z = 0; z < MAX_OBJECT; z++)
                {
                    for (int x = 0; x < MAX_OBJECT; x++)
                    {
                        if (_map[x, y, z]._myObject.Equals(E_OBJECT.BLOCK_TANK))
                        {
                            Debug.Log("座標 x =" + x + " y =" + y + " z =" + z + " に水槽があります" + _map[x, y, z]._number);
                            cnt++;
                        }
                    }
                }
            }
            Debug.Log("水槽の総数 " + cnt);
        }
    }

#endif
}

// EOF