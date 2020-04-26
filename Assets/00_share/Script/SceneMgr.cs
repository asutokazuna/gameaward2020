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
    GAME            = 2,    // SampleScene
    _1_1            = 3,    // 1-1
    _1_2            = 4,    // 1-2
    _1_3            = 5,    // 1-3
    _1_4            = 6,    // 1-4
    RELOAD,                 // リロード
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


        _fadeScript.StartFadeOut();
        Invoke("Load", 2f); //フェード終わるまで遅延
    }

    public void Load()
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
    public void SetScene(E_SCENE nextScene = E_SCENE.RELOAD)
    {
        if (nextScene == E_SCENE.RELOAD)
        {
            _reroad = true;
            return;
        }
        _nowScene = nextScene;
    }


}