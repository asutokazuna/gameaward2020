/*
 * @file	Map.cs
 * @brief   フィールドのマップ情報
 *
 * @author	Kota Nakagami
 * @date1	2020/04/06(月)
 *
 * @version	1.00
 */


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

    //! 変数宣言
    public SquareInfo[,,]       _square;    //!< マップ情報
    public Player[]             _player;    //!< プレイヤーオブジェクト
    public BlockTank[]          _box;       //!< 箱オブジェクト
    [SerializeField] int        _playerCnt; //!< プレイヤーカウント
    [SerializeField] int        _boxCnt;    //!< 箱カウント
    [SerializeField] Vector3Int _direct;    //!< 全プレイヤーが向いてる方向


    /*
     * @brief Awake
     * @return なし
     */
    void Awake()
    {
        _square = new SquareInfo[MAX_FIELD_OBJECT, MAX_FIELD_OBJECT, MAX_FIELD_OBJECT];
    }


    // Start is called before the first frame update
    void Start()
    {
        InitObject();
    }

    // Update is called once per frame
    void Update()
    {
        // 今はコメントアウト
        //MoveObject();
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
        {
            _player[n].Move(_direct);
        }
    }


    /*
     * @brief オブジェクト情報のセット
     * @param1 BaseObject obj, int number
     * @return なし
     */
    private void SetObject(BaseObject obj, int number)
    {
        _square[obj._position.x, obj._position.y, obj._position.z]._myObject    = obj._myObject;
        _square[obj._position.x, obj._position.y, obj._position.z]._number      = number;
        _square[obj._position.x, obj._position.y, obj._position.z]._myObject    = obj._myObject;
    }


    /*
     * @brief マップ情報と各オブジェクト情報の初期化
     * @return なし
     */
    private void InitObject()
    {
        InitPlayerObj();
        InitBlockTankObj();
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


}


// EOF