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
        // ע��һ��������ʱ����5���ִ�У�
        _basicTimer = Timer.Register(5f, () =>
        {
            Debug.Log("������ʱ����ɣ�");
        });
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            _basicTimer.Cancel();
            Debug.Log("��ʱ����ȡ��");
        }
    }


}
