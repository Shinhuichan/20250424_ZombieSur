using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    /* Update 주기

    Update() -> Frame update(1초에 프레임 수만큼)
    FixedUpdate() -> 물리 기반 Update(1초에 약 50회)
    LateUpdate() -> update 후처리(1초에 프레임 수만큼)

    */

    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float rotateSpeed = 180f;

    [SerializeField] private PlayerInput player_Input;

    private Rigidbody player_r;
    private Animator player_ani;

    private Camera cam;

    private void Start()
    {
        /* GetComponent vs TryGetComponent
    큰 의미로 봐서는 대입연산(=)을 사용하는 메소드들은 대부분 대입 전에 Nullptr을 생성한다.
    이러한 문제 때문에 대입연산자를 사용하고 나서는 결론적으로 쓰레기 Collecter의 수집대상이 된다.
    이러한 문제 때문에 tryGetComponent tryParse / 비교연산자 대신 equls() 등등 반환갑이 bool으로
    되어 있고, 컴포넌트 초기화를 위해 Out 키워드를 사용하여서 컴포넌트를 할당한다.
        */
        // transform.TryGetComponent(out player_Input);

        // if (!transform.TryGetComponent(out player_r))
        // {
        //     gameObject.AddComponent<Rigidbody>();
        //     transform.TryGetComponent(out player_r);
        // }
    
        // transform.TryGetComponent(out player_ani);

        transform.TryGetComponent(out player_Input);
        transform.TryGetComponent(out player_r);
        transform.TryGetComponent(out player_ani);

        cam = Camera.main;
    }
        private void FixedUpdate()
    {
        Move();
        //Rotate();
        RotateMouse();
        player_ani.SetFloat("Move", player_Input.Move_Value);
    }

    private void Move()
    {
        Vector3 moveDirection =
                            player_Input.Move_Value * transform.forward * moveSpeed * Time.deltaTime;

        player_r.MovePosition(player_r.position + moveDirection);
    }

    private void Rotate()
    {
        // float turn = player_Input.Rotate_Value * rotateSpeed * Time.deltaTime;
        // player_r.rotation =
        //                 player_r.rotation * Quaternion.Euler(0, turn, 0);

        // 1. Rotate Speed 데이터를 사용하지 않고 캐릭터가 마우스 커서를 바로 바라보게 설정
        /*
        Vector3 mouse_pos = Input.mousePosition;
        Ray ray = cam.ScreenPointToRay(mouse_pos);
        if (Physics.Raycast(ray, out var hit, 100f))
        {
            mouse_pos = hit.point;
            mouse_pos.y = transform.position.y;

            transform.LookAt(mouse_pos);
        }
        */
    }

    private void RotateMouse()
    {
        // 마우스에서 Ray를 발사하여 마우스의 위치에서 게임 화면의 위치까지 닿는 지를 Check
        // ScreenPointToRay() : UnityEngine.InputSystem.Controls.Vector2Control 데이터형을 요구
        // 따라서, Vector3 값에 ReadValue()로 데이터형을 변환해서 사용함.
        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        if(Physics.Raycast(ray, out var hit, 100f))
        {
            // 마우스가 향한 방향을 계산
            Vector3 direction = (hit.point - player_r.position).normalized;
            direction.y = 0f;

            // 바라볼 방향으로 회전
            Quaternion rot = Quaternion.LookRotation(direction);
            player_r.rotation = Quaternion.RotateTowards(player_r.rotation, rot, Time.deltaTime * rotateSpeed);
        }
    }
}