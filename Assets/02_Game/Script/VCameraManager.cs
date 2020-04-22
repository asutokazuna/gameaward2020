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
    int _cnt = 0;
    bool _componentFlg = false;
    GameObject gameObject; //!< プレイヤー

    void Start()
    {
        //!< タグだと個別フォーカスできないかも？
        gameObject = GameObject.FindGameObjectWithTag("Player");
        //!< クリア状態取得
        _gameClear = GameObject.FindWithTag("Map").GetComponent<Map>()._gameClear; ;
    }
    void Update()
    {
        _gameClear = GameObject.FindWithTag("Map").GetComponent<Map>()._gameClear; ;

        //!< Fキーで起動（デバッグ）
        if (Input.GetKeyDown(KeyCode.F))
        {
            vcamera.Priority = 100; //!< 優先度変化？よくわからん

            //!< 有効化・無効化
            GetComponent<CinemachineVirtualCamera>().enabled =
                !GetComponent<CinemachineVirtualCamera>().enabled;//カメラ追跡のオンオフ
            _componentFlg = !_componentFlg;

        }
        if (_componentFlg)
        {
            _cnt++;
        }
        if (_cnt > 1)
        {
            vcamera.Follow = gameObject.transform;//追跡対象の設定
            vcamera.LookAt = gameObject.transform;//追跡対象の設定
        }


        if (_gameClear)
        {
            vcamera.Priority = 100;
            // 有効化・無効化
            GetComponent<CinemachineVirtualCamera>().enabled =
                !GetComponent<CinemachineVirtualCamera>().enabled;//カメラ追跡のオンオフ

            _componentFlg = !_componentFlg;

        }
        //  vcamera.Follow = gameObject.transform;//追跡対象の設定
        //  vcamera.LookAt = gameObject.transform;//追跡対象の設定
    }
}


