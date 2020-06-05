/**
 * @file    ClearObject.cs
 * @brief   クリア後のオブジェクト表示
 * @author  Risa Ito
 * @date    2020/05/07(木)   作成
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @class ClearObject
 * @brief クリア後のオブジェクト表示
 */
public class ClearObject : MonoBehaviour
{
    [SerializeField] GameObject[]   _gameObjects = default;     //!< オブジェクト管理
    private SceneMgr                _sceneMgr;                  //!< フラグ取得用
    static  private bool[]          _isClearFlg = new bool[60]; //!< クリア演出管理フラグ
    static  private bool            _isInit = true;             //!< クリア演出管理初期化フラグ
            public  bool            _isChange;                  //!< クリア演出管理フラグ

    // Start is called before the first frame update
    void Start()
    {
        // 初期化
        if(_isInit)
        {
            Init();
        }

        _isChange = false;
        _sceneMgr = GameObject.Find("SceneManager").GetComponent<SceneMgr>();

        for (int i = (int)E_SCENE._1_1; i < (int)E_SCENE.MAX; i++) 
        {
            if (_gameObjects.Length - 1 < i - (int)E_SCENE._1_1)
            {
                break;
            }
            else if (_sceneMgr.GetStageClear((E_SCENE)i) && !_isClearFlg[i - (int)E_SCENE._1_1])
            {
                _isChange = true;
                _isClearFlg[i - (int)E_SCENE._1_1] = true;
                _gameObjects[i - (int)E_SCENE._1_1].SetActive(true);
            }
        }
    }

    void Init()
    {
        _isInit = false;

        for (int i = 0; i < 60; i++)
        {
            if(_gameObjects.Length <= i)
            {
                break;
            }
            _gameObjects[i].SetActive(false);
        }
    }
}
