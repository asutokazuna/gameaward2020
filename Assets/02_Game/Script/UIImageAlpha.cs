using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIImageAlpha : MonoBehaviour
{
    public CanvasGroup _continueButton = default;
    public CanvasGroup _selectButton = default;
    public CanvasGroup _titleButton = default;
    public CanvasGroup _finishButton = default;
    
    public GameOverManager _gameOverManager = default;
    public Map _map = default;
    //private GameMenu _menu;

    public float _startalpha = default;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("あああああああああああああああああああああああああああああああ");

        _continueButton.alpha = _startalpha;
        _selectButton.alpha = _startalpha;
        _titleButton.alpha = _startalpha;
        _finishButton.alpha = _startalpha;

        _gameOverManager = GameObject.Find("GameOverManager").GetComponent<GameOverManager>();
        _map = GameObject.FindGameObjectWithTag("Map").GetComponent<Map>();
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