/*
 * @file	Controller.cs
 * @brief   入出力装置
 *
 * @author	Kota Nakagami
 * @data    2020/04/21(水)   クラス作成
 *
 * @version	1.00
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * @enum 入力種類
 */
public enum E_INPUT
{
    A,          // A
    B,          // B
    X,          // X
    Y,          // Y
    LB,         // LB
    RB,         // RB
    BACK,       // Back
    START,      // Start
    L_STICK,    // Lスティック押し込み
    R_STICK,    // Rスティック押し込み
    D_PAD_H,    // 十字キー横軸
    D_PAD_V,    // 十字キー縦軸
    MAX,        // 最大値
}


/*
 * @enum 入力モード
 */
public enum E_INPUT_MODE
{
    PUSH,       // 押されたら
    TRIGGER,    // 押した瞬間
    RELEASE,    // 離された瞬間
    REPEAT,     // リピート
}


/*
 * @class Controller
 * @brief キーボード、Xboxの入出力
 */
public class Controller : MonoBehaviour
{

    string[] _name = {
        "joystick button 0",    // A
        "joystick button 1",    // B
        "joystick button 2",    // X
        "joystick button 3",    // Y
        "joystick button 4",    // LB
        "joystick button 5",    // RB
        "joystick button 6",    // Back
        "joystick button 7",    // Start
        "joystick button 8",    // Lスティック押し込み
        "joystick button 9",    // Rスティック押し込み
        "D_Pad_H",              // 十字キー横軸
        "D_Pad_V",              // 十字キー縦軸
        null,                   // 最大値
    };



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }


    /*
     * @brief ボタンの取得
     * @param1 ゲームパッド列挙
     * @param2 入力モード
     * @return 入力モードでボタンが反応したら true
     */
    public bool GetKey(E_INPUT input, E_INPUT_MODE mode)
    {
        switch (mode)
        {
            case E_INPUT_MODE.PUSH :
                if (Input.GetKey(_name[(int)input]))
                {
                    return true;
                }
                break;

            case E_INPUT_MODE.TRIGGER :
                if (Input.GetKeyDown(_name[(int)input]))
                {
                    return true;
                }
                break;

            case E_INPUT_MODE.RELEASE :
                if (Input.GetKeyUp(_name[(int)input]))
                {
                    return true;
                }
                break;
        }

        return false;
    }
}
