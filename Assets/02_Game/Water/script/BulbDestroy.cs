using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulbDestroy : MonoBehaviour
{
    public float fTargetTime;//消滅までの時間
    private float fTime;//経過時間


    // Start is called before the first frame update
    void Start()
    {
        fTime = fTargetTime;
    }

    // Update is called once per frame
    void Update()
    {
        fTime -= Time.deltaTime;

        if(fTime <= 0.0f)
        {
            //this.gameObject.GetComponent<NVIDIA.Flex.FlexSourceActor>().isActive = false;
            Destroy(gameObject);
        }
    }
}
