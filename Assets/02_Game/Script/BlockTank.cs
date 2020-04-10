

//#define MODE_MAP


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * @class 水槽ブロック
 * @brief 水の溜まり判定
 */
public class BlockTank : BaseObject
{
    // 定数定義
    public int TargetCnt;

    //! 変数宣言
    public int          _maxWater;                  //!< 貯める最大値
    public int          _numWater;                  //!< 現在の中身
    FieldController     _fieldCtrl;


    private void Awake()
    {
        /*
         * BaseobjectにBLOCK_TANK追加
         * Playerにこの箱の当たり判定追加
         */
        _myObject = E_FIELD_OBJECT.BLOCK_TANK;
    }

    // Start is called before the first frame update
    override public void Start()
    {
        _fieldCtrl = GameObject.FindGameObjectWithTag("FieldController")
            .GetComponent<FieldController>();   //!< メインのフィールド保持
        Init();
        _maxWater = TargetCnt;
        _numWater = 0;
    }

    // Update is called once per frame
    override public void Update()
    {
        AddWater();
        //Move();

#if !MODE_MAP
        if (_animCnt > 0)
        {// 移動カウント
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
            }
            else
            {// 何も持ってない時
            }

            // 座標の補正
            transform.position = _fieldCtrl.offsetPos(_myObject, _position);
            _animCnt = -1;
            _nowMove = false;
        }
#endif
    }


    /*
     * @brief 水の加算する関数
     * return なし
     */
    public void AddWater()
    {
        if (isCollisitionBox())
        {// ブロックとあたってたら
            if (_numWater >= _maxWater && _fullWater.Equals(false))
            {// 溜まったら
                Debug.Log(name + "満タンになりました。");
                _fullWater = true;
                _fieldCtrl.SetWaterCnt(true);
            }
            else if (_numWater < _maxWater)
            {
                _numWater += 1;
                Debug.Log(name + "現在の水量" + _numWater);
            }
        }
        else
        {// 水が抜ける処理
            if (_numWater <= 0 && _fullWater.Equals(true))
            {
                _numWater = 0;
                _fullWater = false;
                _fieldCtrl.SetWaterCnt(false);
            }
            else if (_numWater > 0)
            {
                _numWater = 0;
                Debug.Log(name + "空になりました");
            }
        }
    }


    /*
     * @brief 水が入ったブロックとの判定
     * @return なし
     */
    public bool isCollisitionBox()
    {

        Vector3Int targetPos = new Vector3Int(_position.x, _position.y, _position.z);

        if (_fieldCtrl.isWater(_position))
        {
            return true;
        }
        return false;
    }
}
