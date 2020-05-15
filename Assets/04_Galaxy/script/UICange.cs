using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//くそ雑実装

public class UICange : MonoBehaviour
{
    [SerializeField] GameObject[] _gameObject = default;
    [SerializeField] int _planetNo = 0;
    [SerializeField] int MAX_UI = 0;
    private StageSelectUI _stageSelectUI;
    private GameObject _setUI;

    // Start is called before the first frame update
    void Start()
    {
        _stageSelectUI = GameObject.Find("StageName").GetComponent<StageSelectUI>();
        _setUI = GameObject.Find("PlanetUI");
        _planetNo = 0;
        for (int i = 0; i < MAX_UI + 1; i++)
        {
            _gameObject[i].SetActive(false);
        }
        _gameObject[_planetNo].SetActive(true);
    }

    public void ChangePlanetName(bool b)    //false : 左, true : 右
    {
        _stageSelectUI.SetSelectFinish();

        _gameObject[_planetNo].SetActive(false);

        if (b){
            _planetNo--;
        } else {
            _planetNo++;
        }
        if(_planetNo < 0) {
            _planetNo = MAX_UI;
        }
        if(_planetNo > MAX_UI) {
            _planetNo = 0;
        }

        _gameObject[_planetNo].SetActive(true);
    }

    public void PlanetNameOff()
    {
        _setUI.SetActive(false);
    }

    public void PlanerNameOn()
    {
        _setUI.SetActive(true);
    }
}
