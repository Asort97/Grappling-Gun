using System.Collections;
using UnityEngine;

public class Box : MonoBehaviour
{   
    [SerializeField] private float lifeTime;
    [SerializeField] private GameObject destroyPart;

    private void OnEnable()
    {
        StartCoroutine(Deactived(lifeTime));
    }

    public void CreateParticles()
    {
        Instantiate(destroyPart, transform.position, Quaternion.identity, null);
    }
    private IEnumerator Deactived(float lifetime)
    {
        yield return new WaitForSeconds(lifetime);

        gameObject.SetActive(false);
    }
}
