using UnityEngine;

public class JW_HeroShip : MonoBehaviour
{
    private GameObject forwardFlameRightGameObject;
    private GameObject forwardFlameLeftGameObject;
    private GameObject leftWingFlameGameObject;
    private GameObject rightWingFlameGameObject;
    private GameObject BigFlameLeftGameObject;
    private GameObject backwardFlameRightGameObject;
    private GameObject backwardFlameLeftGameObject;

    [SerializeField]
    private float rotationSpeed = 100f;

    [SerializeField]
    private float maxRotationSpeed = 200f;

    [SerializeField]
    private float movementSpeed = 5f;

    [SerializeField]
    private float maxSpeed = 10f;

    [SerializeField]
    private float deceleration = 2f;

    [SerializeField]
    private float rotationDeceleration = 2f;

    private Rigidbody2D rb;
    private JW_HeroShipCollisions shipCollisions;
    public AudioSource boosterJetSound;
    private bool isMoving = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        shipCollisions = GetComponent<JW_HeroShipCollisions>();    
        forwardFlameRightGameObject = transform.Find("FlameSmallRightBooster").gameObject;
        forwardFlameLeftGameObject = transform.Find("FlameSmallLeftBooster").gameObject;
        leftWingFlameGameObject = transform.Find("FlameSmallLeftWing").gameObject;
        rightWingFlameGameObject = transform.Find("FlameSmallRightWing").gameObject;
        BigFlameLeftGameObject = transform.Find("flame-big").gameObject;
        backwardFlameRightGameObject = transform.Find("RearRightBooster").gameObject;
        backwardFlameLeftGameObject = transform.Find("RearLeftBooster").gameObject;
    }

    private void Update()
    {
        float rotationInput = Input.GetAxis("Horizontal");
        float rotationAmount = -rotationInput * rotationSpeed * Time.deltaTime;

        float clampedRotationAmount = Mathf.Clamp(rotationAmount, -maxRotationSpeed * Time.deltaTime, maxRotationSpeed * Time.deltaTime);
        transform.Rotate(Vector3.forward * clampedRotationAmount);

        float moveInput = Input.GetAxis("Vertical");
        rb.AddForce(transform.up * moveInput * movementSpeed);

        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }

        if (moveInput == 0 && rb.velocity.magnitude > 0)
        {
            rb.AddForce(-rb.velocity.normalized * deceleration, ForceMode2D.Force);
        }

        if (rotationInput == 0 && rb.angularVelocity != 0)
        {
            rb.angularVelocity = Mathf.Lerp(rb.angularVelocity, 0f, rotationDeceleration * Time.deltaTime);
        }

        if (moveInput > 0)
        {
            forwardFlameRightGameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            forwardFlameRightGameObject.GetComponent<SpriteRenderer>().enabled = false;
        }

        if (moveInput > 0)
        {
            forwardFlameLeftGameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            forwardFlameLeftGameObject.GetComponent<SpriteRenderer>().enabled = false;
        }

        if (moveInput > 0)
        {
            BigFlameLeftGameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            BigFlameLeftGameObject.GetComponent<SpriteRenderer>().enabled = false;
        }

        leftWingFlameGameObject.SetActive(rotationInput > 0);

        rightWingFlameGameObject.SetActive(rotationInput < 0);

        backwardFlameRightGameObject.SetActive(moveInput < 0);

        backwardFlameLeftGameObject.SetActive(moveInput < 0);

        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            if (!isMoving)
            {
                boosterJetSound.Play();
                isMoving = true;
            }
        }
        else
        {
            if (isMoving)
            {
                boosterJetSound.Stop();
                isMoving = false;
            }
        }
    }
}