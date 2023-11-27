using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDead_Controller : MonoBehaviour
{
    [SerializeField] AnimationCurve ForceCurve;
    Rigidbody2D rigidbody;
    [SerializeField] Transform PlayerPosition;

    public float PushTime;
    public float PushStrengh;

    [SerializeField] GameObject bloodExplosion;
   

    void Start()
    {
       
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P)) OnPushBody();
    }
    private void Awake()
    {
       
        if (Random.value > 0.5f) { transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y); } 
        var BloodExplosion = Instantiate(bloodExplosion, transform.position, Quaternion.Euler(0, 0, 0));
        rigidbody = GetComponent<Rigidbody2D>();
        PlayerPosition = GameObject.Find("MainCharacter").transform;
        OnPushBody();
    }
    public void OnPushBody()
    {
        var BloodExplosion = Instantiate(bloodExplosion, transform.position, Quaternion.Euler(0, 0, 0));
        StartCoroutine(ForceOnDeath());
    }
    IEnumerator ForceOnDeath()
    {
        float time = 0;
        float weight = 0;

        Vector2 direction = (transform.position - PlayerPosition.position).normalized;
        while (time < PushTime)
        {
            time = time + Time.deltaTime;
            weight = ForceCurve.Evaluate(time / PushTime);
           

            
            rigidbody.AddForce(direction * weight * PushStrengh *Time.deltaTime);

            
            yield return null;
        }
    }
}
