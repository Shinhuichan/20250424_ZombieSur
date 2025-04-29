using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class ZombieController : LivingEntity
{
    [Header("Tracing Target Layer")]
    public LayerMask targetLayer;
    private LivingEntity targetEntity;

    // 경로를 계산할 AI
    private NavMeshAgent agent;

    [Header("Effect")]
    [SerializeField] private AudioClip deathClip;
    [SerializeField] private AudioClip hitClip;
    [SerializeField] private ParticleSystem hitEffect;

    private Animator animator;
    private AudioSource aud;
    private Renderer ren;

    [Header("Info")]
    [SerializeField] private float damage = 20f;
    [SerializeField] private float timeBetAttack = 0.5f;
    private float lastAttackTimeBet;

    [Header("Dropped Item")]
    [SerializeField] private List<GameObject> itemPrefabs = new List<GameObject>();

    // 추적할 만한 target을 찾았는 지 유무를 확인하는 Bool 데이터
    private bool isTarget
    {
        get
        {
            // Target이 존재하며 && Target이 생존해있을 경우 true를 반환
            if (targetEntity != null && targetEntity.isLive)
                return true;
            // 아니면 false를 반환
            return false;
        }
    }

    private void Awake()
    {
        if (!TryGetComponent(out agent)) 
            Debug.Log($"PlayerHealth ] NavMeshAgent 없음");
        if (!TryGetComponent(out animator)) 
            Debug.Log($"PlayerHealth ] Animator 없음");
        if (!TryGetComponent(out aud)) 
            Debug.Log($"PlayerHealth ] AudioClip 없음");
        ren = GetComponentInChildren<Renderer>();
    }

    public void Setup(ZombieData data)
    {
        maxHealth = data.health;
        damage = data.damage;

        agent.speed = data.moveSpeed;
        ren.material.color = data.skinColor;
    }
    public override void OnDamage(float damage, Vector3 hitPosition, Vector3 hitNormal)
    {
        /*
        좀비의 입장
            Player한테 맞고 소리를 내야하고, 이펙트가 발생해야 된다.
        */
        if (isLive)
        {
            hitEffect.transform.position = hitPosition;
            hitEffect.transform.rotation = Quaternion.LookRotation(hitNormal);
            hitEffect.Play();
            aud.PlayOneShot(hitClip);
        }
        base.OnDamage(damage, hitPosition, hitNormal);
    }

    // Character가 사망했을 때, 실행시키는 메서드
    public override void Die()
    {
        base.Die();

        Collider[] cols = GetComponents<Collider>();
        foreach(Collider c in cols)
            c.enabled = false;

        agent.isStopped = true;
        agent.enabled = false;
        animator.SetTrigger("Die");
    }

    private void OnTriggerStay(Collider other)
    {
        // Character가 살아있을 때 && 공격이 가능한 시간일 때
        if (isLive && Time.time >= lastAttackTimeBet + timeBetAttack)
        {
            if (other.TryGetComponent(out LivingEntity l))
            {
                lastAttackTimeBet = Time.time;

                // ClosestPoint -> 닿는 위치.
                // 즉, 상대방 피격 위치와 피격 방향을 근사값을 계산.

                Vector3 hitPoint = other.ClosestPoint(transform.position);
                Vector3 hitNormal = transform.position - other.transform.position;

                l.OnDamage(damage, hitPoint, hitNormal);
            }
        }
    }

    private void Update()
    {
        // Animator의 Bool 데이터에 직접 데이터를 넣어줌.
        animator.SetBool("hasTarget", isTarget);
    }
    private void Start()
    {
        StartCoroutine(Update_TargetPosition_co());
    }
    private IEnumerator Update_TargetPosition_co()
    {
        // 해당 Character가 살아있는가?
        while(isLive)
        {
            // 해당 Chracter가 target을 가지고 있는가?
            if(isTarget)
            {
                agent.isStopped = false;
                agent.SetDestination(targetEntity.transform.position);
            }
            // target을 가지고 있지 않은가?
            else
            {
                agent.isStopped = true;
                Collider[] cols = 
                    Physics.OverlapSphere(transform.position, 20f, targetLayer);
                foreach(var i in cols)
                {
                    if (i.TryGetComponent(out LivingEntity l))
                    {
                        if (l.isLive)
                        {
                            targetEntity = l;
                            break;
                        }
                    }
                }
            }   
            yield return null; // 한 프레임씩 지연
        }
    }
}