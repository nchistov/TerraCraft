using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public GameObject[] prefabs;

    private GameObject _block;

    void Start()
    {
        for (float x = -57.5f; x < 58.0f; x += 1.15f) {
            for (int i = 1; i < 4; i++) {
                _block = Instantiate(prefabs[i - 1]) as GameObject;
                _block.transform.position = new Vector3(x, -(1.15f * i), 0.0f);
            }
        }
    }

    void Update()
    {
        
    }
}
