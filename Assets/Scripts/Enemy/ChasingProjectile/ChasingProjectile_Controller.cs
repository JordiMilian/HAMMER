using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingProjectile_Controller : MonoBehaviour
{
    public Transform TargetTf;
    [SerializeField] float defaultVelocityPerSecond;
    [SerializeField] float rotationRadiantSpeed;
    [SerializeField] float accelerationPerSecond;
    [SerializeField] float rotationDecelerationPerSecond;
    Transform ownTransform;
    [Header("Testing")]
    [SerializeField] float startingBoostVelocity;
    [SerializeField] float startingBoostDuration;
    [SerializeField] Transform startingBoostDirectionTf;
    [SerializeField] bool TestingStartingBoost;

    Coroutine currentMovement;
    private void Awake()
    {
        ownTransform = transform;
    }
    private void Update()
    {
        if(TestingStartingBoost)
        {
            if (currentMovement != null) { StopCoroutine(currentMovement); }

            Vector2 startingDirection = (startingBoostDirectionTf.position - ownTransform.position).normalized;
            StartingBoost(startingBoostVelocity, startingBoostDuration, startingDirection);
            TestingStartingBoost = false;
        }
    }
    public void StartingBoost(float startingVelocityPerSecond, float duration, Vector2 direction)
    {
        currentMovement = StartCoroutine(startingBoostCoroutine(startingVelocityPerSecond, duration, direction));
    }
    IEnumerator startingBoostCoroutine(float startingVelocityPerSecond, float duration, Vector2 direction)
    {
        float timer = 0;
        ownTransform.up = direction;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float normalizedTime = timer / duration;
            float equivalentVelocity = Mathf.Lerp(startingVelocityPerSecond, defaultVelocityPerSecond, normalizedTime);
            Vector2 movingSpeed = ownTransform.up * equivalentVelocity * Time.deltaTime;
            ownTransform.position += (Vector3)movingSpeed;

            rotateTowardTarget();

            yield return null;
        }
        currentMovement = StartCoroutine(defaultMovement());
    }
    void rotateTowardTarget()
    {
        ownTransform.up = Vector3.RotateTowards(ownTransform.up, TargetTf.position - ownTransform.position, rotationRadiantSpeed, 10);
    }
    IEnumerator defaultMovement()
    {
        while (true)
        {
            defaultVelocityPerSecond += accelerationPerSecond * Time.deltaTime;
            rotationRadiantSpeed -= rotationDecelerationPerSecond * Time.deltaTime;
            ownTransform.position += ownTransform.up * defaultVelocityPerSecond * Time.deltaTime;

            rotateTowardTarget();

            yield return null;
        }
    }
}
