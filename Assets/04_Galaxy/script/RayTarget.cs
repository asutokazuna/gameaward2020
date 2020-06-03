using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayTarget : MonoBehaviour
{
    public GameObject _targetObj;
    public Vector3 _dirVector;
    [SerializeField]
    private Vector3 _startPos;

    private bool _isFirst = false;
    // Start is called before the first frame update
    void Start()
    {
        _dirVector = transform.parent.parent.transform.position - _targetObj.transform.position;
        _startPos = _targetObj.transform.localPosition;
    }


    

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LandMove(bool flg)
    {
        if(flg)
        {
            if(!_isFirst)
            {
                _targetObj.transform.position = Vector3.MoveTowards(_targetObj.transform.position, transform.parent.parent.transform.position, -0.2f);
                _isFirst = true;
            }
           
        }
        else
        {
            _targetObj.transform.localPosition = _startPos;
            _isFirst = false;
        }
    }
}
