using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    [SerializeField] private GameObject world;
    public WorldGenerator wg;

    void Start()
    {
        wg = world.GetComponent<WorldGenerator>();
    }

    void Update()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(pos.x - (pos.x % 1.15f), pos.y - (pos.y % 1.15f), 0);

        if(Input.GetMouseButtonDown(0)) {
            if (!WorldGenerator.RemoveObject(transform.position)) {
                wg.AddObject(transform.position, 0);
            }
        }
    }
}
