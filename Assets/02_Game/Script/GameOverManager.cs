using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameOverManager : MonoBehaviour
{
    enum GameoverMenu
    {
        E_CONTINUE = 0,    //ゲームに戻る
        E_SELECT = 1,      //セレクト
        E_TITLE = 2,       //タイトル
        E_EXIT = 3,         //ゲーム終了
        MAX_MENU
    }

    private GameObject _backGround;
    private GameObject _continueButton;
    private GameObject _selectButton;
    private GameObject _titleButton;
    private GameObject _finishButton;

    private Map _map;
    private Gameover _gameover;

    private bool _isOnce;
    public int _selectnum;
    public bool _isSelect;

    private GameObject _sceneManager;
    /*
    public bool GameOverSwitch;
    public float WaitTime;
    public float DelayTime;

    private Map FlgScript;
    private Gameover GameOverScript;
    private bool _isFirst;
    private float _timer;
    private float _changeDelay;
    bool _isOnce;
     */

    // Start is called before the first frame update
    void Start()
    {
        // オブジェクトの代入
        _map            = GameObject.FindWithTag("Map").GetComponent<Map>();
        _gameover       = GameObject.Find("Gameover").GetComponent<Gameover>();
        _sceneManager   = GameObject.FindWithTag("SceneManager");
        
        _continueButton = GameObject.Find("contine");
        _selectButton   = GameObject.Find("Select");
        _titleButton    = GameObject.Find("title");
        _finishButton   = GameObject.Find("finish");
        _backGround     = GameObject.Find("menuBG");


        // 変数の初期化
        SetGameOverUI(false);
        _backGround.SetActive(false);

        _isSelect = false;
        _isOnce = false;
        _selectnum = 0;
        /*
        FlgScript = GameObject.Find("Map").GetComponent<Map>();
        GameOverScript = GameObject.Find("Gameover").GetComponent<Gameover>();
        _isFirst = true;
        _timer = WaitTime;
        _changeDelay = DelayTime;
        _isOnce = false;
         */
    }

    // Update is called once per frame
    void Update()
    {
        if (_map._gameClear ||
            GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneMgr>().GetTutorial())
        {// 操作受付拒否
            return;
        }

        //ゲームオーバーになったら
        if (_map._gameOver && !_isOnce)
        {
            //UIの表示
            SetGameOverUI(true);
            _isOnce = true;
        }
        if (GameObject.FindGameObjectWithTag("Input").GetComponent<Controller>()
            .isInput(E_INPUT_MODE.TRIGGER, E_INPUT.Y) && _isOnce)
        {
            SetGameOverUI(false);
            _selectnum = 0;
            _isSelect = false;
            _isOnce = false;
            Time.timeScale = 1.0f;
        }
        else if(GameObject.FindGameObjectWithTag("Input").GetComponent<Controller>()
            .isInput(E_INPUT_MODE.TRIGGER, E_INPUT.Y))
        {
            SetGameOverUI(true);
            _isSelect = true;
            _isOnce = true;
            Time.timeScale = 0.0f;
        }


        if (_map._gameOver)
        {// ゲームオーバー中の処理
            SetGameOver();  // セット

            if (GameObject.FindGameObjectWithTag("Input").GetComponent<Controller>()
            .isInput(E_INPUT_MODE.TRIGGER, E_INPUT.L_STICK_UP))
            {// カーソルを上に移動
                if (_selectnum <= (int)GameoverMenu.E_CONTINUE)
                {
                    _selectnum = (int)GameoverMenu.E_EXIT;
                }
                else
                {
                    _selectnum--;
                }
            }
            else if (GameObject.FindGameObjectWithTag("Input").GetComponent<Controller>()
            .isInput(E_INPUT_MODE.TRIGGER, E_INPUT.L_STICK_DOWN))
            {// カーソルを下に移動
                if (_selectnum >= (int)GameoverMenu.E_EXIT)
                {
                    _selectnum = (int)GameoverMenu.E_CONTINUE;
                }
                else
                {
                    _selectnum++;
                }
            }

            if (GameObject.FindGameObjectWithTag("Input").GetComponent<Controller>()
            .isInput(E_INPUT_MODE.TRIGGER, E_INPUT.A))
            {// 決定キーを押した時の処理
                switch (_selectnum)
                {
                    //つづけるへ
                    case 0:
                        _sceneManager.GetComponent<SceneMgr>().SetScene(E_SCENE_MODE.RELOAD);
                        break;
                    //セレクトへ
                    case 1:
                        _sceneManager.GetComponent<SceneMgr>().SetScene(E_SCENE.STAGE_SELECT);
                        break;
　　　　　　　　　　//タイトルへ
                    case 2:
                        _sceneManager.GetComponent<SceneMgr>().SetScene(E_SCENE.TITLE);
                        break;
　　　　　　　　　　//ゲーム終了へ
                    case 3:
#if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false;
#endif
                        Application.Quit();
                        break;

                    default:
                        break;

                }
                _map._gameOver = false;
                SetGameOverUI(false);
                _isOnce = false;
            }
        }
        else if(_isSelect)
        {
            if (GameObject.FindGameObjectWithTag("Input").GetComponent<Controller>()
            .isInput(E_INPUT_MODE.TRIGGER, E_INPUT.L_STICK_UP))
            {// カーソルを上に移動
                if (_selectnum <= (int)GameoverMenu.E_CONTINUE)
                {
                    _selectnum = (int)GameoverMenu.E_EXIT;
                }
                else
                {
                    _selectnum--;
                }
            }
            else if (GameObject.FindGameObjectWithTag("Input").GetComponent<Controller>()
            .isInput(E_INPUT_MODE.TRIGGER, E_INPUT.L_STICK_DOWN))
            {// カーソルを下に移動
                if (_selectnum >= (int)GameoverMenu.E_EXIT)
                {
                    _selectnum = (int)GameoverMenu.E_CONTINUE;
                }
                else
                {
                    _selectnum++;
                }
            }
            if (GameObject.FindGameObjectWithTag("Input").GetComponent<Controller>()
            .isInput(E_INPUT_MODE.TRIGGER, E_INPUT.A))
            {// 決定キーを押した時の処理
                switch (_selectnum)
                {
                    //つづけるへ
                    case 0:
                        _sceneManager.GetComponent<SceneMgr>().SetScene(E_SCENE_MODE.RELOAD);
                        break;
                    //セレクトへ
                    case 1:
                        _sceneManager.GetComponent<SceneMgr>().SetScene(E_SCENE.STAGE_SELECT);
                        break;
                    //タイトルへ
                    case 2:
                        _sceneManager.GetComponent<SceneMgr>().SetScene(E_SCENE.TITLE);
                        break;
                    //ゲーム終了へ
                    case 3:
#if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false;
#endif
                        Application.Quit();
                        break;

                    default:
                        break;

                }
                _isOnce = false;
                SetGameOverUI(false);
                Time.timeScale = 1.0f;
            }
        }

        /*
        //ゲームオーバーになったら
        if (GameOverSwitch && FlgScript._gameOver)
        {
            if (_timer <= 0.0f)
            {
                //アニメーションの開始
                SetGameOver();
            }
            else
            {
                _timer -= Time.deltaTime;
            }
        }

        //アニメーションが終わったらシーンを変える
        if(GameOverScript._finishGameover)
        {
            if ((_changeDelay <= 0.0f || Input.anyKey) && _isOnce == false)
            {
                _isOnce = true;
                //シーン遷移
                GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneMgr>().SetScene(E_SCENE_MODE.RELOAD);
            }
            else
            {
                _changeDelay -= Time.deltaTime;
            }
        }
        */
    }


    /**
     * @brief ゲームオーバーのセット
     * @return なし
     */
    private void SetGameOver()
    {
        /*
        if (_isFirst)
        {
            GameOverScript.StartGameover();
            _isFirst = false;
        }
         */
        _gameover.StartGameover();
    }


    /**
     * @brief UIのセット
     * @param bool型
     * @return なし
     */
    private void SetGameOverUI(bool flag)
    {
        if(_isSelect || Input.GetKeyDown(KeyCode.Q))
        {
            _backGround.SetActive(flag);
        }
        _continueButton.SetActive(flag);
        _selectButton.SetActive(flag);
        _titleButton.SetActive(flag);
        _finishButton.SetActive(flag);
    }


    // 中上皓太が消しました
    //private void UIReset()
    //{
    //    _continueButton.SetActive(false);
    //    _selectButton.SetActive(false);
    //    _titleButton.SetActive(false);
    //    _finishButton.SetActive(false);
    //}
}
