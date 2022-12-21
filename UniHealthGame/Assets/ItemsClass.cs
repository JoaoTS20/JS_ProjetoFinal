using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsClass
{   
    // Information Related
    private string tagName;

    // Health Effects
    private float effectHealth;
    private float effectEnergy;

    // Movement Effects
    private float effectMoveSpeed;
    private float effectMaxSpeed;
    private float effectJumpSpeed;
    
    // Temporary Effect Mechanics
    private float effectDuration;
    private float effectTimer=0;
    private bool effectApplied=false;
    
    //Normal Items Constructor
    public ItemsClass(string tagName, float effectHealth, float effectEnergy){
        this.tagName=tagName;
        this.effectHealth=effectHealth;
        this.effectEnergy=effectEnergy;
    }

    // Temporary Effect Items Constructor
    public ItemsClass(string tagName, float effectDuration,float effectMoveSpeed,float effectMaxSpeed,float effectJumpSpeed){
        this.tagName=tagName;
        this.effectApplied=effectApplied;
        this.effectMoveSpeed=effectMoveSpeed;
        this.effectMaxSpeed=effectMaxSpeed;
        this.effectJumpSpeed=effectJumpSpeed;
    }

    public string TagName{
        get {return tagName;}
    }
    
    public float EffectHealth{
        get {return effectHealth;}
    }
    
    public float EffectEnergy{
        get {return effectEnergy;}
    }
    //

    public float EffectMoveSpeed{
        get {return effectMoveSpeed;}
    }
    
    public float EffectMaxSpeed{
        get {return effectMaxSpeed;}
    }
    
    public float EffectJumpSpeed{
        get {return effectJumpSpeed;}
    }


    //
    public float EffectTimer{
        get {return effectTimer;}
        set {effectTimer = value;}
    }

    public float EffectDuration{
        get {return effectDuration;}
        set {effectDuration = value;}
    }

    public bool EffectApplied{
        get {return effectApplied;}
        set {effectApplied = value;}
    }
}
