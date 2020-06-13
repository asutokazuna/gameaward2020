using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIImageAlpha : MonoBehaviour
{
    [SerializeField] private CanvasGroup _retryButton = default;
    [SerializeField] private CanvasGroup _continueButton = default;
    [SerializeField] private CanvasGroup _selectButton = default;
    [SerializeField] private CanvasGroup _titleButton = default;
    [SerializeField] private CanvasGroup _finishButton = default;

    [SerializeField] private GameOverManager _gameOverManager = default;
    [SerializeField] private Map _map = default;
    //private GameMenu _menu;

    public float _startalpha = default;

    // Start is called before the first frame update
    void Start()
    {
        _gameOverManager = GameObject.Find("GameOverManager").GetComponent<GameOverManager>();
        _map = GameObject.FindGameObjectWithTag("Map").GetComponent<Map>();

        //_retryButton = GameObject.Find("retry").GetComponent<CanvasGroup>();
        //_continueButton = GameObject.Find("contine").GetComponent<CanvasGroup>();
        //_selectButton = GameObject.Find("Select").GetComponent<CanvasGroup>();
        //_titleButton = GameObject.Find("title").GetComponent<CanvasGroup>();
        //_finishButton = GameObject.Find("finish").GetComponent<CanvasGroup>();

        //_retryButton.alpha = _startalpha;
        //_continueButton.alpha = _startalpha;
        //_selectButton.alpha = _startalpha;
        //_titleButton.alpha = _startalpha;
        //_finishButton.alpha = _startalpha;
    }

    // Update is called once per frame
    void Update()
    {
        //if (_map._gameOver || _gameOverManager._isSelect)
        //{
        //    AlphaChange();
        //}
    }

    private void AlphaChange()
    {
    //    if(_map._gameOver)
    //    {
    //        switch (_gameOverManager._selectnum)
    //        {
    //            case 0:
    //                _continueButton.alpha = 1.0f;
    //                _selectButton.alpha = _startalpha;
    //                _titleButton.alpha = _startalpha;
    //                _finishButton.alpha = _startalpha;
    //                break;
    //            case 1:
    //                _continueButton.alpha = _startalpha;
    //                _selectButton.alpha = 1.0f;
    //                _titleButton.alpha = _startalpha;
    //                _finishButton.alpha = _startalpha;
    //                break;
    //            case 2:
    //                _continueButton.alpha = _startalpha;
    //                _selectButton.alpha = _startalpha;
    //                _titleButton.alpha = 1.0f;
    //                _finishButton.alpha = _startalpha;
    //                break;
    //            case 3:
    //                _continueButton.alpha = _startalpha;
    //                _selectButton.alpha = _startalpha;
    //                _titleButton.alpha = _startalpha;
    //                _finishButton.alpha = 1.0f;
    //                break;
    //            default:
    //                break;
    //        }
    //    }
    //    else if(_gameOverManager._isSelect)
    //    {
    //        switch (_gameOverManager._selectnum)
    //        {
    //            case -1:
    //                _retryButton.alpha = 1.0f;
    //                _continueButton.alpha = _startalpha;
    //                _selectButton.alpha = _startalpha;
    //                _titleButton.alpha = _startalpha;
    //                _finishButton.alpha = _startalpha;
    //                break;

    //            case 0:
    //                _retryButton.alpha = _startalpha;
    //                _continueButton.alpha = 1.0f;
    //                _selectButton.alpha = _startalpha;
    //                _titleButton.alpha = _startalpha;
    //                _finishButton.alpha = _startalpha;
    //                break;
    //            case 1:
    //                _retryButton.alpha = _startalpha;
    //                _continueButton.alpha = _startalpha;
    //                _selectButton.alpha = 1.0f;
    //                _titleButton.alpha = _startalpha;
    //                _finishButton.alpha = _startalpha;
    //                break;
    //            case 2:
    //                _retryButton.alpha = _startalpha;
    //                _continueButton.alpha = _startalpha;
    //                _selectButton.alpha = _startalpha;
    //                _titleButton.alpha = 1.0f;
    //                _finishButton.alpha = _startalpha;
    //                break;
    //            case 3:
    //                _retryButton.alpha = _startalpha;
    //                _continueButton.alpha = _startalpha;
    //                _selectButton.alpha = _startalpha;
    //                _titleButton.alpha = _startalpha;
    //                _finishButton.alpha = 1.0f;
    //                break;
    //            default:
    //                break;
    //        }
    //    }
        
    }
}