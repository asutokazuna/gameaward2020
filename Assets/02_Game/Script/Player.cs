/**
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
 * @date	2020/04/21(火)   クリア・ゲームオーバー時に移動できなくなる処理の追加    加藤
 * 
 * @version	1.00
 */


#define MODE_MAP    // 扱うスクリプト


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


/**
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
    public           SquareInfo     _haveObject;    //!< 持っているオブジェクト情報
                     Map            _map;           //!< マップ
    //public PlayerAnimation          _animation;     //!< プレイヤーのアニメーション
    PlayerAnim _animation;
    PlayerManager                   _mgr;           //!< プレイヤー管理スクリプト
    Controller                      _input;         //!< 入力キー
    public bool _putUpdate;
    public bool _liftedMove;

    /*
     * sound
     */
    private AudioSource _audioSource;
    public AudioClip _SEMove;
    public AudioClip _SEjump;
    public AudioClip _SEPut;
    public AudioClip _SETrampoline;
    public AudioClip _SEGameClear;
    public AudioClip _SEGameOver;


#if MODE_MAP
    /**
     * @brief 初期化
     * @return なし
     */
    override public void Init(int number)
    {
        _myObject   = E_OBJECT.PLAYER_01;
        _myNumber   = number;
        _haveObject = new SquareInfo();
        _map        = GameObject.FindGameObjectWithTag("Map").GetComponent<Map>();                      // コンポーネントの取得
        _mgr        = GameObject.FindGameObjectWithTag("PlayerManager").GetComponent<PlayerManager>();  // コンポーネントの取得
        if (_input == null)
            _input = GameObject.FindGameObjectWithTag("Input").GetComponent<Controller>();

        // 座標の補正
        _position = _oldPosition = new Vector3Int(
            (int)(transform.position.x - _map._offsetPos.x),
            (int)((transform.position.y + 0.5f) - _map._offsetPos.y),
            (int)(transform.position.z - _map._offsetPos.z)
            );
        _nextPos = transform.position;

        _lifted     = E_HANDS_ACTION.NONE;

        if (transform.localEulerAngles.y == 0)
        {
            _direct.z = 1;
        }
        else if (transform.localEulerAngles.y == 90)
        {
            _direct.x = 1;
        }
        else if (transform.localEulerAngles.y == 180)
        {
            _direct.z = -1;
        }
        else if (transform.localEulerAngles.y == 270)
        {
            _direct.x = -1;
        }

        _mode       = E_OBJECT_MODE.WAIT;
        _isMove     = false;
        _liftedMove = false;

        _audioSource = GetComponent<AudioSource>();

        //_animation = GameObject.Find(name).GetComponent<PlayerAnimation>();
        _animation = GameObject.Find(name).GetComponent<PlayerAnim>();
        //_animation.SetPlayerState(PlayerAnimation.PlayerState.E_WAIT);
    }
#endif


    /**
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
    /**
     * @brief 更新処理
     * @return なし
     */
    override public void Update()
    {

    }


   /**
    * @brief 向きを変える
    * @return ベクトル
    */
    override public Vector3Int Rotate()
    {
        #region Rotate

        if (_lifted != E_HANDS_ACTION.NONE || _isMove || _map._gameClear || _map._gameOver)
        {// 取り合えずここに書き込む
            return _direct;
        }

        float y = GameObject.FindGameObjectWithTag("MainCamera").transform.localEulerAngles.y;

        if (_input.GetComponent<Controller>()
            .isInput(E_INPUT_MODE.TRIGGER, E_INPUT.L_STICK_RIGHT))
        {// 右
            if (y > -30 && y < 30 || y > 330 && y < 390)    _direct = new Vector3Int(  1, 0,  0);   //  90
            else if (y > 240 && y < 300)                    _direct = new Vector3Int(  0, 0,  1);   //   0
            else if (y > 150 && y < 210)                    _direct = new Vector3Int( -1, 0,  0);   // -90
            else if (y > 60 && y < 120)                     _direct = new Vector3Int(  0, 0, -1);   // 180
            _isMove = true;
            _mode = E_OBJECT_MODE.ROTATE;                                                           // 回転モードセット
            offsetRotate(_direct);
        }
        else if (_input.GetComponent<Controller>()
            .isInput(E_INPUT_MODE.TRIGGER, E_INPUT.L_STICK_LEFT))
        {// 左
            if (y > -30 && y < 30 || y > 330 && y < 390)    _direct = new Vector3Int( -1, 0,  0);   // -90
            else if (y > 240 && y < 300)                    _direct = new Vector3Int(  0, 0, -1);   // 180
            else if (y > 150 && y < 210)                    _direct = new Vector3Int(  1, 0,  0);   //  90
            else if (y > 60 && y < 120)                     _direct = new Vector3Int(  0, 0,  1);   //   0
            _isMove = true;
            _mode = E_OBJECT_MODE.ROTATE;                                                           // 回転モードセット
            offsetRotate(_direct);
        }
        else if (_input.GetComponent<Controller>()
            .isInput(E_INPUT_MODE.TRIGGER, E_INPUT.L_STICK_UP))
        {// 奥
            if (y > -30 && y < 30 || y > 330 && y < 390)    _direct = new Vector3Int(  0, 0,  1);   //   0
            else if (y > 240 && y < 300)                    _direct = new Vector3Int( -1, 0,  0);   // -90
            else if (y > 150 && y < 210)                    _direct = new Vector3Int(  0, 0, -1);   // 180
            else if (y > 60 && y < 120)                     _direct = new Vector3Int(  1, 0,  0);   //  90
            _isMove = true;
            _mode = E_OBJECT_MODE.ROTATE;                                                           // 回転モードセット
            offsetRotate(_direct);
        }
        else if (_input.GetComponent<Controller>()
            .isInput(E_INPUT_MODE.TRIGGER, E_INPUT.L_STICK_DOWN))
        {// 手前
            if (y > -30 && y < 30 || y > 330 && y < 390)    _direct = new Vector3Int(  0, 0, -1);   // 180
            else if (y > 240 && y < 300)                    _direct = new Vector3Int(  1, 0,  0);   //  90
            else if (y > 150 && y < 210)                    _direct = new Vector3Int(  0, 0,  1);   //   0
            else if (y > 60 && y < 120)                     _direct = new Vector3Int( -1, 0,  0);   // -90
            _isMove = true;
            _mode = E_OBJECT_MODE.ROTATE;                                                           // 回転モードセット
            offsetRotate(_direct);
        }
        if (_mode == E_OBJECT_MODE.ROTATE)
        {// 回転の動き
            RotateMove();
        }

        return _direct;

        #endregion
    }


    /**
     * @brief 物を持ち上げる、下す
     * @param1 モード
     * @return なし
     */
    public void HandAction(E_OBJECT_MODE mode)
    {
        //if (_isMove || _lifted != E_HANDS_ACTION.NONE)
        //{// 取り合えずここに書き込む
        //    return;
        //}
        if (!_isMove && mode == E_OBJECT_MODE.LIFT && _haveObject._myObject == E_OBJECT.NONE)
        {// 物を持ち上げる
            if (_lifted == E_HANDS_ACTION.DO &&
                _map.GetObject(_map.GetObject(new Vector3Int(_position.x, _position.y - 1, _position.z)))._mode == E_OBJECT_MODE.PUTED)
            {
                return;
            }
            if (_liftedMove)
            {
                return;
            }
            Lift();
        }
        else if (!_isMove && mode == E_OBJECT_MODE.PUT && _haveObject._myObject != E_OBJECT.NONE)
        {// 物を下す
            Put();
            _audioSource.PlayOneShot(_SEPut);
        }

        _oldPosition = _position;
    }


    /**
     * @brief 通常ブロックの動き
     * @param1 ベクトル
     * @return なし
     */
    override public void Move()
    {
        if (_isMove || _lifted != E_HANDS_ACTION.NONE || _map._gameClear || _map._gameOver)
        {// 取り合えずここに書き込む
            return;
        }

        Vector3Int movement = Rotate();       // 向きたいほうに回転

        _oldPosition    = _position;      //!< 座標の保持
        _position       = new Vector3Int(_position.x + movement.x, _position.y + movement.y, _position.z + movement.z);
        _isMove         = true;

        if (_map.isTrampline(new Vector3Int(_oldPosition.x, _oldPosition.y - 1, _oldPosition.z)))
        {
            _position = _map.GetTramplinepPos(_position);
        }
        //offsetDirect(); // 向いてる方向の補正
        if (_map.isLimitField(_position))
        {// マップ配列へ参照できない値の場合
            _position = _oldPosition;
            _isMove = false;    // 取り合えずの処理
            Debug.Log("エラー : " + name + " はマップ配列外へ移動した");
        }
        else if (_map.isDontMove(_position, _oldPosition) || _lifted != E_HANDS_ACTION.NONE)
        {// 移動出来ない場合
            _position = _oldPosition;
            _mode = E_OBJECT_MODE.DONT_MOVE;
        }
        else if (_map.isGameOver(_position, E_OBJECT_MODE.MOVE))
        {// ゲームオーバー(落下)
            if (_map.isOutsideTheArea(_position))
            {// エリア外
                _mode = E_OBJECT_MODE.AREA_FALL;
            }
            else
            {// 落下
                _mode = E_OBJECT_MODE.FALL;
                _position = _map.GetFallPos(_position);
            }
            _gameOver   = true;
        }
        else if (_map.isGetup(_position))
        {// 何かの上に上る時
            _position = new Vector3Int(_position.x, _position.y + 1, _position.z);
            _mode = E_OBJECT_MODE.GET_UP;
            _audioSource.PlayOneShot(_SEjump);
        }
        else if (_map.isGetoff(_position))
        {// 一段下に降りる時
         //_position = new Vector3Int(_position.x, _position.y - 1, _position.z);
            _position = _map.GetoffPos(_position);
            _mode = E_OBJECT_MODE.GET_OFF;
            _audioSource.PlayOneShot(_SEjump);
        }
        else
        {// 正面への移動
            _mode = E_OBJECT_MODE.MOVE;
            _audioSource.PlayOneShot(_SEMove);
        }

        // 持っているオブジェクトの追従
        if (_haveObject._myObject != E_OBJECT.NONE)
        {// 何か持っている時
            _map.Follow(_haveObject, _position, _direct, _mode);    // 追従させる
        }
    }


    override public void MapUpdate()
    {
        offSetTransform();
        // 座標移動
        if (_mode == E_OBJECT_MODE.MOVE || _mode == E_OBJECT_MODE.DONT_MOVE || _mode == E_OBJECT_MODE.AREA_FALL)
        {// 移動
            MoveMode(); // アニメーションのセット
        }
        else if (_mode == E_OBJECT_MODE.GET_UP || _mode == E_OBJECT_MODE.GET_OFF || _mode == E_OBJECT_MODE.FALL)
        {// ジャンプ
            JumpMode(); // アニメーションのセット

        }
        else if (_mode == E_OBJECT_MODE.LIFTED)
        {
            LiftedMode();   // 持ち上げられる
        }
        else if (_mode == E_OBJECT_MODE.PUTED)
        {
            PutedMode();    // 置かれる
        }
    }


    /**
     * @brief オブジェクトの追従
     * @param1 目的座標
     * @param2 方向
     * @param3 モード
     * @return なし
     */
    override public void Follow(Vector3Int pos, Vector3Int direct, E_OBJECT_MODE mode)
    {
        _oldPosition    = _position;
        _position       = pos;
        _direct         = direct;
        _mode           = mode;
        // 持っているオブジェクトの追従
        if (_haveObject._myObject != E_OBJECT.NONE)
        {// 何か持っている時
            _map.Follow(_haveObject, _position, _direct, _mode);    // 追従させる
        }
        offsetRotate(_direct);
    }


    /**
     * @brief  持ち上げられる
     * @param1 ターゲット座標
     * @return なし
     */
    override public void Lifted(Vector3Int pos)
    {
        _oldPosition    = _position;
        _position       = pos;
        _lifted         = E_HANDS_ACTION.NOW_PLAY;
        _mode           = E_OBJECT_MODE.LIFTED;
        if (_lifted != E_HANDS_ACTION.NONE && _haveObject._myObject != E_OBJECT.NONE)
        {// 取り合えずの処理
            _map.Poop(_haveObject, new Vector3Int(_position.x, _position.y + 1, _position.z));
        }
    }


    /**
     * @brief  置かれる
     * @param1 ターゲット座標
     * @return なし
     */
    override public void Puted(Vector3Int pos)
    {
        _oldPosition    = _position;
        _position       = pos;
        _lifted         = E_HANDS_ACTION.NOW_PLAY;
        _mode           = E_OBJECT_MODE.PUTED;
        _liftedMove     = true;
        // 持っているオブジェクトの追従
        if (_haveObject._myObject != E_OBJECT.NONE)
        {// 何か持っている時
            _map.Follow(_haveObject, _position, _direct, _mode);    // 追従させる
        }
    }


    /**
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
            _haveObject = _map.isLift(havePos);


            if(_haveObject._myObject == E_OBJECT.PLAYER_01)
            {
                _animation.SetPlayerInfo(PlayerAnim.PlayerInfo.E_CHARA);
            }
            else if(_haveObject._myObject != E_OBJECT.NONE)
            {
                _animation.SetPlayerInfo(PlayerAnim.PlayerInfo.E_BOX);
            }

            if (_haveObject._myObject != E_OBJECT.NONE)
            {// 何かのオブジェクトを持てる場合
                _map.LiftToObject(_haveObject, new Vector3Int(_position.x, _position.y + 1, _position.z));
                LiftMode(n);    // アニメーションのセット
                break;
            }
        }
    }


    /**
     * @brief 物を下す
     * @return なし
     */
    public void Put()
    {
        bool flag = false;
        Vector3Int putPos;              //!< 降ろすオブジェクトを探索するための座標
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
                PutMode(n);                             // アニメーションのセット
                _haveObject = new SquareInfo();         // オブジェクトを手放す
                _isMove = true;
                _putUpdate = false;
                flag = true;
                break;
            }
        }
        if (_putUpdate && !flag)
        {
            _putUpdate = false;
        }
    }


    /**
     * @brief トーテムポールで挟まれてるか
     * @return 挟まれてるなら true
     */
    public bool isCenter()
    {
        if (_lifted == E_HANDS_ACTION.DO && _haveObject._myObject != E_OBJECT.NONE)
        {
            return true;
        }
        return false;
    }


    /**
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


    /**
     * @brief 向きの調整
     * @param1 向き
     * @return なし
     */
    override protected void offsetRotate(Vector3Int direct)
    {
        if (direct.z == -1)
        {
            transform.DORotate(
                new Vector3(0f, 180, 0f),
                GameObject.FindGameObjectWithTag("PlayerManager").GetComponent<PlayerManager>().MoveTime
                ).OnComplete(() =>
            {
                WaitMode();
            });
        }
        else
        {
            transform.DORotate(
                new Vector3(0f, 90f * direct.x, 0f),
                GameObject.FindGameObjectWithTag("PlayerManager").GetComponent<PlayerManager>().MoveTime
                ).OnComplete(() =>
            {
                WaitMode();
            });
        }
    }


    /**
     * @brief 向き変えた時の足踏み
     * @return なし
     */
    override protected void RotateMove()
    {
        // 持っているオブジェクトの追従
        if (_haveObject._myObject != E_OBJECT.NONE)
        {// 何か持っている時
            _map.Follow(_haveObject, _position, _direct, _mode);    // 追従させる
        }

        //if (_haveObject._myObject == E_OBJECT.NONE ||
        //    _haveObject._myObject == E_OBJECT.MAX)
        //{// 何も持っていない時
        //    _animation.SetPlayerState(PlayerAnimation.PlayerState.E_WALK);
        //}
        //else if (_haveObject._myObject == E_OBJECT.PLAYER_01)
        //{// プレイヤーを持っている時
        //    _animation.SetPlayerState(PlayerAnimation.PlayerState.E_WALK_CHARA);
        //}
        //else if (_haveObject._myObject == E_OBJECT.PLAYER_01)
        //{// プレイヤー以外を持っている時
        //    _animation.SetPlayerState(PlayerAnimation.PlayerState.E_WALK);
        //}

        _animation.SetPlayerState(PlayerAnim.PlayerState.E_WALK);
    }


    /**
     * @brief 待機モード
     * @return なし
     */
    override protected void WaitMode()
    {
        _mode       = E_OBJECT_MODE.WAIT;
        _isMove     = false;
        _liftedMove = false;

        _oldPosition = _position;

        if (_map.isUse(new Vector3Int(_position.x, _position.y - 1, _position.z), E_OBJECT.PLAYER_01))
        {
            _lifted = E_HANDS_ACTION.DO;
        }

        //if (_haveObject._myObject == E_OBJECT.NONE ||
        //    _haveObject._myObject == E_OBJECT.MAX)
        //{// 何も持っていない時
        //    //if (_map.isTrampline(new Vector3Int(_position.x, _position.y - 1, _position.z)))
        //    //{
        //    //    _animation.SetPlayerState(PlayerAnimation.PlayerState.E_WAIT_TP);
        //    //}
        //    //else
        //    {
        //        _animation.SetPlayerState(PlayerAnimation.PlayerState.E_WAIT);
        //    }
        //}
        //else if (_haveObject._myObject == E_OBJECT.PLAYER_01)
        //{// プレイヤーを持ち上げたとき
        //    _animation.SetPlayerState(PlayerAnimation.PlayerState.E_WAIT_CHARA);
        //}
        //else
        //{// 何かを持っている時
        //    _animation.SetPlayerState(PlayerAnimation.PlayerState.E_WAIT_BOX);
        //}
    }


    /**
     * @brief 移動モード
     * @return なし
     */
    override protected void MoveMode()
    {
        if (_mode == E_OBJECT_MODE.MOVE)
        {
            //if (_haveObject._myObject == E_OBJECT.NONE ||
            //    _haveObject._myObject == E_OBJECT.MAX)
            //{// 何も持っていない時
            //    _animation.SetPlayerState(PlayerAnimation.PlayerState.E_WALK);
            //}
            //else if (_haveObject._myObject == E_OBJECT.PLAYER_01)
            //{// プレイヤーを持っている時
            //    _animation.SetPlayerState(PlayerAnimation.PlayerState.E_WALK_CHARA);
            //}
            //else
            //{// プレイヤー以外を持っている時
            //    _animation.SetPlayerState(PlayerAnimation.PlayerState.E_WALK_BOX);
            //}

            _animation.SetPlayerState(PlayerAnim.PlayerState.E_WALK);
        }
        else if (_mode == E_OBJECT_MODE.DONT_MOVE)
        {
            //if (_haveObject._myObject == E_OBJECT.NONE ||
            //    _haveObject._myObject == E_OBJECT.MAX)
            //{// 何も持っていない時
            //    _animation.SetPlayerState(PlayerAnimation.PlayerState.E_BUMP);
            //}
            //else if (_haveObject._myObject == E_OBJECT.PLAYER_01)
            //{// プレイヤーを持っている時
            //    _animation.SetPlayerState(PlayerAnimation.PlayerState.E_BUMP_CHARA);
            //}
            //else
            //{// プレイヤー以外を持っている時
            //    _animation.SetPlayerState(PlayerAnimation.PlayerState.E_BUMP_BOX);
            //}

            _animation.SetPlayerState(PlayerAnim.PlayerState.E_BUMP);
        }
        else if (_mode == E_OBJECT_MODE.AREA_FALL)
        {
            if (_haveObject._myObject == E_OBJECT.NONE ||
                _haveObject._myObject == E_OBJECT.MAX)
            {// 何も持っていない時
                //_animation.SetPlayerState(PlayerAnimation.PlayerState.E_FALL);
            }
            else if (_haveObject._myObject == E_OBJECT.PLAYER_01)
            {// プレイヤーを持っている時
                //_animation.SetPlayerState(PlayerAnimation.PlayerState.E_FALL);
            }
            else
            {// 何かを持っている時
                //_animation.SetPlayerState(PlayerAnimation.PlayerState.E_FALL);
            }
        }
        //                              移動先座標, 移動時間(秒)
        transform.DOLocalMove(_nextPos, _mgr.MoveTime).OnComplete(() =>
        {
            if (_gameOver)
            {// ゲームオーバーの時
                _map._gameOver = true;
                transform.DOLocalMove(      //取り合えずの数値
                    new Vector3(_nextPos.x, _nextPos.y - 1f, _nextPos.z),// 目的座標
                    _mgr.MoveTime
                ).OnComplete(() =>
                {
                    transform.DOScale(new Vector3(), _mgr.OutsideTheEreaTime);
                    WaitMode();
                });
            }
            else
            {// そうじゃない場合
                WaitMode();
            }
        });
    }


    /**
     * @brief ジャンプモード
     * @return なし
     */
    override protected void JumpMode()
    {
        if (_mode == E_OBJECT_MODE.GET_UP)
        {// 登りのジャンプ
            //if (_haveObject._myObject == E_OBJECT.NONE ||
            //    _haveObject._myObject == E_OBJECT.MAX)
            //{// 何も持っていない時
            //    _animation.SetPlayerState(PlayerAnimation.PlayerState.E_JUMP);
            //}
            //else if (_haveObject._myObject == E_OBJECT.PLAYER_01)
            //{// プレイヤーを持っている時
            //    _animation.SetPlayerState(PlayerAnimation.PlayerState.E_JUMP_CHARA);
            //}
            //else
            //{// 何かを持っている時
            //    _animation.SetPlayerState(PlayerAnimation.PlayerState.E_JUMP_BOX);
            //}
            _animation.SetPlayerState(PlayerAnim.PlayerState.E_JUMP);

            transform.DOJump(_nextPos, 1, 1, _mgr.MoveTime, false).OnComplete(() =>
            {
                WaitMode();
            });
        }
        else if(_mode == E_OBJECT_MODE.GET_OFF)
        {// 降りのジャンプ
            //if (_haveObject._myObject == E_OBJECT.NONE ||
            //    _haveObject._myObject == E_OBJECT.MAX)
            //{// 何も持っていない時
            //    _animation.SetPlayerState(PlayerAnimation.PlayerState.E_JUMP);
            //}
            //else if (_haveObject._myObject == E_OBJECT.PLAYER_01)
            //{// プレイヤーを持っている時
            //    _animation.SetPlayerState(PlayerAnimation.PlayerState.E_JUMP_CHARA);
            //}
            //else
            //{// 何かを持っている時
            //    _animation.SetPlayerState(PlayerAnimation.PlayerState.E_JUMP_BOX);
            //}
            _animation.SetPlayerState(PlayerAnim.PlayerState.E_JUMP);

            transform.DOJump(new Vector3(_nextPos.x, _nextPos.y, _nextPos.z), 1, 1, _mgr.MoveTime, false).OnComplete(() =>
            {
                WaitMode();
            });
        }
        else if (_mode == E_OBJECT_MODE.FALL)
        {// 降りのジャンプ
            if (_haveObject._myObject == E_OBJECT.NONE ||
                _haveObject._myObject == E_OBJECT.MAX)
            {// 何も持っていない時
                //_animation.SetPlayerState(PlayerAnimation.PlayerState.E_FALL_FAINT);
            }
            else if (_haveObject._myObject == E_OBJECT.PLAYER_01)
            {// プレイヤーを持っている時
                //_animation.SetPlayerState(PlayerAnimation.PlayerState.E_FALL_FAINT);
            }
            else
            {// 何かを持っている時
                //_animation.SetPlayerState(PlayerAnimation.PlayerState.E_FALL_FAINT);
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
                _gameOver = true;
            });
        }
    }


    /**
     * @brief 持ち上げられるモード
     * @return なし
     */
    void LiftedMode()
    {
        transform.DOLocalMove(_nextPos, _mgr.MoveTime).OnComplete(() =>
        {//　取り合えずこれで行く
            _lifted = E_HANDS_ACTION.DO;
            WaitMode();
        });
    }


    /**
     * @brief 置かれるモード
     * @return なし
     */
    void PutedMode()
    {
        transform.DOLocalMove(_nextPos, _mgr.MoveTime).OnComplete(() =>
        {//　取り合えずこれで行く
            _lifted = E_HANDS_ACTION.NONE;
            WaitMode();
        });
    }


    /**
     * @brief 持ち上げるモード
     * @return なし
     */
    private void LiftMode(int n)
    {
        if (_haveObject._myObject == E_OBJECT.NONE ||
            _haveObject._myObject == E_OBJECT.MAX)
        {// 何も持っていない時(呼び出し場所の間違えかエラー)
            return;
        }
        else if (_haveObject._myObject == E_OBJECT.PLAYER_01)
        {// プレイヤーを持つとき
            //if (n == 0)
            //    _animation.SetPlayerState(PlayerAnimation.PlayerState.E_LIFT_UP_CHARA);
            //else if (n == 1)
            //    _animation.SetPlayerState(PlayerAnimation.PlayerState.E_LIFT_CHARA);
            //else if (n == 2)
            //    _animation.SetPlayerState(PlayerAnimation.PlayerState.E_LIFT_LOW_CHARA);

            _animation.SetPlayerInfo(n);
            _animation.SetPlayerState(PlayerAnim.PlayerState.E_LIFT);

            transform.DOLocalMove(transform.position, _mgr.MoveTime).OnComplete(() =>
            {//　取り合えずこれで行く
                //GameObject.Find(_map.GetLiftObject(_haveObject).name).transform.parent = transform; // 追従
                WaitMode();
            });
        }
        else
        {// プレイヤー以外を持つ時
            //if (n == 0)
            //    _animation.SetPlayerState(PlayerAnimation.PlayerState.E_LIFT_UP_BOX);
            //else if (n == 1)
            //    _animation.SetPlayerState(PlayerAnimation.PlayerState.E_LIFT_BOX);
            //else if (n == 2)
            //    _animation.SetPlayerState(PlayerAnimation.PlayerState.E_LIFT_LOW_BOX);

            _animation.SetPlayerInfo(n);
            _animation.SetPlayerState(PlayerAnim.PlayerState.E_LIFT);

            transform.DOLocalMove(transform.position, _mgr.MoveTime).OnComplete(() =>
            {//　取り合えずこれで行く
                GameObject.Find(_map.GetLiftObject(_haveObject).name).transform.parent = transform; // 追従
                WaitMode();
            });
        }
    }


    /**
     * @brief 置くモード
     * @return なし
     */
    private void PutMode(int n)
    {
        if (_haveObject._myObject == E_OBJECT.NONE ||
            _haveObject._myObject == E_OBJECT.MAX)
        {// 何も持っていない時(呼び出し場所の間違えかエラー)
            return;
        }
        else if (_haveObject._myObject == E_OBJECT.PLAYER_01)
        {// プレイヤーを持つとき
            //if (n == 0)
            //    _animation.SetPlayerState(PlayerAnimation.PlayerState.E_PUT_UP_CHARA);
            //else if (n == 1)
            //    _animation.SetPlayerState(PlayerAnimation.PlayerState.E_PUT_CHARA);
            //else if (n == 2)
            //    _animation.SetPlayerState(PlayerAnimation.PlayerState.E_PUT_LOW_CHARA);

            //_animation.SetPlayerState(PlayerAnimation.PlayerState.E_PUT_CHARA);

            _animation.SetPlayerInfo(n);
            _animation.SetPlayerState(PlayerAnim.PlayerState.E_PUT);
            transform.DOLocalMove(transform.position, _mgr.MoveTime).OnComplete(() =>
            {//　取り合えずこれで行く
                WaitMode();
            });
        }
        else
        {// プレイヤー以外を持つ時
            //if (n == 0)
            //    _animation.SetPlayerState(PlayerAnimation.PlayerState.E_PUT_UP_BOX);
            //else if (n == 1)
            //    _animation.SetPlayerState(PlayerAnimation.PlayerState.E_PUT_BOX);
            //else if (n == 2)
            //    _animation.SetPlayerState(PlayerAnimation.PlayerState.E_PUT_LOW_BOX);

            _animation.SetPlayerInfo(n);
            _animation.SetPlayerState(PlayerAnim.PlayerState.E_PUT);

            transform.DOLocalMove(transform.position, _mgr.MoveTime).OnComplete(() =>
            {//　取り合えずこれで行く
                WaitMode();
            });
        }
    }


    public void GameClear()
    {
        //_animation.SetPlayerState(PlayerAnimation.PlayerState.E_HAPPY);
        _animation.SetPlayerState(PlayerAnim.PlayerState.E_HAPPY);
        //if (_haveObject._myObject == E_FIELD_OBJECT.NONE ||
        //    _haveObject._myObject == E_FIELD_OBJECT.MAX)
        //{// 何も持っていない時
        //    _animation.SetPlayerState(PlayerAnimation.PlayerState.E_HAPPY);
        //}
        //else if (_haveObject._myObject == E_FIELD_OBJECT.PLAYER_01)
        //{// プレイヤーを持つとき
        //    _animation.SetPlayerState(PlayerAnimation.PlayerState.E_HAPPY_CHARA);
        //}
        //else
        //{// プレイヤー以外を持つ時
        //    _animation.SetPlayerState(PlayerAnimation.PlayerState.E_HAPPY);
        //}
    }


    /**
     * @brief アニメーション中かどうかの判定
     * @return _animation.AnimFinish
     */
    public bool isAnim()
    {
        if (_mgr.Debug)
        {
            return true;
        }
        return _animation.GetAnimFinish();
    }


    /**
     * @brief デバッグ用関数
     * @param1 表示したい文字列
     * @return なし
     */
    public void CallDebug(object message)
    {
        Debug.Log(message);
    }


#else
    #region FiealController
    /**
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


    /**
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


    /**
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


    /**
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


    /**
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


    /**
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
    #endregion
#endif
}

// EOF