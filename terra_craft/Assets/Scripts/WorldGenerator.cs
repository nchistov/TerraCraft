using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public GameObject player;

    public int worldWidth = 100;

    public int worldType = 0; // 0 - Default, 1 - Flat
    private int height = 4;
    private enum State {
        hill = 0,
        plat = 1
    }
    private State _state = State.plat;

    private int aim_h = 0;
    private int aim_l = 0;

    private int init_trees_probability = 50;
    private int trees_probability = 50;

    private WorldController wc;

    void Start()
    {
        wc = gameObject.GetComponent<WorldController>();

        for (float x = -((worldWidth / 2) * wc.cubeSize); x < ((worldWidth / 2) * wc.cubeSize) + 0.5f; x += wc.cubeSize) {
            if (worldType == 0) {
                if (aim_l == 0 && _state == State.plat) {
                    _state = State.hill;
                    aim_h = Random.Range(-5, 6);
                } else if (_state == State.hill && aim_h != 0) {
                    if (aim_h < 0) {
                        if (height > 3) {
                            height -= 1;
                            aim_h += 1;
                        } else {
                            aim_h = 0;
                        }
                    } else if (aim_h > 0) {
                        height += 1;
                        aim_h -= 1;
                    }
                } else if (aim_h == 0 && _state == State.hill) {
                    _state = State.plat;
                    aim_l = Random.Range(1, 6);
                } else if (_state == State.plat) {
                    aim_l -= 1;
                }
            }

            for (int i = height - 5; i > -20; i--) {
                wc.AddObject(new Vector3(x, (wc.cubeSize * i), 0.0f), 2);
            }

            for (int i = height - 4; i < height + 1; i++) {
                wc.AddObject(new Vector3(x, (wc.cubeSize * i), 0.0f), 1);
            }

            wc.AddObject(new Vector3(x, (wc.cubeSize * height+1) + 0.15f, 0.0f), 0);

            if (Mathf.Round(x) == 0) {
                player.transform.position = new Vector3(x, (wc.cubeSize * height+1) + 1.0f, 0.0f);
            }

            wc.AddObject(new Vector3(x, (wc.cubeSize * -20), 0.0f), 5);

            if (Random.Range(0, trees_probability) == 0) {
                trees_probability = init_trees_probability;
                wc.AddTree(new Vector3(x, (wc.cubeSize * (height+2)), 0.0f), 0);
            } else {
                trees_probability -= 1;
            }
        }
    }

    void Update()
    {
        
    }
}
