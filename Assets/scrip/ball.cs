using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ball : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] balls; // �洢����С�������
    public float xRange = 10f; // X�������Χ
    public float zRange = 10f; // Z�������Χ



    void Start()
    {
        balls = GameObject.FindGameObjectsWithTag("ball");

        // ��ʼ����ƶ�Э��
        StartCoroutine(RandomizePositions());

        // ��ʼλ����־���Э��
        StartCoroutine(LogPositions());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator RandomizePositions()
    {
        while (true)
        {
            // �ȴ�1��
            yield return new WaitForSeconds(1f);

            // ��������С���������λ��
            foreach (GameObject ball in balls)
            {
                // ��ȡ��ǰλ�õ�Yֵ�����ֲ��䣩
                float currentY = ball.transform.position.y;

                // ���������X��Z����
                float randomX = Random.Range(-xRange, xRange);
                float randomZ = Random.Range(-zRange, zRange);

                // ������λ�ã�����Y���겻�䣩
                ball.transform.position = new Vector3(randomX, currentY, randomZ);
            }

            Debug.Log("����С��λ���Ѹ���");
        }
    }

    IEnumerator LogPositions()
    {
        while (true)
        {
            // �ȴ�0.5��
            yield return new WaitForSeconds(0.5f);

            // �������С���λ����Ϣ
            Debug.Log("=== С��λ�ñ��� ===");
            for (int i = 0; i < balls.Length; i++)
            {
                if (balls[i] != null) // ȷ��С����Ȼ����
                {
                    Vector3 pos = balls[i].transform.position;
                    // ���С��������Ϣ
                    string ballName = balls[i].name;
                    Debug.Log($"С�� {i} (����: {ballName}): λ��(X={pos.x:F2}, Y={pos.y:F2}, Z={pos.z:F2})");
                }
                else
                {
                    Debug.LogWarning($"С�� {i} �ѱ����ٻ򲻴���");
                }
            }
            Debug.Log("==================");
        }
    }
}
