/**
 * @file	Controller.cs
 * @brief   入出力装置
 *
 * @author	Kota Nakagami
 * @data    2020/04/21(火)   クラス作成
 * @data    2020/04/22(水)   スティック、十字キーのトリガー、リリースの実装
 *
 * @version	1.00
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
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
    MAX,            // 配列最大値
    ENY_BUTTON,     // 何かしらのキー
}


/**
 * @enum 入力モード
 */
public enum E_INPUT_MODE
{
    BUTTON,     // 押されたら
    TRIGGER,    // 押した瞬間
    RELEASE,    // 離された瞬間
    REPEAT,     // リピート
}


/**
 * @enum 入力システム
 */
public enum E_INPUT_SYSTEM
{
    KEYBOARD,   // キーボード
    GAME_PAD,   // ゲームパッド
}



/**
 * @class Controller
 * @brief キーボード、Xboxの入出力
 */
public class Controller : MonoBehaviour
{
    /**
     * @class -1～1までの値で取得するボタン
     */
    public class AxisButton
    {
        public float _now_H { get; private set; }   //!< 現在の横軸

        public float _now_V { get; private set; }   //!< 現在の縦軸

        private float _old_H;   //!< 過去の横軸
        private float _old_V;   //!< 過去の縦軸

        /**
         * @brief 更新処理
         * @param1 横軸
         * @param1 縦軸
         * @return なし
         */
        public void Update(string H, string V)
        {
            _old_H = this._now_H;
            _old_V = this._now_V;
            this._now_H = Input.GetAxis(H);
            this._now_V = Input.GetAxis(V);
            //Debug.Log("横軸 = " + _now_H + "   縦軸" + _now_V);
        }


        /**
         * @brief 傾けていれば
         * @param1 方向
         * @param2 感度
         * @return 反応したら true
         */
        public bool isButton(E_INPUT key, float sensitivity)
        {
            if (key == E_INPUT.L_STICK_RIGHT || key == E_INPUT.R_STICK_RIGHT ||
                key == E_INPUT.D_PAD_RIGHT)
            {// 右
                if (_now_H > sensitivity)
                    return true;
            }
            else if (key == E_INPUT.L_STICK_LEFT || key == E_INPUT.R_STICK_LEFT ||
                key == E_INPUT.D_PAD_LEFT)
            {// 左
                if (_now_H < -sensitivity)
                    return true;
            }
            else if (key == E_INPUT.L_STICK_UP || key == E_INPUT.R_STICK_UP ||
                key == E_INPUT.D_PAD_UP)
            {// 上
                if (_now_V > sensitivity)
                    return true;
            }
            else if (key == E_INPUT.L_STICK_DOWN || key == E_INPUT.R_STICK_DOWN ||
                key == E_INPUT.D_PAD_DOWN)
            {// 下
                if (_now_V < -sensitivity)
                    return true;
            }
            return false;
        }


        /**
         * @brief トリガー処理
         * @param1 方向
         * @param2 感度
         * @return 反応したら true
         */
        public bool isTrigger(E_INPUT key, float sensitivity)
        {
            if (key == E_INPUT.L_STICK_RIGHT || key == E_INPUT.R_STICK_RIGHT ||
                key == E_INPUT.D_PAD_RIGHT)
            {// 右
                if (_now_H > sensitivity && _old_H <= sensitivity)
                    return true;
            }
            else if (key == E_INPUT.L_STICK_LEFT || key == E_INPUT.R_STICK_LEFT ||
                key == E_INPUT.D_PAD_LEFT)
            {// 左
                if (_now_H < -sensitivity && _old_H >= -sensitivity)
                    return true;
            }
            else if (key == E_INPUT.L_STICK_UP || key == E_INPUT.R_STICK_UP ||
                key == E_INPUT.D_PAD_UP)
            {// 上
                if (_now_V > sensitivity && _old_V <= sensitivity)
                    return true;
            }
            else if (key == E_INPUT.L_STICK_DOWN || key == E_INPUT.R_STICK_DOWN ||
                key == E_INPUT.D_PAD_DOWN)
            {// 下
                if (_now_V < -sensitivity && _old_V >= -sensitivity)
                    return true;
            }
            return false;
        }

        
        /**
         * @brief トリガー処理
         * @param1 方向
         * @param2 感度
         * @return 反応したら true
         */
        public bool isRelease(E_INPUT key, float sensitivity)
        {
            if (key == E_INPUT.L_STICK_RIGHT || key == E_INPUT.R_STICK_RIGHT ||
                key == E_INPUT.D_PAD_RIGHT)
            {// 右
                if (_now_H <= sensitivity && _old_H > sensitivity)
                    return true;
            }
            else if (key == E_INPUT.L_STICK_LEFT || key == E_INPUT.R_STICK_LEFT ||
                key == E_INPUT.D_PAD_LEFT)
            {// 左
                if (_now_H >= -sensitivity && _old_H < -sensitivity)
                    return true;
            }
            else if (key == E_INPUT.L_STICK_UP || key == E_INPUT.R_STICK_UP ||
                key == E_INPUT.D_PAD_UP)
            {// 上
                if (_now_V <= sensitivity && _old_V > sensitivity)
                    return true;
            }
            else if (key == E_INPUT.L_STICK_DOWN || key == E_INPUT.R_STICK_DOWN ||
                key == E_INPUT.D_PAD_DOWN)
            {// 下
                if (_now_V >= -sensitivity && _old_V < -sensitivity)
                    return true;
            }
            return false;
        }
    }


    [Range(0, 1)] [SerializeField] float sensitivity = 0;                       // 感度
    [SerializeField] E_INPUT_SYSTEM inputSystem = E_INPUT_SYSTEM.GAME_PAD;      // 入力システム


    static private Controller _instance;            //!< 自身のインスタンス
    public AxisButton _LStick   = new AxisButton(); //!< Lスティック
    public AxisButton _RStick   = new AxisButton(); //!< Rスティック
    public AxisButton _DPad     = new AxisButton(); //!< 十字




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


    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        _LStick.Update(_name[(int)E_INPUT.L_STICK_RIGHT], _name[(int)E_INPUT.L_STICK_UP]);
        _RStick.Update(_name[(int)E_INPUT.R_STICK_RIGHT], _name[(int)E_INPUT.R_STICK_UP]);
        _DPad.Update(_name[(int)E_INPUT.D_PAD_RIGHT], _name[(int)E_INPUT.D_PAD_UP]);
    }


    /**
     * @brief 入力
     * @param1 入力モード
     * @param2 ボタン
     * @return 入力モードでボタンが反応したら true
     */
    public bool isInput(E_INPUT_MODE mode, E_INPUT key)
    {
        if (isInputKeyboard(key, mode))
        {
            return true;
        }
        else if (isInputGamePad(key, mode))
        {
            return true;
        }
        return false; // エラー
    }


    public bool isAnyTrigger()
    {
        if (Input.anyKey)
        {
            return true;
        }
        else
        {
            for (int i = 0; i < 10; i++)
            {
                if (Input.GetKeyDown(_name[(int)i]))
                {
                    return true;
                }
            }
            if (isDPad(E_INPUT.D_PAD_DOWN, E_INPUT_MODE.TRIGGER) ||
                isDPad(E_INPUT.D_PAD_UP, E_INPUT_MODE.TRIGGER) ||
                isDPad(E_INPUT.D_PAD_RIGHT, E_INPUT_MODE.TRIGGER) ||
                isDPad(E_INPUT.D_PAD_LEFT, E_INPUT_MODE.TRIGGER) ||

                isLStick(E_INPUT.D_PAD_DOWN, E_INPUT_MODE.TRIGGER) ||
                isLStick(E_INPUT.D_PAD_UP, E_INPUT_MODE.TRIGGER) ||
                isLStick(E_INPUT.D_PAD_RIGHT, E_INPUT_MODE.TRIGGER) ||
                isLStick(E_INPUT.D_PAD_LEFT, E_INPUT_MODE.TRIGGER) ||

                isRStick(E_INPUT.D_PAD_DOWN, E_INPUT_MODE.TRIGGER) ||
                isRStick(E_INPUT.D_PAD_UP, E_INPUT_MODE.TRIGGER) ||
                isRStick(E_INPUT.D_PAD_RIGHT, E_INPUT_MODE.TRIGGER) ||
                isRStick(E_INPUT.D_PAD_LEFT, E_INPUT_MODE.TRIGGER))
            {
                return true;
            }
        }
        return false;
    }


    /**
     * @brief キーボードでの入力
     * @param1 キーコード
     * @param2 入力モード
     * @return 入力モードでボタンが反応したら true
     */
    private bool isInputKeyboard(E_INPUT key, E_INPUT_MODE mode)
    {
        if (key == E_INPUT.LB)
        {
            if (mode == E_INPUT_MODE.BUTTON) return Input.GetKey(KeyCode.LeftArrow);
            if (mode == E_INPUT_MODE.TRIGGER) return Input.GetKeyDown(KeyCode.LeftArrow);
            if (mode == E_INPUT_MODE.RELEASE) return Input.GetKeyUp(KeyCode.LeftArrow);
        }
        else if (key == E_INPUT.RB)
        {
            if (mode == E_INPUT_MODE.BUTTON) return Input.GetKey(KeyCode.RightArrow);
            if (mode == E_INPUT_MODE.TRIGGER) return Input.GetKeyDown(KeyCode.RightArrow);
            if (mode == E_INPUT_MODE.RELEASE) return Input.GetKeyUp(KeyCode.RightArrow);
        }
        else if (key == E_INPUT.L_STICK_RIGHT || key == E_INPUT.L_STICK_LEFT ||
                 key == E_INPUT.L_STICK_UP || key == E_INPUT.L_STICK_DOWN)
        {// Lスティックの場合
            if (key == E_INPUT.L_STICK_LEFT)
            {
                if (mode == E_INPUT_MODE.BUTTON) return Input.GetKey(KeyCode.A);
                if (mode == E_INPUT_MODE.TRIGGER) return Input.GetKeyDown(KeyCode.A);
                if (mode == E_INPUT_MODE.RELEASE) return Input.GetKeyUp(KeyCode.A);
            }
            if (key == E_INPUT.L_STICK_RIGHT)
            {
                if (mode == E_INPUT_MODE.BUTTON) return Input.GetKey(KeyCode.D);
                if (mode == E_INPUT_MODE.TRIGGER) return Input.GetKeyDown(KeyCode.D);
                if (mode == E_INPUT_MODE.RELEASE) return Input.GetKeyUp(KeyCode.D);
            }
            if (key == E_INPUT.L_STICK_UP)
            {
                if (mode == E_INPUT_MODE.BUTTON) return Input.GetKey(KeyCode.W);
                if (mode == E_INPUT_MODE.TRIGGER) return Input.GetKeyDown(KeyCode.W);
                if (mode == E_INPUT_MODE.RELEASE) return Input.GetKeyUp(KeyCode.W);
            }
            if (key == E_INPUT.L_STICK_DOWN)
            {
                if (mode == E_INPUT_MODE.BUTTON) return Input.GetKey(KeyCode.S);
                if (mode == E_INPUT_MODE.TRIGGER) return Input.GetKeyDown(KeyCode.S);
                if (mode == E_INPUT_MODE.RELEASE) return Input.GetKeyUp(KeyCode.S);
            }
        }
        //else if (key == E_INPUT.R_STICK_RIGHT || key == E_INPUT.R_STICK_LEFT ||
        //         key == E_INPUT.R_STICK_UP || key == E_INPUT.R_STICK_DOWN)
        //{// Rスティックの場合
        //    if (key == E_INPUT.R_STICK_LEFT)
        //    {
        //        if (mode == E_INPUT_MODE.BUTTON) return Input.GetKey(KeyCode.LeftArrow);
        //        if (mode == E_INPUT_MODE.TRIGGER) return Input.GetKeyDown(KeyCode.LeftArrow);
        //        if (mode == E_INPUT_MODE.RELEASE) return Input.GetKeyUp(KeyCode.LeftArrow);
        //    }
        //    if (key == E_INPUT.R_STICK_RIGHT)
        //    {
        //        if (mode == E_INPUT_MODE.BUTTON) return Input.GetKey(KeyCode.RightArrow);
        //        if (mode == E_INPUT_MODE.TRIGGER) return Input.GetKeyDown(KeyCode.RightArrow);
        //        if (mode == E_INPUT_MODE.RELEASE) return Input.GetKeyUp(KeyCode.RightArrow);
        //    }
        //    if (key == E_INPUT.R_STICK_UP)
        //    {
        //        if (mode == E_INPUT_MODE.BUTTON) return Input.GetKey(KeyCode.UpArrow);
        //        if (mode == E_INPUT_MODE.TRIGGER) return Input.GetKeyDown(KeyCode.UpArrow);
        //        if (mode == E_INPUT_MODE.RELEASE) return Input.GetKeyUp(KeyCode.UpArrow);
        //    }
        //    if (key == E_INPUT.R_STICK_DOWN)
        //    {
        //        if (mode == E_INPUT_MODE.BUTTON) return Input.GetKey(KeyCode.DownArrow);
        //        if (mode == E_INPUT_MODE.TRIGGER) return Input.GetKeyDown(KeyCode.DownArrow);
        //        if (mode == E_INPUT_MODE.RELEASE) return Input.GetKeyUp(KeyCode.DownArrow);
        //    }
        //}
        else if (key == E_INPUT.A)
        {// Aボタン
            if (mode == E_INPUT_MODE.BUTTON) return Input.GetKey(KeyCode.Space);
            if (mode == E_INPUT_MODE.TRIGGER) return Input.GetKeyDown(KeyCode.Space);
            if (mode == E_INPUT_MODE.RELEASE) return Input.GetKeyUp(KeyCode.Space);
        }
        else if (key == E_INPUT.B)
        {// Bボタン
            if (mode == E_INPUT_MODE.BUTTON) return Input.GetKey(KeyCode.LeftShift);
            if (mode == E_INPUT_MODE.TRIGGER) return Input.GetKeyDown(KeyCode.LeftShift);
            if (mode == E_INPUT_MODE.RELEASE) return Input.GetKeyUp(KeyCode.LeftShift);
        }
        else if (key == E_INPUT.Y)
        {// Yボタン
            if (mode == E_INPUT_MODE.BUTTON) return Input.GetKey(KeyCode.M);
            if (mode == E_INPUT_MODE.TRIGGER) return Input.GetKeyDown(KeyCode.M);
            if (mode == E_INPUT_MODE.RELEASE) return Input.GetKeyUp(KeyCode.M);
        }
        Debug.Log("ゲームパッドで設定されてないボタンです");
        return false;   // ゲームパッドで設定されてない値
    }


    /**
     * @brief ゲームパッドでの入力
     * @param1 ゲームパッド列挙
     * @param2 入力モード
     * @return 入力モードでボタンが反応したら true
     */
    private bool isInputGamePad(E_INPUT key, E_INPUT_MODE mode)
    {
        if (key == E_INPUT.L_STICK_RIGHT || key == E_INPUT.L_STICK_LEFT ||
                 key == E_INPUT.L_STICK_UP || key == E_INPUT.L_STICK_DOWN)
        {// Lスティックの場合
            if (isLStick(key, mode))
            {
                return true;
            }
            else if (isDPad(key, mode))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (key == E_INPUT.R_STICK_RIGHT || key == E_INPUT.R_STICK_LEFT ||
                 key == E_INPUT.R_STICK_UP || key == E_INPUT.R_STICK_DOWN)
        {// Rスティックの場合
            return isRStick(key, mode);
        }

        switch (mode)
        {
            case E_INPUT_MODE.BUTTON:   // ボタンの押し込み
                if (Input.GetKey(_name[(int)key]))
                {
                    return true;
                }
                break;

            case E_INPUT_MODE.TRIGGER:  // 押した瞬間
                if (Input.GetKeyDown(_name[(int)key]))
                {
                    return true;
                }
                break;

            case E_INPUT_MODE.RELEASE:  // 離した瞬間
                if (Input.GetKeyUp(_name[(int)key]))
                {
                    return true;
                }
                break;
        }
        return false;   // 列挙値のエラー
    }


    /**
     * @brief 十字キーの取得
     * @param1 入力ボタン
     * @param2 入力モード (まだ未対応)
     * @return 入力モードで反応したら true
     */
    private bool isDPad(E_INPUT key, E_INPUT_MODE mode)
    {
        if (mode == E_INPUT_MODE.BUTTON)
        {
            return _DPad.isButton(key, sensitivity);
        }
        else if (mode == E_INPUT_MODE.TRIGGER)
        {
            return _DPad.isTrigger(key, sensitivity);
        }
        else if (mode == E_INPUT_MODE.RELEASE)
        {
            return _DPad.isRelease(key, sensitivity);
        }
        return false;
    }


    /**
     * @brief Lスティックの取得
     * @param1 入力ボタン
     * @param2 入力モード (まだ未対応)
     * @return 入力モードで反応したら true
     */
    private bool isLStick(E_INPUT key, E_INPUT_MODE mode)
    {
        {// ゲームパッド
            if (mode == E_INPUT_MODE.BUTTON)
            {
                return _LStick.isButton(key, sensitivity);
            }
            else if (mode == E_INPUT_MODE.TRIGGER)
            {
                return _LStick.isTrigger(key, sensitivity);
            }
            else if (mode == E_INPUT_MODE.RELEASE)
            {
                return _LStick.isRelease(key, sensitivity);
            }
        }

        return false;
    }


    /**
     * @brief Rスティックの取得
     * @param1 入力ボタン
     * @param2 入力モード (まだ未対応)
     * @return 入力モードで反応したら true
     */
    private bool isRStick(E_INPUT key, E_INPUT_MODE mode)
    {
        {// ゲームパッド
            if (mode == E_INPUT_MODE.BUTTON)
            {
                return _RStick.isButton(key, sensitivity);
            }
            else if (mode == E_INPUT_MODE.TRIGGER)
            {
                return _RStick.isTrigger(key, sensitivity);
            }
            else if (mode == E_INPUT_MODE.RELEASE)
            {
                return _RStick.isRelease(key, sensitivity);
            }
        }
        return false;
    }
}


// EOF