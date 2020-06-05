using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearObjAnim : MonoBehaviour
{
    [SerializeField] Animator[]   _setObject;
    [SerializeField] float          _startTime;
    private int         _count;
    private ClearObject _clearObject;

    // Start is called before the first frame update
    void Start()
    {
        _clearObject = GameObject.Find("ClearObjectManager").GetComponent<ClearObject>();
        _count = 0;

        float time;

        for (int i = 0; i < _setObject.Length; i++)
        {
            if (_clearObject._isChange)
            {
                time = _startTime + i * 0.1f;

                Invoke("SetObject", time);
            }
            else
            {
                _setObject[i].SetBool("Finished", true);
            }
        }
    }

    void SetObject()
    {
        _setObject[_count].SetBool("Set", true);
        _count++;
    }
}
