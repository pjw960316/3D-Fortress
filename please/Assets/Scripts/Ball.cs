using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Ball : MonoBehaviour
{

    public float explosionRadius = 5f;

    public float maxDamage = 50f;
    public float explosionForce = 100f;
    public LayerMask isPlayer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

   
    private void OnTriggerEnter(Collider other){
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, isPlayer);

        for(int i=0; i< colliders.Length; i++){
            Rigidbody target = colliders[i].GetComponent<Rigidbody>();

            target.AddExplosionForce(explosionForce, transform.position, explosionRadius);

            PlayerHealth playerHealth = colliders[i].GetComponent<PlayerHealth>();

            Vector3 vectordiff = target.position - transform.position;

            float distance = vectordiff.magnitude;

            float damage = (explosionRadius - distance) / explosionRadius * maxDamage;

            damage = Mathf.Max(damage, 0);

            playerHealth.ApplyDamage((int)damage);

            
        }

        Destroy(gameObject);
    }
}
