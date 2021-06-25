using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KitchenTools {
    NONE, PAN, POT
};

public class KitchenTool : Object {
    public KitchenTools tool;
    private Ingredient ingredient;
    private ParticleSystem smoke;

    public void Start() {
        smoke = transform.GetChild(1).GetComponent<ParticleSystem>();
    }

    public void Update() {
        if(HasIngredient()) {
            if(!smoke.isPlaying)
                smoke.Play();
        } else
            smoke.Stop();
    }

    public bool Cook() {
        if(ingredient != null)
            return ingredient.Cook();
        return true;
    }

    public bool HasIngredient(){
        return ingredient != null;
    }

    public override void Burn(){
        if(ingredient != null)
            ingredient.Burn();
    }

    public bool AddIngredient(Ingredient ingredient) {
        if(this.ingredient == null && IsValid(ingredient)) {
            ingredient.transform.SetParent(transform.GetChild(0));
            if(tool == KitchenTools.PAN)
                ingredient.transform.localPosition = new Vector3(0.0f, 0.0f, 0.005f);
            else
                ingredient.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            this.ingredient = ingredient;
            return true;
        } else
            return false;
    }

    private bool IsValid(Ingredient ingredient) {
        return ingredient.cookable && ingredient.kitchenTool == this.tool
            && ((ingredient.choppable && ingredient.GetState() ==  State.CHOPPED) || (!ingredient.choppable && ingredient.GetState() ==  State.RAW));
    }

    public Ingredient GetIngredient(){
        return ingredient;
    }

    public void DeleteIngredient(){
        ingredient = null;
    }

    override public void ThrowToBin() {
        if(ingredient != null) {
            Destroy(ingredient.gameObject);
            ingredient = null;
        }
    }
}