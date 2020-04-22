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


/*
 * @enum シーン列挙
 * @brief ビルドセッティングと同じ値にしてね
 */
public enum E_SCENE
{
    TITLE = 0,
    STAGE_SELECT = 1,
    GAME = 2,
}



/*
 * @class シーンマネージャー
 * @brief 全てのシーン管理
 */
public class SceneMgr : MonoBehaviour
{
    Controller _input;
    static private SceneMgr _instance;          //!< 自身のインスタンス
    [SerializeField] private E_SCENE _nowScene; //!< 今のシーン
    [SerializeField] private E_SCENE _oldScene; //!< 前のシーン


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
        _oldScene = _nowScene;
        _input = GameObject.FindGameObjectWithTag("Input").GetComponent<Controller>();
    }

    // Update is called once per frame
    void Update()
    {
        LoadScene();
    }


    private void LoadScene()
    {
        if (_oldScene == _nowScene)
        {// シーンの切り替えが発生してないで
            Debug.Log("弾き返していくぅ");
            return;
        }

        _oldScene = _nowScene;  // 過去シーンの保存

        SceneManager.LoadScene((int)_nowScene);
    }
}