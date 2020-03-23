using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MainCamera : MonoBehaviour
{
    //フィールドを変数に格納
    public GameObject Field;
    //回転角度
    float fAngle = 0.0f;
    ////回転させるスピード
    public float fRotateflame = 60.0f;
    //回転する時間
    int nCnt = 0;
    // Use this for initialization
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        //初期化
        if (nCnt == 0 && fAngle == 0.0f)
        {
            nCnt = (int)fRotateflame / 1;
        }
        
        //左右矢印キーで回転
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            fAngle = 90.0f / fRotateflame;
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            fAngle = -90.0f / fRotateflame;
        }

        //フィールド位置情報
        Vector3 fieldPos = Field.transform.position;
        //カウント中は回転
        if (nCnt > 0 && fAngle != 0)
        {
            //カメラを回転させる
           transform.RotateAround(fieldPos, Vector3.up, fAngle);
           //transform.RotateAround(new Vector3(0.0f,0.0f,0.0f), Vector3.up, fAngle);

            nCnt -= 1;
        }
        //回転フレーム数終了で初期化
        else if (nCnt <= 0 && fAngle != 0)
        {
            nCnt = (int)fRotateflame;
            fAngle = 0.0f;
        }
     //  Debug.Log(nCnt);
    }
}

