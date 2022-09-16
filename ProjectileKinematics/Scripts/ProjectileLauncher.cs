using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is made by Ahmed Elshazly
//the script illustrates how you can use some simple kinematic equations to simulate a projectile reaching a specific target
//there is a lot of improvements that can be done so feel free to improve upon it or use it
//as it is
//Have fun :D

public enum AirResistanceState
{
    Active,
    Ignore,
    Negate
}

public class ProjectileLauncher : MonoBehaviour
{
    //Setting up values
    [Header("Values")]
    [Range(0f, 30f)]
    public float maxHeight = 5f;
    [Range(0f,-50f)]
    public float gravitationalAcceleration = -9.8f;
    [Range(0f,10f)]
    public float simulationSpeed = 1f;
    [Range(-30f,30f)]
    public float airResistanceVelocity = 0f;

    public AirResistanceState airResistance = AirResistanceState.Active;

    public Vector2 targetOffset;


    

    bool Launch = false;
    bool startTimer = false;

    public Transform Projectile;
    public Transform Target;


    [HideInInspector] public Rigidbody2D projectileRigidBody;
    [HideInInspector] public float initialVerticalVelocity;
    [HideInInspector] public float initialHorizontalVelocity;
    [HideInInspector] public float elapsedTime;
    [HideInInspector] public float currentMaxHeight;
    [HideInInspector] public Vector2 initialPosition; 
    [HideInInspector] public float timeinst;


    void Start()
    {
        //Checking if the projectile has a rigidbody
        if (Projectile.GetComponent<Rigidbody2D>())
            projectileRigidBody = Projectile.GetComponent<Rigidbody2D>();

        //If not then add a rigidbody to it
        else
            projectileRigidBody = Projectile.gameObject.AddComponent<Rigidbody2D>();

        //Setting the global gravity to zero so the projctile wont fall down immediately after the scene plays
        Physics2D.gravity = Vector2.zero;
    }

   
    private void Update()
    {
        Time.timeScale = simulationSpeed;

        //Checking if the player presses space button 
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //If the player did then we will set the launch boolean to true and start the timer
            Launch = true;
            startTimer = true;
        }

        if (startTimer && timeinst > 0f) timeinst -= Time.deltaTime;

        CalculateProjectile(initialPosition);
    }

    void FixedUpdate()
    {
        //Checking if the Launch boolean is set to true
        if (Launch)
        {
            //if so then launch the ball
            LaunchBall();
            Launch = false;
        }
        //if the timer started and the air resistance is not negated then we add air resistance force to the projectile
        if(startTimer && airResistance != AirResistanceState.Negate) projectileRigidBody.AddForce(Vector2.right * airResistanceVelocity);
    }

    void LaunchBall()
    {
        //Setting the global gravity variable to our gravitational accleration float
        Physics2D.gravity = gravitationalAcceleration * Vector2.up;

        //Adding our initial velocity to the projectile velocity
        projectileRigidBody.velocity = new Vector2(initialHorizontalVelocity, initialVerticalVelocity);
    }

    public void CalculateProjectile(Vector2 Position)
    {
        //We will have to split our time when the projectile is rising and when it's falling because the projectile
        //Wont be launched with a symmetrical curve because the target might not be at the same vertical position as the projectile
        //First of all lets start by finding the time the projectile takes while its rising we know that the final vertical velocity
        //is 0 and we have the max height distance and luckily we also have our gravitational acceleration value
        //So we can use this formula D = vf*t - 1/2*g*t^2
        //D is our max height which we already have and vf is our final vertical velocity which is 0
        //And g is our gravitational acceleration if we use some basic algebra and isolated t it will be
        //t = sqrt(2D/-g) Boom we found our time up
        float timeUp = Mathf.Sqrt((2f * (currentMaxHeight)) / -gravitationalAcceleration);

        //Now we will find the time the projectile takes while it's falling
        //Again we will use this formula D = vf*t - 1/2*g*t^2 but this time our distance is going to be the
        //Difference between the maxheight and the projectile y position
        //Using algebra we will get this formula t = sqrt((2(D-y)/-g)
        //Now we have the time down
        float timeDown = Mathf.Sqrt((2f * (currentMaxHeight - Position.y)) / -gravitationalAcceleration);

        //if we add the time up and the time down we will get the flight time of the projectile until it reaches the target
        elapsedTime = timeUp + timeDown;

        //To find our initial vertical velocity we will use this formula D = vi*t + 1/2*g*t^2
        //vi is the initial velocity and D is the distance between the projectile and the target
        //this time we dont have the initial vertical velocity but we have our flight time so we will use it
        //if we again use some algebra and isolate vi it will be vi = (2D - gt^2)/2t
        //now we have our Initial Vertical Velocity
        initialVerticalVelocity = (2f * (Target.position.y + targetOffset.y - Position.y) + -gravitationalAcceleration * elapsedTime * elapsedTime) / (2f * elapsedTime);

        if (airResistance == AirResistanceState.Ignore || airResistance == AirResistanceState.Negate)
            //if the air resistance is negated or ignored by the calculations then we will use this formula
            //to find our initial x velocity vi = d/t
            //d is the horizontal distance between the projectile and the target
            initialHorizontalVelocity = (Target.position.x + targetOffset.x - Position.x) / elapsedTime;
        else if (airResistance == AirResistanceState.Active)
            //if the air resistance has to be calculated then we will use this formula to find our horizontal initial velocity
            //D = vi*t + 1/2*a*t^2
            //D is the distance
            //a is the acceleration due to air resistance which we already have
            //now if we use some algebra one last time to find vi
            //it will be vi = (2D-at^2)/2t
            initialHorizontalVelocity = (2f * (Target.position.x + targetOffset.x - Position.x) - airResistanceVelocity * elapsedTime * elapsedTime) / (2f * elapsedTime); 

        if (!startTimer)timeinst = elapsedTime;
    }


}
