using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTimer;

//public class ball : MonoBehaviour
//{
//    // Start is called before the first frame update
//    public GameObject[] balls; // 存储所有小球的数组
//    public float xRange = 10f; // X轴随机范围
//    public float zRange = 10f; // Z轴随机范围



//    void Start()
//    {
//        balls = GameObject.FindGameObjectsWithTag("ball");

//        // 开始随机移动协程
//        StartCoroutine(RandomizePositions());

//        // 开始位置日志输出协程
//        StartCoroutine(LogPositions());
//    }

//    // Update is called once per frame
//    void Update()
//    {
        
//    }

//    IEnumerator RandomizePositions()
//    {
//        while (true)
//        {
//            // 等待1秒
//            yield return new WaitForSeconds(1f);

//            // 遍历所有小球并随机设置位置
//            foreach (GameObject ball in balls)
//            {
//                // 获取当前位置的Y值（保持不变）
//                float currentY = ball.transform.position.y;

//                // 生成随机的X和Z坐标
//                float randomX = Random.Range(-xRange, xRange);
//                float randomZ = Random.Range(-zRange, zRange);

//                // 设置新位置（保持Y坐标不变）
//                ball.transform.position = new Vector3(randomX, currentY, randomZ);
//            }

//            Debug.Log("所有小球位置已更新");
//        }
//    }

//    IEnumerator LogPositions()
//    {
//        while (true)
//        {
//            // 等待0.5秒
//            yield return new WaitForSeconds(0.5f);

//            // 输出所有小球的位置信息
//            Debug.Log("=== 小球位置报告 ===");
//            for (int i = 0; i < balls.Length; i++)
//            {
//                if (balls[i] != null) // 确保小球仍然存在
//                {
//                    Vector3 pos = balls[i].transform.position;
//                    // 添加小球名称信息
//                    string ballName = balls[i].name;
//                    Debug.Log($"小球 {i} (名称: {ballName}): 位置(X={pos.x:F2}, Y={pos.y:F2}, Z={pos.z:F2})");
//                }
//                else
//                {
//                    Debug.LogWarning($"小球 {i} 已被销毁或不存在");
//                }
//            }
//            Debug.Log("==================");
//        }
//    }
//}






public class ball : MonoBehaviour
{
    public GameObject[] balls; // 存储所有小球的数组
    public float xRange = 10f; // X轴随机范围
    public float zRange = 10f; // Z轴随机范围

    // 使用 UnityTimer 替代协程
    private Timer _positionTimer;
    private Timer _logTimer;

    void Awake()
    {
        balls = GameObject.FindGameObjectsWithTag("ball");

        // 使用 UnityTimer 替代协程
        // 每1秒随机更新位置
        _positionTimer = Timer.Register(1f, RandomizePositions, isLooped: true);

        // 每0.5秒输出位置信息
        _logTimer = Timer.Register(0.5f, LogPositions, isLooped: true);
    }

    void Update()
    {
        // Update 方法现在可以保持为空，或者添加其他逻辑
    }

    void RandomizePositions()
    {
        // 遍历所有小球并随机设置位置
        foreach (GameObject ball in balls)
        {
            if (ball == null) continue;

            // 获取当前位置的Y值（保持不变）
            float currentY = ball.transform.position.y;

            // 生成随机的X和Z坐标
            float randomX = Random.Range(-xRange, xRange);
            float randomZ = Random.Range(-zRange, zRange);

            // 设置新位置（保持Y坐标不变）
            ball.transform.position = new Vector3(randomX, currentY, randomZ);
        }

        Debug.Log("所有小球位置已更新");
    }

    void LogPositions()
    {
        // 输出所有小球的位置信息
        Debug.Log("=== 小球位置报告 ===");
        for (int i = 0; i < balls.Length; i++)
        {
            if (balls[i] != null) // 确保小球仍然存在
            {
                Vector3 pos = balls[i].transform.position;
                // 添加小球名称信息
                string ballName = balls[i].name;
                Debug.Log($"小球 {i} (名称: {ballName}): 位置(X={pos.x:F2}, Y={pos.y:F2}, Z={pos.z:F2})");
            }
            else
            {
                Debug.LogWarning($"小球 {i} 已被销毁或不存在");
            }
        }
        Debug.Log("==================");
    }

    void OnDestroy()
    {
        // 销毁时取消所有 Timer
        if (_positionTimer != null)
            _positionTimer.Cancel();

        if (_logTimer != null)
            _logTimer.Cancel();
    }
}

