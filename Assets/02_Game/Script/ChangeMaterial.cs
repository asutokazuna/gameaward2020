using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterial : MonoBehaviour
{
    [SerializeField] GameObject _changeObject;
    [SerializeField] Material[] _materials;
    [SerializeField] int _maxMaterial = 10;
    int _count;

    // Start is called before the first frame update
    void Start()
    {
        _count = -1;
    }

    void SetMaterial()
    {
        _count++;

        if (_count < 0)
        {
            _count = 1;
        }

        if(_count >= _materials.Length)
        {
            _count = _materials.Length - 1;
        }
        Debug.Log(_count);
        Debug.Log(_materials.Length);
        _changeObject.GetComponent<Renderer>().material = _materials[_count];
    }
}
