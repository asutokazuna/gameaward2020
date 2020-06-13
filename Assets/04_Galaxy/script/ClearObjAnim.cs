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
    private StageID                 _stageID;       //!< ステージID取得
    private AudioSource             _audioSource;   //!< 音再生管理
    public  AudioClip               _SEPop;         //!< 登場する時の音

    // Start is called before the first frame update
    void Start()
    {
        // 初期化
        _clearObject = GameObject.Find("ClearObjectManager").GetComponent<ClearObject>();
        _audioSource = GetComponent<AudioSource>();
        _stageID = GetComponentInParent<StageID>();
        _count = 0;

        float time;

        for (int i = 0; i < _setObject.Length; i++)
        {
            // 演出が必要かどうかチェック
            if (_stageID._stageID == _clearObject._scene)
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
        _audioSource.PlayOneShot(_SEPop);
        _setObject[_count].SetBool("Set", true);
        _count++;
    }
}
