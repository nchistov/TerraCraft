using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    private static Dictionary<GameObject, int> blocks = new Dictionary<GameObject, int>();

    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private GameObject cursor;
    [SerializeField] private GameObject removing;

    private static GameObject _block;

    void Start()
    {
        
    }

    public GameObject GetObject(Vector3 position)
    {
        foreach (GameObject block in blocks.Keys) {
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
        blocks.Add(_block, type);
    }

    public bool ExistObject(Vector3 position)
    {
        foreach (GameObject block in blocks.Keys) {
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
