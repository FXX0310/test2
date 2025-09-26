using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class disstopwatch : MonoBehaviour
{
    private GameObject[] allBalls;
    private List<GameObject> nearestBalls = new List<GameObject>();
    private List<float> distances = new List<float>();
    private Stopwatch stopwatch = new Stopwatch();


    // Start is called before the first frame update
    void Start()
    {
       allBalls = GameObject.FindGameObjectsWithTag("ball");
        // 开始距离计算协程
        StartCoroutine(CalculateDistances());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator CalculateDistances()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f); // 每1秒计算一次

            CalculateNearestBalls();
        }
    }

    void CalculateNearestBalls()
    {
        stopwatch.Restart();

        if (allBalls == null || allBalls.Length == 0)
            return;

        // 当前小球的位置（只使用X和Z坐标）
        Vector2 currentPos = new Vector2(transform.position.x, transform.position.z);

        // 计算到其他小球的距离（使用XZ平面的欧几里得距离）
        List<(GameObject ball, float distance)> ballDistances = new List<(GameObject, float)>();

        for (int i = 0; i < allBalls.Length; i++)
        {
            if (allBalls[i] == null || allBalls[i] == this.gameObject)
                continue;

            Vector2 otherPos = new Vector2(allBalls[i].transform.position.x, allBalls[i].transform.position.z);
            // 计算平方距离：(Δx)² + (Δz)²，避免开方运算
            float dx = otherPos.x - currentPos.x;
            float dz = otherPos.y - currentPos.y; // 注意：Vector2的y对应的是3D空间中的z
            float distance = dx * dx + dz * dz;

            //float distance = Vector2.Distance(currentPos, otherPos);
            ballDistances.Add((allBalls[i], distance));
        }

        // 按距离排序并取最近的5个
        ballDistances.Sort((a, b) => a.distance.CompareTo(b.distance));

        // 更新最近小球列表
        nearestBalls.Clear();
        distances.Clear();

        int count = Mathf.Min(5, ballDistances.Count);
        for (int i = 0; i < count; i++)
        {
            nearestBalls.Add(ballDistances[i].ball);
            distances.Add(ballDistances[i].distance);
        }

        stopwatch.Stop();

        // 输出结果
        LogDistanceResults();
    }

    void LogDistanceResults()
    {
        UnityEngine.Debug.Log($"=== 距离计算完成 (耗时: {stopwatch.Elapsed.TotalMilliseconds:F4}ms) ===");
        UnityEngine.Debug.Log($"当前小球: {gameObject.name}");
        UnityEngine.Debug.Log($"位置: X={transform.position.x:F2}, Z={transform.position.z:F2}");

        UnityEngine.Debug.Log("最近的5个小球 (XZ平面距离):");
        for (int i = 0; i < nearestBalls.Count; i++)
        {
            if (nearestBalls[i] != null)
            {
                Vector2 otherPos = new Vector2(nearestBalls[i].transform.position.x, nearestBalls[i].transform.position.z);
                UnityEngine.Debug.Log($"{i + 1}. {nearestBalls[i].name} - 距离: {distances[i]:F2} - 位置(X={otherPos.x:F2}, Z={otherPos.y:F2})");
            }
        }
        UnityEngine.Debug.Log("=============================");
    }


}
