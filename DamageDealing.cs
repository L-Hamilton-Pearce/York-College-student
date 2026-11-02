using UnityEngine;

public class DamageDealing : MonoBehaviour
{
    [SerializeField] private int damageAmount;
    [SerializeField] private int fireCooldown;
    [SerializeField] private int maxAmo;
    [SerializeField] private int reloadSpeed = 6;
    [SerializeField] GameObject playerRef;
    private float maxSpeed;

    private int amo;


    // states 
    private enum fireState { fired, unfired, relouding }

    // set the state so can fire 
    private fireState currentState = fireState.unfired;

    // define the line renderer 
    private LineRenderer lr;

    PlayerMover PlayerMover;
    private void Awake()
    {
        // hte line renderer 
        lr = GetComponent<LineRenderer>();
        PlayerMover = playerRef.GetComponent<PlayerMover>();
        amo = maxAmo;
        maxSpeed = PlayerMover.groundSpeed;


    }
    private void Update()
    {


        // all the ray stuff
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))


        {
            if (Input.GetKey(KeyCode.Mouse0) && amo > 0)
            {
                if (currentState == fireState.unfired)
                {
                    // sets the stated so can't fire again 
                    currentState = fireState.fired;

                    // draws the line from the point of fire to the point of hit on the ray 
                    lr.positionCount = 2;
                    lr.SetPosition(0, transform.position);
                    lr.SetPosition(1, hit.point);
                    Invoke(nameof(resetLine), 0.1f);


                    if (transform.root == hit.transform.root)
                    {
                        return;
                    }

                    // checks the object that the ray hit for the health component 
                    if (hit.collider.gameObject.TryGetComponent<Health>(out Health targetHealth))
                    {
                        targetHealth.TakeDamage(damageAmount);
                    }
                    // runs a method that that changes the state so can fire again 
                    Invoke(nameof(ResetFire), fireCooldown);
                }
            }


        }
        if (Input.GetKey(KeyCode.R) && currentState != fireState.relouding)
        {
            currentState = fireState.relouding;
            PlayerMover.groundSpeed /= maxSpeed;
            Invoke(nameof(RefilAmo), reloadSpeed);
        }
    }
    void resetLine()
    {
        // removes the line 
        lr.positionCount = 0;
    }
    void ResetFire()
    {
        // changs state 
        amo -= 1;
        currentState = fireState.unfired;
    }
    void RefilAmo()
    {
        PlayerMover.groundSpeed = maxSpeed;
        amo = maxAmo;
        currentState = fireState.unfired;
    }
}

