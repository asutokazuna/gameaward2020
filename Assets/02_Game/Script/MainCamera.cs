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
//using Boo.Lang.Environments;
/**
* @class MainCamera
* @brief カメラ回転
*/
public class MainCamera : MonoBehaviour
{
    //フィールドブロックを変数に格納
    //配列にしたかったけどやり方わからなかった
    public GameObject _fieldBlock1; //!< フィールドブロック1
    public GameObject _fieldBlock2; //!< フィールドブロック2
    public GameObject _fieldBlock3; //!< フィールドブロック3
    public GameObject _fieldBlock4; //!< フィールドブロック4

 
    public float _angle = 90.0f; //!<カメラを回転させたときに回転する角度
    public float _rotateFlame = 60.0f; //!<回転時間
    public Vector3 _setCameraPos; //!<カメラ座標設定
    public Vector3 _setCameraRot; //!<カメラ注視点設定
    public bool _manualSetCamera; //!<カメラの位置を手動設定
    public Vector3 _correctionValueClear;//!<クリア時のカメラの補正値
    public Vector3 _correctionValueOver;//!<ゲームオーバー時のカメラの補正値

    Vector3 _fieldPos; //!<フィールド中心座標
    int _cnt = 0; //!<フレーム数カウンター
    int _flgCheck;
    bool _gameClear; //!< クリア状態
    bool _gameOver; //!< ゲームオーバー状態
    bool _systemflg;
    bool[] _Doflg = { false, false };
    int _listCnt;//!<リストの要素数カウント
    float _TimeCnt = 5.0f;
    public float _waitTime = 2.0f;
    List<GameObject> _gameObjectOver; //!< フォーカスオブジェクト
    GameObject _gameObjectPlayer; //!< Playerオブジェクトの格納
    Transform myTransform;
    Vector3 _cameraPos;　//<!カメラ座標いじる用
    Vector3 _lookAtObject; //<!追跡オブジェクト
    Vector3 _startPos;
    Vector3Int _direct;
    
               


    // Use this for initialization
    // @details 　publicで設定した値を設定する
    void Start()
    {
        Init();

        
    }

    // Update is called once per frame
    void Update()
    {
        _direct = _direct = GameObject.FindWithTag("Player").GetComponent<Player>()._direct;
        GameClear();
        GameOver();
        CameraRotate();

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
        Transform myTransform = this.transform;//変数に取得
        myTransform.position = _setCameraPos;  // 座標を設定
        _cameraPos = _setCameraPos;
        if (!_manualSetCamera)
        {
            myTransform.LookAt(_setCameraRot);  // 向きを設定
        }
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
        //フィールドx中心座標取得計算
        _fieldPos.x = (((_fieldBlock1.transform.position.x + _fieldBlock2.transform.position.x) / 2) + 
            ((_fieldBlock3.transform.position.x + _fieldBlock4.transform.position.x) / 2)) / 2;
        //フィールドy中心座標取得計算
        _fieldPos.y = (((_fieldBlock1.transform.position.y + _fieldBlock2.transform.position.y) / 2) + 
            ((_fieldBlock3.transform.position.y + _fieldBlock4.transform.position.y) / 2)) / 2;
        //フィールドz中心座標取得計算
        _fieldPos.z = (((_fieldBlock1.transform.position.z + _fieldBlock2.transform.position.z) / 2) + 
            ((_fieldBlock3.transform.position.z + _fieldBlock4.transform.position.z) / 2)) / 2;
        //Debug.Log(_fieldPos.x);
        //Debug.Log(_fieldPos.y);
        //Debug.Log(_fieldPos.z);

    }
  
    void Init()
    {
        SetCamera();
        SetFieldCenter();
        _gameObjectPlayer = GameObject.FindGameObjectWithTag("Player");//!< タグだと個別フォーカスできないかも？

        //!< クリア状態取得
        _gameClear = GameObject.FindWithTag("Map").GetComponent<Map>()._gameClear;
        _gameOver = false;
        _direct = GameObject.FindWithTag("Player").GetComponent<Player>()._direct;
    }


    void GameClear()
    {
        _gameClear = GameObject.FindWithTag("Map").GetComponent<Map>()._gameClear;

        if (_gameClear && !_systemflg)
        {
            _waitTime -= Time.deltaTime;
            if (_waitTime < 0)
            {
                myTransform = this.transform;
                _systemflg = true;
                _Doflg[0] = true;
                
                // _angle = 360.0f / (_TimeCnt * 60);
            }
            /*
            //vcamera.Follow = _gameObjectPlayer.transform;//追跡対象の設定
            vcamera.LookAt = _gameObjectPlayer.transform;//追跡対象の設定
            CameraPos.x -= _gameObjectPlayer.transform.position.x;
            CameraPos.y -= _gameObjectPlayer.transform.position.y;
            CameraPos.z -= _gameObjectPlayer.transform.position.z;

            CameraPos.x /= 30;
            CameraPos.y /= 30;
            CameraPos.z /= 30;
            */
        }
        else if (_gameClear && _systemflg)
        {
            if (_TimeCnt > 0)
            {
               
                myTransform.LookAt(_fieldPos);
                
//                transform.RotateAround(_fieldPos, Vector3.up,1.0f );
                _TimeCnt -= Time.deltaTime;
                _startPos = this.transform.position;

              //  UnityEngine.Debug.Log(_angle);
            }
            
             else if (_TimeCnt < 0)
            {
                /*
                 myTransform = this.transform;//変数に取得
                _cameraPos = _gameObjectPlayer.transform.position + (_direct * 2);//追跡対象の設定
                _cameraPos.y += _correctionValueClear.y;
                Vector3 _relayPoint = new Vector3(_startPos.x - _cameraPos.x, _startPos.y - _cameraPos.y, _startPos.z - _cameraPos.z);
                _relayPoint = _relayPoint + _direct;
                Vector3[] _path = { new Vector3(_startPos.x, _startPos.y, _startPos.z), new Vector3(_relayPoint.x, _relayPoint.y, _relayPoint.z), new Vector3(_cameraPos.x, _cameraPos.x, _cameraPos.x) };

                _lookAtObject = _gameObjectPlayer.transform.position;//追跡対象の設定
                _lookAtObject.y = _gameObjectPlayer.transform.position.y + _correctionValueClear.y;

               // myTransform.transform.DOLocalPath(_path, 1.0f, PathType.CatmullRom);
                myTransform.transform.DOMove(_cameraPos, 1.0f);
            */
                _lookAtObject = _gameObjectPlayer.transform.position;//追跡対象の設定
                _lookAtObject.y = _gameObjectPlayer.transform.position.y + _correctionValueClear.y;

                myTransform.LookAt(_lookAtObject);  // 向きを設定
            }

            if (_Doflg[0] == true && _Doflg[1] == false)
            {
                Sequence seq = DOTween.Sequence();
                //myTransform.transform.DOMove(new Vector3(-4, 3, -15), 1.0f);
                _cameraPos = _gameObjectPlayer.transform.position+(_direct*2);//追跡対象の設定
                Vector3[] _path = { new Vector3(_fieldPos.x + 4, 3, _fieldPos.z - 4),
                                    new Vector3(_fieldPos.x + 4, 3, _fieldPos.z + 4),
                                    new Vector3(_fieldPos.x - 4, 3, _fieldPos.z + 4),
                                    new Vector3(_fieldPos.x - 4, 3, _fieldPos.z - 4),
                                    new Vector3(_cameraPos.x + 3, _cameraPos.y + 3.0f, _cameraPos.z - 3) ,
                                    new Vector3(_cameraPos.x + 2, _cameraPos.y + 2.0f, _cameraPos.z + 2) ,
                                    new Vector3(_cameraPos.x + 2, _cameraPos.y + 1.5f, _cameraPos.z + 2) ,
                                    new Vector3(_cameraPos.x, _cameraPos.y, _cameraPos.z) };
                //seq.Append(
                    myTransform.DOLocalPath(_path, 10.0f, PathType.CatmullRom).SetOptions(false);
               
                _Doflg[1] = true;

            }
        }
    }

    void GameOver()
    {
        _gameOver = GameObject.FindWithTag("Map").GetComponent<Map>()._gameOver;

        if (_gameOver)
        {
            // _waitTime -= Time.deltaTime;
            //  if (_waitTime < 0)
            //{
            Transform myTransform = this.transform;//変数に取得
                                                   //!< タグだと個別フォーカスできないかも？
            _gameObjectOver = GameObject.FindWithTag("Map").GetComponent<Map>().GetGameOverObjects();
            _cameraPos = _gameObjectOver[_listCnt].transform.position - _direct + _correctionValueOver;//追跡対象の設定

            _lookAtObject = _gameObjectOver[_listCnt].transform.position;//追跡対象の設定

            myTransform.transform.DOMove(_cameraPos, 1);
            myTransform.LookAt(_lookAtObject);  // 向きを設定
           // }
        }
    }

    void CameraRotate()
    {
        //カウンター初期化
        if (_cnt == 0 && _angle == 0.0f)
        {
            _cnt = (int)_rotateFlame / 1;
        }

        //左右矢印キーで回転
        if (_cnt == (int)_rotateFlame / 1)//回転中じゃなければ
        {
            if (GameObject.FindGameObjectWithTag("Input").GetComponent<Controller>()
                .isInput(E_INPUT_MODE.RELEASE, E_INPUT.R_STICK_LEFT))
            {
                _angle = 90.0f / _rotateFlame;
            }
            else if (GameObject.FindGameObjectWithTag("Input").GetComponent<Controller>()
                .isInput(E_INPUT_MODE.RELEASE, E_INPUT.R_STICK_RIGHT))
            {
                _angle = -90.0f / _rotateFlame;
            }
        }
        //カウント中は回転
        if (_cnt > 0 && _angle != 0)
        {
            //カメラを回転させる
            transform.RotateAround(_fieldPos, Vector3.up, _angle);

            _cnt -= 1;//カウンター減算
        }
        //回転フレーム数終了で初期化
        else if (_cnt <= 0 && _angle != 0)
        {
            _cnt = (int)_rotateFlame / 1;//カウンター初期化
            _angle = 0.0f;
        }
        //  Debug.Log(_cnt);
    }
}

