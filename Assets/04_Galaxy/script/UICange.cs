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


    // Start is called before the first frame update
    void Start()
    {
        _planetNo = 0;
        for (int i = 0; i < _planetNo; i++) 
        {
            _gameObject[i].SetActive(false);
        }
    }

    public void ChangePlanetName(bool b)    //false : 左, true : 右
    {
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
}
