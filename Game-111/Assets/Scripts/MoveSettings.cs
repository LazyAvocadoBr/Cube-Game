using UnityEngine;

public class MoveSettings : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Move speed is a multiplier")]
    private float moveSpeed = 1;
    [SerializeField]
    private MoveToGA moveToGA;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveToGA.MoveSpeed = moveSpeed;
    }
}
