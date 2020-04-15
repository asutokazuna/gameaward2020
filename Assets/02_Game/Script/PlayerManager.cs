using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // 定数定義
    [SerializeField] float _moveTime;   // 移動時間

    // Start is called before the first frame update
    void Start()
    {
        if (_moveTime <= 0)
        {// 移動時間が0を下回った状態でセットされていたら
            _moveTime = 1;  // デフォルトタイムにセット
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float MoveTime
    {
        get { return _moveTime; }
    }
}
