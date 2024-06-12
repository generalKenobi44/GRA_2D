using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    AudioSource pickUpSource;
    public int healthRestore = 50;

    private void Awake()
    {
        pickUpSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageable = collision.GetComponent<Damageable>();
        if(damageable != null)
        {
            bool wasHealed = damageable.Heal(healthRestore);
            if (wasHealed) 
            {
                if (pickUpSource) { AudioSource.PlayClipAtPoint(pickUpSource.clip, gameObject.transform.position, pickUpSource.volume); }
                Destroy(gameObject); 
            }
        }
    }

}
