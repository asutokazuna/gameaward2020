/**
 * @file ファイル名　VCameraManager.cs
 * @brief ファイル概要　バーチャルカメラのマネージャー
 * @author 作成者　柳井魁星
 * @date 2020/4/16 作成
 * @date 2020/4/22 更新内容 クリア判定の取得
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Runtime.InteropServices;

/**
 * @class VCameraManager
 * @brief VisuaCameraのクラス
 */

public class VCameraManager : MonoBehaviour
{
    public CinemachineVirtualCamera vcamera;

    //Map _mapScript; //!< クリア状態取得オブジェクト

    bool _gameClear; //!< クリア状態
    bool _gameOver; //!< ゲームオーバー状態
    bool _systemflg;
    int _cnt = 0;
    int _listCnt;//!<リストの要素数カウント
    bool _componentFlg = false;
    List<GameObject> _gameObjectOver; //!< フォーカスオブジェクト
    GameObject _gameObjectPlayer;
    Vector3 CameraPos;
    void Start()
    {
        _gameObjectPlayer = GameObject.FindGameObjectWithTag("Player");//!< タグだと個別フォーカスできないかも？

        //!< クリア状態取得
        _gameClear = GameObject.FindWithTag("Map").GetComponent<Map>()._gameClear;
        _gameOver = false;

        CameraPos = vcamera.Follow.position;
    }
    void Update()
    {
        GameClear();
        GameOver();

      
    }

    void GameClear()
    {
        _gameClear = GameObject.FindWithTag("Map").GetComponent<Map>()._gameClear;

        if (_gameClear && !_systemflg)
        {
            vcamera.Priority = 100;//!< 優先度変化？よくわからん
            // 有効化・無効化
            GetComponent<CinemachineVirtualCamera>().enabled =
                !GetComponent<CinemachineVirtualCamera>().enabled;//カメラ追跡のオンオフ
            _systemflg = true;
            _componentFlg = !_componentFlg;
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


            if (_cnt > 30)
            {
                vcamera.Follow = _gameObjectPlayer.transform;//追跡対象の設定
                vcamera.LookAt = _gameObjectPlayer.transform;//追跡対象の設定

               // vcamera.Follow.position = CameraPos * _cnt;
            }
        }
    }

    void GameOver()
    {
        _gameOver = GameObject.FindWithTag("Map").GetComponent<Map>()._gameOver;

        if (_gameOver)
        {
            vcamera.Priority = 100;//!< 優先度変化？よくわからん
            // 有効化・無効化
            GetComponent<CinemachineVirtualCamera>().enabled =
                !GetComponent<CinemachineVirtualCamera>().enabled;//カメラ追跡のオンオフ

            _componentFlg = !_componentFlg;


            if (_componentFlg)
            {
                _cnt++;
            }
            if (_cnt > 1)
            {
                //!< タグだと個別フォーカスできないかも？
                _gameObjectOver = GameObject.FindWithTag("Map").GetComponent<Map>().GetGameOverObjects();
                _listCnt = _gameObjectOver.Count - 1;
                vcamera.Follow = _gameObjectOver[_listCnt].transform;//追跡対象の設定
               // Vector3 _followObject = vcamera.Follow.position;
               // _followObject.y -= 2;
               // vcamera.Follow.position = _followObject;
                vcamera.LookAt = _gameObjectOver[_listCnt].transform;//追跡対象の設定

              //vcamera.
            }
        }
    }


}



