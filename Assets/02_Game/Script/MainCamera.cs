/**
 * @file MainCamera.cs
 * @brief カメラ制御
 * @author 柳井魁星
 * @date 2020/03/24 作成
 * @date 2020/03/30 カメラの初期値設定と、フィールド中心座標の取得
 */

using Boo.Lang.Environments;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
/**
 * @class MainCamera
 * @brief カメラ回転
 */
public class MainCamera : MonoBehaviour
{
    //フィールドブロックを変数に格納
    //配列にしたかったけどやり方わからなかった
    public GameObject __fieldBlock1; //!< フィールドブロック1
    public GameObject __fieldBlock2; //!< フィールドブロック2
    public GameObject __fieldBlock3; //!< フィールドブロック3
    public GameObject __fieldBlock4; //!< フィールドブロック4

 
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
    int _listCnt;//!<リストの要素数カウント
    public float _waitTime = 2.0f;
    List<GameObject> _gameObjectOver; //!< フォーカスオブジェクト
    GameObject _gameObjectPlayer; //!< Playerオブジェクトの格納
    Vector3 _cameraPos;　//<!カメラ座標いじる用
    Vector3 _lookAtObject; //<!追跡オブジェクト


    // Use this for initialization
    // @details 　publicで設定した値を設定する
    void Start()
    {
        Init();

        
    }

    // Update is called once per frame
    void Update()
    {
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
        _fieldPos.x = (((__fieldBlock1.transform.position.x + __fieldBlock2.transform.position.x) / 2) + 
            ((__fieldBlock3.transform.position.x + __fieldBlock4.transform.position.x) / 2)) / 2;
        //フィールドy中心座標取得計算
        _fieldPos.y = (((__fieldBlock1.transform.position.y + __fieldBlock2.transform.position.y) / 2) + 
            ((__fieldBlock3.transform.position.y + __fieldBlock4.transform.position.y) / 2)) / 2;
        //フィールドz中心座標取得計算
        _fieldPos.z = (((__fieldBlock1.transform.position.z + __fieldBlock2.transform.position.z) / 2) + 
            ((__fieldBlock3.transform.position.z + __fieldBlock4.transform.position.z) / 2)) / 2;
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
    }


    void GameClear()
    {
        _gameClear = GameObject.FindWithTag("Map").GetComponent<Map>()._gameClear;

        if (_gameClear && !_systemflg)
        {
            _waitTime -= Time.deltaTime;
            if (_waitTime < 0)
            {
                _systemflg = true;
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
            _cnt++;


            //if (_cnt > 30)
            // {
            Transform myTransform = this.transform;//変数に取得
            _cameraPos = _gameObjectPlayer.transform.position;//追跡対象の設定

            _lookAtObject = _gameObjectPlayer.transform.position;//追跡対象の設定
            _lookAtObject.y = _gameObjectPlayer.transform.position.y + 0.5f;
            _cameraPos.y += _correctionValueClear.y;
            _cameraPos.z += _correctionValueClear.z;
            _flgCheck = 0;

            if (!(myTransform.position.x + 0.1f > _cameraPos.x && myTransform.position.x - 0.1 < _cameraPos.x))
            {
                if (myTransform.position.x < _cameraPos.x)
                {
                    _cameraPos.x = (_cameraPos.x * -1) / 120;
                }
                else
                {
                    _cameraPos.x /= 120;

                }
                    _flgCheck++;
            }
            else
            {
                _cameraPos.x = 0;
            }
            if (!(myTransform.position.y + 0.1f > _cameraPos.y && myTransform.position.y - 0.1 < _cameraPos.y))
            {

                if (myTransform.position.y < _cameraPos.y)
                {

                    _cameraPos.y /= 120;
                }
                else
                {
                    _cameraPos.y = (_cameraPos.y * -1) / 120;
                }
                _flgCheck++;
            }
            else
            {
                _cameraPos.y = 0;
            }
            if (!(myTransform.position.z + 0.1f > _cameraPos.z && myTransform.position.z - 0.1 < _cameraPos.z))
            {

                if (myTransform.position.z < _cameraPos.z)
                {//+
                    _cameraPos.z = (_cameraPos.z * -1) / 120;
                }
                else
                {//-
                    _cameraPos.z /= 120;
                }
                _flgCheck++;
            }
            else
            {
                _cameraPos.z = 0;
            }
                myTransform.position += _cameraPos;
            myTransform.LookAt(_lookAtObject);  // 向きを設定

                                                //  }
        }
    }

    void GameOver()
    {
        _gameOver = GameObject.FindWithTag("Map").GetComponent<Map>()._gameOver;

        if (_gameOver)
        {
            _waitTime -= Time.deltaTime;
            if (_waitTime < 0)
            {
                _systemflg = true;
            }
            if (_systemflg)
            {
                _cnt++;
            }
            if (_cnt > 1)
            {
                Transform myTransform = this.transform;//変数に取得
                //!< タグだと個別フォーカスできないかも？
                _gameObjectOver = GameObject.FindWithTag("Map").GetComponent<Map>().GetGameOverObjects();
                _listCnt = _gameObjectOver.Count - 1;

                _cameraPos = _gameObjectOver[_listCnt].transform.position;//追跡対象の設定
                _lookAtObject = _gameObjectOver[_listCnt].transform.position;//追跡対象の設定
                _cameraPos.y += _correctionValueOver.y;
                _cameraPos.z += _correctionValueOver.z;

                transform.position = _cameraPos;
                myTransform.LookAt(_lookAtObject);  // 向きを設定
            }
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

