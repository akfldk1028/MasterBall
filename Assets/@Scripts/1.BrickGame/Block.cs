using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{


    [SerializeField] private Transform bricksTransform = null;

    public bool movable = false;
    public Transform topOfBlocksTransform = null;


    private GameManager gameManager;
    // private SkillManager skillManager;
    private Plank plank;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        plank = GameObject.Find("Plank").GetComponent<Plank>();
        // skillManager = GameObject.FindObjectOfType<SkillManager>();
    }


    void Update()
    {

        if (movable)
        {
            transform.Translate(Vector3.down * GameManager.BLOCK_SPEED * Time.deltaTime);
        }


 

        if (bricksTransform.childCount == 0)
        {
            
            gameManager.instantiateBlock();
            // skillManager.syncCurrentSkillsWithPending();
            Destroy(gameObject);
        }

    }






}
