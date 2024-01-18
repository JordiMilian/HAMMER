using System.Collections;
using UnityEngine;

public class GreenBoss_ProjectileThrower : MonoBehaviour
{
    [SerializeField] GameObject ProjectilePrefab;
    [SerializeField] GameObject DestinationPrefab;
    public Transform Destination;
    public Transform Origin;
    [SerializeField] AnimationClip ProjectileClip;
    float AnimationLenght;
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            GreenBoss_ThrowProjectile(Destination.position);
        }
    }
    public void GreenBoss_ThrowProjectile(Vector3 DestinationPosition)
    {
        AnimationLenght = ProjectileClip.length;

        GameObject Instantiated_DestinationUI = Instantiate(DestinationPrefab, DestinationPosition, Quaternion.identity);
        GameObject InstantiatedProjectile =  Instantiate(ProjectilePrefab, Origin.position, Quaternion.identity);

        StartCoroutine(MoveProjectileThroghTime(InstantiatedProjectile, Instantiated_DestinationUI, Origin.position, DestinationPosition,  AnimationLenght));

    }
    IEnumerator MoveProjectileThroghTime(GameObject ProjectileGO, GameObject DestinationGO, Vector3 initialPosition, Vector3 destinationPosition,  float timeToDestination)
    {
        Vector3 projectileDirection = destinationPosition - initialPosition;
        float timer = 0;
        float adder = 0;
        while (timer < timeToDestination)
        {
            timer += Time.deltaTime;
            adder = projectileDirection.magnitude * (timer / timeToDestination);
            Vector3 CurrentProjectilePosition = initialPosition + (projectileDirection.normalized * adder);

            ProjectileGO.transform.position = CurrentProjectilePosition;
            yield return null;
        }

        OnLanded(ProjectileGO, DestinationGO);

    }
    void OnLanded(GameObject ProjectileGO, GameObject DestinationGO)
    {
        Animator DestinatioAnimator = DestinationGO.GetComponent<Animator>();
        DestinatioAnimator.SetTrigger("Landed");
        Destroy(ProjectileGO);
    }

}
