using UnityEngine;
using System;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{
    public static PlayerController singleton {get; private set;}

    public static Action<int> AddPoint;

    [Header("Player Data")]
    [SerializeField] private float distanceGrapple;
    [SerializeField] private float forceImpulse;

    [Space(10)]

    [SerializeField] private LayerMask layerHook;
    [SerializeField] private SpringJoint2D joint;
    [SerializeField] private LineRenderer currentLine;
    [SerializeField] private GameObject windParticles;
    private Transform currentTarget;

    public Rigidbody2D rb;


    private void Awake()
    {
        singleton = this;
    }
    private void Start()
    {
        joint.autoConfigureDistance = false;
        joint.enabled = false;
    }

    private void Update()
    {   

        if(Input.GetMouseButtonDown(0))
        {   
            StartGrapple();

            if(currentTarget)
            {
                rb.velocity = new Vector2(rb.velocity.x * 2, rb.velocity.y);
            }            
        }        

        if(Input.GetMouseButtonUp(0))
        {
            currentTarget = null;
            StopGrapple();
        }

        if(joint.enabled)
        {
            currentLine.SetPosition(1, transform.position);
        }

        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -18f , 20f), rb.velocity.y);


        BallisticPlayer();
        SpeedEffect();
    }


    private void StartGrapple()
    {
        
        Vector3 posClick = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 15f));

        Collider2D [] enemies = Physics2D.OverlapCircleAll(posClick, 3f, layerHook);

        Vector2 dir = transform.position - posClick;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, distanceGrapple, layerHook);
        
        if(enemies[0] != null)
        {
            joint.connectedBody = enemies[0].attachedRigidbody;

            float distance = Vector3.Distance(transform.position, enemies[0].transform.position);
            joint.distance = distance * 0.6f;

            CastLine(transform.position, enemies[0].transform.position);

            currentTarget = enemies[0].transform;
            currentLine.enabled = true;
            joint.enabled = true;

        }
    }

    private void StopGrapple()
    {
        if(joint.enabled)
        {
            currentLine.enabled = false;
            joint.enabled = false;
        }
            
    }

    private void BallisticPlayer()
    {
        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }


    private void SpeedEffect()
    {
        float sizeY = transform.localScale.y / (Mathf.Abs(rb.velocity.x) / 10);
        Vector2 size = new Vector2(transform.localScale.x, Mathf.Clamp(sizeY, 0.5f, 1f));

        transform.localScale = Vector2.Lerp(transform.localScale, size, 4 * Time.deltaTime);
    }

    private void CastLine(Vector3 startPos, Vector3 endPos)
    {
        currentLine.SetPosition(0, endPos);
        currentLine.SetPosition(1, startPos);
    }

    private void Die()
    {
        SceneManager.LoadScene(1);
    }

    private void AddForcePlayer(Vector2 direction)
    {
        rb.AddForce(direction * forceImpulse);        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("BoxTrigger"))
        {

            AddForcePlayer(new Vector2(1, 1));
            CameraShaker.Shake(0.8f, 1, CameraShaker.ShakeMode.XY);
            
            AddPoint?.Invoke(UnityEngine.Random.Range(100, 400));

            other.GetComponent<Box>().CreateParticles();
            other.gameObject.SetActive(false);
        }

        if(other.CompareTag("Lava"))
        {
            Die();
        }
    }
}
