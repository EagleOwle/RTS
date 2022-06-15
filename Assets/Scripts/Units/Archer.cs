using UnityEngine;
using System.Collections;

public class Archer : Unit
{
    [SerializeField] private Projectile arrowPrefab;
    [SerializeField] private int range = 5;
    [SerializeField] private int cooldown = 5;
    [SerializeField] private float projectileFlightDuration = 1f;

    public override void SetTarget(Squad target)
    {
        enemyTarget = target;

        if(enemyTarget != null && enemyTarget.SquadState != UnitState.Dead)
        {
            StartCoroutine(Reload(cooldown));
        }
    }

    private IEnumerator Reload(float time)
    {
        yield return new WaitForSeconds(time);
 
        if (enemyTarget != null && enemyTarget.SquadState != UnitState.Dead)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if((enemyTarget.GetWorldPosition() - transform.position).magnitude <= range)
        {
            LookRotation(enemyTarget.GetWorldPosition());
            Debug.DrawLine(transform.position, enemyTarget.GetWorldPosition(), Color.red, 0.1f);
            PullTheArrow();
        }
    }

    private void LookRotation(Vector3 targetPosition)
    {
        Vector3 relativePos = targetPosition - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos);
        transform.rotation = rotation;
    }

    private void PullTheArrow()
    {
        Projectile projectile = Instantiate(arrowPrefab);
        projectile.transform.SetParent(transform);
        projectile.transform.localPosition = Vector3.one;
        projectile.transform.localEulerAngles = Vector3.zero;
        projectile.transform.localScale = Vector3.one;

        Vector3 rnd = new Vector3(UnityEngine.Random.Range(-2, 2), 0, UnityEngine.Random.Range(-2, 2));
        Vector3 targetPosition = enemyTarget.GetWorldPosition() + rnd;
        Vector3 startPosition = projectile.transform.position;
        Vector3 velocity = Ballistik.CalculateBestThrowSpeed(startPosition,targetPosition,projectileFlightDuration);

        projectile.Launch(velocity);
    }
}
