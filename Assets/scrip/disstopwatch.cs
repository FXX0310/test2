using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityTimer;

public class disstopwatch : MonoBehaviour
{
    private GameObject[] allBalls;
    private List<GameObject> nearestBalls = new List<GameObject>();
    private List<float> distances = new List<float>();
    private Stopwatch stopwatch = new Stopwatch();
    private List<double> calculationTimes = new List<double>();
    private int maxRecordCount = 10;

    // 使用 UnityTimer 替代协程
    private Timer _distanceTimer;

    void Start()
    {
        allBalls = GameObject.FindGameObjectsWithTag("ball");

        // 使用 UnityTimer 替代协程 - 每1秒计算一次距离
        _distanceTimer = Timer.Register(1f, CalculateNearestBalls, isLooped: true);
    }

    void Update()
    {
        // Update 方法可以保持为空
    }

    void CalculateNearestBalls()
    {
        

        if (allBalls == null || allBalls.Length == 0)
            return;

        // 当前小球的位置
        Vector2 currentPos = new Vector2(transform.position.x, transform.position.z);

        // 计算到其他小球的距离
        List<(GameObject ball, float distance)> ballDistances = new List<(GameObject, float)>();

        stopwatch.Restart();
        for (int i = 0; i < allBalls.Length; i++)
        {
            if (allBalls[i] == null || allBalls[i] == this.gameObject)
                continue;
            
            Vector2 otherPos = new Vector2(allBalls[i].transform.position.x, allBalls[i].transform.position.z);
            // 计算平方距离：(Δx)² + (Δz)²，避免开方运算
            float dx = otherPos.x - currentPos.x;
            float dz = otherPos.y - currentPos.y; // 注意：Vector2的y对应的是3D空间中的z
            //float distance = dx * dx + dz * dz;
            float distance = Mathf.Sqrt(dx * dx + dz * dz);
            //float distance = Vector2.Distance(currentPos, otherPos);
            ballDistances.Add((allBalls[i], distance));
           
      
        }
        stopwatch.Stop();



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

        

        double currentTime = stopwatch.Elapsed.TotalMilliseconds;
        calculationTimes.Add(currentTime);

        // 如果超过最大记录数，移除最早的数据
        if (calculationTimes.Count > maxRecordCount)
        {
            calculationTimes.RemoveAt(0);
        }

        // 输出结果
        LogDistanceResults(currentTime);
    }

    void LogDistanceResults(double currentTime)
    {
        double averageTime = CalculateAverageTime();
        UnityEngine.Debug.Log($"=== 距离计算完成 ===");
        UnityEngine.Debug.Log($"本次耗时: {currentTime:F4}ms");
        UnityEngine.Debug.Log($"平均耗时: {averageTime:F4}ms (基于最近{calculationTimes.Count}次计算)");

        // 注释掉的详细日志输出，可按需启用
        /*
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
        */
    }

    double CalculateAverageTime()
    {
        if (calculationTimes.Count == 0)
            return 0;

        double sum = 0;
        foreach (double time in calculationTimes)
        {
            sum += time;
        }
        return sum / calculationTimes.Count;
    }

    void OnDestroy()
    {
        // 销毁时取消 Timer
        if (_distanceTimer != null)
            _distanceTimer.Cancel();
    }
}
//////////////////////////////////////////////////////////
//public class disstopwatch : MonoBehaviour
//{
//    private GameObject[] allSpheres;
//    private List<GameObject> nearestSpheres = new List<GameObject>(); // 这就是存储最近5个小球的List
//    private Stopwatch stopwatch = new Stopwatch();

//    // 用于比较的两种方法
//    private List<GameObject> nearestSpheresMethod1 = new List<GameObject>(); // 方法b1的结果
//    private List<GameObject> nearestSpheresMethod2 = new List<GameObject>(); // 方法b2的结果

//    private double method1Time; // 方法b1的计算时间
//    private double method2Time; // 方法b2的计算时间

//    void Start()
//    {
//        allSpheres = GameObject.FindGameObjectsWithTag("ball");
//        StartCoroutine(CalculateDistances());
//    }

//    IEnumerator CalculateDistances()
//    {
//        while (true)
//        {
//            yield return new WaitForSeconds(1f);
//            CalculateNearestSpheres();
//        }
//    }

//    void CalculateNearestSpheres()
//    {
//        // 方法b2: 使用实际距离 √(ΔX² + ΔZ²)
//        stopwatch.Restart();
//        CalculateUsingActualDistance();
//        stopwatch.Stop();
//        method2Time = stopwatch.Elapsed.TotalMilliseconds;


//        // 方法b1: 使用平方距离 (ΔX² + ΔZ²)
//        stopwatch.Restart();
//        CalculateUsingSquaredDistance();
//        stopwatch.Stop();
//        method1Time = stopwatch.Elapsed.TotalMilliseconds;


//        // 输出结果
//        LogResults();
//    }

//    void CalculateUsingSquaredDistance()
//    {
//        // 计算平方距离并找出最近的5个小球
//        Vector2 currentPos = new Vector2(transform.position.x, transform.position.z);
//        List<(GameObject sphere, float sqrDistance)> sphereDistances = new List<(GameObject, float)>();

//        for (int i = 0; i < allSpheres.Length; i++)
//        {
//            if (allSpheres[i] == null || allSpheres[i] == this.gameObject)
//                continue;

//            Vector2 otherPos = new Vector2(allSpheres[i].transform.position.x, allSpheres[i].transform.position.z);
//            float dx = otherPos.x - currentPos.x;
//            float dz = otherPos.y - currentPos.y;
//            float sqrDistance = dx * dx + dz * dz; // 方法b1: 平方距离

//            sphereDistances.Add((allSpheres[i], sqrDistance));
//        }

//        // 按平方距离排序
//        sphereDistances.Sort((a, b) => a.sqrDistance.CompareTo(b.sqrDistance));

//        // 将结果存储在List中
//        nearestSpheresMethod1.Clear();
//        int count = Mathf.Min(5, sphereDistances.Count);
//        for (int i = 0; i < count; i++)
//        {
//            nearestSpheresMethod1.Add(sphereDistances[i].sphere);
//        }

//        // 可以选择一种方法作为主要结果
//        nearestSpheres = new List<GameObject>(nearestSpheresMethod1);
//    }

//    void CalculateUsingActualDistance()
//    {
//        // 计算实际距离并找出最近的5个小球
//        Vector2 currentPos = new Vector2(transform.position.x, transform.position.z);
//        List<(GameObject sphere, float distance)> sphereDistances = new List<(GameObject, float)>();

//        for (int i = 0; i < allSpheres.Length; i++)
//        {
//            if (allSpheres[i] == null || allSpheres[i] == this.gameObject)
//                continue;

//            Vector2 otherPos = new Vector2(allSpheres[i].transform.position.x, allSpheres[i].transform.position.z);
//            float dx = otherPos.x - currentPos.x;
//            float dz = otherPos.y - currentPos.y;
//            float distance = Mathf.Sqrt(dx * dx + dz * dz); // 方法b2: 实际距离

//            sphereDistances.Add((allSpheres[i], distance));
//        }

//        // 按实际距离排序
//        sphereDistances.Sort((a, b) => a.distance.CompareTo(b.distance));

//        // 将结果存储在List中
//        nearestSpheresMethod2.Clear();
//        int count = Mathf.Min(5, sphereDistances.Count);
//        for (int i = 0; i < count; i++)
//        {
//            nearestSpheresMethod2.Add(sphereDistances[i].sphere);
//        }
//    }

//    void LogResults()
//    {
//        UnityEngine.Debug.Log($"=== 距离计算完成 ===");
//        UnityEngine.Debug.Log($"方法b1 (平方距离) 耗时: {method1Time:F4}ms");
//        UnityEngine.Debug.Log($"方法b2 (实际距离) 耗时: {method2Time:F4}ms");
//UnityEngine.Debug.Log($"时间差异: {method2Time - method1Time}ms");

//UnityEngine.Debug.Log("最近的5个小球 (方法b1 - 平方距离):");
//for (int i = 0; i < nearestSpheresMethod1.Count; i++)
//{
//    if (nearestSpheresMethod1[i] != null)
//    {
//        Vector2 otherPos = new Vector2(nearestSpheresMethod1[i].transform.position.x,
//                                      nearestSpheresMethod1[i].transform.position.z);
//        UnityEngine.Debug.Log($"{i + 1}. {nearestSpheresMethod1[i].name}");
//    }
//}

//UnityEngine.Debug.Log("最近的5个小球 (方法b2 - 实际距离):");
//for (int i = 0; i < nearestSpheresMethod2.Count; i++)
//{
//    if (nearestSpheresMethod2[i] != null)
//    {
//        Vector2 otherPos = new Vector2(nearestSpheresMethod2[i].transform.position.x,
//                                      nearestSpheresMethod2[i].transform.position.z);
//        UnityEngine.Debug.Log($"{i + 1}. {nearestSpheresMethod2[i].name}");
//    }
//}

//UnityEngine.Debug.Log("存储在小球List中的结果:");
//for (int i = 0; i < nearestSpheres.Count; i++)
//{
//    if (nearestSpheres[i] != null)
//    {
//        UnityEngine.Debug.Log($"{i + 1}. {nearestSpheres[i].name}");
//    }
//}

//UnityEngine.Debug.Log("=============================");
//}



////////////////////////////////////////////////////////////////////



//void CalculateNearestSpheres()
//{
//    // 方法 b1: 使用平方距离 (ΔX² + ΔZ²)
//    stopwatch.Restart();
//    CalculateUsingSquaredDistance_Linear(); // ✅ 用线性版本
//    stopwatch.Stop();
//    method1Time = stopwatch.Elapsed.TotalMilliseconds;

//    // 方法 b2: 使用实际距离 √(ΔX² + ΔZ²)
//    stopwatch.Restart();
//    CalculateUsingActualDistance_Linear(); // ✅ 用线性版本
//    stopwatch.Stop();
//    method2Time = stopwatch.Elapsed.TotalMilliseconds;

//    // 输出结果
//    LogResults();
//}

//void CalculateUsingSquaredDistance_Linear()
//{
//    Vector2 currentPos = new Vector2(transform.position.x, transform.position.z);

//    nearestSpheresMethod1.Clear();
//    List<(GameObject sphere, float sqrDistance)> nearestList = new List<(GameObject, float)>();

//    for (int i = 0; i < allSpheres.Length; i++)
//    {
//        if (allSpheres[i] == null || allSpheres[i] == this.gameObject)
//            continue;

//        Vector2 otherPos = new Vector2(allSpheres[i].transform.position.x, allSpheres[i].transform.position.z);
//        float dx = otherPos.x - currentPos.x;
//        float dz = otherPos.y - currentPos.y;
//        float sqrDistance = dx * dx + dz * dz;

//        if (nearestList.Count < 5)
//        {
//            nearestList.Add((allSpheres[i], sqrDistance));
//        }
//        else
//        {
//            // 找出当前列表里最远的
//            int maxIndex = 0;
//            float maxDist = nearestList[0].sqrDistance;
//            for (int j = 1; j < nearestList.Count; j++)
//            {
//                if (nearestList[j].sqrDistance > maxDist)
//                {
//                    maxDist = nearestList[j].sqrDistance;
//                    maxIndex = j;
//                }
//            }

//            // 如果新的更近，就替换掉最远的
//            if (sqrDistance < maxDist)
//            {
//                nearestList[maxIndex] = (allSpheres[i], sqrDistance);
//            }
//        }
//    }

//    // 存储结果
//    foreach (var item in nearestList)
//    {
//        nearestSpheresMethod1.Add(item.sphere);
//    }
//    nearestSpheres = new List<GameObject>(nearestSpheresMethod1);
//}

//void CalculateUsingActualDistance_Linear()
//{
//    Vector2 currentPos = new Vector2(transform.position.x, transform.position.z);

//    nearestSpheresMethod2.Clear();
//    List<(GameObject sphere, float distance)> nearestList = new List<(GameObject, float)>();

//    for (int i = 0; i < allSpheres.Length; i++)
//    {
//        if (allSpheres[i] == null || allSpheres[i] == this.gameObject)
//            continue;

//        Vector2 otherPos = new Vector2(allSpheres[i].transform.position.x, allSpheres[i].transform.position.z);
//        float dx = otherPos.x - currentPos.x;
//        float dz = otherPos.y - currentPos.y;
//        float distance = Mathf.Sqrt(dx * dx + dz * dz);

//        if (nearestList.Count < 5)
//        {
//            nearestList.Add((allSpheres[i], distance));
//        }
//        else
//        {
//            // 找出最远的
//            int maxIndex = 0;
//            float maxDist = nearestList[0].distance;
//            for (int j = 1; j < nearestList.Count; j++)
//            {
//                if (nearestList[j].distance > maxDist)
//                {
//                    maxDist = nearestList[j].distance;
//                    maxIndex = j;
//                }
//            }

//            // 替换
//            if (distance < maxDist)
//            {
//                nearestList[maxIndex] = (allSpheres[i], distance);
//            }
//        }
//    }

//    // 存储结果
//    foreach (var item in nearestList)
//    {
//        nearestSpheresMethod2.Add(item.sphere);
//    }
//}
//void LogResults()
//{
//    UnityEngine.Debug.Log($"=== 距离计算完成 ===");
//    UnityEngine.Debug.Log($"方法b1 (平方距离) 耗时: {method1Time:F4}ms");
//    UnityEngine.Debug.Log($"方法b2 (实际距离) 耗时: {method2Time:F4}ms");
//    //UnityEngine.Debug.Log($"时间差异: {method2Time - method1Time}ms");
//}
//}




