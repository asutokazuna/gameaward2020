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
    A,              // A
    B,              // B
    X,              // X
    Y,              // Y
    LB,             // LB
    RB,             // RB
    BACK,           // Back
    START,          // Start
    L_STICK,        // Lスティック押し込み
    R_STICK,        // Rスティック押し込み
    D_PAD_RIGHT,    // 十字キー右
    D_PAD_LEFT,     // 十字キー左
    D_PAD_UP,       // 十字キー上
    D_PAD_DOWN,     // 十字キー下
    L_STICK_RIGHT,  // Lスティック右
    L_STICK_LEFT,   // Lスティック左
    L_STICK_UP,     // Lスティック上
    L_STICK_DOWN,   // Lスティック下
    R_STICK_RIGHT,  // Rスティック右
    R_STICK_LEFT,   // Rスティック左
    R_STICK_UP,     // Rスティック上
    R_STICK_DOWN,   // Rスティック下
    MAX,            // 最大値
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
 * @enum 入力システム
 */
public enum E_INPUT_SYSTEM
{
    KEYBOARD,   // キーボード
    GAME_PAD,   // ゲームパッド
}



/*
 * @class Controller
 * @brief キーボード、Xboxの入出力
 */
public class Controller : MonoBehaviour
{

    [SerializeField] float sensitivity;
    [SerializeField] E_INPUT_SYSTEM inputSystem;

    string[] _name = {
        "joystick button 0",        // A
        "joystick button 1",        // B
        "joystick button 2",        // X
        "joystick button 3",        // Y
        "joystick button 4",        // LB
        "joystick button 5",        // RB
        "joystick button 6",        // Back
        "joystick button 7",        // Start
        "joystick button 8",        // Lスティック押し込み
        "joystick button 9",        // Rスティック押し込み
        "D_Pad_H","D_Pad_H",        // 十字キー横軸
        "D_Pad_V","D_Pad_V",        // 十字キー縦軸
        "L_Stick_H","L_Stick_H",    // Lスティック横軸
        "L_Stick_V","L_Stick_V",    // Lスティック縦軸
        "R_Stick_H","R_Stick_H",    // Rスティック横軸
        "R_Stick_V","R_Stick_V",    // Rスティック縦軸
        null,                       // 最大値
    };


    // Start is called before the first frame update
    void Start()
    {
        if (sensitivity <= 0)
        {
            sensitivity = 0.5f; // デフォルト値
        }
        else if (sensitivity > 1)
        {
            sensitivity = 1f; // でか過ぎ
        }
    }

    // Update is called once per frame
    void Update()
    {

    }


    /*
     * @brief 入力
     * @param1 入力モード
     * @param2 ボタン
     * @return 入力モードでボタンが反応したら true
     */
    public bool isInput(E_INPUT_MODE mode, E_INPUT key)
    {
        if (inputSystem == E_INPUT_SYSTEM.KEYBOARD)
        {// キーボード
            return isInputKeyboard(key, mode);
        }
        else if (inputSystem == E_INPUT_SYSTEM.GAME_PAD)
        {// ゲームパッド
            return isInputGamePad(key, mode);
        }
        return false; // エラー、入力装置が機能していない
    }


    /*
     * @brief キーボードでの入力
     * @param1 キーコード
     * @param2 入力モード
     * @return 入力モードでボタンが反応したら true
     */
    private bool isInputKeyboard(E_INPUT key, E_INPUT_MODE mode)
    {
        if (key == E_INPUT.D_PAD_RIGHT || key == E_INPUT.D_PAD_LEFT ||
            key == E_INPUT.D_PAD_UP || key == E_INPUT.D_PAD_DOWN)
        {// 十字キーの場合
            return isDPad(key, mode);
        }
        else if (key == E_INPUT.L_STICK_RIGHT || key == E_INPUT.L_STICK_LEFT ||
                 key == E_INPUT.L_STICK_UP || key == E_INPUT.L_STICK_DOWN)
        {// Lスティックの場合
            return isLStick(key, mode);
        }
        else if (key == E_INPUT.R_STICK_RIGHT || key == E_INPUT.R_STICK_LEFT ||
                 key == E_INPUT.R_STICK_UP || key == E_INPUT.R_STICK_DOWN)
        {// Lスティックの場合
            return isRStick(key, mode);
        }
        else if (key == E_INPUT.B)
        {// Bボタン
            if (mode == E_INPUT_MODE.PUSH) return Input.GetKey(KeyCode.Space);
            if (mode == E_INPUT_MODE.TRIGGER) return Input.GetKeyDown(KeyCode.Space);
            if (mode == E_INPUT_MODE.RELEASE) return Input.GetKeyUp(KeyCode.Space);
        }
        else if (key == E_INPUT.LB)
        {// LBボタン
            if (mode == E_INPUT_MODE.PUSH) return Input.GetKey(KeyCode.LeftShift);
            if (mode == E_INPUT_MODE.TRIGGER) return Input.GetKeyDown(KeyCode.LeftShift);
            if (mode == E_INPUT_MODE.RELEASE) return Input.GetKeyUp(KeyCode.LeftShift);
        }


        return false;   // ゲームパッドで設定されてない値
    }


    /*
     * @brief ゲームパッドでの入力
     * @param1 ゲームパッド列挙
     * @param2 入力モード
     * @return 入力モードでボタンが反応したら true
     */
    private bool isInputGamePad(E_INPUT key, E_INPUT_MODE mode)
    {
        if (key == E_INPUT.D_PAD_RIGHT || key == E_INPUT.D_PAD_LEFT ||
            key == E_INPUT.D_PAD_UP || key == E_INPUT.D_PAD_DOWN)
        {// 十字キーの場合
            return isDPad(key, mode);
        }
        else if (key == E_INPUT.L_STICK_RIGHT || key == E_INPUT.L_STICK_LEFT ||
                 key == E_INPUT.L_STICK_UP || key == E_INPUT.L_STICK_DOWN)
        {// Lスティックの場合
            return isLStick(key, mode);
        }
        else if (key == E_INPUT.R_STICK_RIGHT || key == E_INPUT.R_STICK_LEFT ||
                 key == E_INPUT.R_STICK_UP || key == E_INPUT.R_STICK_DOWN)
        {// Lスティックの場合
            return isRStick(key, mode);
        }

        switch (mode)
        {// ボタンの押し込み
            case E_INPUT_MODE.PUSH :
                if (Input.GetKey(_name[(int)key]))
                {
                    return true;
                }
                break;

            case E_INPUT_MODE.TRIGGER :
                if (Input.GetKeyDown(_name[(int)key]))
                {
                    return true;
                }
                break;

            case E_INPUT_MODE.RELEASE :
                if (Input.GetKeyUp(_name[(int)key]))
                {
                    return true;
                }
                break;
        }
        return false;   // 列挙値のエラー
    }


    /*
     * @brief 十字キーの取得
     * @param1 入力ボタン
     * @param2 入力モード (まだ未対応)
     * @return 入力モードで反応したら true
     */
    private bool isDPad(E_INPUT key, E_INPUT_MODE mode)
    {
        if (inputSystem == E_INPUT_SYSTEM.KEYBOARD)
        {

        }
        else if (inputSystem == E_INPUT_SYSTEM.GAME_PAD)
        {
            if (key == E_INPUT.D_PAD_LEFT)
            {// 左
                if (Input.GetAxis(_name[(int)key]) < -sensitivity)
                    return true;
            }
            else if (key == E_INPUT.D_PAD_RIGHT)
            {// 右
                if (Input.GetAxis(_name[(int)key]) > sensitivity)
                    return true;
            }
            else if (key == E_INPUT.D_PAD_UP)
            {// 上
                if (Input.GetAxis(_name[(int)key]) > sensitivity)
                    return true;
            }
            else if (key == E_INPUT.D_PAD_DOWN)
            {// 下
                if (Input.GetAxis(_name[(int)key]) < -sensitivity)
                    return true;
            }
        }

        return false;
    }


    /*
     * @brief Lスティックの取得
     * @param1 入力ボタン
     * @param2 入力モード (まだ未対応)
     * @return 入力モードで反応したら true
     */
    private bool isLStick(E_INPUT key, E_INPUT_MODE mode)
    {
        if (inputSystem == E_INPUT_SYSTEM.KEYBOARD)
        {
            if (key == E_INPUT.L_STICK_LEFT)
            {
                if (mode == E_INPUT_MODE.PUSH) return Input.GetKey(KeyCode.A);
                if (mode == E_INPUT_MODE.TRIGGER) return Input.GetKeyDown(KeyCode.A);
                if (mode == E_INPUT_MODE.RELEASE) return Input.GetKeyUp(KeyCode.A);
            }
            if (key == E_INPUT.L_STICK_RIGHT)
            {
                if (mode == E_INPUT_MODE.PUSH) return Input.GetKey(KeyCode.D);
                if (mode == E_INPUT_MODE.TRIGGER) return Input.GetKeyDown(KeyCode.D);
                if (mode == E_INPUT_MODE.RELEASE) return Input.GetKeyUp(KeyCode.D);
            }
            if (key == E_INPUT.L_STICK_UP)
            {
                if (mode == E_INPUT_MODE.PUSH) return Input.GetKey(KeyCode.W);
                if (mode == E_INPUT_MODE.TRIGGER) return Input.GetKeyDown(KeyCode.W);
                if (mode == E_INPUT_MODE.RELEASE) return Input.GetKeyUp(KeyCode.W);
            }
            if (key == E_INPUT.L_STICK_DOWN)
            {
                if (mode == E_INPUT_MODE.PUSH) return Input.GetKey(KeyCode.S);
                if (mode == E_INPUT_MODE.TRIGGER) return Input.GetKeyDown(KeyCode.S);
                if (mode == E_INPUT_MODE.RELEASE) return Input.GetKeyUp(KeyCode.S);
            }
        }
        else if (inputSystem == E_INPUT_SYSTEM.GAME_PAD)
        {
            if (key == E_INPUT.L_STICK_RIGHT)
            {// 右
                if (Input.GetAxis(_name[(int)key]) > sensitivity)
                    return true;
            }
            else if (key == E_INPUT.L_STICK_LEFT)
            {// 左
                if (Input.GetAxis(_name[(int)key]) < -sensitivity)
                    return true;
            }
            else if (key == E_INPUT.L_STICK_UP)
            {// 上
                if (Input.GetAxis(_name[(int)key]) > sensitivity)
                    return true;
            }
            else if (key == E_INPUT.L_STICK_DOWN)
            {// 下
                if (Input.GetAxis(_name[(int)key]) < -sensitivity)
                    return true;
            }
        }

        return false;
    }


    /*
     * @brief Rスティックの取得
     * @param1 入力ボタン
     * @param2 入力モード (まだ未対応)
     * @return 入力モードで反応したら true
     */
    private bool isRStick(E_INPUT key, E_INPUT_MODE mode)
    {
        if (inputSystem == E_INPUT_SYSTEM.KEYBOARD)
        {
            if (key == E_INPUT.R_STICK_LEFT)
            {
                if (mode == E_INPUT_MODE.PUSH) return Input.GetKey(KeyCode.LeftArrow);
                if (mode == E_INPUT_MODE.TRIGGER) return Input.GetKeyDown(KeyCode.LeftArrow);
                if (mode == E_INPUT_MODE.RELEASE) return Input.GetKeyUp(KeyCode.LeftArrow);
            }
            if (key == E_INPUT.R_STICK_RIGHT)
            {
                if (mode == E_INPUT_MODE.PUSH) return Input.GetKey(KeyCode.RightArrow);
                if (mode == E_INPUT_MODE.TRIGGER) return Input.GetKeyDown(KeyCode.RightArrow);
                if (mode == E_INPUT_MODE.RELEASE) return Input.GetKeyUp(KeyCode.RightArrow);
            }
            if (key == E_INPUT.R_STICK_UP)
            {
                if (mode == E_INPUT_MODE.PUSH) return Input.GetKey(KeyCode.UpArrow);
                if (mode == E_INPUT_MODE.TRIGGER) return Input.GetKeyDown(KeyCode.UpArrow);
                if (mode == E_INPUT_MODE.RELEASE) return Input.GetKeyUp(KeyCode.UpArrow);
            }
            if (key == E_INPUT.R_STICK_DOWN)
            {
                if (mode == E_INPUT_MODE.PUSH) return Input.GetKey(KeyCode.DownArrow);
                if (mode == E_INPUT_MODE.TRIGGER) return Input.GetKeyDown(KeyCode.DownArrow);
                if (mode == E_INPUT_MODE.RELEASE) return Input.GetKeyUp(KeyCode.DownArrow);
            }
        }
        else if (inputSystem == E_INPUT_SYSTEM.GAME_PAD)
        {
            if (key == E_INPUT.R_STICK_RIGHT)
            {// 右
                if (Input.GetAxis(_name[(int)key]) > sensitivity)
                    return true;
            }
            else if (key == E_INPUT.R_STICK_LEFT)
            {// 左
                if (Input.GetAxis(_name[(int)key]) < -sensitivity)
                    return true;
            }
            else if (key == E_INPUT.R_STICK_UP)
            {// 上
                if (Input.GetAxis(_name[(int)key]) > sensitivity)
                    return true;
            }
            else if (key == E_INPUT.R_STICK_DOWN)
            {// 下
                if (Input.GetAxis(_name[(int)key]) < -sensitivity)
                    return true;
            }
        }
        return false;
    }
}
