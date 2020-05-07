/**
     * @file MainCamera.cs
     * @brief カメラ制御
     * @author 柳井魁星
     * @date 2020/03/24 作成
     * @date 2020/03/30 カメラの初期値設定と、フィールド中心座標の取得
 */


using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using DG.Tweening;
using System.Diagnostics;
//!<using Boo.Lang.Environments;
/**
    * @class MainCamera
    * @brief カメラ回転
*/
public class MainCamera : MonoBehaviour
{
    //!<フィールドブロックを変数に格納
    //!<配列にしたかったけどやり方わからなかった
    [SerializeField] List<GameObject> _fieldBlock;

 
    public float _angle = 90.0f; //!<カメラを回転させたときに回転する角度
    public float _rotateFlame = 60.0f; //!<回転時間
    public Vector3 _setCameraPos; //!<カメラ座標設定
    public Vector3 _correctionValueClear;//!<クリア時のカメラの補正値
    public Vector3 _correctionValueOver;//!<ゲームオーバー時のカメラの補正値

    Vector3 _fieldPos; //!<フィールド中心座標
    int _cnt = 0; //!<フレーム数カウンター
    bool _gameClear; //!< クリア状態
    bool _gameOver; //!< ゲームオーバー状態
    bool _systemflg;//!<クリア演出のフラグ
    bool _initFlg=false;//!<Start演出後のInitフラグ
    bool _cameraMove = false;
    int _listCnt = 0;//!<リストの要素数カウント
    float _TimeCnt = 1.0f;
    float _time2 = 0;//!<演出用タイマー
    float _flameMoveSpeed;//!<1タイムあたりの移動量
    //!<float _oldtime = 0;
    public float _waitTime = 0.0f;
    List<GameObject> _gameObjectOver; //!< フォーカスオブジェクト
    GameObject _gameObjectPlayer; //!< Playerオブジェクトの格納
    Transform myTransform;//!<カメラのトランスフォーム
    Quaternion _setCameraRot = new Quaternion(30, 10, 0, 0); //!<カメラ注視点設定
    Vector3 _cameraPos;　//!<<!カメラ座標いじる用
    Quaternion _holdCameraRotate;//!<CameraのRotate保存
    Vector3 _lookAtObject; //!<<!追跡オブジェクト
    Vector3Int _direct;//!<プレイヤーの方向取得　複数キャラバグの予感
    

    [SerializeField] float _rotateTimeClear = 8;//!<!（手ブレ軽減用(数字が大きいほど手ブレが減り回転が遅くなる))
    [SerializeField] float _rotateSpeedClear = 2;//!<回転するはやさ（数字が小さいほど回転が早くなる）
    [SerializeField] Vector3 _circleSizeClear = new Vector3(4, 2, 4);//!<回転するときの円の大きさ
    [SerializeField] bool _focusClear = true;//!<フォーカス対象の設定（trueがPlayer,falseがFieldCenter)


    [SerializeField] float _rotateTimeStart = 8;//!<!（手ブレ軽減用(数字が大きいほど手ブレが減り回転が遅くなる))
    [SerializeField] float _rotateSpeedStart = 1.3f;//!<回転するはやさ（数字が小さいほど回転が早くなる）
    [SerializeField] Vector3 _circleSizeStart = new Vector3(8, 0, 8);//!<回転するときの円の大きさ
    [SerializeField] bool _focusStart = true;//!<フォーカス対象の設定（trueがPlayer,falseがFieldCenter)
    [SerializeField] float _startHigh = 14.0f;//!<スタート演出のカメラの高さ
    [SerializeField] float _startTime = 8.0f;//!<スタート演出秒数
    [SerializeField] float _skipTime = 2.0f;//!<スキップ時や最後補正するときのカメラ移動時間

    float _holdStartTime;//!<スタート演出の秒数保存
    float _holdStartHigh;//!<スタート演出のカメラの高さ保存




    // Use this for initialization
    // @details 　publicで設定した値を設定する
    void Start()
    {
        myTransform = this.transform;//!<カメラのトランスフォーム取得
        float time = 0;//!<計算用タイマー
        time += Time.deltaTime;//!<タイム取得
        _setCameraRot = myTransform.transform.rotation;//!<Rotateの初期値保存
        _holdCameraRotate = _setCameraRot;//!<Rotateの初期値保存２
        _holdStartTime =_startTime;//!<スタート演出の秒数保存
        _holdStartHigh = _startHigh;//!<スタート演出のカメラの高さ保存
        _flameMoveSpeed = (_startHigh - _setCameraPos.y) / (_startTime / time); //!<1タイムあたりの移動量計算
        UnityEngine.Debug.Log(_flameMoveSpeed);//!<画面最大化するとずれるのでデバッグで確認
        Init();//Init
    }

    //!< Update is called once per frame
    void Update()
    {
        StartCamera();//!<スタート演出の関数
        _direct = _direct = GameObject.FindWithTag("Player").GetComponent<Player>()._direct;//Playerの方向取得　ここじゃなくてもいいかもしれない
        GameClear();//!<クリア演出の関数
        GameOver();//!<ゲームオーバー演出の関数
        CameraRotate();//!<ゲーム中のカメラ回転の関数
    }
    /**
         * @brief 関数概要　カメラの初期値設定
         * @param[in] なし
         * @param[out] なし
         * @return なし
         * @details 　publicで設定した値を設定する
     */

    public void SetCamera()
    {
        Transform myTransform = this.transform;//!<変数に取得
        myTransform.position = _setCameraPos;  //!< 座標を設定
        _cameraPos = _setCameraPos;//!<カメラ座標
      
            myTransform.transform.rotation = _holdCameraRotate;//!<カメラの向き
    }
    /**
         * @brief 関数概要　フィールド中心座標取得
         * @param[in] なし
         * @param[out] なし
         * @return なし
         * @details 　publicで設定された4隅のフィールドブロックをもとに中心座標を求めて設定する
     */
    void SetFieldCenter()
    {
        //!<フィールドx中心座標取得計算
        _fieldPos.x = (((_fieldBlock[0].transform.position.x + _fieldBlock[1].transform.position.x) / 2) + 
            ((_fieldBlock[2].transform.position.x + _fieldBlock[3].transform.position.x) / 2)) / 2;
        //!<フィールドy中心座標取得計算
        _fieldPos.y = (((_fieldBlock[0].transform.position.y + _fieldBlock[1].transform.position.y) / 2) + 
            ((_fieldBlock[2].transform.position.y + _fieldBlock[3].transform.position.y) / 2)) / 2;
        //!<フィールドz中心座標取得計算
        _fieldPos.z = (((_fieldBlock[0].transform.position.z + _fieldBlock[1].transform.position.z) / 2) + 
            ((_fieldBlock[2].transform.position.z + _fieldBlock[3].transform.position.z) / 2)) / 2;
        //!<Debug.Log(_fieldPos.x);
        //!<Debug.Log(_fieldPos.y);
        //!<Debug.Log(_fieldPos.z);

    }
    /**
         * @brief 関数概要　初期化
         * @param[in] なし
         * @param[out] なし
         * @return なし
         * @details 　ゲーム開始時に必要なものの初期化
     */

    void Init()
    {
        SetCamera();//SetCamera
        SetFieldCenter();//!<フィールド中心計算
        _gameObjectPlayer = GameObject.FindGameObjectWithTag("Player");//!< タグだと個別フォーカスできないかも？

        _gameClear = GameObject.FindWithTag("Map").GetComponent<Map>()._gameClear;//!< クリア状態初期化
        _gameOver = false;//!<ゲームオーバー状態の初期化
        _direct = GameObject.FindWithTag("Player").GetComponent<Player>()._direct;//!<プレイヤーの向き取得
    }

    /**
     * @brief 関数概要　ゲームクリア演出
     * @param[in] なし
     * @param[out] なし
     * @return なし
     * @details 　クリアフラグを取得しTrueなら演出を開始する
 */

    void GameClear()
    {
        _gameClear = GameObject.FindWithTag("Map").GetComponent<Map>()._gameClear;//!<クリア状態の取得

        if (_gameClear && !_systemflg)//クリアかつシステムフラグFalseなら
        {
            if (_waitTime > 0&&!_cameraMove)//カメラが吹っ飛ばないように
            {
                _time2 += Time.deltaTime;
                Vector3 _clearStartPos = new Vector3(_fieldPos.x + _circleSizeClear.x * Mathf.Sin(_time2), _fieldPos.y + _circleSizeClear.y, _fieldPos.z + _circleSizeClear.z * Mathf.Cos(_time2));
                myTransform.DOMove(_clearStartPos, _waitTime);
                _cameraMove = true;
            }
            _waitTime -= Time.deltaTime;
            if (_waitTime < 0)//他演出とのディレイ
            {
                //!< UnityEngine.Debug.Break();
                myTransform = this.transform;
                _systemflg = true;
            
            }

           
        }
        else if (_gameClear && _systemflg)//システムフラグtrue
        {
            if (_TimeCnt > 0)//ディレイない場合の保険で1秒は待てるように
            {
                _TimeCnt -= Time.deltaTime;
            }
            else if (_TimeCnt < 0)
            {
                Sequence _seq = DOTween.Sequence();
                _gameObjectPlayer = GameObject.FindGameObjectWithTag("Player");//!< タグだと個別フォーカスできないかも？
                if (_focusClear)
                {
                    _lookAtObject = _gameObjectPlayer.transform.position;//!<追跡対象の設定（プレイヤー）
                    _lookAtObject.y = _gameObjectPlayer.transform.position.y + _correctionValueClear.y;//y座標の補正
                }
                else
                {
                     _lookAtObject = _fieldPos;//!<追跡対象の設定（フィールド中心）
                    _lookAtObject.y = 1;//!<y座標の補正
                }
                float _time = 0;
                _time += Time.deltaTime * _rotateTimeClear;//1フレーム？あたりの移動時間取得（細かすぎると手ブレするので最低8倍）

                _time2 += Time.deltaTime / _rotateSpeedClear;//sin,cosの移動先計算用
               
                Vector3 _setPos = new Vector3(_fieldPos.x + _circleSizeClear.x * Mathf.Sin(_time2),
                    _fieldPos.y + _circleSizeClear.y, _fieldPos.z + _circleSizeClear.z * Mathf.Cos(_time2));//!<次の移動先座標計算
                _seq.Append(myTransform.DOMove(_setPos, _time));//移動
                
                myTransform.LookAt(_lookAtObject);  //!< 向きを設定
                
            }

           
        }
    }
    /**
     * @brief 関数概要　ゲームオーバー演出
     * @param[in] なし
     * @param[out] なし
     * @return なし
     * @details 　ゲームオーバーフラグを取得しTrueなら演出を開始する
 */

    void GameOver()
    {
        _gameOver = GameObject.FindWithTag("Map").GetComponent<Map>()._gameOver;

        if (_gameOver)
        {
            // _waitTime -= Time.deltaTime;
            //  if (_waitTime < 0)//ゲームオーバーにディレイが必要な場合
            //{
            Transform myTransform = this.transform;//!<変数に取得
                                                   //!< タグだと個別フォーカスできないかも？
            _gameObjectOver = GameObject.FindWithTag("Map").GetComponent<Map>().GetGameOverObjects();//ゲームオーバーの原因のオブジェクトを取得
            _listCnt = _gameObjectOver.Count;//リスト数確認
            _cameraPos = _gameObjectOver[_listCnt].transform.position - _direct + _correctionValueOver;//!<追跡対象の上にカメラ位置を設定
            _lookAtObject = _gameObjectOver[_listCnt].transform.position;//!<追跡対象にフォーカス

            myTransform.transform.DOMove(_cameraPos, 1);//移動
            myTransform.LookAt(_lookAtObject);  //!< 向きを設定
           // }
        }
    }
    /**
    * @brief 関数概要　カメラ回転
    * @param[in] なし
    * @param[out] なし
    * @return なし
    * @details 　左右矢印キーでカメラを回転
    */

    void CameraRotate()
    {
        //!<カウンター初期化
        if (_cnt == 0 && _angle == 0.0f)
        {
            _cnt = (int)_rotateFlame / 1;//
        }

        //!<左右矢印キーで回転
        if (_cnt == (int)_rotateFlame / 1)//!<回転中じゃなければ
        {
            if (GameObject.FindGameObjectWithTag("Input").GetComponent<Controller>()
                .isInput(E_INPUT_MODE.RELEASE, E_INPUT.R_STICK_LEFT))
            {
                _angle = 90.0f / _rotateFlame;//!<1フレームあたりの回転角度
            }
            else if (GameObject.FindGameObjectWithTag("Input").GetComponent<Controller>()
                .isInput(E_INPUT_MODE.RELEASE, E_INPUT.R_STICK_RIGHT))
            {
                _angle = -90.0f / _rotateFlame;//!<1フレームあたりの回転角度
            }
        }
        //!<カウント中は回転
        if (_cnt > 0 && _angle != 0)
        {
            //!<カメラを回転させる
            transform.RotateAround(_fieldPos, Vector3.up, _angle);

            _cnt -= 1;//!<カウンター減算
        }
        //!<回転フレーム数終了で初期化
        else if (_cnt <= 0 && _angle != 0)
        {
            _cnt = (int)_rotateFlame / 1;//!<カウンター初期化
            _angle = 0.0f;
        }
        //!<  Debug.Log(_cnt);
    }
    /**
    * @brief 関数概要　スタート演出
    * @param[in] なし
    * @param[out] なし
    * @return なし
    * @details 　スタート演出
    */

    void StartCamera()
    {
        if (!_initFlg)
        {
            UnityEngine.Debug.Log("Start");
            Sequence _seq = DOTween.Sequence();
            _gameObjectPlayer = GameObject.FindGameObjectWithTag("Player");//!< タグだと個別フォーカスできないかも？
            if (_focusStart)
            {
                _lookAtObject = _gameObjectPlayer.transform.position;//!<追跡対象の設定
                _lookAtObject.y = _gameObjectPlayer.transform.position.y + _correctionValueClear.y;

            }
            else
            {
                _lookAtObject = _fieldPos;
                _lookAtObject.y = 1;
            }
            float _time = 0;
            _time += Time.deltaTime;//1フレーム？あたりの移動時間取得
            _startHigh -= (_holdStartHigh - _setCameraPos.y) / (_holdStartTime / _time);//1回の移動量計算
            _time *= _rotateTimeStart;//移動時間（細かすぎると手ブレするので最低8倍）
            _time2 += Time.deltaTime / _rotateSpeedStart;
            _startTime -= Time.deltaTime;//!<スタート演出時間減算
            if(_startHigh<_cameraPos.y)//例外処理（何らかのバグによって下に行き過ぎないように
            {
                _startHigh = _cameraPos.y;
            }
            Vector3 _setPos = new Vector3(_fieldPos.x - _circleSizeStart.x * Mathf.Sin(_time2),
                _startHigh, _fieldPos.z - _circleSizeStart.z * Mathf.Cos(_time2));//!<移動先計算
           
            _seq.Append(myTransform.DOMove(_setPos, _time));//移動

            myTransform.LookAt(_lookAtObject);  //!< 向きを設定
        }
        if (!_initFlg)//初期化してなければ
        {
            if (_startTime < 0 ||Input.anyKeyDown)//ゲーム開始初期座標に移動（anyKeyでスタート演出スキップ）
            {
                myTransform.DOMove(_setCameraPos, _skipTime);
                _initFlg = true;
            }
        }
        if (_startTime < 0 || _initFlg)//!<スタート演出が終わっていたら
        {
            _skipTime -= Time.deltaTime;
            myTransform.LookAt(_lookAtObject);//!<注視点
            if (_skipTime < 0)
            {
                myTransform.transform.rotation = _holdCameraRotate;//!<どうしてもカメラ飛んじゃうかも
            }
        }
    }
}

