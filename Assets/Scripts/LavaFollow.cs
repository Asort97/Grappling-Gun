using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaFollow : MonoBehaviour
{
    private Transform _player;
    private void Start()
    {
        _player = PlayerController.singleton.transform;
    }

    private void Update()
    {
        Vector2 pos = new Vector2(_player.position.x, transform.position.y);
        transform.position = pos;
    }
}
