using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeController : MonoBehaviour
{
    private WorldController wc;

    void Start()
    {
        
    }

    public void AddTree(Vector3 position, int type)
    {
        wc = GetComponent<WorldController>();

        if (type == 0) {
            AddOak(position);
        }
    }

    private void AddOak(Vector3 position)
    {
        float top = position.y + (Random.Range(5, 7)) * wc.cubeSize;

        wc.AddObject(new Vector3(position.x, top + wc.cubeSize, position.z), 4);

        for (float y = position.y; y <= top; y += wc.cubeSize) {
            if (wc.ExistObject(new Vector3(position.x, y, position.z))) {
                wc.RemoveObject(new Vector3(position.x, y, position.z));
            }
            wc.AddObject(new Vector3(position.x, y, position.z), 3);
        }

        for (float x = position.x - wc.cubeSize; x  <= position.x + wc.cubeSize + 1; x += wc.cubeSize) {
            for (float y = top; y > position.y + (wc.cubeSize * 4); y -= wc.cubeSize) {
                wc.AddObject(new Vector3(x, y, position.z), 4);
            }
        }
    }

    void Update()
    {
        
    }
}
