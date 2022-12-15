using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthEnergy : MonoBehaviour
{
    [Header("Max Values")]
    public float maxHealth=100f;
    public float maxEnergy=100f;

    [Header("Current Values")]
    public float currentHealth=100f;
    public float currentEnergy=100f;
    
    [Header("Reduction Values")]
    //TODO: Acertar Melhor Valores
    public float normalReduction=0.001f;
    public float jumpReduction=0.2f;
    public float runReduction=0.002f;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth=100f;
        currentEnergy=100f;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentEnergy<0){
            currentEnergy=0;
        }
    }

    public void reduceHealthEnergy(string movement){
        float amountReduce=0;

        if(movement=="Normal"){
            amountReduce=normalReduction;

        }else if(movement=="Jump"){
            amountReduce=jumpReduction;

        }else if(movement=="Run"){
            amountReduce=runReduction;
        }
        if(currentEnergy<=0){
            currentHealth-=amountReduce;
        }
        else{
            currentEnergy-=amountReduce;
        }

    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("ItemTest")){
            
            if(currentEnergy<=0){
                currentHealth-=20;
            }
            else{
                currentEnergy-=20;
            }

            Debug.Log("Colisão e tirei vida");
            Destroy(other.gameObject);
        }
    }
    //TODO: Adicionar diferentes tipos de tag para os items a colidir e planear os valores a tirar/aumetar
}
