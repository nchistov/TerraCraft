// Класс контролирующий мир.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    private Dictionary<GameObject, int> blocks = new Dictionary<GameObject, int>();

    // Список для хронения данных о кубиках.
    private static Dictionary<int, Block> BLOCK_DATA = new Dictionary<int, Block>();

    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private GameObject cursor;
    [SerializeField] private GameObject removing;  // GameObject обозначающий текущюю крепкость кубика.

    [SerializeField] private Sprite grass2;

    public TextAsset jsonFile;

    public float cubeSize = 1.15f;

    private static GameObject _block;
    private Block add = new Block();

    public TreeController tc;

    private int count = 0;

    public float currentInstremunt = 0.0f;

    void Start()
    {
        Blocks blocksInJson = JsonUtility.FromJson<Blocks>(jsonFile.text);
 
        foreach (Block block in blocksInJson.blocks)
        {
            // add.Add(block.worst); add.Add(block.best); add.Add(block.instrument);
            add.id = block.id; add.ru_name = block.ru_name; add.en_name = block.en_name;
            add.worst = block.worst; add.best = block.best; add.instrument = block.instrument;
            BLOCK_DATA.Add(block.id, add);
        }
    }

    public void AddTree(Vector3 position, int type)
    {
        tc = GetComponent<TreeController>();
        tc.AddTree(position, type);
    }

    public void AddToBlocks(GameObject block, int type)
    {
        blocks.Add(block, type);
    }

    public GameObject InstantiateObject(Vector3 position, int type)
    {
        _block = Instantiate(prefabs[type]) as GameObject;
        _block.transform.position = position;

        return _block;
    }

    public GameObject GetObject(Vector3 position)
    {
        foreach (GameObject block in blocks.Keys) {
            if (block != null) {
                if (position.y > block.transform.position.y - 0.28f && position.y < block.transform.position.y + 0.28f) {
                    if (position.x > block.transform.position.x - 0.28f && position.x < block.transform.position.x + 0.28f) {
                        return block;
                    }
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
            if (!(blocks[block] == 5)) {
                float time;
                Vector3 pos1 = position;
                removing.SetActive(true);
                removing.transform.position = pos1;
                Block data = BLOCK_DATA[blocks[block]];
                if (data.instrument == currentInstremunt) {
                    time = data.best;
                } else {
                    time = data.worst;
                }
                for(int i = 0; i < 8; i++) {
                    removing.transform.localScale = new Vector3(removing.transform.localScale.x + 0.1f, removing.transform.localScale.y + 0.1f,
                                                            removing.transform.localScale.z);
                    yield return new WaitForSeconds(time);
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
    }

    public void AddObject(Vector3 position, int type)
    {
        _block = Instantiate(prefabs[type]) as GameObject;
        _block.transform.position = position;

        if (type == 0) {
            if (Random.Range(0, 5) == 0) {
                _block.GetComponent<SpriteRenderer>().sprite = grass2;
            }
        }

        blocks.Add(_block, type);
    }

    public bool ExistObject(Vector3 position)
    {
        foreach (GameObject block in blocks.Keys) {
            if (block != null) {
                if (position.y > block.transform.position.y - 0.28f && position.y < block.transform.position.y + 0.28f) {
                    if (position.x > block.transform.position.x - 0.28f && position.x < block.transform.position.x + 0.28f) {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    void Update()
    {
        if (count >= 180) {
            count = 0;
            FrameUpdate();
        } else {
            count += 1;
        }
    }

    private void FrameUpdate()
    {
        Dictionary<Vector3, int> added = new Dictionary<Vector3, int>();

        foreach (GameObject block in blocks.Keys) {
            if (block != null) {
                if (block.GetComponent<SpriteRenderer>().isVisible) {
                    if (blocks[block] == 0 && ExistObject(new Vector3(block.transform.position.x, block.transform.position.y + cubeSize, 0))) {
                            Vector3 pos = new Vector3(block.transform.position.x, block.transform.position.y, 0);
                            added.Add(pos, 1);
                            Destroy(block);
                    }
                }
            }
        }

        foreach (Vector3 pos in added.Keys) {
            AddObject(pos, added[pos]);
        }

        added.Clear();
    }
}
