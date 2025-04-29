using System;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamage // 다중 상속(Interface)
{
    // 모든 상호작용 혹은 생명체에게 붙힐 Component -> 추상화
    /*
        전체 체력
        현재 체력
        isLive? -> Event
        Damage 계산
    */

    public float maxHealth = 100f;
    public float health
    {
        get;
        protected set;
    }
    public bool isLive 
    {
        get;
        protected set;
    }
    public event Action onDead;
    /*
        우리가 Event를 사용하는 이유는?
    각 Script 혹은 Component 등 직접 참조를 하게 되면 구조가본의 아니게 상당히 복잡해진다.
    간접 참조를 통해서 각 Script의 유연성을 전달한다.
    즉, 각 Script 별로 결합도 -> Coupling을 약화하기 위해 사용된다.
        -> Observer Pattern을 사용하였다!
    */

    protected virtual void OnEnable()
    {
        // virtual : 구현의 강제성을 부여하지 않습니다.
        // 즉, 구현을 못 하지 않고 부모 클래스에서도 구현이 가능합니다.
        isLive = true;
        health = maxHealth;
    }

    // Character가 damage를 받았을 때, health가 감소되고 0 이하가 되었을 때, 죽는 메서드를 실행하는 메서드
    public virtual void OnDamage(float damage, Vector3 hitPosition, Vector3 hitNormal)
    {
        // Debug.Log(health);
        health -= damage;
        
        // 살았는지... 죽었는지...
        if (health <= 0 && isLive)
        {
            // 죽는 메서드를 호출
            Die();
        }
    }

    // 죽을 때, 발생되는 메서드
    public virtual void Die()
    {
        if (onDead != null)
        {
            onDead();
        }
        isLive = false;
    }

    // 변경된 Health 데이터를 최신화하는 메서드
    public virtual void Restore_Health(float newHeath)
    {
        if (!isLive)
            return;
        health += newHeath;
    }
}
