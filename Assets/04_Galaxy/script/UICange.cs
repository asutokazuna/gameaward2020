using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//くそ雑実装

public class UICange : MonoBehaviour
{
    [SerializeField] GameObject[] _gameObject = default;
    [SerializeField] int _planetNo;
    [SerializeField] int MAX_UI;
    private StageSelectUI _stageSelectUI;

    // Start is called before the first frame update
    void Start()
    {
        _stageSelectUI = GameObject.Find("Number").GetComponent<StageSelectUI>();

        _planetNo = 0;
        PlanerNameOff();
    }

    public void ChangePlanetName(bool b)    //false : 左, true : 右
    {
        _stageSelectUI.SetSelectFinish();
        //_gameObject[_planetNo].SetActive(false);

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

        //_gameObject[_planetNo].SetActive(true);
    }

    public void PlanerNameOff()
    {
        for (int i = 0; i < _planetNo; i++)
        {
            _gameObject[i].SetActive(false);
        }
    }

    public void PlanerNameOn()
    {
        _gameObject[_planetNo].SetActive(true);
    }
}
