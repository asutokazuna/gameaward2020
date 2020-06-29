/**
 * @file    BulbCreate.cs
 * @brief   箱の中の水の生成
 * @author  Shun Kato
 * @date    2020/06/28      コメントをきれいに
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @class BulbCreate
 * @brief 箱の中の水を生成するクラス
 */
public class BulbCreate : MonoBehaviour
{
    private WaterFlow script;
    private bool bFirst;
    private bool bFill;
    private float fTimer;
    public float WAIT_TIME;                 //！< こぼれる水の生成待機時間

    private int nOldWater;

    public int nTargetCnt;

    //public bool ParticleSwitch;

    public AudioClip _SEwaterIn;            //!< 水の生成音
    private AudioSource _audioSource = default;

    /**
     * @brief 初期化処理
     * @return なし
     */
    void Start()
    {
        bFirst = false;
        bFill = false;
        script = this.GetComponent<WaterFlow>();
        this.GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);

        _audioSource = GetComponent<AudioSource>();
    }

    /**
     * @brief 更新処理
     * @return なし
     */
    void Update()
    {
        if(script._currentWater == 0)
        {
            bFirst = false;
            bFill = false;
            

            foreach (Transform childTransform in this.transform)
            {
                if (childTransform.tag == "WaterLeak")
                {
                    childTransform.GetComponent<ParticleSystem>().Stop(false, ParticleSystemStopBehavior.StopEmitting);
                    childTransform.GetComponent<ParticleSystem>().Clear(true);
                }
            }

            // 箱に水が入っていてなくなった時（持ち上げられたとき）
            if (nOldWater > 0)
            {
                fTimer = WAIT_TIME;
               
            }

            if(fTimer > 0.0f)
            {
                fTimer -= Time.deltaTime;
                // 箱を持ち上げてこぼれる水の生成
                if(fTimer <= 0.0f)
                {
                    fTimer = -1.0f;
                    GameObject obj = (GameObject)Resources.Load("WaterCube");

                    Vector3 pos = this.transform.position;
                    pos.y += 0.0f;

                    
                    Instantiate(obj, pos, Quaternion.Euler(-90, 0, 0));
                   
                }
            }
        }

        // 箱内の水の生成
        if (script._currentWater > nTargetCnt)
        {
            if (!bFirst && gameObject.tag == "WaterBlock")
            {
                bFirst = true;

                GameObject obj = (GameObject)Resources.Load("WaterBulb_in");

                Vector3 pos = this.transform.position;
                pos.y -= 0.2f;

                GameObject _instantObj;
                _instantObj = Instantiate(obj, pos, Quaternion.Euler(-90, 0, 0));
                _instantObj.GetComponent<BulbDestroy>()._parent = this.gameObject;

                _audioSource.PlayOneShot(_SEwaterIn);
            }
        }

        /* 使われていない処理 */
        //if (script._isFullWater)
        //{
        //    if (!bFill)
        //    {
        //        bFill = true;
        //
        //        if(ParticleSwitch)
        //        {
        //            this.GetComponent<ParticleSystem>().Play(true);
        //        }
        //    }
        //}


        nOldWater = script._currentWater;
    }
}

// EOF