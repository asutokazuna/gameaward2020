/**
 * @file	SceneManager.cs
 * @brief   シーン管理
 * 
 * @author	Kota Nakagami
 * 
 * @data    2020/04/22(水)   クラス作成
 *
 * @version	1.00
 */


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/**
 * @enum シーン列挙
 * @brief ビルドセッティングと同じ値にしてね
 */
public enum E_SCENE
{
    TITLE           = 0,    // タイトル
    STAGE_SELECT    = 1,    // ステージ選択
    _1_1            = 2,    // 1-1
    _1_2            = 3,    // 1-2
    _1_3            = 4,    // 1-3
    _1_4            = 5,    // 1-4
    _1_5            = 6,    // 1-5
    _1_6            = 7,    // 1-6
    _1_7            = 8,    // 1-7
    _1_8            = 9,    // 1-8
    _1_9            = 10,   // 1-9
    _1_10           = 11,   // 1-10
    _2_1            = 12,   // 2-1
    _2_2            = 13,   // 2-2
    _2_3            = 14,   // 2-3
    _2_4            = 15,   // 2-4
    _2_5            = 16,   // 2-5
    _2_6            = 17,   // 2-6
    _2_7            = 18,   // 2-7
    _2_8            = 19,   // 2-8
    _2_9            = 20,   // 2-9
    _2_10           = 21,   // 2-10
    _3_1            = 22,   // 3-1
    _3_2            = 23,   // 3-2
    _3_3            = 24,   // 3-3
    _3_4            = 25,   // 3-4
    _3_5            = 26,   // 3-5
    _3_6            = 27,   // 3-6
    _3_7            = 28,   // 3-7
    _3_8            = 29,   // 3-8
    _3_9            = 30,   // 3-9
    _3_10           = 31,   // 3-10
    _4_1            = 32,   // 4-1
    _4_2            = 33,   // 4-2
    _4_3            = 34,   // 4-3
    _4_4            = 35,   // 4-4
    _4_5            = 36,   // 4-5
    _4_6            = 37,   // 4-6
    _4_7            = 38,   // 4-7
    _4_8            = 39,   // 4-8
    _4_9            = 40,   // 4-9
    _4_10           = 41,   // 4-10
    _5_1            = 42,   // 5-1
    _5_2            = 43,   // 5-2
    _5_3            = 44,   // 5-3
    _5_4            = 45,   // 5-4
    _5_5            = 46,   // 5-5
    _5_6            = 47,   // 5-6
    _5_7            = 48,   // 5-7
    _5_8            = 49,   // 5-8
    _5_9            = 50,   // 5-9
    _5_10           = 51,   // 5-10
    _6_1            = 52,   // 6-1
    _6_2            = 53,   // 6-2
    _6_3            = 54,   // 6-3
    _6_4            = 55,   // 6-4
    _6_5            = 56,   // 6-5
    _6_6            = 57,   // 6-6
    _6_7            = 58,   // 6-7
    _6_8            = 59,   // 6-8
    _6_9            = 60,   // 6-9
    _6_10           = 61,   // 6-10
    MAX,                    // クリアシーン
}


/**
 * @enum シーン切り替えのモード
 */
public enum E_SCENE_MODE
{
    RELOAD,                 // リロード
    NEXT_STAGE,             // 次のステージ
}




/**
 * @class シーンマネージャー
 * @brief 全てのシーン管理
 */
public class SceneMgr : MonoBehaviour
{
    static private SceneMgr _instance;                              //!< 自身のインスタンス
    [SerializeField] private E_SCENE _nowScene = E_SCENE.TITLE;     //!< 今のシーン
    [SerializeField] private E_SCENE _oldScene = E_SCENE.TITLE;     //!< 前のシーン
    [NonSerialized] public E_SCENE _lastScene = E_SCENE.TITLE;                      //!< 過去のシーン

    [SerializeField] private bool _reroad = false;

    [SerializeField] private bool _isDebug = false;     //!< かとしゅん頼む

    private GameObject _mainCamera; //!<フェード用
    private Fade _fadeScript;

    public static bool[] _stageClear = new bool[(int)E_SCENE.MAX];
    public static bool _tutorial;

    public bool _stageReroad = false;     //!< スタート演出カット用
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            for (int n = 0; n < (int)E_SCENE.MAX; n++)
            {
                _stageClear[n] = false;
            }
            _tutorial = true;
            //for (int n = (int)E_SCENE._1_1; n <= (int)E_SCENE._1_10; n++)
            //{
            //    if (n == (int)E_SCENE._1_1)
            //        continue;
            //    _stageClear[n] = true;
            //}
        }
        else
        {
            Destroy(gameObject);
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        _mainCamera = GameObject.Find("Main Camera");
        _fadeScript = _mainCamera.GetComponent<Fade>();
    }



    // Start is called before the first frame update
    void Start()
    {
        int nowHandle = SceneManager.GetActiveScene().buildIndex;
        Debug.Log("ハンドル値" + nowHandle);
        _nowScene = _oldScene = (E_SCENE)nowHandle;
        //_mainCamera = GameObject.Find("Main Camera");
        //_fadeScript = _mainCamera.GetComponent<Fade>();
        //_fadeScript = GetComponent<Fade>();
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            CallDebug();
        }

        ChangeScene();
    }


    public void ChangeScene()
    {
        if (_oldScene == _nowScene && !_reroad)
        {// シーンの切り替えが発生してないで
            return;
        }
        _oldScene   = _nowScene;        // 過去シーンの保存
        _reroad     = false;            // リロードしないよ

        //OnFinishedCoroutine();
        //StartCoroutine(_fadeScript.FadeOut(OnFinishedCoroutine)); // ごめん分からない...
        _fadeScript.SetFadeSpeed(0.01f);
        _fadeScript.StartFadeOut(OnFinishedCoroutine);
    }


    public void OnFinishedCoroutine()
    {
        SceneManager.LoadScene((int)_nowScene);
    }


    public bool isDebug()
    {
        return _isDebug;
    }


    /**
     * @brief シーンのセット
     * @param1 シーン列挙 デフォルト引数でリロード
     * @return なし
     */
    public void SetScene(E_SCENE_MODE mode)
    {
        _lastScene = _nowScene;
        if (mode == E_SCENE_MODE.RELOAD)
        {// シーンの再読み込み
            _reroad = true;
            _stageReroad = true;
            return;
        }
        else if (mode == E_SCENE_MODE.NEXT_STAGE)
        {// 次のシーンへ移行
            //if (isLimitStage())
            //{
            //    _nowScene = E_SCENE.STAGE_SELECT;
            //}
            //else if (isNextScene())
            //{// まだ同じ星の中でステージがある
            //    _nowScene += 1;
            //}
            //else
            //{// ステージ選択へ戻る
            if (_nowScene == E_SCENE._1_1)
            {
                _tutorial = false;
            }
                _nowScene = E_SCENE.STAGE_SELECT;
            //}
        }
    }


    /**
     * @brief シーンのセット
     * @param1 シーン列挙 デフォルト引数でリロード
     * @return なし
     */
    public void SetScene(E_SCENE scene)
    {
        if (_nowScene == E_SCENE._1_1)
        {
            _tutorial = false;
        }
        _lastScene = _nowScene;
        _nowScene = scene;

        //Debug.Log("SceneChange");
    }


    /**
     * @brief 次のステージへ移行できるか
     * @return 行けるなら true を返す
     */
    private bool isNextScene()
    {
        if (!GetPlanetClear(_nowScene))
        {
            return true;
        }
        return false;
    }


    /**
     * @brief 現在のシーンの取得
     */
    public E_SCENE NowScene //!< 移動フラグ
    {
        get { return _nowScene; }  // ゲッター
    }


    /**
     * @brief クリアフラグのセット
     * @return なし
     */
    public void SetClear()
    {
        _stageClear[(int)_nowScene] = true;
    }


    /**
     * @brief デバッグ呼び出し
     * @return なし
     */
    public void CallDebug()
    {
        for (int n = (int)E_SCENE._1_1; n < (int)E_SCENE.MAX; n++)
        {
            if (!_stageClear[n])
            {// まだ未クリアがあるよ
                Debug.Log(n + 1 - (int)E_SCENE._1_1 + " 未クリア");
            }
            else
            {
                Debug.Log(n + 1 - (int)E_SCENE._1_1 + " クリアしてる");
            }
        }
    }


    /**
     * @brief 惑星の全てのステージをクリアしたかの取得
     * @param1 ステージ列挙(惑星1 なら E_SCENE._1_1 ～ E_SCENE._1_10)
     * @return 惑星がクリアされていたら true
     */
    public bool GetPlanetClear(E_SCENE stage)
    {
        if (stage == E_SCENE.TITLE || stage == E_SCENE.STAGE_SELECT || stage == E_SCENE.MAX)
        {
            Debug.Log("参照外やで^^");
            return false;
        }
        else if (stage >= E_SCENE._1_1 && stage <= E_SCENE._1_10)
        { // 惑星1のクリア取得
            for (int n = (int)E_SCENE._1_1; n <= (int)E_SCENE._1_10; n++)
                if (!_stageClear[n])
                    return false;
        }
        else if (stage >= E_SCENE._2_1 && stage <= E_SCENE._2_10)
        {   // 惑星2のクリア取得
            for (int n = (int)E_SCENE._2_1; n <= (int)E_SCENE._2_10; n++)
                if (!_stageClear[n])
                    return false;
        }
        else if (stage >= E_SCENE._3_1 && stage <= E_SCENE._3_10)
        {   // 惑星3のクリア取得
            for (int n = (int)E_SCENE._3_1; n <= (int)E_SCENE._3_10; n++)
                if (!_stageClear[n])
                    return false;
        }
        else if (stage >= E_SCENE._4_1 && stage <= E_SCENE._4_10)
        {   // 惑星4のクリア取得
            for (int n = (int)E_SCENE._4_1; n <= (int)E_SCENE._4_10; n++)
                if (!_stageClear[n])
                    return false;
        }
        else if (stage >= E_SCENE._5_1 && stage <= E_SCENE._5_10)
        { // 惑星5のクリア取得
            for (int n = (int)E_SCENE._5_1; n <= (int)E_SCENE._5_10; n++)
                if (!_stageClear[n])
                    return false;
        }
        else if (stage >= E_SCENE._6_1 && stage <= E_SCENE._6_10)
        { // 惑星6のクリア取得
            for (int n = (int)E_SCENE._6_1; n <= (int)E_SCENE._6_10; n++)
                if (!_stageClear[n])
                    return false;
        }
        return true;
    }


    /**
     * @brief 最終ステージクリア時、ステージ選択に戻る処理
     * @return 遊んでたのが最終ステージなら true
     */
    private bool isLimitStage()
    {
        if (_nowScene == E_SCENE._1_10 ||
            _nowScene == E_SCENE._2_10 || 
            _nowScene == E_SCENE._3_10 || 
            _nowScene == E_SCENE._4_10 ||
            _nowScene == E_SCENE._5_10 ||
            _nowScene == E_SCENE._6_10)
        {
            return true;
        }
        return false;
    }

    public bool GetStageClear(E_SCENE stage)
    {
        //int num = (int)(stage - E_SCENE._1_1);
        return _stageClear[(int)stage];
    }


    public bool GetTutorial()
    {
        if (_tutorial && _nowScene == E_SCENE._1_1)
        {
            return true;
        }
        return false;
    }
}


// EOF