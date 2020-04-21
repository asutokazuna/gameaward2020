/**
 * @file MainCamera.cs
 * @brief カメラ制御
 * @author 柳井魁星
 * @date 2020/03/24 作成
 * @date 2020/03/30 カメラの初期値設定と、フィールド中心座標の取得
 */

using System.Collections;
using System.Collections.Generic;
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
    Vector3 _fieldPos; //!<フィールド中心座標
    int _cnt = 0;　//!<フレーム数カウンター

    // Use this for initialization
    // @details 　publicで設定した値を設定する
    void Start()
    {
        SetCamera();
        SetFieldCenter();
    }

    // Update is called once per frame
    void Update()
    {
        //カウンター初期化
        if (_cnt == 0 && _angle == 0.0f)
        {
            _cnt = (int)_rotateFlame / 1;
        }

        //左右矢印キーで回転
        if (_cnt == (int)_rotateFlame / 1)//回転中じゃなければ
        {
            if (GameObject.FindGameObjectWithTag("Map").GetComponent<Controller>()
                .isInput(E_INPUT_MODE.RELEASE, E_INPUT.R_STICK_LEFT))
            {
                _angle = 90.0f / _rotateFlame;
            }
            else if (GameObject.FindGameObjectWithTag("Map").GetComponent<Controller>()
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
}

