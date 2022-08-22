using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D body;
    [SerializeField]
    private float moveSpeed = 6;
    [SerializeField]
    private new SpriteRenderer renderer;

    [SerializeField]
    private Animator animator;

    private static readonly int anim_movingBool = Animator.StringToHash("Moving");

    void Start()
    {
        body = body ?? GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input;
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");
        input = Vector2.ClampMagnitude(input, 1);

        body.velocity = input * moveSpeed;

        animator.SetBool(anim_movingBool, input.sqrMagnitude > 0.01);
        renderer.flipX = input.x < 0;
    }
}
