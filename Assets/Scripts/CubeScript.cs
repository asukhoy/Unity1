using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CubeScript : MonoBehaviour
{
    public int hp = 5;
    public float forwardSpeed = 10f;
    public float acceleration = 0.1f;
    public float sideSpeed = 12f;
    public float jumpForce = 8f;
    public float gravity = -20f;
    public float[] lanes = new float[] { -2f, 0f, 2f };

    private int currentLaneIndex = 1;
    private float targetLaneX;
    private CharacterController controller;
    private float verticalVelocity;
    private bool isDead = false;

    public void Start()
    {
        controller = GetComponent<CharacterController>();
        targetLaneX = lanes[currentLaneIndex];

        Vector3 pos = transform.position;
        transform.position = pos;
    }

    public void Update()
    {
        if (isDead)
        {
            return;
        }
        if (Keyboard.current.aKey.wasPressedThisFrame || Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            if (currentLaneIndex > 0)
            {
                currentLaneIndex--;
                targetLaneX = lanes[currentLaneIndex];
            }
        }
        if (Keyboard.current.dKey.wasPressedThisFrame || Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            if (currentLaneIndex < lanes.Length - 1)
            {
                currentLaneIndex++;
                targetLaneX = lanes[currentLaneIndex];
            }
        }
        if (Keyboard.current.spaceKey.wasPressedThisFrame && controller.isGrounded)
        {
            verticalVelocity = jumpForce;
        }


        if (!controller.isGrounded)
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        float newX = Mathf.MoveTowards(transform.position.x, targetLaneX, sideSpeed * Time.deltaTime);
        float deltaX = newX - transform.position.x;

        forwardSpeed += acceleration * Time.deltaTime;
        Vector3 move = new Vector3(deltaX, verticalVelocity * Time.deltaTime, forwardSpeed * Time.deltaTime);
        controller.Move(move);

        
    }

    public void TakeDamage(int amount)
    {
        hp -= amount;
        if (hp <= 0)
        {
            isDead = true;
            enabled = false;
            Invoke(nameof(RestartLevel), 2f);
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}