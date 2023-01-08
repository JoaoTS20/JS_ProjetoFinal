using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PlayerHealthEnergy : MonoBehaviour
{   
    private float lerpspeed;

    [Header("Max Values")]
    private float maxHealth=100f;
    private float maxEnergy=100f;

    [Header("Health Bar Variables")]
    private TMP_Text healthText;
    private Image healthBar;


    [Header("Energy Bar Variables")]
    private TMP_Text energyText;
    private Image energyBar;

    [Header("Sound Effects")]
    [SerializeField] private AudioSource catchItemSoundEffect;
    

    [Header("Current Values")]
    [SerializeField] private float currentHealth=100f;
    [SerializeField] private float currentEnergy=100f;

    private TMP_Text effectText;

    private bool firstItemCollected = false;


    [Header("Movement Reduction Values")]
    [SerializeField] private float normalReduction=0.04f;
    [SerializeField] private float jumpReduction=0.15f;
    [SerializeField] private float runReduction=0.065f;

    [Header("Items Effect")]
    private Dictionary<string,ItemsClass> itemsEffectDict = new Dictionary<string,ItemsClass>(){
        {"Fruta", new ItemsClass("Fruta",0.15f,0.15f)}, // Aumento 15%
        {"Exercicio", new ItemsClass("Exercicio",0.10f,-0.05f)}, // Aumento 10% Saúde, diminui 5% energia
        {"Doces", new ItemsClass("Chocolates",0.0f,0.05f)}, // Aumento 5% Energia
        {"Legumes", new ItemsClass("Legumes",0.15f,0.05f)}, // Aumento Saúde 15% e Aumento 5% Energia 
        {"FastFood", new ItemsClass("FastFood",-0.20f,-0.10f)}, // Diminui Saúde 20% e Diminui 10% Energia 
        {"Alcool", new ItemsClass("Alcool",-0.10f,0.5f)}, // Diminui Saúde 10% e Aumenta 5% Energia 
        {"BebidasEnergeticas", new ItemsClass("BebidaEnergetica",-0.25f,0.20f)}, // Diminui Saúde 25% e Aumenta 20% Energia 
        {"Cafe", new ItemsClass("Cafe",0.0f,0.20f)}, // Aumenta 20% Energia 
        {"Netflix", new ItemsClass("Netflix",0.0f,-0.10f)}, // Diminui 10% Energia 

    };

    private Dictionary<string, string> itemsEffectRealNames = new Dictionary<string, string>()
    {
        {"Exercicio","Exercice"},
        {"Alcool","Alcohol"},
        {"BebidasEnergeticas","Energy Drink"},
        {"Cafe","Coffee"},
        {"Legumes","Vegetable"},
        {"Fruta","Fruit"},
        {"Doces","Candy"},
        {"FastFood","Fast Food"},
        
    };

    private GameObject collectForAnimation;

    [SerializeField] private GameObject itemDialogue;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth=100f;
        currentEnergy=100f;

        energyBar=GameObject.Find("EnergyBar").GetComponent<Image>();
        healthBar=GameObject.Find("HealthBar").GetComponent<Image>();
        energyText=GameObject.Find("EnergyTextTMP").GetComponent<TMP_Text>();
        healthText=GameObject.Find("HealthTextTMP").GetComponent<TMP_Text>();
        effectText= GameObject.Find("EffectTextTMP").GetComponent<TMP_Text>();
        effectText.enabled = false;

        collectForAnimation = GameObject.Find("CollectForAnimation");

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
        healthText.text = "Health: " + currentHealth.ToString("N0") + "%";
        energyText.text = "Energy: " + currentEnergy.ToString("N0") + "%";

        //Update Bars
        updateBars();

    }

    private void updateBars(){
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

        if (!firstItemCollected )
        {
            firstItemCollected = true;
            Debug.Log("First Item Collected!");

        }

        if (itemsEffectDict.ContainsKey(other.gameObject.tag)){

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
            collectForAnimation.GetComponent<ItemCollected>().activateAnimation(other.transform.position);

            activateEffectText(other.gameObject.tag);
            Invoke("disableEffectText", 1.5f);
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

    public void disableEffectText()
    {
        effectText.text = "";
        effectText.enabled = false;

    }

    public void activateEffectText(string effect)
    {
        effectText.enabled = true;

        if (itemsEffectRealNames.ContainsKey(effect)){
            effectText.text = itemsEffectRealNames[effect] + "\n Effect Activated!";
        }
        else
        {
            effectText.text = effect + "\n Effect Activated!";
        }
    }

    public bool IsfirstItemCollected()
    {
        return firstItemCollected;
    }
}
