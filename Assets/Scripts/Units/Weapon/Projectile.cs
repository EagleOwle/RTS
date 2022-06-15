using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayer = 0;
    [SerializeField] private LayerMask environmentLayer = 0;
    [SerializeField] private int damage = 10;
    [SerializeField] private float lifetime = 10f;
    private new Rigidbody rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    public void Launch(Vector3 velocity)
    {
        transform.parent = null;
        Invoke(nameof(SelfDestroy), lifetime);
        rigidbody.isKinematic = false;
        rigidbody.velocity = velocity;
    }

    private void Update()
    {
        if (rigidbody.isKinematic == false)
        {
            transform.LookAt(transform.position + rigidbody.velocity);
        }
    }

    private void SelfDestroy()
    {
        Destroy(gameObject);
    }

    private void HitToEnvironment(GameObject target)
    {
        SelfDestroy();
    }

    private void HitToTarget(GameObject target)
    {
        if (target.TryGetComponent(out IDamageTaker damageListener))
        {
             damageListener.TakeDamage(damage);
        }

        SelfDestroy();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((1 << collision.gameObject.layer & targetLayer) != 0)
        {

            HitToTarget(collision.gameObject);
        }
        else
        {
            if ((1 << collision.gameObject.layer & environmentLayer) != 0)
            {
                HitToEnvironment(collision.gameObject);
            }
        }
    }



}
