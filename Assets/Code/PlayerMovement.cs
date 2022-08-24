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

    private List<NPCController> followers = new List<NPCController>(10);

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

    public int RegisterFollower(NPCController t)
    {
        int c = followers.Count;
        followers.Add(t);
        return c;
    }

    public Vector3 GetFollowTarget(int id)
    {
        return id == 0 ? transform.position : followers[id - 1].transform.position;
    }

    public void UnregisterFollower(NPCController t)
    {
        if(followers.Contains(t))
        {
            followers.Remove(t);
            for(int i = 0; i < followers.Count; i++)
            {
                followers[i].SetFollowerID(i);
            }
        }
    }
}
