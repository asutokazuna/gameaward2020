using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//くそ雑実装

public class UICange : MonoBehaviour
{
    [SerializeField] GameObject[] _gameObject = default;
    private int             _planetNo = 0;
    private StageSelectUI   _stageSelectUI;
    private GameObject      _setUI;

    // Start is called before the first frame update
    void Start()
    {
        _stageSelectUI = GameObject.Find("StageName").GetComponent<StageSelectUI>();
        _setUI = GameObject.Find("PlanetUI");
        _planetNo = GameObject.Find("CameraObj").GetComponent<CameraMove>()._currentID - 1;
        for (int i = 0; i < _gameObject.Length; i++)
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
            _planetNo = _gameObject.Length - 1;
        }
        if(_planetNo > _gameObject.Length - 1) {
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
