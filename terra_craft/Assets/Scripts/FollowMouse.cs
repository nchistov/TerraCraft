using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    [SerializeField] private GameObject world;
    public WorldController wc;

    private Vector3 lastPos = new Vector3(1, 1, 0);

    public int cubeType = 0;

    void Start()
    {
        wc = world.GetComponent<WorldController>();
    }

    void Update()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(pos.x - (pos.x % 1.15f), pos.y - (pos.y % 1.15f), 0);

        if(Input.GetMouseButton(0)) {
            if (transform.position != lastPos) {
                if (!wc.ExistObject(transform.position)) {
                    wc.AddObject(transform.position, cubeType);
                } else {
                    StartCoroutine(wc.RemoveBlock(transform.position));
                }
                lastPos = transform.position;
            }
        } else {
            lastPos = new Vector3(1, 1, 0);
        }
    }
}
