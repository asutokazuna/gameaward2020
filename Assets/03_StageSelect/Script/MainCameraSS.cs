using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraSS: MonoBehaviour
{
    //フィールドブロックを変数に格納
    public GameObject _stageSelectManager; //!< ステージセレクトマネージャーを中心に置く

    public float _angle = 90.0f; //!<カメラを回転させたときに回転する角度
    public float _rotateFlame = 60.0f; //!<回転時間
    public Vector3 _setCameraPos; //!<カメラ座標設定
    public Vector3 _setCameraRot; //!<カメラ注視点設定
    Vector3 _fieldPos; //!<フィールド中心座標
    
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
       
    }
    /**
     * @brief 関数概要　カメラの初期値設定
     * @param[in] なし
     * @param[out] なし
     * @return なし
     * @details 　publicで設定した値を設定する
     */

    void SetCamera()
    {
        Transform myTransform = this.transform;//変数に取得
        myTransform.position = _setCameraPos;  // 座標を設定
        myTransform.LookAt(_setCameraRot);  // 向きを設定
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
        _fieldPos.x = _stageSelectManager.transform.position.x;
        _fieldPos.y = _stageSelectManager.transform.position.y;
        _fieldPos.z = _stageSelectManager.transform.position.z;

        //Debug.Log(_fieldPos.x);
        //Debug.Log(_fieldPos.y);
        //Debug.Log(_fieldPos.z);

    }
}
