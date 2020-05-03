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
    _1_2               ,    // 1-2
    _1_3               ,    // 1-3
    _1_4               ,    // 1-4
    _1_5               ,    // 1-5
    _1_6               ,    // 1-6
    _1_7               ,    // 1-7
    _1_8               ,    // 1-8
    _2_1               ,    // 2-1
    _2_2               ,    // 2-2
    _2_3               ,    // 2-3
    _2_4               ,    // 2-4
    _2_5               ,    // 2-5
    _2_6               ,    // 2-6
    _2_7               ,    // 2-7
    _2_8               ,    // 2-8
    _3_1               ,    // 3-1
    CLEAR,                  // クリアシーン
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
    [SerializeField] private bool _reroad = false;

    [SerializeField] private bool _isDebug = false;     //!< かとしゅん頼む

    private GameObject _mainCamera; //!<フェード用
    private Fade _fadeScript;


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
        int nowHandle = SceneManager.GetActiveScene().buildIndex;
        Debug.Log("ハンドル値" + nowHandle);
        _nowScene = _oldScene = (E_SCENE)nowHandle;

        _mainCamera = GameObject.Find("Main Camera");
        _fadeScript = _mainCamera.GetComponent<Fade>();
    }


    // Update is called once per frame
    void Update()
    {
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
        if (mode == E_SCENE_MODE.RELOAD)
        {// シーンの再読み込み
            _reroad = true;
            return;
        }
        else if (mode == E_SCENE_MODE.NEXT_STAGE)
        {// 次のシーンへ移行
            if (isNextScene())
            {// まだ同じ星の中でステージがある
                _nowScene += 1;
            }
            else
            {// ステージ選択へ戻る
                _nowScene = E_SCENE.STAGE_SELECT;
            }
        }
        else if (isGameClear())
        {// 取り合えずベータ版クリア
            _nowScene = E_SCENE.CLEAR;
        }
    }


    /**
     * @brief シーンのセット
     * @param1 シーン列挙 デフォルト引数でリロード
     * @return なし
     */
    public void SetScene(E_SCENE scene)
    {
        _nowScene = scene;
    }


    /**
     * @brief 次のステージへ移行できるか
     * @return 行けるなら true を返す
     */
    private bool isNextScene()
    {
        if ((int)_nowScene + 1 <= (int)E_SCENE._1_3)
        {// 取り合えずの処理
            return true;
        }
        return false;
    }


    private bool isGameClear()
    {// 取り合えずベータ版クリア
        if (GameObject.FindGameObjectWithTag("GameClearManager").GetComponent<ClearManager>().isGameClear())
        {// ゲームクリア
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
}


// EOF