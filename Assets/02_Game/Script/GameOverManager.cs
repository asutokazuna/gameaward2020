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

    public GameObject _continueButton;
    public GameObject _selectButton;
    public GameObject _titleButton;
    public GameObject _finishButton;

    private Map _map;
    Gameover _gameover;

    private bool _isOnce;
    private int _selectnum;

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
        _map = GameObject.Find("Map").GetComponent<Map>();
        _gameover = GameObject.Find("Gameover").GetComponent<Gameover>();

        _sceneManager = GameObject.FindWithTag("SceneManager");

        _continueButton.SetActive(false);
        _selectButton.SetActive(false);
        _titleButton.SetActive(false);
        _finishButton.SetActive(false);

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
        //ゲームオーバーになったら
        if (_map._gameOver && !_isOnce)
        {
            //UIの表示
            _continueButton.SetActive(true);
            _selectButton.SetActive(true);
            _titleButton.SetActive(true);
            _finishButton.SetActive(true);

            _isOnce = true;
        }

        if (_map._gameOver)
        {
            SetGameOver();

            if (Input.GetKeyDown(KeyCode.W))
            {
                if (_selectnum < (int)GameoverMenu.E_CONTINUE)
                {
                    _selectnum--;
                    Debug.Log("w--");

                }
                else
                {
                    _selectnum = (int)GameoverMenu.E_TITLE;
                    Debug.Log("--w");

                }
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                if (_selectnum > (int)GameoverMenu.MAX_MENU)
                {
                    Debug.Log("s--");

                    _selectnum = (int)GameoverMenu.E_CONTINUE;
                }
                else
                {
                    Debug.Log("--s");
                    _selectnum++;
                }
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
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
                        UnityEditor.EditorApplication.isPlaying = false;
                        break;

                    default:
                        break;

                }
                _map._gameOver = false;
                UIReset();
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

    private void UIReset()
    {
        _continueButton.SetActive(false);
        _selectButton.SetActive(false);
        _titleButton.SetActive(false);
        _finishButton.SetActive(false);
    }
}
