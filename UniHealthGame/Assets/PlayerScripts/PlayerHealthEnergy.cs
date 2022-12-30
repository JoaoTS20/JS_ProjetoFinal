using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealthEnergy : MonoBehaviour
{   
    private float lerpspeed;

    [Header("Max Values")]
    public float maxHealth=100f;
    public float maxEnergy=100f;

    [Header("Health Bar Variables")]
    public TMP_Text healthText;
    public Image healthBar;


    [Header("Energy Bar Variables")]
    public TMP_Text energyText;
    public Image energyBar;

    [SerializeField] private AudioSource catchItemSoundEffect;



    [Header("Current Values")]
    public float currentHealth=100f;
    public float currentEnergy=100f;
    
    [Header("Movement Reduction Values")]
    //TODO: Acertar Melhor Valores
    public float normalReduction=0.01f;
    public float jumpReduction=0.2f;
    public float runReduction=0.02f;

    [Header("Items Effect")]
    public Dictionary<string,ItemsClass> itemsEffectDict = new Dictionary<string,ItemsClass>(){
        {"ItemTest", new ItemsClass("ItemTest",-0.2f,-0.2f)},
        {"Fruta", new ItemsClass("Fruta",0.15f,0.15f)}, // Aumento 15%
        {"Exercicio", new ItemsClass("Exercicio",0.10f,-0.05f)}, // Aumento 10% Saúde, diminui 5% energia
        {"Chocolates", new ItemsClass("Chocolates",0.0f,0.05f)}, // Aumento 5% Energia
        {"Legumes", new ItemsClass("Legumes",0.15f,0.05f)}, // Aumento Saúde 15% e Aumento 5% Energia 
        {"FastFood", new ItemsClass("FastFood",-0.20f,-0.10f)}, // Diminui Saúde 20% e Diminui 10% Energia 
        {"Alcool", new ItemsClass("Alcool",-0.10f,0.5f)}, // Diminui Saúde 10% e Aumenta 5% Energia 
        {"BebidaEnergetica", new ItemsClass("BebidaEnergetica",-0.25f,0.20f)}, // Diminui Saúde 25% e Aumenta 20% Energia 
        {"Cafe", new ItemsClass("Cafe",0.0f,0.20f)}, // Aumenta 20% Energia 
        {"Netflix", new ItemsClass("Netflix",0.0f,-0.10f)}, // Diminui 10% Energia 

    };


    // Start is called before the first frame update
    void Start()
    {
        currentHealth=100f;
        currentEnergy=100f;

        energyBar=GameObject.Find("EnergyBar").GetComponent<Image>();
        healthBar=GameObject.Find("HealthBar").GetComponent<Image>();
        energyText=GameObject.Find("EnergyTextTMP").GetComponent<TMP_Text>();
        healthText=GameObject.Find("HealthTextTMP").GetComponent<TMP_Text>();



        //TODO ver melhor forma de obter valores
    }

    // Update is called once per frame
    void Update()
    {
        lerpspeed=3f*Time.deltaTime;

        if(currentEnergy<0){
            currentEnergy=0;
        }

        if(currentHealth<0){
            currentHealth=0;
        }

        //Update Text
        healthText.text = "Health: " + currentHealth.ToString("N2") + "%";
        energyText.text = "Energy: " + currentEnergy.ToString("N2") + "%";

        //Update Bars
        updateBars();

    }

    public void updateBars(){
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount,currentHealth/maxHealth,lerpspeed);
        energyBar.fillAmount = Mathf.Lerp(energyBar.fillAmount,currentEnergy/maxEnergy,lerpspeed); 
        
        healthBar.color=Color.Lerp(Color.red,Color.green,(currentHealth/maxHealth));
        energyBar.color=Color.Lerp(Color.red,Color.green,(currentEnergy/maxEnergy));

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

    public bool isHealthy()
    {
        return currentHealth > 50; 
    }

    private void OnTriggerEnter2D(Collider2D other) {
        
        if(itemsEffectDict.ContainsKey(other.gameObject.tag)){

            catchItemSoundEffect.Play();

            if (currentHealth + currentHealth* itemsEffectDict[other.gameObject.tag].EffectHealth < 100)
            {
                currentHealth += currentHealth * itemsEffectDict[other.gameObject.tag].EffectHealth;
            }
            else
            {
                currentHealth = 100;
            }

            if (currentEnergy + currentEnergy * itemsEffectDict[other.gameObject.tag].EffectEnergy < 100)
            {
                currentEnergy += currentEnergy * itemsEffectDict[other.gameObject.tag].EffectEnergy;
            }
            else
            {
                currentEnergy = 100;
            }

            Debug.Log("Colisão com item "+other.gameObject.tag);
            Destroy(other.gameObject);
        }

    }

    public bool isDead()
    {
        return currentHealth <= 0;
    }

    public float getHealth()
    {
        return currentHealth;
    }

    public float getEnergy()
    {
        return currentEnergy;
    }
}