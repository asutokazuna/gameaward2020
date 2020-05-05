using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectUI : MonoBehaviour
{
    [SerializeField] int _stageNum;     //!< 1惑星当たりのステージ数
    NumberImage _stageSelectImage;      //!< ステージNoを画像
    E_SCENE     _oldStageId;            //!< 選ばれてるステージID管理用
    E_SCENE     _stageId;               //!< 選ばれてるステージID管理用
    Animator    _stageSelectAnim;       //!< アニメーター取得用

    // Start is called before the first frame update
    void Start()
    {
        // 初期化処理
        _stageSelectAnim = GetComponent<Animator>();
        _stageSelectImage = GameObject.Find("CanvasUI").GetComponent<NumberImage>();
        _stageId = GameObject.Find("CameraObj").GetComponent<RaySystem>().GetID();
    }

    // Update is called once per frame
    void Update()
    {
        // 選択されているIDの取得
        _stageId = GameObject.Find("CameraObj").GetComponent<RaySystem>().GetID();
        if (_stageId != _oldStageId)
        {
            SetStageNo();
        }
    }

    void SetStageNo()
    {
        int stageNo = (int)_stageId - 1;

        while(stageNo > _stageNum)
        {
            stageNo -= _stageNum;
        }

        _oldStageId = _stageId;

        if (stageNo > 0)
        {
            _stageSelectImage.SetValue(stageNo);
            _stageSelectAnim.SetBool("Select", true);
        }
        else
        {
            _stageSelectAnim.SetBool("Select", false);
        }
    }

    void SetNoChange()
    {

    }
}
