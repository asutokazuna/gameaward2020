using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//くそ雑実装

public class YURAYURA : MonoBehaviour
{
    [SerializeField] float _moveScale = default;
    [SerializeField] float _moveSpeed = default;
    private RectTransform _rect;
    private Vector3 _start;

    // Start is called before the first frame update
    void Start()
    {
        _rect = GetComponent<RectTransform>();
        _start = _rect.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        _rect.localPosition = new Vector3(_start.x + Mathf.Sin(Time.time * _moveSpeed) * _moveScale, _start.y, _start.z);
    }
}
