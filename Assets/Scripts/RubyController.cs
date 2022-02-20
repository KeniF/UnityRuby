using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RubyController : MonoBehaviour
{
    public int maxHealth;
    public int maxCogs;
    public int currentCogs = 0;
    public float speed = 3.0f;
    public int health { get { return currentHealth; } }
    public float timeInvincible = 1.0f;
    public GameObject projectilePrefab;
    public int currentHealth;
    private AudioSource audioSource;
    private AudioSource walkAudioSource;
    public AudioClip throwClip;
    public AudioClip footstepsClip;
    public AudioClip attackClip;
    bool isInvincible;
    float invincibleTimer;
    Vector2 lookDirection = new Vector2(1, 0);
    Rigidbody2D rigidbody2d;
    Animator animator;
    float horizontal;
    float vertical;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.priority = 0;
        walkAudioSource = gameObject.AddComponent<AudioSource>();
        walkAudioSource.clip = footstepsClip;
        walkAudioSource.priority = 100;
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        Vector2 move = new Vector2(horizontal, vertical);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
            if (!walkAudioSource.isPlaying)
            {
                walkAudioSource.Play(0);
            }
        }
        else
        {
            walkAudioSource.Pause();
        }

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            LaunchProjectile();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                NPC character = hit.collider.GetComponent<NPC>();
                if (character != null)
                {
                    character.DisplayDialog();
                }
            }
        }
    }

    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;
        rigidbody2d.MovePosition(position);
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
                return;

            animator.SetTrigger("Hit");
            isInvincible = true;
            invincibleTimer = timeInvincible;
            PlaySound(attackClip);
        }
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
        if (currentHealth <= 0) {
            KillRuby();
        }
    }

    public void GetCogs(int amount)
    {
        currentCogs = Mathf.Clamp(currentCogs + amount, 0, maxCogs);
        UICogsCount.instance.SetCogs(currentCogs);
    }

    public void KillRuby() {
        gameObject.SetActive(false);
        SceneManager.LoadScene("GameOverScene");
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    void LaunchProjectile()
    {
        if (currentCogs > 0)
        {
            GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
            Projectile projectile = projectileObject.GetComponent<Projectile>();
            projectile.Launch(lookDirection, 300);

            animator.SetTrigger("Launch");
            currentCogs--;
            PlaySound(throwClip);
            UICogsCount.instance.SetCogs(currentCogs);
        }
    }

}
