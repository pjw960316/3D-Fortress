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
        Debug.Log("other:" + other.name);
        Destroy(gameObject);
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, isPlayer);


        HashSet<string> damaged = new HashSet<string>();

        for(int i=0; i< colliders.Length; i++){

            if (!damaged.Contains(colliders[i].name)){
                damaged.Add(colliders[i].name);
                PlayerMovement target = colliders[i].GetComponent<PlayerMovement>();

                Vector3 vectordiff = target.transform.position - transform.position;

                float distance = vectordiff.magnitude;
                
                target.hitforce = ((explosionRadius - distance) / explosionRadius) * explosionForce;
                target.hitvector = target.transform.position - transform.position;
                target.hitradius = explosionRadius;
                target.isHit = true;

                
                PlayerHealth playerHealth = colliders[i].GetComponent<PlayerHealth>();

                

                float damage = ((explosionRadius - distance) / explosionRadius) * maxDamage;

                damage = Mathf.Max(damage, 0);

                Debug.Log("Damage:" + damage);
                Debug.Log(colliders[i].name);
                Debug.Log(colliders.Length);
                playerHealth.ApplyDamage((int)damage);
            }else{
                continue;
            }
            
        }

        

    }
}
