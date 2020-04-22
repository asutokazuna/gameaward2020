using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public bool GameOverSwitch;
    public float WaitTime;
    public float DelayTime;

    private Map FlgScript;
    private Gameover GameOverScript;
    private bool _isFirst;
    private float _timer;
    private float _changeDelay;

    // Start is called before the first frame update
    void Start()
    {
        FlgScript = GameObject.Find("Map").GetComponent<Map>();
        GameOverScript = GameObject.Find("Gameover").GetComponent<Gameover>();
        _isFirst = true;
        _timer = WaitTime;
        _changeDelay = DelayTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameOverSwitch && FlgScript._gameOver)
        {
            if (_timer <= 0.0f)
            {
                SetGameOver();
            }
            else
            {
                _timer -= Time.deltaTime;
            }
        }

        if(GameOverScript._finishGameover)
        {
            if (_changeDelay <= 0.0f || Input.anyKey)
            {
                //シーン遷移
                SceneManager.LoadScene("SampleScene");
            }
            else
            {
                _changeDelay -= Time.deltaTime;
            }
        }
    }

    private void SetGameOver()
    {
        if (_isFirst)
        {
            GameOverScript.StartGameover();
            _isFirst = false;
        }
    }
}
