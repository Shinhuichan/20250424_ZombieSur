using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : LivingEntity
{
    /*
        삶과 죽음을 택하는 Component
    죽었을 때 -> 움직이면 안 되고, 총도 쏘면 안 됨.
    */
    public Slider healthSlider;

    public AudioClip deathClip;
    public AudioClip hitClip;

    private AudioSource aud;
    private Animator animator;

    private PlayerMovement playerMovement;
    private PlayerShooter playerShooter;

    private void Awake()
    {
        if (!TryGetComponent(out aud)) 
            Debug.Log($"PlayerHealth ] AudioClip 없음");
        if (!TryGetComponent(out animator)) 
            Debug.Log($"PlayerHealth ] Animator 없음");
        if (!TryGetComponent(out playerMovement)) 
            Debug.Log($"PlayerHealth ] PlayerMovement 없음");
        if (!TryGetComponent(out playerShooter)) 
            Debug.Log($"PlayerHealth ] PlayerShooter 없음");
    }
    protected override void OnEnable()
    {
        base.OnEnable(); // base : 부모의 Class의 메서드를 호출
        // UI Update
        healthSlider.gameObject.SetActive(true); // UI 활성화
        healthSlider.maxValue = maxHealth; // 최대 Health 설정(부모의 데이터를 가져옴)
        healthSlider.value = health; // 현재 Health 설정(부모의 데이터를 가져옴)
        // 죽었을 때, Move, Shot 등을 비활성화할 예정
        // 확인차 활성화시키기
        playerMovement.enabled = true;
        playerShooter.enabled = true;
    }

    public override void OnDamage(float damage, Vector3 hitPosition, Vector3 hitNormal)
    {
        if (isLive)
            aud.PlayOneShot(hitClip);
        base.OnDamage(damage, hitPosition, hitNormal);

        // UI Update
        healthSlider.value = health;
    }

    public override void Die()
    {
        base.Die();

        // UI Update
        healthSlider.gameObject.SetActive(false);
        
        // 시각적, 청각적 효과
        animator.SetTrigger("Die");
        aud.PlayOneShot(deathClip);
        
        // 죽었을 때, COmponent 비활성화
        playerMovement.enabled = false;
        playerShooter.enabled = false;
    }
    public void Healing(float gainHealth)
    {
        if (maxHealth < health + gainHealth)
            health = maxHealth;
        else
            health += gainHealth;
    }
}