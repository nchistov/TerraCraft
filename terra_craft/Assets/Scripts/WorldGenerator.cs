using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    private static List<GameObject> blocks = new List<GameObject>();

    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private GameObject cursor;
    [SerializeField] private GameObject removing;
    public GameObject player;

    public int worldWidth = 100;

    public int worldType = 0; // 0 - Default, 1 - Flat

    private static GameObject _block;
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

            for (int i = height - 5; i > -20; i--) {
                AddObject(new Vector3(x, (1.15f * i), 0.0f), 2);
            }

            for (int i = height - 4; i < height + 1; i++) {
                AddObject(new Vector3(x, (1.15f * i), 0.0f), 1);
            }

            AddObject(new Vector3(x, (1.15f * height+1) + 0.15f, 0.0f), 0);

            if (Mathf.Round(x) == 0) {
                player.transform.position = new Vector3(x, (1.15f * height+1) + 1.0f, 0.0f);
            }
        }
    }

    public GameObject GetObject(Vector3 position)
    {
        foreach (GameObject block in blocks) {
            if (position.y > block.transform.position.y - 0.28f && position.y < block.transform.position.y + 0.28f) {
                if (position.x > block.transform.position.x - 0.28f && position.x < block.transform.position.x + 0.28f) {
                    return block;
                }
            }
        }
        return null;
    }

    public void RemoveObject(Vector3 position)
    {
        if (ExistObject(position)) {
            GameObject block = GetObject(position);
            blocks.Remove(block);
            Destroy(block.gameObject);
        }
    }

    public IEnumerator RemoveBlock(Vector3 position)
    {
        if (ExistObject(position)) {
            GameObject block = GetObject(position);
            Vector3 pos1 = position;
            removing.SetActive(true);
            removing.transform.position = pos1;
            for(int i = 0; i < 8; i++) {
                removing.transform.localScale = new Vector3(removing.transform.localScale.x + 0.1f, removing.transform.localScale.y + 0.1f,
                                                        removing.transform.localScale.z);
                yield return new WaitForSeconds(0.05f);
                removing.SetActive(true);  // Этот код должен замениться.

                if (!Input.GetMouseButton(0) || !(cursor.transform.position == pos1)) {
                    removing.transform.localScale = new Vector3(0.1f, 0.1f, 1);
                    removing.SetActive(false);
                    yield break;
                }
            }
            Destroy(block.gameObject);
            blocks.Remove(block);

            removing.transform.localScale = new Vector3(0.1f, 0.1f, 1);
            removing.SetActive(false);
        }
    }

    public void AddObject(Vector3 position, int type)
    {
        _block = Instantiate(prefabs[type]) as GameObject;
        _block.transform.position = position;
        blocks.Add(_block);
    }

    public bool ExistObject(Vector3 position)
    {
        foreach (GameObject block in blocks) {
            if (position.y > block.transform.position.y - 0.28f && position.y < block.transform.position.y + 0.28f) {
                if (position.x > block.transform.position.x - 0.28f && position.x < block.transform.position.x + 0.28f) {
                    return true;
                }
            }
        }
        return false;
    }

    void Update()
    {
        
    }
}
