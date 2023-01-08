using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollected : MonoBehaviour
{

    [SerializeField] private GameObject itemAnimationObject;

    [SerializeField] private Animator animator;
    

    // Start is called before the first frame update
    void Start()
    {
        animator= GetComponent<Animator>();
        itemAnimationObject = GameObject.Find("CollectForAnimation");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void activateAnimation(Vector3 itemCollectedPosition)
    {
        transform.position = itemCollectedPosition;
        itemAnimationObject.SetActive(true);
        animator.Play("Collect");

        Invoke("hideObject", 0.5f);
    }

    private void hideObject()
    {
        itemAnimationObject.SetActive(false);
    }
}
    
