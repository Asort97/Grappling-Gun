using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Camera Camera;
    [SerializeField] private float smoothFollow;
    private Transform playerTransform;
    private Rigidbody2D rbPlayer;

    private void Start()
    {
        playerTransform = PlayerController.singleton.transform;
        rbPlayer = PlayerController.singleton.rb;
    }

    private void Update()
    {
        if(playerTransform)
        {
            Vector3 pos = new Vector3(playerTransform.position.x, playerTransform.position.y, transform.position.z);

            transform.position = Vector3.Lerp(transform.position, pos, smoothFollow);
            float multiplier = Camera.fieldOfView * (Mathf.Abs(rbPlayer.velocity.x) / 10);

            Camera.fieldOfView = Mathf.Clamp(Mathf.Lerp(Camera.fieldOfView, multiplier, 0.2f * Time.deltaTime), 60, 80);
        }
    
    }
}
