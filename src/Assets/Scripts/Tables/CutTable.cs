using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutTable : Table {
    private ParticleSystem smoke;

    void Start() {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        smoke = transform.GetChild(3).GetComponent<ParticleSystem>();
        fire = this.transform.Find("ParticleSystem").GetComponent<Fire>();
    }

    public void Cut() {
        if(placedObject != null) {
            ((Ingredient) placedObject).Cut();

            if(!audioManager.IsPlaying("Cutting"))
                audioManager.Play("Cutting");
        }
    }

    public void SetKnifeState(bool state) {
        if(!state) 
            smoke.Play();
        else {
            smoke.Stop();
            audioManager.Play("Delivery");
        }

        this.transform.GetChild(2).gameObject.SetActive(state);
    }

    public bool HasCuttableObject() {
        if(placedObject == null)
            return false;

        return ((Ingredient) placedObject).IsChoppable() && ((Ingredient) placedObject).GetState() == State.RAW;
    }

    override public bool PutObject(Object NewObject) {
        if(fire.IsPlaying())
            return false;
        if(placedObject == null && NewObject is Ingredient){
            this.placedObject = NewObject;
            placedObject.transform.SetParent(this.transform.GetChild(0));
            placedObject.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            return true;
        } else if(NewObject is Plate && ((Plate) NewObject).AddIngredient((Ingredient) placedObject)) {
            placedObject = null;
            return false;
        } else
            return false;   
    }
}