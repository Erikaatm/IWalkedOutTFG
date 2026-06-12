using UnityEngine;

public class FloatEffect : MonoBehaviour
{
    [SerializeField] private float amplitude = 0.07f;
    [SerializeField] private float speed = 2f;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!animator.GetBool("isWalking"))
        {
            float y = Mathf.Sin(Time.time * speed) * amplitude;
            transform.localPosition = new Vector3(0, y, 0);
        }
        else
        {
            transform.localPosition = Vector3.zero;
        }
    }
}