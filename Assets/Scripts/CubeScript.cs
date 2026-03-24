using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;
using DG.Tweening;

public class CubeScript : MonoBehaviour
{
    [Header("Hp")]
    public int hp = 5;
    public int maxHp = 5;
    public TMP_Text hpText;
    [Header("Speed")]
    public float forwardSpeed = 10f;
    public float acceleration = 0.1f;
    public float sideSpeed = 12f;
    public float jumpForce = 8f;
    public float gravity = -20f;

    private float[] lanes = new float[] { -2f, 0f, 2f };
    [Header("Score")]
    public int score = 0;
    public float scoreInterval = 0.1f;
    public int scorePerInterval = 1;
    private int record;
    public TMP_Text scoreText;
    public TMP_Text recordText;
    [Header("Damage Effect")]
    public Renderer playerRenderer;
    public Color damageColor = new Color(1f, 0f, 0f, 0.5f);
    public Color healColor = new Color(0f, 1f, 0f, 0.5f);
    public float flashDuration = 1f;
    public float beginFlash = 0.2f;

    private Color originalColor;
    private Coroutine damageFlashCoroutine;
    private int currentLaneIndex = 1;
    private float targetLaneX;
    private float originalForwardSpeed;
    private float speedBoostMultiplier = 1f;
    private float speedBoostEndTime = 0f;
    private CharacterController controller;
    private float verticalVelocity;
    public bool isDead = false;
    private float nextScoreTime;
    private float maxAnimationSpeed = 50f;
    public Animator animator;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        targetLaneX = lanes[currentLaneIndex];

        Vector3 pos = transform.position;
        transform.position = pos;

        originalForwardSpeed = forwardSpeed;

        nextScoreTime = Time.time + scoreInterval;
        record = PlayerPrefs.GetInt("Record", 0);
        animator = GetComponent<Animator>();
        originalColor = playerRenderer.material.color;
        UpdateUI();
    }

    void Update()
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
            animator.SetTrigger("Jump");
        } else if (controller.isGrounded)
        {
            animator.SetTrigger("Grounded");
        }




        if (!controller.isGrounded)
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        float newX = Mathf.MoveTowards(transform.position.x, targetLaneX, sideSpeed * Time.deltaTime);
        float deltaX = newX - transform.position.x;

        forwardSpeed += acceleration * Time.deltaTime;

        float currentForwardSpeed = forwardSpeed * speedBoostMultiplier;
        animator.SetFloat("Speed", currentForwardSpeed);
        Vector3 move = new Vector3(deltaX, verticalVelocity * Time.deltaTime, currentForwardSpeed * Time.deltaTime);
        controller.Move(move);
        float normalizedSpeed = Mathf.Min(currentForwardSpeed / maxAnimationSpeed, 1);
        animator.speed = Mathf.Lerp(1f, 3f, normalizedSpeed);

        if (speedBoostMultiplier > 1f && Time.time >= speedBoostEndTime)
        {
            BoostFinished();
        }

        if (Time.time >= nextScoreTime)
        {
            AddScore(scorePerInterval);
            nextScoreTime = Time.time + scoreInterval;
        }
    }

    public void ApplyBonus(BonusData bonus)
    {
        switch (bonus.bonusType)
        {
            case BonusData.BonusType.Health:
                hp += bonus.healthAmount;
                hp = Mathf.Min(hp, maxHp);
                FlashLight(healColor);
                break;

            case BonusData.BonusType.SpeedBoost:
                speedBoostMultiplier = bonus.speedMultiplier;
                speedBoostEndTime = Time.time + bonus.duration;
                animator.SetTrigger("SpeedBonus");
                break;
        }
    }

    void BoostFinished()
    {
        speedBoostMultiplier = 1f;
        animator.SetTrigger("BoostEnded");
    }

    public void TakeDamage(int amount)
    {
        hp -= amount;
        BoostFinished();
        if (hp <= 0)
        {
            hp = 0;
            PlayerPrefs.SetInt("Record", Mathf.Max(record, score));
            isDead = true;
            enabled = false;
            UpdateUI();
            animator.SetTrigger("Death");
            Invoke(nameof(RestartLevel), 2f);
        }
        FlashLight(damageColor);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void AddScore(int amount)
    {
        score += amount;
        UpdateUI();
    }

    void UpdateUI()
    {
        int tmp_record = Mathf.Max(record, score);
        if (scoreText != null)
            scoreText.text = "Score: " + score;
        if (recordText != null)
            recordText.text = "Record: " + tmp_record;
        if (hpText != null)
            hpText.text = "Hp: " + hp;
    }

    void FlashLight(Color destColor)
    {
        if (playerRenderer != null && playerRenderer.material.HasProperty("_Color"))
        {
            playerRenderer.material.DOColor(destColor, beginFlash).SetDelay(0f);
            playerRenderer.material.DOColor(originalColor, flashDuration).SetDelay(beginFlash);
        }
    }
}