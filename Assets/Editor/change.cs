using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;


//public class change : MonoBehaviour
//{
    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}

    public class ReplaceBallsSimple
    {
        [MenuItem("Tools/Replace Balls With Remy")]
        static void ReplaceBalls()
        {
            // 加载 remy 预制体（放在 Resources 文件夹里，名字必须是 "remy"）
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/prefabs/Remy.prefab");
            if (prefab == null)
            {
                Debug.LogError("未找到 remy 预制体！请确保它在 Resources 文件夹中。");
                return;
            }

            
        GameObject[] spheres = GameObject.FindObjectsOfType<GameObject>()
            .Where(obj => obj.name.Contains("Sphere")).ToArray();


        int count = 0;

        foreach (GameObject sphere in spheres)
        {
            // 记录原始对象的变换信息
            Vector3 pos = sphere.transform.position;
            Quaternion rot = sphere.transform.rotation;
            Vector3 scale = sphere.transform.localScale;

            // 记录层级关系信息
            Transform parent = sphere.transform.parent;
            string sphereName = sphere.name;

            // 销毁原始对象
            Object.DestroyImmediate(sphere);

            // 实例化新预制体
            GameObject newObj = (GameObject)PrefabUtility.InstantiatePrefab(prefab);

            // 应用原始对象的变换信息
            newObj.transform.position = pos;
            newObj.transform.rotation = rot;
            newObj.transform.localScale = scale;

            // 保持原始层级关系
            newObj.transform.SetParent(parent);
            newObj.name = sphereName;

            count++;
        }

        Debug.Log($"替换完成，共替换 {count} 个名称包含'Sphere'的对象。");
    }
    }

