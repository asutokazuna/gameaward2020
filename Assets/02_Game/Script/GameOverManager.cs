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
        // オブジェクトの代入
        _map            = GameObject.FindWithTag("Map").GetComponent<Map>();
        _gameover       = GameObject.Find("Gameover").GetComponent<Gameover>();
        _sceneManager   = GameObject.FindWithTag("SceneManager");
        
        _continueButton = GameObject.Find("contine");
        _selectButton   = GameObject.Find("Select");
        _titleButton    = GameObject.Find("title");
        _finishButton   = GameObject.Find("finish");


        // 変数の初期化
        SetGameOverUI(false);

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
            SetGameOverUI(true);

            _isOnce = true;
        }


        if (_map._gameOver)
        {// ゲームオーバー中の処理
            SetGameOver();  // セット

            if (Input.GetKeyDown(KeyCode.W))
            {// カーソルを上に移動
                if (_selectnum < (int)GameoverMenu.E_CONTINUE)
                {
                    _selectnum--;
                    //Debug.Log("w--");
                }
                else
                {
                    _selectnum = (int)GameoverMenu.E_TITLE;
                    //Debug.Log("--w");
                }
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {// カーソルを下に移動
                if (_selectnum > (int)GameoverMenu.MAX_MENU)
                {
                    _selectnum = (int)GameoverMenu.E_CONTINUE;
                    //Debug.Log("s--");
                }
                else
                {
                    _selectnum++;
                    //Debug.Log("--s");
                }
            }

            if (Input.GetKeyDown(KeyCode.Space))
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
