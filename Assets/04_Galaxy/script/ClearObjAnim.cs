/**
 * @file    ClearObjAnim.cs
 * @brief   クリア後のオブジェクトのアニメーション管理
 * @author  Risa Ito
 * @date    2020/06/06(土)   作成
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @class ClearObjAnim
 * @brief クリア後のオブジェクトのアニメーション管理
 */
public class ClearObjAnim : MonoBehaviour
{
    [SerializeField] Animator[]     _setObject;     //!< アニメーション管理用
    [SerializeField] float          _startTime;     //!< 開始時間
    private int                     _count;         //!< 順番管理用
    private ClearObject             _clearObject;   //!< フラグ取得用

    // Start is called before the first frame update
    void Start()
    {
        // 初期化
        _clearObject = GameObject.Find("ClearObjectManager").GetComponent<ClearObject>();
        _count = 0;

        float time;

        for (int i = 0; i < _setObject.Length; i++)
        {
            // 演出が必要かどうかチェック
            if (_clearObject._isChange)
            {
                time = _startTime + i * 0.1f;    // タイミング計算
                Invoke("SetObject", time);
            }
            else
            {
                _setObject[i].SetBool("Finished", true);    // 演出不要
            }
        }
    }

    /**
    * @brief        アニメーション開始セット
    * @return       なし
    * @details      クリアオブジェクトが出てくる演出をセットする関数です
    */
    void SetObject()
    {
        _setObject[_count].SetBool("Set", true);
        _count++;
    }
}
