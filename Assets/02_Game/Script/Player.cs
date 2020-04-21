/*
 * @file	Player01.cs
 * @brief   プレイヤーの管理
 *
 * @author	Kota Nakagami
 * @date	2020/02/21(金)   作成
 * @data    2020/04/10(金)   マップ配列の参照を FieldController.cs から Map.cs に変更した
 * @deta    2020/04/14(火)   DoTweenによる動きの追加
 * @data    2020/04/15(水)   複数プレイヤーでの移動処理の追加
 * @data    2020/04/18(土)   落下移動の追加
                             箱を落とす処理の追加
                             カメラの向きに合わせて回転、移動を行うようになった
 * 
 * @version	1.00
 */


#define MODE_MAP    // 扱うスクリプト


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


/*
 * @class Player01
 * @brief プレイヤーの動き
 */
public class Player : BaseObject
{

    //! 変数宣言
#if !MODE_MAP
    [SerializeField] Vector3Int     _havePos;           //!< 持ってるオブジェクトの座標の保持
                     bool           _isUpdate;          //!< 更新flag
    FieldController _fieldCtrl;
#endif
    [SerializeField] SquareInfo     _haveObject;    //!< 持っているオブジェクト情報
                     Map            _map;           //!< マップ
    public PlayerAnimation          _animation;     //!< プレイヤーのアニメーション
    PlayerManager                   _mgr;           //!< プレイヤー管理スクリプト
    BaseObject                      _obj;           //!< 後で改善するから許して
    Controller                      _input;         //!< 入力キー


    /*
     * @brief 移動中かどうかの判定
     */
    public bool _isMove //!< 移動フラグ
    {
        get;            // ゲッター
        private set;    // セッターはこのクラス内でのみ
    }


#if MODE_MAP
    /*
     * @brief 初期化
     * @return なし
     */
    override public void Init(int number)
    {
        _myObject   = E_FIELD_OBJECT.PLAYER_01;
        _myNumber   = number;
        _haveObject = new SquareInfo();
        _map        = GameObject.FindGameObjectWithTag("Map").GetComponent<Map>();                      // コンポーネントの取得
        _mgr        = GameObject.FindGameObjectWithTag("PlayerManager").GetComponent<PlayerManager>();  // コンポーネントの取得
        _input      = GameObject.FindGameObjectWithTag("Map").GetComponent<Controller>();               // コンポーネントの取得

        // 座標の補正
        _position = _oldPosition = new Vector3Int(
            (int)(transform.position.x - _map._offsetPos.x),
            (int)((transform.position.y + 0.5f) - _map._offsetPos.y),
            (int)(transform.position.z - _map._offsetPos.z)
            );
        _nextPos = transform.position;

        _lifted     = false;
        _direct     = new Vector3Int(0, 0, 1);  // 取り合えずの処理
        _mode       = E_OBJECT_MODE.WAIT;
        _isMove     = false;

        _animation = GameObject.Find(name).GetComponent<PlayerAnimation>();
        _animation.SetPlayerState(PlayerAnimation.PlayerState.E_WAIT);
    }
#endif


    /*
     * @brief 初期化
     * @return なし
     */
    override public void Start()
    {
#if !MODE_MAP
        _fieldCtrl = GameObject.FindGameObjectWithTag("FieldController")
            .GetComponent<FieldController>();   //!< メインのフィールド保持
        Init();
        _playerAnimation.SetPlayerState(PlayerAnimation.PlayerState.E_WAIT);
#else
        
#endif
    }


#if MODE_MAP
    /*
     * @brief 更新処理
     * @return なし
     */
    override public void Update()
    {
        
    }


    /*
    * @brief 向きを変える
    * @return ベクトル
    */
    public Vector3Int Rotate()
    {
        if (_lifted || _isMove)
        {// 取り合えずここに書き込む
            return _direct;
        }

        if (_input.isInput(E_INPUT_MODE.PUSH, E_INPUT.LB))
        {// 回転モードかのチェック
            _mode = E_OBJECT_MODE.ROTATE;
        }

        float y = GameObject.FindGameObjectWithTag("MainCamera").transform.localEulerAngles.y;

        if (_input.isInput(E_INPUT_MODE.PUSH, E_INPUT.L_STICK_RIGHT))
        {// 右
            if (y > -30 && y < 30 || y > 330 && y < 390)    _direct = new Vector3Int(  1, 0,  0);   //  90
            else if (y > 240 && y < 300)                    _direct = new Vector3Int(  0, 0,  1);   //   0
            else if (y > 150 && y < 210)                    _direct = new Vector3Int( -1, 0,  0);   // -90
            else if (y > 60 && y < 120)                     _direct = new Vector3Int(  0, 0, -1);   // 180
            _isMove = true;
            offsetRotate(_direct);
        }
        else if (_input.isInput(E_INPUT_MODE.PUSH, E_INPUT.L_STICK_LEFT))
        {// 左
            if (y > -30 && y < 30 || y > 330 && y < 390)    _direct = new Vector3Int( -1, 0,  0);   // -90
            else if (y > 240 && y < 300)                    _direct = new Vector3Int(  0, 0, -1);   // 180
            else if (y > 150 && y < 210)                    _direct = new Vector3Int(  1, 0,  0);   //  90
            else if (y > 60 && y < 120)                     _direct = new Vector3Int(  0, 0,  1);   //   0
            _isMove = true;
            offsetRotate(_direct);
        }
        else if (_input.isInput(E_INPUT_MODE.PUSH, E_INPUT.L_STICK_UP))
        {// 奥
            if (y > -30 && y < 30 || y > 330 && y < 390)    _direct = new Vector3Int(  0, 0,  1);   //   0
            else if (y > 240 && y < 300)                    _direct = new Vector3Int( -1, 0,  0);   // -90
            else if (y > 150 && y < 210)                    _direct = new Vector3Int(  0, 0, -1);   // 180
            else if (y > 60 && y < 120)                     _direct = new Vector3Int(  1, 0,  0);   //  90
            _isMove = true;
            offsetRotate(_direct);
        }
        else if (_input.isInput(E_INPUT_MODE.PUSH, E_INPUT.L_STICK_DOWN))
        {// 手前
            if (y > -30 && y < 30 || y > 330 && y < 390)    _direct = new Vector3Int(  0, 0, -1);   // 180
            else if (y > 240 && y < 300)                    _direct = new Vector3Int(  1, 0,  0);   //  90
            else if (y > 150 && y < 210)                    _direct = new Vector3Int(  0, 0,  1);   //   0
            else if (y > 60 && y < 120)                     _direct = new Vector3Int( -1, 0,  0);   // -90
            _isMove = true;
            offsetRotate(_direct);
        }
        if (_mode == E_OBJECT_MODE.ROTATE)
        {// 回転の動き
            RotateMove();
        }

        return _direct;
    }


    /*
     * @brief 物を持ち上げる、下す
     * @return なし
     */
    public void HandAction()
    {
        if (_isMove || _lifted)
        {// 取り合えずここに書き込む
            return;
        }

        if (_haveObject._myObject == E_FIELD_OBJECT.NONE)
        {// 物を持ち上げる
            Lift();
        }
        else
        {// 物を下す
            Put();
        }

        _oldPosition = _position;   // なんでこれでバグが直るの...
    }


    /*
     * @brief 通常ブロックの動き
     * @param1 ベクトル
     * @return なし
     */
    override public void Move()
    {
        if (_isMove || _lifted)
        {// 取り合えずここに書き込む
            return;
        }

        Vector3Int movement = Rotate();       // 向きたいほうに回転

        _oldPosition    = _position;      //!< 座標の保持
        _position       = new Vector3Int(_position.x + movement.x, _position.y + movement.y, _position.z + movement.z);
        _isMove         = true;

        //offsetDirect(); // 向いてる方向の補正

        if (_map.isLimitField(_position))
        {// マップ配列へ参照できない値の場合
            _position   = _oldPosition;
            _isMove     = false;    // 取り合えずの処理
            Debug.Log("エラー : " + name + " はマップ配列外へ移動した");
        }
        else if (_map.isGameOver(_position, E_OBJECT_MODE.MOVE))
        {// ゲームオーバー(落下)
            _position   = _map.GetFallPos(_position);
            _mode       = E_OBJECT_MODE.FALL;
        }
        else if (_map.isDontMove(_position, _oldPosition) || _lifted == true)
        {// 移動出来ない場合
            _position   = _oldPosition;
            _isMove     = false;    // 取り合えずの処理
        }
        else if (_map.isGetup(_position))
        {// 何かの上に上る時
            _position = new Vector3Int(_position.x, _position.y + 1, _position.z);
            _mode = E_OBJECT_MODE.GET_UP;
        }
        else if (_map.isGetoff(_position))
        {// 一段下に降りる時
            _position = new Vector3Int(_position.x, _position.y - 1, _position.z);
            _mode = E_OBJECT_MODE.GET_OFF;
        }
        else
        {// 正面への移動
            _mode = E_OBJECT_MODE.MOVE;
        }
        
        // 後で修正
        offSetTransform();

        // 持っているオブジェクトの追従
        if (_haveObject._myObject != E_FIELD_OBJECT.NONE)
        {// 何か持っている時
            _map.Follow(_haveObject, _position);    // 追従させる
        }

        // 座標移動
        if (_mode == E_OBJECT_MODE.MOVE)
        {// 移動
            MoveMode(); // アニメーションのセット
        }
        else if (_mode == E_OBJECT_MODE.GET_UP)
        {// ジャンプで登る
         //transform.DOJump(endValue, jumpPower, numJumps, duration)
            JumpMode(); // アニメーションのセット

        }
        else if (_mode == E_OBJECT_MODE.GET_OFF)
        {// ジャンプで降りる
            JumpMode(); // アニメーションのセット
        }
        else if (_mode == E_OBJECT_MODE.FALL)
        {// ジャンプで落ちる
            JumpMode(); // アニメーションのセット
        }
    }


    /*
     * @brief  持ち上げられる
     * @param1 ターゲット座標
     * @return なし
     */
    override public void Lifted(Vector3Int pos)
    {
        _oldPosition    = _position;
        _position       = pos;
        _lifted         = true;
        _mode           = E_OBJECT_MODE.LIFTED;
        offSetTransform();
        LiftedMode();   // 持ち上げられる
    }


    /*
     * @brief  置かれる
     * @param1 ターゲット座標
     * @return なし
     */
    override public void Puted(Vector3Int pos)
    {
        _oldPosition    = _position;
        _position       = pos;
        _lifted         = false;
        _mode           = E_OBJECT_MODE.PUTED;
        offSetTransform();
        PutedMode();    // 置かれる
    }


    /*
     * @brief 物を持ち上げる
     * @return なし
     */
    public void Lift()
    {
        Vector3Int havePos;             //!< 持ち上げるオブジェクトを探索するための座標
        havePos = new Vector3Int(       // 向いてる方向の一段上から
            _position.x + _direct.x, _position.y + _direct.y + 1, _position.z + _direct.z
            );
        for (int n = 0; n <= 2; n++, havePos.y -= 1)
        {// 一段上から一段下まで、探索をする
            if (_map.isLimitField(havePos))
            {// マップ配列へ参照できない値の場合
                Debug.Log("エラー : " + name + " はマップ配列外への参照をしようとした");
                continue;
            }
            else if (_map.isLift(havePos))
            {// 何かのオブジェクトを持てる場合
                _obj = _map.LiftToObject(_position, havePos);            // これから持つオブジェクトの情報取得
                if (_obj)
                {// メモ(mapのプレイヤー配列にソートがかかるため持ち上げにバグがでた)
                    _haveObject._myObject   = _obj._myObject;                // オブジェクト情報のセット
                    _haveObject._number     = _obj._myNumber;                // オブジェクトナンバーセット
                    _isMove = true;
                }
                else
                {
                    Debug.Log(name + " 持ち上げるオブジェクトが参照できないよ？");
                }
                LiftMode();                                             // アニメーションのセット
                break;
            }
        }
    }


    /*
     * @brief 物を下す
     * @return なし
     */
    public void Put()
    {
        Vector3Int putPos;              //!< 持ち上げるオブジェクトを探索するための座標
        putPos = new Vector3Int( _position.x + _direct.x, _position.y + _direct.y + 1, _position.z + _direct.z);
        if (_map.isGameOver(putPos, E_OBJECT_MODE.PUT))
        {// ゲームオーバー
            _map.FallToObject(_haveObject, _map.GetFallPos(putPos));  // 置く処理
            _isMove = true;
            return;
        }
        for (int n = 0; n <= 2; n++, putPos.y -= 1)
        {// 一段上から一段下まで、探索をする
            if (_map.isLimitField(putPos))
            {// マップ配列へ参照できない値の場合
                Debug.Log("エラー : " + name + " はマップ配列外への参照をしようとした");
                continue;
            }
            else if (_map.isPut(putPos))
            {// 置くことができる
                _map.PutToObject(_haveObject, putPos);  // 置く処理
                PutMode();                              // アニメーションのセット
                _haveObject = new SquareInfo();         // オブジェクトを手放す
                _isMove = true;
                break;
            }
        }
    }


    /*
     * @brief 向きの調整
     * @param1 向き
     * @return なし
     */
    private void offsetRotate(Vector3Int direct)
    {
        if (direct.z == -1)
        {
            transform.DORotate(new Vector3(0f, 180, 0f), _mgr.MoveTime).OnComplete(() =>
            {
                WaitMode();
            });
        }
        else
        {
            transform.DORotate(new Vector3(0f, 90f * direct.x, 0f), _mgr.MoveTime).OnComplete(() =>
            {
                WaitMode();
            });
        }
    }


    /*
     * @brief 移動後の座標の調整
     * @return なし
     */
    override public void offSetTransform()
    {
        _nextPos = new Vector3(
            (_position.x + _map._offsetPos.x),
            (_position.y + _map._offsetPos.y) - 0.5f,
            (_position.z + _map._offsetPos.z)
            );
    }


    /*
    * @brief 向き変えた時の足踏み
    * @return なし
    */
    private void RotateMove()
    {
        if (_haveObject._myObject == E_FIELD_OBJECT.NONE ||
            _haveObject._myObject == E_FIELD_OBJECT.MAX)
        {// 何も持っていない時
            _animation.SetPlayerState(PlayerAnimation.PlayerState.E_WALK);
        }
        else if (_haveObject._myObject == E_FIELD_OBJECT.PLAYER_01)
        {// プレイヤーを持っている時
            _animation.SetPlayerState(PlayerAnimation.PlayerState.E_WALK_CHARA);
        }
        else if (_haveObject._myObject == E_FIELD_OBJECT.PLAYER_01)
        {// プレイヤー以外を持っている時
            _animation.SetPlayerState(PlayerAnimation.PlayerState.E_WALK);
        }
    }


    /*
     * @brief 待機モード
     * @return なし
     */
    override protected void WaitMode()
    {
        _mode       = E_OBJECT_MODE.WAIT;
        _isMove     = false;
        if (_haveObject._myObject == E_FIELD_OBJECT.NONE ||
            _haveObject._myObject == E_FIELD_OBJECT.MAX)
        {// 何も持っていない時
            _animation.SetPlayerState(PlayerAnimation.PlayerState.E_WAIT);
        }
        else if (_haveObject._myObject == E_FIELD_OBJECT.PLAYER_01)
        {// プレイヤーを持ち上げたとき
            _animation.SetPlayerState(PlayerAnimation.PlayerState.E_WAIT_CHARA);
        }
        else
        {// 何かを持っている時
            _animation.SetPlayerState(PlayerAnimation.PlayerState.E_WAIT_BOX);
        }
    }


    /*
     * @brief 移動モード
     * @return なし
     */
    private void MoveMode()
    {
        if (_haveObject._myObject == E_FIELD_OBJECT.NONE ||
            _haveObject._myObject == E_FIELD_OBJECT.MAX)
        {// 何も持っていない時
            _animation.SetPlayerState(PlayerAnimation.PlayerState.E_WALK);
        }
        else if (_haveObject._myObject == E_FIELD_OBJECT.PLAYER_01)
        {// プレイヤーを持っている時
            _animation.SetPlayerState(PlayerAnimation.PlayerState.E_WALK_CHARA);
        }
        else
        {// プレイヤー以外を持っている時
            _animation.SetPlayerState(PlayerAnimation.PlayerState.E_WALK_BOX);
        }
        //                              移動先座標, 移動時間(秒)
        transform.DOLocalMove(_nextPos, _mgr.MoveTime).OnComplete(() =>
        {
            WaitMode();
        });
    }


    /*
     * @brief ジャンプモード
     * @return なし
     */
    override protected void JumpMode()
    {
        if (_mode == E_OBJECT_MODE.GET_UP)
        {// 登りのジャンプ
            if (_haveObject._myObject == E_FIELD_OBJECT.NONE ||
                _haveObject._myObject == E_FIELD_OBJECT.MAX)
            {// 何も持っていない時
                _animation.SetPlayerState(PlayerAnimation.PlayerState.E_JUMP);
            }
            else if (_haveObject._myObject == E_FIELD_OBJECT.PLAYER_01)
            {// プレイヤーを持っている時
                _animation.SetPlayerState(PlayerAnimation.PlayerState.E_JUMP_CHARA);
            }
            else
            {// 何かを持っている時
                _animation.SetPlayerState(PlayerAnimation.PlayerState.E_JUMP_BOX);
            }
            transform.DOJump(_nextPos, 1, 1, _mgr.MoveTime, false).OnComplete(() =>
            {
                WaitMode();
            });
        }
        else if(_mode == E_OBJECT_MODE.GET_OFF)
        {// 降りのジャンプ
            if (_haveObject._myObject == E_FIELD_OBJECT.NONE ||
                _haveObject._myObject == E_FIELD_OBJECT.MAX)
            {// 何も持っていない時
                _animation.SetPlayerState(PlayerAnimation.PlayerState.E_JUMP);
            }
            else if (_haveObject._myObject == E_FIELD_OBJECT.PLAYER_01)
            {// プレイヤーを持っている時
                _animation.SetPlayerState(PlayerAnimation.PlayerState.E_JUMP_CHARA);
            }
            else
            {// 何かを持っている時
                _animation.SetPlayerState(PlayerAnimation.PlayerState.E_JUMP_BOX);
            }
            transform.DOJump(new Vector3(_nextPos.x, _nextPos.y, _nextPos.z), 1, 1, _mgr.MoveTime, false).OnComplete(() =>
            {
                WaitMode();
            });
        }
        else if (_mode == E_OBJECT_MODE.FALL)
        {// 降りのジャンプ
            if (_haveObject._myObject == E_FIELD_OBJECT.NONE ||
                _haveObject._myObject == E_FIELD_OBJECT.MAX)
            {// 何も持っていない時
                _animation.SetPlayerState(PlayerAnimation.PlayerState.E_JUMP);
            }
            else if (_haveObject._myObject == E_FIELD_OBJECT.PLAYER_01)
            {// プレイヤーを持っている時
                _animation.SetPlayerState(PlayerAnimation.PlayerState.E_JUMP_CHARA);
            }
            else
            {// 何かを持っている時
                _animation.SetPlayerState(PlayerAnimation.PlayerState.E_JUMP_BOX);
            }
            transform.DOJump(new Vector3(_nextPos.x, _nextPos.y, _nextPos.z),   // 目的座標
                (_oldPosition.y - _position.y), // ジャンプパワー
                1,  // ジャンプ回数
                _mgr.MoveTime, // 時間
                false
                ).OnComplete(() =>
            {
                WaitMode();
                _map._gameOver = true;  // ゲームオーバーやで
            });
        }
    }


    /*
     * @brief 持ち上げられるモード
     * @return なし
     */
    void LiftedMode()
    {
        transform.DOLocalMove(_nextPos, _mgr.MoveTime).OnComplete(() =>
        {//　取り合えずこれで行く
            WaitMode();
        });
    }


    /*
     * @brief 置かれるモード
     * @return なし
     */
    void PutedMode()
    {
        transform.DOLocalMove(_nextPos, _mgr.MoveTime).OnComplete(() =>
        {//　取り合えずこれで行く
            WaitMode();
        });
    }


    /*
     * @brief 持ち上げるモード
     * @return なし
     */
    private void LiftMode()
    {
        if (_haveObject._myObject == E_FIELD_OBJECT.NONE ||
            _haveObject._myObject == E_FIELD_OBJECT.MAX)
        {// 何も持っていない時(呼び出し場所の間違えかエラー)
            return;
        }
        else if (_haveObject._myObject == E_FIELD_OBJECT.PLAYER_01)
        {// プレイヤーを持つとき
            _animation.SetPlayerState(PlayerAnimation.PlayerState.E_LIFT_CHARA);
            transform.DOLocalMove(transform.position, _mgr.MoveTime).OnComplete(() =>
            {//　取り合えずこれで行く
                GameObject.Find(_obj.name).transform.parent = transform; // 追従
                WaitMode();
            });
        }
        else
        {// プレイヤー以外を持つ時
            _animation.SetPlayerState(PlayerAnimation.PlayerState.E_LIFT_BOX);
            transform.DOLocalMove(transform.position, _mgr.MoveTime).OnComplete(() =>
            {//　取り合えずこれで行く
                GameObject.Find(_obj.name).transform.parent = transform; // 追従
                WaitMode();
            });
        }
    }


    /*
     * @brief 置くモード
     * @return なし
     */
    private void PutMode()
    {
        if (_haveObject._myObject == E_FIELD_OBJECT.NONE ||
            _haveObject._myObject == E_FIELD_OBJECT.MAX)
        {// 何も持っていない時(呼び出し場所の間違えかエラー)
            return;
        }
        else if (_haveObject._myObject == E_FIELD_OBJECT.PLAYER_01)
        {// プレイヤーを持つとき
            _animation.SetPlayerState(PlayerAnimation.PlayerState.E_PUT_CHARA);
            transform.DOLocalMove(transform.position, _mgr.MoveTime).OnComplete(() =>
            {//　取り合えずこれで行く
                WaitMode();
            });
        }
        else
        {// プレイヤー以外を持つ時
            _animation.SetPlayerState(PlayerAnimation.PlayerState.E_PUT_BOX);
            transform.DOLocalMove(transform.position, _mgr.MoveTime).OnComplete(() =>
            {//　取り合えずこれで行く
                WaitMode();
            });
        }
    }


    /*
     * @brief デバッグ用関数
     * @param1 表示したい文字列
     * @return なし
     */
    public void CallDebug(object message)
    {
        Debug.Log(message);
    }

#else
    /*
     * @brief 更新処理
     * @return なし
     */
    override public void Update()
    {
        _isUpdate = false;
        
        //playerAnimation.SetPlayerState(PlayerAnimation.PlayerState.E_WAIT);
        
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
        if (!isAction())
        {// 移動出来ない場合
            return;
        }
    
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
                if (!_haveObj.Equals(E_FIELD_OBJECT.NONE))
                    _playerAnimation.SetPlayerState(PlayerAnimation.PlayerState.E_JUMP_BOX);
                else
                    _playerAnimation.SetPlayerState(PlayerAnimation.PlayerState.E_JUMP);
            }
        }
        // 何とも衝突しない
        else
        {
            // 下に降りる処理
            if (_fieldCtrl.isGetoff(_position))
            {
                _position = new Vector3Int(_position.x, _position.y - 1, _position.z);
                if (!_haveObj.Equals(E_FIELD_OBJECT.NONE))
                    _playerAnimation.SetPlayerState(PlayerAnimation.PlayerState.E_JUMP_BOX);
                else
                    _playerAnimation.SetPlayerState(PlayerAnimation.PlayerState.E_JUMP);
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
            return;
        }
        
        Vector3Int targetPos = new Vector3Int(_position.x + _direct.x, _position.y + _direct.y + 1, _position.z + _direct.z);
        
        // 前方に何かオブジェクトがあったら
        if (_fieldCtrl.isCollisionToObject(targetPos) &&
            !_fieldCtrl.isCollisionToObject(new Vector3Int(targetPos.x, targetPos.y, targetPos.z), E_FIELD_OBJECT.BLOCK_WATER_SOURCE))
        {
            if (!LiftObject(targetPos))
            {// 持てなかったら更新flagをたてない
                return;
            }
        }
        else if (_fieldCtrl.isCollisionToObject(targetPos
            = new Vector3Int(_position.x + _direct.x, _position.y + _direct.y, _position.z + _direct.z)) &&
            !_fieldCtrl.isCollisionToObject(new Vector3Int(targetPos.x, targetPos.y, targetPos.z), E_FIELD_OBJECT.BLOCK_WATER_SOURCE))
        {
            if (!LiftObject(targetPos))
            {// 持てなかったら更新flagをたてない
                return;
            }
        }
        
        _isUpdate = true;
    }


    /*
     * @brief オブジェクトを持つ処理
     * @param1 持ち上げる対象ぼフィールド座標
     * @return オブジェクトを持つことができるなら true
     */
    private bool LiftObject(Vector3Int targetPos)
    {
        if (_fieldCtrl.isCollisionToObject(new Vector3Int(targetPos.x, targetPos.y, targetPos.z), E_FIELD_OBJECT.BLOCK_TANK) &&
                _fieldCtrl.isCollisionToObject(new Vector3Int(targetPos.x, targetPos.y + 1, targetPos.z)))
        {// 上に物が置いてあったら
            return false;
        }
        
        // 持ち上げる
        Debug.Log(name + " が" + _fieldCtrl._field[targetPos.x, targetPos.y, targetPos.z].name + " を持ち上げました");
        _haveObj = _fieldCtrl.LiftObject(_position, targetPos);
        
        // 追従
        GameObject.Find(_fieldCtrl._field[_position.x, _position.y + 1, _position.z].name).transform.parent = transform;
        _havePos = new Vector3Int(_position.x, _position.y + 1, _position.z);
        
        _animCnt = MAX_ANIM_WALK;
        _nowMove = false;
        _playerAnimation.SetPlayerState(PlayerAnimation.PlayerState.E_LIFT_BOX);
        
        // もし既に何かを持っていたら
        if (!_fieldCtrl._field[_position.x, _position.y + 1, _position.z]._haveObj.Equals(E_FIELD_OBJECT.NONE))
        {
            // もう一度持ち上げる
            _fieldCtrl._field[_position.x, _position.y + 1, _position.z].Lift();
        }
        return true;
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
            return;
        }
        
        _animCnt = MAX_ANIM_WALK;
        _nowMove = false;
        
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
        
            // アニメーションカウント
            _animCnt = MAX_ANIM_WALK;
            _nowMove = false;
            _playerAnimation.SetPlayerState(PlayerAnimation.PlayerState.E_PUT_BOX);
        
            // もし既に何かを持っていたら
            if (!_fieldCtrl._field[targetPos.x, targetPos.y, targetPos.z]._haveObj.Equals(E_FIELD_OBJECT.NONE))
            {
                _fieldCtrl._field[targetPos.x, targetPos.y, targetPos.z].LetDown();   // もう一度置く
            }
        }
        _isUpdate = true;
    }


    /*
     * @brief 配列座標の補正
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
#endif
}

// EOF