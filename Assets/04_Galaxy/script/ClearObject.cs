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
    [SerializeField] GameObject[] _gameObjects = default;   // オブジェクト管理
    SceneMgr _sceneMgr;     // フラグ取得用

    // Start is called before the first frame update
    void Start()
    {
        _sceneMgr = GameObject.Find("SceneManager").GetComponent<SceneMgr>();

        for (int i = (int)E_SCENE._1_1; i < (int)E_SCENE.MAX; i++) 
        {
            if(_gameObjects.Length - 1 < i - (int)E_SCENE._1_1)
            {
                break;
            }
            _gameObjects[i - (int)E_SCENE._1_1].SetActive(_sceneMgr.GetStageClear((E_SCENE)i));
        }
    }
}
