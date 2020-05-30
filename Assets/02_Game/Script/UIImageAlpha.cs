using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIImageAlpha : MonoBehaviour
{
    public CanvasGroup _continueButton;
    public CanvasGroup _selectButton;
    public CanvasGroup _titleButton;
    public CanvasGroup _finishButton;

    private GameOverManager _gameOverManager;
    private Map _map;
    //private GameMenu _menu;

    public float _startalpha;

    // Start is called before the first frame update
    void Start()
    {
        _continueButton.alpha = _startalpha;
        _selectButton.alpha = _startalpha;
        _titleButton.alpha = _startalpha;
        _finishButton.alpha = _startalpha;

        //_menu = default;
        _gameOverManager = GameObject.Find("GameOverManager").GetComponent<GameOverManager>();
        _map = GameObject.FindWithTag("Map").GetComponent<Map>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_map._gameOver || _gameOverManager._isSelect)
        {
            AlphaChange();
        }
    }

    private void AlphaChange()
    {
        switch (_gameOverManager._selectnum)
        {
            case 0:
                _continueButton.alpha = 1.0f;
                _selectButton.alpha = _startalpha;
                _titleButton.alpha = _startalpha;
                _finishButton.alpha = _startalpha;
                break;
            case 1:
                _continueButton.alpha = _startalpha;
                _selectButton.alpha = 1.0f;
                _titleButton.alpha = _startalpha;
                _finishButton.alpha = _startalpha;
                break;
            case 2:
                _continueButton.alpha = _startalpha;
                _selectButton.alpha = _startalpha;
                _titleButton.alpha = 1.0f;
                _finishButton.alpha = _startalpha;
                break;
            case 3:
                _continueButton.alpha = _startalpha;
                _selectButton.alpha = _startalpha;
                _titleButton.alpha = _startalpha;
                _finishButton.alpha = 1.0f;
                break;
            default:
                break;
        }
    }
}