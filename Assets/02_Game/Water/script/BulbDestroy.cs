using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NVIDIA.Flex;


public class BulbDestroy : MonoBehaviour
{
    public float fTargetTime;//消滅までの時間
    private float fTime;//経過時間
    private bool bFlg;

    // Start is called before the first frame update
    void Start()
    {
        fTime = fTargetTime;
        bFlg = false;
    }

    // Update is called once per frame
    void Update()
    {
       
        fTime -= Time.deltaTime;

        if(fTime <= 0.0f)
        {
            // this.gameObject.GetComponent<NVIDIA.Flex.FlexSourceActor>().startSpeed = 0.5f;
            //this.gameObject.GetComponent<NVIDIA.Flex.FlexSourceActor>().isActive = false;
            Destroy(gameObject);
            
            bFlg = !bFlg;
            fTime = fTargetTime;
        }

        if (bFlg)
        {
            //this.gameObject.GetComponent<NVIDIA.Flex.FlexFluidRenderer>().enabled = false;
        }
        else
        {
           // this.gameObject.GetComponent<NVIDIA.Flex.FlexFluidRenderer>().enabled = true;
        }
    }
}
