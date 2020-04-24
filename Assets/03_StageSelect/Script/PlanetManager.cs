using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetManager : MonoBehaviour
{
    //フィールドブロックを変数に格納
    public GameObject _stageSelectManager; //!< ステージセレクトマネージャーを中心に置く
 
    [SerializeField] private List<GameObject> _planetObject;
    public float rotateSpeed = 4.0f;            //回転の速さ

    public float _angle = 90.0f; //!<星を回転させたときに回転する角度
    float _oldAngle;//!<回転角度の退避
    public float _rotateFlame = 60.0f; //!<回転時間
    public Vector3 _setPlanetPos; //!<カメラ座標設定
    public Vector3 _setPlanetRot; //!<カメラ注視点設定

    Vector3 _rotateCenter; //!<回転中心座標
    GameObject _selectPlanet;//!<現在選択中の星
    int _cnt = 0; //!<フレーム数カウンター

    // Use this for initialization
    // @details 　publicで設定した値を設定する



    void Start()
    {
        _oldAngle = _angle;
        _rotateCenter = _stageSelectManager.transform.position;
        SetPlanet();
       
    }

    // Update is called once per frame
    void Update()
    {

        //rotatePlanetの呼び出し
        RotatePlanetM();
        RotatePlanetS();
      
    }
    /**
     * @brief 関数概要　カメラの初期値設定
     * @param[in] なし
     * @param[out] なし
     * @return なし
     * @details 　publicで設定した値を設定する
     */

    private void SetPlanet()
    {
        Vector3 _pos = _setPlanetPos;
        _planetObject[0].transform.position = new Vector3(_pos.x, _pos.y, _pos.z);  // 座標を設定
        _planetObject[1].transform.position = new Vector3(_pos.x, _pos.y, _pos.z * -1);  // 座標を設定
        _planetObject[2].transform.position = new Vector3(_pos.x * -1, _pos.y, _pos.z * -1);  // 座標を設定
        _planetObject[3].transform.position = new Vector3(_pos.x * -1, _pos.y, _pos.z);  // 座標を設定
        Debug.Log("a");
        _selectPlanet = _planetObject[0];
    }
    /**
     * @brief 関数概要　フィールド中心座標取得
     * @param[in] なし
     * @param[out] なし
     * @return なし
     * @details 　publicで設定された4隅のフィールドブロックをもとに中心座標を求めて設定する
     */

    // マウスで星を回転させる関数
    private void RotatePlanetM()
    {
        //Vector3でX,Y方向の回転の度合いを定義
        Vector3 angle = new Vector3(Input.GetAxis("Mouse X") * rotateSpeed, Input.GetAxis("Mouse Y") * rotateSpeed, 0);

        for (int i = 0; i < 4; i++)
        {
            if (_selectPlanet == _planetObject[0])
            {
                //transform.RotateAround()をしようして星を回転させる
                _planetObject[i].transform.RotateAround(_planetObject[i].transform.position, Vector3.up, angle.x);
                _planetObject[i].transform.RotateAround(_planetObject[i].transform.position, transform.right, angle.y);
            }
     
        }


    }

    private void ResetPlanet()
    {

        for (int i = 0; i < 4; i++)
        {
            _planetObject[i].transform.localEulerAngles = Vector3.zero;
        }
    }
    // キーで星を回転させる関数
    private void RotatePlanetS()
    {

        //カウンター初期化
        if (_cnt == 0 && _angle == 0.0f)
        {
            _cnt = (int)_rotateFlame / 1;
        }

        //左右矢印キーで回転
        if (_cnt == (int)_rotateFlame / 1)//回転中じゃなければ
        {
            ChangeChoicePlanet();
        }

        //カウント中は回転
        if (_cnt > 0 && _angle != 0)
        {

            for (int i = 0; i < 4; i++)
            {
                //星を回転させる
                _planetObject[i].transform.RotateAround(_rotateCenter, Vector3.up, _angle);
            }

            _cnt -= 1;//カウンター減算
        }
        //回転フレーム数終了で初期化
        else if (_cnt <= 0 && _angle != 0)
        {
            _cnt = (int)_rotateFlame / 1;//カウンター初期化
            _angle = 0.0f;
        }
        //  Debug.Log(_cnt);
    }

    private void ChangeChoicePlanet()
    {

        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            ResetPlanet();
            if (_selectPlanet == _planetObject[0])
                _selectPlanet = _planetObject[3];
            else if (_selectPlanet == _planetObject[3])
                _selectPlanet = _planetObject[2];
            else if (_selectPlanet == _planetObject[2])
                _selectPlanet = _planetObject[1];
            else if (_selectPlanet == _planetObject[1])
                _selectPlanet = _planetObject[0];

            _angle = _oldAngle / _rotateFlame;
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            ResetPlanet();
         
            if (_selectPlanet == _planetObject[0])
                _selectPlanet = _planetObject[1];
            else if (_selectPlanet == _planetObject[1])
                _selectPlanet = _planetObject[2];
            else if (_selectPlanet == _planetObject[2])
                _selectPlanet = _planetObject[3];
            else if (_selectPlanet == _planetObject[3])
                _selectPlanet = _planetObject[0];

            _angle = (_oldAngle * -1) / _rotateFlame;
        }
        
    }
}
