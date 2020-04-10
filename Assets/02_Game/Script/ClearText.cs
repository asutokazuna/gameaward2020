/**
 * @file    CleatText.cs
 * @brief   クリアテキスト表示用
 * @author  Risa Ito
 * @date    2020/03/31(月)   作成
 * @data    2020/04/10(金)   マクロによるFieldControllerへの参照をブロック
 */


#define MODE_MAP


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @class CleatText
 * @brief クリアテキストの表示
 */
public class ClearText : MonoBehaviour
{
    //! クリアテキスト表示用
    public GameObject _clearObject;

    //! FieldController取得用
    public FieldController _fieldController;

    // Start is called before the first frame update
    void Start()
    {
        _clearObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
#if !MODE_MAP
        _clearObject.SetActive(_fieldController.GetCraer());
#endif
    }
}


// EOF