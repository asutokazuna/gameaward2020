using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    enum MENU
    {
        GAME = 0,       //ゲームに戻る
        SELECT = 1,     //セレクト
        TITLE = 2,      //タイトル
        EXIT = 3       //ゲーム終了
    }

    public int _select;
    private bool _isMenu;
    [SerializeField]
    private GameObject _menuUI;
    [SerializeField]
    private GameObject[] UIPanel = new GameObject[4];
    [SerializeField]
    private GameObject _selectBG;

    private GameObject _sceneManager;
    
    // Start is called before the first frame update
    void Start()
    {
        _select = 0;
        _sceneManager = GameObject.FindWithTag("SceneManager");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("q"))
        {
            //フラグの切り替え
            _isMenu = !_isMenu;
            _select = 0;
        }

        if(_isMenu)
        {
            _menuUI.SetActive(true);
            Time.timeScale = 0.0f;
        }
        else
        {
            _menuUI.SetActive(false);
            Time.timeScale = 1.0f;
        }

        if(_isMenu)
        {
            //キー割り当て
            if (Input.GetKeyDown("up"))
            {
                if (_select == 2 || _select == 3)
                {
                    _select -= 2;
                }

            }
            if (Input.GetKeyDown("down"))
            {
                if (_select == 0 || _select == 1)
                {
                    _select += 2;
                }

            }
            if (Input.GetKeyDown("left"))
            {
                if (_select == 1 || _select == 3)
                {
                    _select -= 1;
                }
            }
            if (Input.GetKeyDown("right"))
            {
                if (_select == 0 || _select == 2)
                {
                    _select += 1;
                }
            }

            _selectBG.transform.position = UIPanel[_select].transform.position;

            
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Time.timeScale = 1.0f;
                _isMenu = false;
                switch (_select)
                {
                    case 0: //ポーズ画面を閉じる
                        //_isMenu = false;
                        break;
                    case 1: //セレクト画面に遷移
                        _sceneManager.GetComponent<SceneMgr>().SetScene(E_SCENE.STAGE_SELECT);
                        break;
                    case 2: //タイトル画面に遷移
                        _sceneManager.GetComponent<SceneMgr>().SetScene(E_SCENE.TITLE);
                        break;
                    case 3: //ゲーム終了
#if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false;
#endif

                        Application.Quit();


                        break;
                }
            }
        }

        
    }
}
