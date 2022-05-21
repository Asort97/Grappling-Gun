using System.Collections;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{

    [SerializeField] private float delaySpawn;
    [SerializeField] private int poolCount;
    [SerializeField] private bool autoExpand = false;
    [SerializeField] private Box boxPrefab;

    private PoolMono<Box> pool;

    private Transform _player;


    private void Start()
    {
        _player = PlayerController.singleton.transform;

        this.pool = new PoolMono<Box>(this.boxPrefab, this.poolCount, this.transform);
        StartCoroutine(spawnBox());
    }
    IEnumerator spawnBox()
    {
        while (true)
        {
            GenerateBox();
            yield return new WaitForSeconds(delaySpawn);
        }

    }
    private void GenerateBox()
    {
        float randX = _player.position.x + 20 + Random.Range(-30, 30);
        float randY = _player.position.y + Random.Range(-30, 30);

        Vector2  newPos = new Vector2(randX, randY);

        var box = pool.GetFreeElement();
        box.transform.position = newPos;

    }
}
