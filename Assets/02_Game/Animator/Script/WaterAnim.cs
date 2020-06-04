/**
 * @file    WaterAnim.cs
 * @brief   水の満たされた箱の個数に応じたUI管理
 * @author  Risa Ito
 * @date    2020/05/30(土)  作成
 * @date    2020/06/04(木)  減った時の挙動修正
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @class WaterAnim
 * @brief 箱の全体数や水の満たされた箱の個数を表示するUI管理
 */
public class WaterAnim : MonoBehaviour
{
    Animator    _waterAnim;     //!< アニメーション管理用
    int         _oldCount;      //!< 前回分保存用

    // Start is called before the first frame update
    void Start()
    {
        _oldCount = -1;
        _waterAnim = GetComponent<Animator>();
    }

    /**
    * @brief        満たされた箱の数に応じたUIのアニメーションのセット
    * @param[in]    int どの程度満たされたか
    * @return       なし
    * @details      満たされた箱の数に応じたUIのアニメーションをセットする関数です
    */
    public void SetWaterAnim(int count)
    {
        // 増えた時
        if (count > _oldCount)
        {
            for (int i = 1; i < count + 1; i++)
            {
                switch (i)
                {
                    case 1:
                        _waterAnim.SetBool("SetWater", true);
                        break;
                    case 2:
                        _waterAnim.SetBool("WaterAnimQuater", true);
                        break;
                    case 3:
                        _waterAnim.SetBool("WaterAnimHalf", true);
                        break;
                    case 4:
                        _waterAnim.SetBool("WaterAnimThreeQuaters", true);
                        break;
                    case 5:
                        _waterAnim.SetBool("WaterAnim", true);
                        break;
                }
            }
        }
        // 減った時
        else
        {
            for (int i = _oldCount; count <= i; i--)
            {
                switch (i)
                {
                    case 0:
                        _waterAnim.SetBool("SetWater", false);
                        break;
                    case 1:
                        _waterAnim.SetBool("WaterAnimQuater", false);
                        break;
                    case 2:
                        _waterAnim.SetBool("WaterAnimHalf", false);
                        break;
                    case 3:
                        _waterAnim.SetBool("WaterAnimThreeQuaters", false);
                        break;
                }
            }
        }

        // 値を保存
        _oldCount = count;
    }
}
