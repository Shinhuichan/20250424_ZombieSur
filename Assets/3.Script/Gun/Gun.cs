using System;
using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    /*
    총알(가시적인 총알) -> Raycast(실질적) / LineRenderer 
    총의 사거리(float)
    발사될 위치(Vector3)
    GunData
    Effect -> 발사 Effect, 탄피 Effect -> Particle
    총의 상태 -> Enum(상태를 정의할 때, 대체로 사용됨)
        -> 재장전 중
        -> 탄창이 비었을 때
        -> 발사 준비완료
    AudioSource

    method
        발사 -> Fire
        Reload
        Effect Play
    */

    public enum State {
        Ready, // 발사 준비
        Empty, // 탄창 빔
        Reloading // 재장전 중
    }

    public State state {get; private set;}

    public Transform fire_Tr;
    public LineRenderer line;

    public GunData gunData;
    public ParticleSystem shot_Effect;
    public ParticleSystem shell_Effect;
    public int ammoRemain = 100;
    public int MagAmmo;



    private AudioSource aud;
    private float distance = 50f;
    private float lastFireTime;

    private void Awake()
    {
        aud = GetComponent<AudioSource>();
        line = GetComponent<LineRenderer>();

        line.positionCount = 2; // 활성화, 비활성화
        line.enabled = false; // Component 비활성화
    }
    private void OnEnable()
    {
        // 현재 총의 여분 탄약 데이터와 현재 총의 탄창 크기의 데이터를 가져옴
        ammoRemain = gunData.startAmmoRemain;
        MagAmmo = gunData.MAGCapacity;

        // 총의 초기 상태를 Ready(발사 준비 완료)로 설정
        state = State.Ready;

        lastFireTime = 0;
    }

    #region Shooting
    public void Fire()
    {
        if (state.Equals(State.Ready) // Player의 현재 총 상태가 Ready(발사 준비 완료)인 경우
            && // 그리고
        Time.time >= lastFireTime + gunData.timeBetFire) // 마지막 발사 시간이 현재 시간보다 작을 경우
        {
            lastFireTime = Time.time;
            // 발사
            Shot();
        }
    }

    private void Shot()
    {
        RaycastHit hit;
        Vector3 hitPosition = Vector3.zero;

        if (Physics.Raycast(fire_Tr.position, fire_Tr.forward, out hit, distance))
        {
            // 총에 맞은 경우
            // 우리가 만든 Interface를 가지고 와서
            // 맞은 오브젝트한테 데미지를 줘야함...

            // 특정 컴포넌트가 있던없던 가져오려고 시도(없으면 NUll)
            IDamage target = hit.collider.GetComponent<IDamage>();
            if (target != null) target.OnDamage(gunData.damage, hit.point, hit.normal);

            // // 특정 컴포넌트가 있을 때만 조건문의 내부를 실행(없으면 Continue)
            // if (hit.collider.TryGetComponent(out IDamage d))
            // {
            //     d.OnDamage(gunData.damage, hit.point, hit.normal);
            // }

            hitPosition =  hit.point;
        }
        else 
            // ray에 충돌된 물체가 없다면
                // 탄알이 최대 사거리까지 날라갔음을 가시적으로 표현
            hitPosition = fire_Tr.position + fire_Tr.forward * distance; // 현재 위치 + 현재 방향 * 최대 사거리

        // Effect 표현
        StartCoroutine(ShotEffect(hitPosition));
        
        // 현재 총의 남은 탄이 0 이하일 경우, 총의 상태를 Empty로 변경
        if (--MagAmmo <= 0) state = State.Empty;
    }

    private IEnumerator ShotEffect(Vector3 point)
    {
        // Effect 재생
        shell_Effect.Play();
        shot_Effect.Play();

        // Audio 재생
        aud.PlayOneShot(gunData.shot_Clip);

        // LineRenderer 설정
        line.SetPosition(0, fire_Tr.position);
        line.SetPosition(1, point);

        // Component 활성화 및 일정 시간 후, 비활성화
        line.enabled = true;
        yield return new WaitForSeconds(0.03f);

        line.enabled = false;
    }
    #endregion

    #region Reloading
    public bool Reload()
    {
        // 현재 재장전이 필요한지를 return할 Method

        // 이미 재장전 중이거나
        // 총알이 없거나
        // 탄창이 이미 총알이 가득하거나
        if (state.Equals(State.Reloading) || ammoRemain <= 0 || MagAmmo >= gunData.MAGCapacity) return false;

        StartCoroutine(Reload_co());
        return true;
    }
    public IEnumerator Reload_co()
    {
        state = State.Reloading;
        aud.PlayOneShot(gunData.reload_Clip);
        yield return new WaitForEndOfFrame();

        // 재장전 후에 계산
        int ammofill = gunData.MAGCapacity - MagAmmo;
        // 탄창에 채워야 할 탄약이 남은 탄약보다 많다면
        if (ammoRemain < ammofill) ammofill = ammoRemain;

        // 탄창을 채우고 전체 탄창 수를 줄인다.
        MagAmmo += ammofill;
        ammoRemain -= ammofill;
        state = State.Ready;
    }
    #endregion
}