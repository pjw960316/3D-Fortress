using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Ball : MonoBehaviour
{

    public float explosionRadius = 5f;

    Rigidbody rigidbody;

    public HashSet<string> damaged;
    
    public Quaternion prevRotation;
    public float angle;
    public float maxDamage = 50f;
    public float explosionForce = 100f;

    private Vector3 prevPosition;
    public LayerMask isPlayer;
    // Start is called before the first frame update

    public Vector3 startRotation;
    private Vector3 wind;

    void Start()
    {
        angle = transform.rotation.eulerAngles.x;
        damaged = new HashSet<string>();
        prevRotation = transform.rotation;
        rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate(){

        // 미사일의 속도에 따른 미사일 회전 상태 구현
        float angle1 = Mathf.Atan2(rigidbody.velocity.y, rigidbody.velocity.z) * Mathf.Rad2Deg; 
        float angle2 = Mathf.Atan2(rigidbody.velocity.y, rigidbody.velocity.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(-angle1, 0, -angle2);

        // 미사일에 가해지는 바람 구현
        // addforce는 연속적으로 힘이 가해지므로 가속이 붙음 , 그러므로 position으로 이용합니다.
        transform.position += WindManager.wind_path;        
        Debug.Log("미사일 위치 : " + transform.position);
    }
   
    private void OnTriggerEnter(Collider other){ 
        Destroy(gameObject);
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, isPlayer);

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
                // playermovement의 addExplosionForce 함수에서 물체를 얼마만큼 이동시킬지 계산하기 위해 target에 할당해줌
                
                PlayerHealth playerHealth = colliders[i].GetComponent<PlayerHealth>();

                float damage = ((explosionRadius - distance) / explosionRadius) * maxDamage;

                damage = Mathf.Max(damage, 0);

                playerHealth.ApplyDamage((int)damage);
            }else{
                continue;
            }
            
        }

        

    }
}
