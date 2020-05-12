/**
 * @file MainCamera.cs
 * @brief カメラ制御
 * @author 柳井魁星
 * @date 2020/03/24 作成
 * @date 2020/03/30 カメラの初期値設定と、フィールド中心座標の取得
 * @date 2020/05/07 リファクタリング
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/**
 * @class MainCamera
 * @brief カメラ回転
 */
public class MainCamera : MonoBehaviour
{
    //! フィールドブロックを変数に格納
    //! 配列にしたかったけどやり方わからなかった
    [SerializeField] List<GameObject> _fieldBlock;

 
    public float _angle = 90.0f;                //!< カメラを回転させたときに回転する角度
    [SerializeField] float _rotateTime = 1.0f;  //!< 回転時間
    private Vector3 _setCameraPos;               //!< カメラ座標設定
    [SerializeField] Vector3 _correctionValueClear = new Vector3(0, 0.5f, 0);       //!< クリア時のカメラの補正値
    [SerializeField] Vector3 _correctionValueOver = new Vector3(0, 2, 0);     //!< ゲームオーバー時のカメラの補正値
    [SerializeField] float _gameOverDistance = 2.0f;
    Vector3 _fieldPos;                          //!< フィールド中心座標
    int _cnt = 0;                               //!< フレーム数カウンター
    bool _gameClear;                            //!< クリア状態
    bool _gameOver;                             //!< ゲームオーバー状態
    bool _systemflg;                            //!< クリア演出のフラグ
    bool _initFlg = false;                      //!< Start演出後のInitフラグ
    int _listCnt = 0;                           //!< リストの要素数カウント
    float _time2 = 0;                           //!< 演出用タイマー
    //!<float _oldtime = 0;
    float _waitTime = 1.0f;
    float _rotTimer = 0;
    List<GameObject> _gameObjectOver;           //!< フォーカスオブジェクト
    GameObject _gameObjectPlayer;               //!< Playerオブジェクトの格納
    Transform myTransform;                      //!< カメラのトランスフォーム
    Vector3 _setCameraRot = new Vector3(30, 10, 0); //!< カメラ注視点設定
    Vector3 _cameraPos;　                       //! カメラ座標いじる用
    Vector3 _holdCameraRotate;               //!< CameraのRotate保存
    Vector3 _lookAtObject;                      //!< 追跡オブジェクト
    Vector3Int _direct;                         //!< プレイヤーの方向取得　複数キャラバグの予感
    Vector3 _calDirect;
    [SerializeField] float _gameOverTime = 0.5f;//!< ゲームオーバー時のカメラ移動時間

    [SerializeField] float _rotateSpeedClear = 2;   //!< 回転するはやさ（数字が小さいほど回転が早くなる）
    [SerializeField] Vector3 _circleSizeClear = new Vector3(4, 2, 4);//!< 回転するときの円の大きさ
    [SerializeField] bool _focusClear = true;       //!< フォーカス対象の設定（trueがPlayer,falseがFieldCenter)


    [SerializeField] float _rotateSpeedStart = 1.3f;    //!< 回転するはやさ（数字が小さいほど回転が早くなる）
    [SerializeField] Vector3 _circleSizeStart = new Vector3(8, 0, 8);//!< 回転するときの円の大きさ
    [SerializeField] bool _focusStart = true;           //!< フォーカス対象の設定（trueがPlayer,falseがFieldCenter)
    [SerializeField] float _startHigh = 14.0f;          //!< スタート演出のカメラの高さ
    [SerializeField] float _startTime = 8.0f;           //!< スタート演出秒数
    [SerializeField] float _skipTime = 2.0f;            //!< スキップ時や最後補正するときのカメラ移動時間

    float _holdStartTime;                       //!< スタート演出の秒数保存
    float _holdStartHigh;                       //!< スタート演出のカメラの高さ保存

    public bool _startMove { get; private set; }

    bool _finishStart = false;//!<スタート演出諸々が完全に終了したかのフラグ
    bool _rotateCheck = false;//!<回転中か
    int _cameraRotNum = 0;//!<カメラの位置管理
    bool _inputKey = false;//!<キー入力受付状態か
    bool _waitTimeCheck = false;//!<waitが終わってるか
    public float _delayTime = 1.0f;

    private AudioSource _audioSource;

    StageNameUI _stageNameUI;   //!< ステージ名のフェードアウトセット
    GameObject _gazingPoint;
    /**
     * @brief 初期化処理
     * @return なし
     */
    private void Awake()
    {
        _setCameraPos = transform.position;
    }

    /**
     * @brief 初期化処理
     * @return なし
     */
    void Start()
    {
        myTransform = this.transform;                   //!< カメラのトランスフォーム取得
        float time = 0;                                 //!< 計算用タイマー
        time += Time.deltaTime;                         //!< タイム取得
        _gazingPoint = GameObject.Find("GazingPoint");
        myTransform.LookAt(_gazingPoint.transform.position);
        _setCameraRot = myTransform.transform.rotation.eulerAngles; //!< Rotateの初期値保存
        _holdCameraRotate = _setCameraRot;              //!< Rotateの初期値保存２
        //UnityEngine.Debug.Log(_holdCameraRotate);
        _holdStartTime =_startTime;                     //!< スタート演出の秒数保存
        _holdStartHigh = _startHigh;                    //!< スタート演出のカメラの高さ保存
        _startMove = true;
        _systemflg = false;
        _audioSource = GetComponent<AudioSource>();
        Init();
        _stageNameUI = GameObject.Find("StageName").GetComponent<StageNameUI>();
    }

    /**
     * @brief 更新処理
     * @return なし
     */
    void Update()
    {
        _direct = _direct = GameObject.FindWithTag("Player").GetComponent<Player>()._direct;//Playerの方向取得　ここじゃなくてもいいかもしれない
        _calDirect = new Vector3(_direct.x * _gameOverDistance, _direct.y * _gameOverDistance, _direct.z * _gameOverDistance);
        //UnityEngine.Debug.Log("init" + _initFlg);
        //UnityEngine.Debug.Log("skipTime" + _skipTime);
        if (_finishStart)
        {
            DelayTime();        //!< クリア演出の関数
            GameOver();         //!< ゲームオーバー演出の関数
            CameraRotate();     //!< ゲーム中のカメラ回転の関数
        }
    }

    /**
     * @brief 更新処理
     * @return なし
     * @details 1秒に50フレーム呼ばれる
     */
    private void FixedUpdate()
    {
        StartCamera();          //!< スタート演出の関数
    }

    /**
     * @brief 更新処理
     * @return なし
     * @details 一番最後に呼ばれる更新処理
     */
    private void LateUpdate()
    {
        if (!_gameClear && !_gameOver)
        {
            myTransform.LookAt(_gazingPoint.transform.position);
        }
    }

    /**
     * @brief カメラの初期値設定
     * @return なし
     * @details publicで設定した値を設定する
     */
    public void SetCamera()
    {
        Transform myTransform = this.transform;                 //!< 変数に取得
        _cameraPos = _setCameraPos;                             //!< カメラ座標
        myTransform.DOLookAt(_gazingPoint.transform.position, 0.0f);
        //myTransform.transform.DORotate(_holdCameraRotate, 0.0f);     //!< カメラの向き
       
    }

    /**
     * @brief フィールド中心座標取得
     * @return なし
     * @details 　publicで設定された4隅のフィールドブロックをもとに中心座標を求めて設定する
     */
    void SetFieldCenter()
    {
        //! フィールドx中心座標取得計算
        _fieldPos.x = (((_fieldBlock[0].transform.position.x + _fieldBlock[1].transform.position.x) / 2) + 
            ((_fieldBlock[2].transform.position.x + _fieldBlock[3].transform.position.x) / 2)) / 2;
        //! フィールドy中心座標取得計算
        _fieldPos.y = (((_fieldBlock[0].transform.position.y + _fieldBlock[1].transform.position.y) / 2) + 
            ((_fieldBlock[2].transform.position.y + _fieldBlock[3].transform.position.y) / 2)) / 2;
        //! フィールドz中心座標取得計算
        _fieldPos.z = (((_fieldBlock[0].transform.position.z + _fieldBlock[1].transform.position.z) / 2) + 
            ((_fieldBlock[2].transform.position.z + _fieldBlock[3].transform.position.z) / 2)) / 2;
        //Debug.Log(_fieldPos);
        _fieldPos.y = _fieldPos.y + 1.5f;
    }

    /**
     * @brief 初期化
     * @return なし
     * @details 　ゲーム開始時に必要なものの初期化
     */
    void Init()
    {
        SetCamera();                    // SetCamera
        SetFieldCenter();               //!< フィールド中心計算
        _gameObjectPlayer = GameObject.FindGameObjectWithTag("Player");     //!< タグだと個別フォーカスできないかも？

        _gameClear = GameObject.FindWithTag("Map").GetComponent<Map>()._gameClear;  //!< クリア状態初期化
        _gameOver = false;//!<ゲームオーバー状態の初期化
        _direct = GameObject.FindWithTag("Player").GetComponent<Player>()._direct;  //!<プレイヤーの向き取得
    }

    /**
     * @brief ゲームクリア演出
     * @return なし
     * @details 　クリアフラグを取得しTrueなら演出を開始する
     */
    void GameClear()
    {
        
        _gameObjectPlayer = GameObject.FindGameObjectWithTag("Player"); //!< タグだと個別フォーカスできないかも？
        _lookAtObject = _gameObjectPlayer.transform.position;   //!< 追跡対象の設定（プレイヤー）
        _lookAtObject.y = _gameObjectPlayer.transform.position.y + _correctionValueClear.y; // y座標の補正
        //myTransform.LookAt(_lookAtObject);  //!< 向きを設定    
        //BGMを止める
        _audioSource.Stop();

        Vector3 _clearStartPos = new Vector3(_fieldPos.x + _circleSizeClear.x * Mathf.Sin(_time2), _fieldPos.y + _circleSizeClear.y, _fieldPos.z + _circleSizeClear.z * Mathf.Cos(_time2));
        if (_systemflg == false)
        {
            _systemflg = true;
            if (_focusClear)//フォーカス対象の設定(インスペクタで切り替え）
            {
                _lookAtObject = _gameObjectPlayer.transform.position;   //!< 追跡対象の設定（プレイヤー）
                _lookAtObject.y = _gameObjectPlayer.transform.position.y + _correctionValueClear.y; // y座標の補正
            }
            else
            {
                _lookAtObject = _fieldPos;     //!<追跡対象の設定（フィールド中心）
                _lookAtObject.y = 1;            //!<y座標の補正
            }
            myTransform.DOMove(_clearStartPos, _waitTime).OnComplete(() =>//カメラ吹っ飛ばないように移動（ホントはいらないはず）
            {
                _waitTimeCheck = true;
            });
        }
        else if (_systemflg == true)
        {
            transform.DOLookAt(_lookAtObject, 0.0f);
        }

        if (_waitTimeCheck == true)
        {
           _time2 += Time.deltaTime / _rotateSpeedClear;   //sin,cosの移動先計算用
            Vector3 _setPos = new Vector3(_fieldPos.x + _circleSizeClear.x * Mathf.Sin(_time2),
                _fieldPos.y + _circleSizeClear.y, _fieldPos.z + _circleSizeClear.z * Mathf.Cos(_time2));//!<次の移動先座標計算
            myTransform.transform.position = _setPos;
           // myTransform.DOLookAt(_lookAtObject, 0.0f);  //!< 向きを設定     


            //if (!_systemflg)  //クリアかつシステムフラグFalseなら
            //{
            //    if (_waitTime > 0 && !_cameraMove)//カメラが吹っ飛ばないように
            //    {
            //        _time2 += Time.deltaTime;

            //        _cameraMove = true;
            //    }
            //    _waitTime -= Time.deltaTime;
            //    if (_waitTime < 0)//他演出とのディレイ
            //    {
            //        //!< UnityEngine.Debug.Break();
            //        myTransform = this.transform;
            //        _systemflg = true;

            //    }
            //    //myTransform.LookAt(_fieldPos);

            //}
            //else if (_systemflg)  //システムフラグtrue
            //{
            //    if (_TimeCnt > 0)   //ディレイない場合の保険で1秒は待てるように
            //    {
            //        _TimeCnt -= Time.deltaTime;
            //    }
            //    else if (_TimeCnt < 0)
            //    {
            //        if (_focusClear)
            //        {
            //            _lookAtObject = _gameObjectPlayer.transform.position;   //!< 追跡対象の設定（プレイヤー）
            //            _lookAtObject.y = _gameObjectPlayer.transform.position.y + _correctionValueClear.y; // y座標の補正
            //        }
            //        else
            //        {
            //             _lookAtObject = _fieldPos;     //!<追跡対象の設定（フィールド中心）
            //            _lookAtObject.y = 1;            //!<y座標の補正
            //        }

            //        _time2 += Time.deltaTime / _rotateSpeedClear;   //sin,cosの移動先計算用

            //        Vector3 _setPos = new Vector3(_fieldPos.x + _circleSizeClear.x * Mathf.Sin(_time2),
            //            _fieldPos.y + _circleSizeClear.y, _fieldPos.z + _circleSizeClear.z * Mathf.Cos(_time2));//!<次の移動先座標計算
            //        myTransform.transform.position = _setPos;
            //        myTransform.LookAt(_lookAtObject);  //!< 向きを設定     
            //    }


            //}

        }
    }
    /**
     * @brief ゲームオーバー演出
     * @return なし
     * @details 　ゲームオーバーフラグを取得しTrueなら演出を開始する
     */
    void GameOver()
    {
        _gameOver = GameObject.FindWithTag("Map").GetComponent<Map>()._gameOver;
        if (_gameOver == false)
        {
            return;
        }

        if (!_systemflg)
        {
            // _waitTime -= Time.deltaTime;
            //  if (_waitTime < 0)//ゲームオーバーにディレイが必要な場合
            //{
            _gameObjectOver = GameObject.FindWithTag("Map").GetComponent<Map>().GetGameOverObjects();//ゲームオーバーの原因のオブジェクトを取得
            _listCnt = _gameObjectOver.Count;//リスト数確認
            //            _cameraPos = _gameObjectOver[_listCnt - 1].transform.position - _direct + _correctionValueOver;//!<追跡対象の上にカメラ位置を設定
            _cameraPos = _gameObjectOver[_listCnt - 1].transform.position + _calDirect + _correctionValueOver;//!<追跡対象の上にカメラ位置を設定
            _lookAtObject = _gameObjectOver[_listCnt - 1].transform.position;   //!< 追跡対象にフォーカス

            myTransform.transform.DOMove(_cameraPos, _gameOverTime);            // 移動
            _systemflg = true;
            // }
        }
        myTransform.LookAt(_lookAtObject);  //!< 向きを設定
        //BGMを止める
        _audioSource.Stop();
    }

    /**
     * @brief カメラ回転
     * @return なし
     * @details 　左右矢印キーでカメラを回転
     */
    void CameraRotate()
    {
        if (_gameClear || _gameOver)
        {
            return;
        }

        Vector3[] _pos = { new Vector3(-4, 6, -10), new Vector3(-10, 6, -1), new Vector3(-1, 6, 5), new Vector3(5, 6, -4), };

        if (!_rotateCheck && !_inputKey)//!<回転中じゃなければ
        {
            if (GameObject.FindGameObjectWithTag("Input").GetComponent<Controller>()
                .isInput(E_INPUT_MODE.RELEASE, E_INPUT.R_STICK_LEFT))//押されたら
            {
                _inputKey = true;//キー入力受付拒否
                _cameraRotNum++;//回転先座標
                if (_cameraRotNum > 3)//補正
                {
                    _cameraRotNum = 0;
                }

            }
            else if (GameObject.FindGameObjectWithTag("Input").GetComponent<Controller>()
                .isInput(E_INPUT_MODE.RELEASE, E_INPUT.R_STICK_RIGHT))//押されたら
            {
                _inputKey = true;//キー入力拒否
                _cameraRotNum--;//回転先座標
                if (_cameraRotNum < 0)//補正
                {
                    _cameraRotNum = 3;
                }
            }
        }

        if (_inputKey)//キー入力されたら
        {
            _inputKey = false;//キー入力受付
            _rotateCheck = true;//回転中
            _rotTimer = _rotateTime;
            myTransform.DOMove(_pos[_cameraRotNum], _rotateTime).OnComplete(() =>//回転が終わったら
            {
                _rotateCheck = false;//回転中じゃない
                //myTransform.DORotate(new Vector3(_holdCameraRotate.x, _holdCameraRotate.y + (_cameraRotNum * 90), 0.0f), 0.1f);//回転

            });

            //myTransform.DORotate(rot, _rotateTime);
            //myTransform.DORotate(new Vector3(_holdCameraRotate.x * 100, _holdCameraRotate.y * 100 + (_cameraRotNum * 90), _holdCameraRotate.z * 100), _rotateTime);
            //myTransform.DORotate(new Vector3(30, 10 + (_cameraRotNum * 90), 0), _rotateTime);

            // myTransform.DORotate(new Vector3(_holdCameraRotate.x, _holdCameraRotate.y + (_cameraRotNum * 90), 0.0f), _rotateTime);//回転
            //UnityEngine.Debug.Log(_holdCameraRotate);
        }
    }
    

    /**
     * @brief スタート演出
     * @return なし
     */
    void StartCamera()
    {
       
        if (_initFlg)
        {
            return;
        }

        // プレイヤーが複数いる場合がめんどくさいのでとりあえずフィールドを注視点に 要検討
        _lookAtObject = _fieldPos;
        _lookAtObject.y = 1;

        // 座標の計算
        _startHigh -= (_holdStartHigh - _setCameraPos.y) / (_holdStartTime / 0.02f); // 1回の移動量計算
        _time2 += 0.02f / _rotateSpeedStart;
        _startTime -= 0.02f; // スタート演出時間減算
        Vector3 _setPos = new Vector3(_fieldPos.x - _circleSizeStart.x * Mathf.Sin(_time2),
            _startHigh, _fieldPos.z - _circleSizeStart.z * Mathf.Cos(_time2));  //!< 移動先計算

        myTransform.transform.position = _setPos;
        myTransform.LookAt(_gazingPoint.transform.position);  //!< 向きを設定

        if (_startTime < 0 || Input.anyKeyDown)
        { // ゲーム開始初期座標に移動（anyKeyでスタート演出スキップ）
            _startTime = 0.0f;
            _initFlg = true;
            myTransform.DOMove(_setCameraPos, _skipTime).OnComplete(() =>
            {
                _startMove = false;
                _finishStart = true;
                _stageNameUI.FadeOutStageName();
           });
            myTransform.DORotate(_holdCameraRotate, _skipTime);
            //myTransform.DOLookAt(_gazingPoint.transform.position, _skipTime);
        }
      
        
    }

    void DelayTime()
    {
        _gameClear = GameObject.FindWithTag("Map").GetComponent<Map>()._gameClear;  //!<クリア状態の取得
        if (_gameClear == false)
        {
            return;
        }
        _delayTime -= Time.deltaTime;
        if (_delayTime < 0)//!<他演出とのディレイ
        {
            GameClear();
        }
    }

}

// EOF