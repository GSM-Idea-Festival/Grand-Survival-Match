using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] prefabs;
    GameObject[] buf;

    void Start()
    {
        buf = new GameObject[prefabs.Length];
        for (int i = 0; i < prefabs.Length; i++)
        {
            buf[i] = Instantiate(prefabs[i], transform.position, transform.rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDestroy()
    {
        foreach(GameObject prefab in buf)
        {
            Destroy(prefab);
        }
    }
}
