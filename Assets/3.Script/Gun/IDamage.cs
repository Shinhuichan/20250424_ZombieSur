using UnityEngine;

/*
        추상클래스          Interface
다중상속    X                   O
구현여부    abstruct(x)         X
           virtual(o)  
*/
public interface IDamage 
{
    // 매개 변수 : 피해량, 맞은 위치, 맞은 각도
    void OnDamage(float damage, Vector3 hitPostion, Vector3 hitNormal);
}