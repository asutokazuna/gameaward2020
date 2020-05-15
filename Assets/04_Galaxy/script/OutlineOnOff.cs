using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OutlineOnOff : MonoBehaviour
{
    private MeshRenderer _meshRend;
    [SerializeField] float _outlineWidth = 0.0f;
    [SerializeField] Color _outlineColor = default;
    [SerializeField] Color _color1 = default;
    [SerializeField] Color _color2 = default;

    // Start is called before the first frame update
    void Start()
    {
        _meshRend = GetComponent<MeshRenderer>();
        _meshRend.material.SetFloat("_Outline_Width", 0.0f);
        _meshRend.material.SetColor("_Outline_Color", _outlineColor);

        //Invoke("OutlineOn", 5.0f);
        //Invoke("OutlineOff", 10.0f);

    }

    private void Update()
    {
        _outlineColor = Color.Lerp(_color1, _color2, Mathf.PingPong(Time.time, 1));
        _meshRend.material.SetColor("_Outline_Color", _outlineColor);
    }

    public void OutlineOn()
    {
        _meshRend.material.SetFloat("_Outline_Width", _outlineWidth);
    }

    public void OutlineOff()
    {
        _meshRend.material.SetFloat("_Outline_Width", 0.0f);
    }

    //ColorとWidthを実行後に変更したい時用. ※OutlineOnを呼ぶまでは適用されない.
    public void OutlineCustom(Color col, float width)
    {
        _outlineWidth = width;
        _outlineColor = col;
    }

}
