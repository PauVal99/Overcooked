using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientTable : Table {
    public GameObject ingredient;

    private bool cookCheat = false;

    private void Start() {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        InstantiateIngredient();
    }

    private void Update() {
        SpreadFire();
        if(Input.GetKeyDown(KeyCode.Alpha3)) {
            audioManager.Play("Delivery");
            cookCheat = !cookCheat;
        }
    }

    private void InstantiateIngredient() {
        fire = this.transform.Find("ParticleSystem").GetComponent<Fire>();
        placedObject = Instantiate(ingredient).GetComponent<Object>();
        placedObject.transform.SetParent(transform.GetChild(0));
        placedObject.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        placedObject.transform.rotation = ingredient.transform.rotation;
        ((Ingredient) placedObject).cookCheat = cookCheat;
    }

    override public bool PutObject(Object Object) {
        return false;
    }

    override public Object GetObject(){
        Object objectToReturn = placedObject;
        InstantiateIngredient();
        return objectToReturn;
    }
}