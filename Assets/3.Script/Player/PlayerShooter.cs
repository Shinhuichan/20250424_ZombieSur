using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    /* 총 쏘기

    */

    public Gun gun;
    
    // 총기 위치 맞추기 위한 transform
    public Transform GunPivot;
    public Transform LeftHand_Mount;
    public Transform RightHand_Mount;

    [SerializeField] private Animator animator;
    [SerializeField] private PlayerInput input;

    private void Start()
    {
        animator = GetComponent<Animator>();
        input = GetComponent<PlayerInput>(); 
    }

    private void Update()
    {
        
        // INput 입력 시
        if (input.isFire)
            gun.Fire();
        else if (input.isReload)
            if (gun.Reload())
                animator.SetTrigger("Reload");
        // UI Update
        UIManager.instance.Update_AmmoText(gun.MagAmmo, gun.ammoRemain);
    }
    private void OnAnimatorIK(int layerIndex)
    {
        // 총의 기준점을 오른쪽 팔꿈치로 이동
        GunPivot.position = animator.GetIKHintPosition(AvatarIKHint.RightElbow);

        // ik를 사용하여 왼손의 위치와 회전을 총 왼쪽 손잡이에 맞춤
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);

        animator.SetIKPosition(AvatarIKGoal.LeftHand, LeftHand_Mount.position);
        animator.SetIKRotation(AvatarIKGoal.LeftHand, LeftHand_Mount.rotation);

        // ik를 사용하여 오른손의 위치와 회전을 총 손잡이에 맞춤.
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);

        animator.SetIKPosition(AvatarIKGoal.RightHand, RightHand_Mount.position);
        animator.SetIKRotation(AvatarIKGoal.RightHand, RightHand_Mount.rotation);
    }
}
