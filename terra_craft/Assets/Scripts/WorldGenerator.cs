using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    private static Dictionary<Vector3,GameObject> blocks = new Dictionary<Vector3,GameObject>();

    public GameObject[] prefabs;
    public GameObject player;

    public int worldWidth = 100;

    public int worldType = 0; // 0 - Default, 1 - Flat

    private GameObject _block;
    private int height = 4;
    private enum State {
        hill = 0,
        plat = 1
    }
    private State _state = State.plat;

    private int aim_h = 0;
    private int aim_l = 0;

    void Start()
    {
        for (float x = -((worldWidth / 2) * 1.15f); x < ((worldWidth / 2) * 1.15f) + 0.5f; x += 1.15f) {
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

            for (int i = 1; i > -20; i--) {
                _block = Instantiate(prefabs[2]) as GameObject;
                _block.transform.position = new Vector3(x, (1.15f * i), 0.0f);
                blocks.Add(_block.transform.position, _block);
            }

            for (int i = 2; i < height + 1; i++) {
                _block = Instantiate(prefabs[1]) as GameObject;
                _block.transform.position = new Vector3(x, (1.15f * i), 0.0f);
                blocks.Add(_block.transform.position, _block);
            }

            _block = Instantiate(prefabs[0]) as GameObject;
            _block.transform.position = new Vector3(x, (1.15f * height+1) + 0.15f, 0.0f);
            blocks.Add(_block.transform.position, _block);

            if (Mathf.Round(x) == 0) {
                player.transform.position = new Vector3(x, (1.15f * height+1) + 1.0f, 0.0f);
            }
        }
    }

    public static void RemoveObject(Vector3 position) {
        foreach (Vector3 key in blocks.Keys) {
            if(position==key) {
                MonoBehaviour.Destroy(blocks[key]);
                blocks.Remove(key);
            }
        }
    }

    void Update()
    {
        
    }
}
