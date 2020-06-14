using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeFace : MonoBehaviour
{
    [SerializeField] SkinnedMeshRenderer _meshRend;
    private Vector2[] _faceOffset = new Vector2[4];

    // Start is called before the first frame update
    void Start()
    {
        //_meshRend = GetComponent<SkinnedMeshRenderer>();

        _faceOffset[0] = new Vector2(0.22f, 0.0f);
        _faceOffset[1] = new Vector2(0.72f, 0.0f);
        _faceOffset[2] = new Vector2(0.22f, -0.5f);
        _faceOffset[3] = new Vector2(0.72f, -0.5f);


        _meshRend.material.SetTextureOffset("_MainTex", _faceOffset[0]);
        _meshRend.material.SetTextureOffset("_1st_ShadeMap", _faceOffset[0]);
    }



    // Update is called once per frame
    void Update()   //仮
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.F1))
        {
            SetFace(0);
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            SetFace(1);
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            SetFace(2);
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            SetFace(3);
        }
#endif

    }

    //引数:No ... 0:通常, 1:喜び, 2:はわわの民, 3:>ㇸ<
    public void SetFace(int No)
    {
        if (No < 0 || No > 3) return;
        _meshRend.material.SetTextureOffset("_MainTex", _faceOffset[No]);
        _meshRend.material.SetTextureOffset("_1st_ShadeMap", _faceOffset[No]);
    }
}
