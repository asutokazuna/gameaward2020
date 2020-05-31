/**
 * @file    MenuUI.cs
 * @brief   メニューの管理
 * @author  Risa Ito
 * @date    2020/05/31(火)  作成
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * @class MenuUI
 * @brief メニューの管理
 */
public class MenuUI : MonoBehaviour
{
    [SerializeField] Image     _menuObj;
    [SerializeField] Image[]   _setMenuObj;
    private SceneMgr _sceneManager;
    public  bool     _isMenu = false;
    private int      _selectMenu = 0;
    private Image[]  _menuImage;

    // Start is called before the first frame update
    void Start()
    {
        _sceneManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneMgr>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isMenu)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                _setMenuObj[_selectMenu].color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                _selectMenu--;

                if (_selectMenu < 0)
                {
                    _selectMenu = _setMenuObj.Length - 1;
                }

                _setMenuObj[_selectMenu].color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                _setMenuObj[_selectMenu].color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                _selectMenu++;

                if (_selectMenu >= _setMenuObj.Length)
                {
                    _selectMenu = 0;
                }
                _setMenuObj[_selectMenu].color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                switch (_selectMenu)
                {
                    case 0: // つづける
                        _menuObj.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                        for (int i = 0; i < _setMenuObj.Length; i++)
                        {
                            _setMenuObj[i].color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                        }
                        _isMenu = false;
                        break;
                    case 1: // タイトルへ遷移
                        _sceneManager.SetScene(E_SCENE.TITLE);
                        break;
                    case 2: // ゲーム終了
#if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false;
#endif
                        Application.Quit();
                        break;
                }
            }
            else if (Input.GetKeyDown(KeyCode.Y))
            {
                _menuObj.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                for (int i = 0; i < _setMenuObj.Length; i++)
                {
                    _setMenuObj[i].color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                }
                _isMenu = false;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Y))
            {
                _isMenu = true;
                _menuObj.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                _setMenuObj[0].color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                for (int i = 1; i < _setMenuObj.Length;i++)
                {
                    _setMenuObj[i].color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                }
            }
        }
    }
}
