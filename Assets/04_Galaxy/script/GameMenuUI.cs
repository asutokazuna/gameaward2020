/**
 * @file    GameMenuUI.cs
 * @brief   ゲームシーンのメニューの管理
 * @author  Risa Ito
 * @date    2020/05/31(日)  作成
 * @date    2020/06/01(月)  キー入力対応
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * @class GameMenuUI
 * @brief ゲームシーンのメニューの管理
 */
public class GameMenuUI : MonoBehaviour
{
    enum MenuType
    {
        E_CONTINUE,
        E_RETRY,
        E_TITLE,
        E_STAGE,
        E_FINISH,
    }

    [SerializeField] Image      _menuObj;           //!< メニュー表示管理用
    [SerializeField] Image[]    _setMenuImage;      //!< メニュー項目表示管理用
    [SerializeField] MenuType[] _setMenuType;       //!< メニュー項目内容管理用
    private SceneMgr            _sceneManager;      //!< シーンマネージャー取得用
    public  bool                _isMenu = false;    //!< メニューフラグ
    private bool                _isKey = false;     //!< キー入力フラグ
    private int                 _selectMenu = 0;    //!< 現在選ばれているメニュー管理用
    private Controller          _input;             //!< 入力取得用
    private Map                 _map;               //!< ゲームオーバーフラグ取得用
    private bool                _gameover;          //!< ゲームオーバーフラグ
    private AudioSource         _audioSource;       //!< 音再生管理
    public  AudioClip           _SEPopUp;           //!< ポップアップ音
    public  AudioClip           _SESelect;          //!< 選択音
    public  AudioClip           _SEDecision;        //!< 決定音

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _sceneManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneMgr>();
        _input = GameObject.FindGameObjectWithTag("Input").GetComponent<Controller>();
        _map = GameObject.FindWithTag("Map").GetComponent<Map>();
    }

    // Update is called once per frame
    void Update()
    {
        // メニュー画面が開いてる場合
        if (_isMenu)
        {
            if (_isKey) // キーの入力終了
            {
                _isMenu = false;
                _isKey = false;
            }
            else if (_input.isInput(E_INPUT_MODE.TRIGGER, E_INPUT.L_STICK_UP))
            {
                _audioSource.PlayOneShot(_SESelect);

                _setMenuImage[_selectMenu].color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                _selectMenu--;

                // ゲームオーバーかどうかチェック
                if (!_gameover)
                {
                    if (_selectMenu < 0)
                    {
                        _selectMenu = _setMenuImage.Length - 1;
                    }
                }
                else
                {
                    if (_selectMenu < 1)
                    {
                        _selectMenu = _setMenuImage.Length - 1;
                    }
                }

                _setMenuImage[_selectMenu].color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
            else if (_input.isInput(E_INPUT_MODE.TRIGGER, E_INPUT.L_STICK_DOWN))
            {
                _audioSource.PlayOneShot(_SESelect);

                _setMenuImage[_selectMenu].color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                _selectMenu++;

                if (_selectMenu >= _setMenuImage.Length)
                {
                    // ゲームオーバーかどうかチェック
                    if (!_gameover)
                    {
                        _selectMenu = 0;
                    }
                    else
                    {
                        _selectMenu = 1;
                    }
                    
                }
                _setMenuImage[_selectMenu].color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
            else if (_input.isInput(E_INPUT_MODE.TRIGGER, E_INPUT.A))
            {
                _isKey = true;
                _audioSource.PlayOneShot(_SEDecision);

                SetMenu();
            }
            else if (_input.isInput(E_INPUT_MODE.TRIGGER, E_INPUT.Y))
            {
                // ゲームオーバーかどうかチェック
                if (!_gameover)
                {
                    _audioSource.PlayOneShot(_SEPopUp);

                    _menuObj.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                    for (int i = 0; i < _setMenuImage.Length; i++)
                    {
                        _setMenuImage[i].color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                    }
                    _isKey = true;
                }
            }
        }
        // メニュー画面が閉じてる場合
        else
        {
            // ゲームオーバーかどうかチェック
            if (!_map._gameOver)
            {
                // クリアシーンかどうかチェック
                if (!_map._gameClear)
                {
                    _gameover = false;
                    if (_input.isInput(E_INPUT_MODE.TRIGGER, E_INPUT.Y))
                    {
                        _audioSource.PlayOneShot(_SEPopUp);

                        _isMenu = true;
                        _selectMenu = 0;
                        _menuObj.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                        _setMenuImage[0].color = new Color(1.0f, 1.0f, 1.0f, 1.0f); // つづける
                        for (int i = 1; i < _setMenuImage.Length; i++)
                        {
                            _setMenuImage[i].color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                        }
                    }
                }
            }
            else
            {
                _gameover = true;
                _isMenu = true;
                _selectMenu = 1;
                _setMenuImage[1].color = new Color(1.0f, 1.0f, 1.0f, 1.0f); 
                for (int i = 2; i < _setMenuImage.Length; i++)
                {
                    _setMenuImage[i].color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                }
            }
        }
    }

    /**
    * @brief        メニュー処理
    * @return       なし
    * @details      指定されたメニューの処理をする関数です
    */
    void SetMenu()
    {
        switch (_setMenuType[_selectMenu])
        {
            case MenuType.E_CONTINUE: // つづける
                _menuObj.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                for (int i = 0; i < _setMenuImage.Length; i++)
                {
                    _setMenuImage[i].color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                }
                _isKey = true;
                break;
            case MenuType.E_RETRY:  // リトライ
                _sceneManager.SetScene(E_SCENE_MODE.RELOAD);
                break;
            case MenuType.E_STAGE:  // ステージセレクトへ遷移
                _sceneManager.SetScene(E_SCENE.STAGE_SELECT);
                break;
            case MenuType.E_TITLE:  // タイトルへ遷移
                _sceneManager.SetScene(E_SCENE.TITLE);
                break;
            case MenuType.E_FINISH: // ゲーム終了
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
                Application.Quit();
                break;
        }
    }
}
