using UnityEngine;


//모든 입력의 관리
public class PlayerInput : MonoBehaviour
{

    [SerializeField] private string MoveAxis_name = "Vertical";
    [SerializeField] private string RotateAxis_name = "Horizontal";
    [SerializeField] private string Fire = "Fire1";
    [SerializeField] private string Reload = "Reload";


    // getAxis > float

    public float Move_Value { get; private set; } // Property
    public float Rotate_Value { get; private set; }


    // getButton > bool
    public bool isFire { get; private set; }
    public bool isReload { get; private set; }


    private void Update()
    {
        Move_Value = Input.GetAxis(MoveAxis_name);
        Rotate_Value = Input.GetAxis(RotateAxis_name);

        isFire = Input.GetButton(Fire);
        isReload = Input.GetButton(Reload);
    }
}