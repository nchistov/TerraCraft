using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public GameObject[] prefabs;

    private GameObject _block;
    private int height = 4;

    void Start()
    {
        for (float x = -57.5f; x < 58.0f; x += 1.15f) {
            int j = Random.Range(0, 2);
            if (j == 0) {
                height++;
            } else {
                if (height > 4) {
                    height--;
                }
            }

            _block = Instantiate(prefabs[2]) as GameObject;
                _block.transform.position = new Vector3(x, 1.15f, 0.0f);
            for (int i = 2; i < height + 1; i++) {
                _block = Instantiate(prefabs[1]) as GameObject;
                _block.transform.position = new Vector3(x, (1.15f * i), 0.0f);
            }
            _block = Instantiate(prefabs[0]) as GameObject;
            _block.transform.position = new Vector3(x, (1.15f * height+1) + 0.15f, 0.0f);
        }
    }

    void Update()
    {
        
    }
}
