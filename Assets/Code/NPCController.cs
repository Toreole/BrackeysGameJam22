using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class NPCController : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private SpriteRenderer sprite;

    [SerializeField]
    private AIPath pathFinder;

    private PlayerMovement playerFollow = null;
    private List<NPCController> nearbyNPCs;

    private int followId = -1;
    private Vector3 lastPos;
    private float lastMoveTime = 0;

    private bool inConversation = false;
    private bool controllingConvo = false;
    private NPCController conversationPartner;
    private Coroutine conversationRoutine;

    //animator properties.
    private static readonly int anim_moving = Animator.StringToHash("Moving");

    private static readonly int anim_like = Animator.StringToHash("Like");
    private static readonly int anim_dislike = Animator.StringToHash("Dislike");
    private static readonly int anim_surprise = Animator.StringToHash("Surprise");
    private static readonly int anim_talk = Animator.StringToHash("Talk");
    private static readonly int anim_question = Animator.StringToHash("Question");

    public NPCController()
    {
        nearbyNPCs = new List<NPCController>(3);
    }

    private void Start()
    {
        animator = animator ?? GetComponent<Animator>();
    }

    private void Update()
    {
        //self position for movement.
        Vector3 pos = transform.position;
        Vector3 delta = pos - lastPos;

        //following player
        if (playerFollow)
        {

            //buffer some positions and deltas.
            Vector3 followPos = playerFollow.GetFollowTarget(followId);

            pathFinder.destination = followPos;

            sprite.flipX = followPos.x - pos.x < 0;

        }

        bool moving = delta != Vector3.zero;
        animator.SetBool(anim_moving, moving);

        if (!moving)
        {
            if (inConversation) 
                FaceConvoPartner();
            //start conversation if not already in one and havent moved in some time.
            else if (Time.time - lastMoveTime > 2.2f)
            {
                StartConversation();
            }
        }
        else
        {
            lastMoveTime = Time.time;
            if (inConversation) //stop conversations when moving.
            {
                StopConversation();
            }
        }

        //update cache.
        lastPos = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && !playerFollow)
        {
            animator.SetTrigger(anim_surprise);
            playerFollow = collider.GetComponent<PlayerMovement>();
            followId = playerFollow.RegisterFollower(transform);
        }
        //track nearby npcs.
        else if (collider.CompareTag("Friendly NPC"))
        {
            NPCController other = collider.GetComponent<NPCController>();
            if(nearbyNPCs.Contains(other) == false)
                nearbyNPCs.Add(other);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.CompareTag("Friendly NPC"))
            nearbyNPCs.Remove(collider.GetComponent<NPCController>());
    }

    private void StartConversation()
    {
        Debug.Log("trying to start conversation");
        if(nearbyNPCs.Count > 0)
        {
            //get a random conversation partner.
            int rn = Random.Range(0, nearbyNPCs.Count);
            controllingConvo = true;

            conversationPartner = nearbyNPCs[rn];
            conversationPartner.conversationPartner = this;

            conversationPartner.inConversation = true;
            this.inConversation = true;

            conversationRoutine = StartCoroutine(DoConversation());
        }
    }

    private IEnumerator DoConversation()
    {
        //only controller has permission to do something in the conversation.
        while(inConversation && controllingConvo)
        {
            this.ShowRandomEmote();
            yield return new WaitForSeconds(Random.Range(0.8f, 1.2f));

            conversationPartner.ShowRandomEmote();
            yield return new WaitForSeconds(Random.Range(0.8f, 1.2f));
        }
    }

    private void FaceConvoPartner()
    {
        sprite.flipX = conversationPartner.transform.position.x < transform.position.x;
    }

    private void ShowRandomEmote()
    {
        switch(Random.Range(0, 10))
        {
            case 0:
            case 1:
            case 2:
            case 3:
            case 4:
                animator.SetTrigger(anim_talk);
                break;
            case 5:
            case 6:
                animator.SetTrigger(anim_question);
                break;
            case 7:
            case 8:
                animator.SetTrigger(anim_like);
                break;
            case 9:
                animator.SetTrigger(anim_dislike);
                break;
            default:
                break;
        }
    }

    private void StopConversation(bool callOnPartner = true)
    {
        inConversation = false;
        controllingConvo = false;
        if (conversationRoutine != null)
            StopCoroutine(conversationRoutine);
        if (callOnPartner && conversationPartner != null)
            conversationPartner.conversationPartner.StopConversation(false);
        conversationPartner = null;
    }

}
