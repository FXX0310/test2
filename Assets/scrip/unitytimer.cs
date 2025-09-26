using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTimer;

public class unitytimer : MonoBehaviour
{
    // Start is called before the first frame update
    private Timer _basicTimer;

    void Start()
    {
        
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}

    private void Awake()
    {
        // 注册一个基础计时器（5秒后执行）
        _basicTimer = Timer.Register(5f, () =>
        {
            Debug.Log("基础计时器完成！");
        });
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            _basicTimer.Cancel();
            Debug.Log("计时器已取消");
        }
    }


}
