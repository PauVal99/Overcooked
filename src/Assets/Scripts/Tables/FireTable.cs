using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTable : Table {
    public GameObject tool;
    [SerializeField]
    private AudioSource grillingSound;

    public void Start() {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        fire = this.transform.Find("ParticleSystem").GetComponent<Fire>();
        placedObject = Instantiate(tool).GetComponent<Object>();
        placedObject.transform.SetParent(transform.GetChild(0));
        placedObject.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        placedObject.transform.rotation = transform.rotation;
    }

    public void Update() {
        Cook();
        SpreadFire();

        if(placedObject != null && ((KitchenTool) placedObject).HasIngredient()) {
            if(!grillingSound.isPlaying)
                 grillingSound.Play();
        } else
            grillingSound.Stop();
            
    }

    public void Cook(){
        if(placedObject != null && !((KitchenTool) placedObject).Cook()) {
            ActivateFire();
        }
    }

    override public bool PutObject(Object NewObject) {
        if(fire.IsPlaying())
            return false;

        if(placedObject == null && NewObject is KitchenTool) {
            this.placedObject = NewObject;
            placedObject.transform.SetParent(this.transform.GetChild(0));
            placedObject.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            return true;
        } else if(placedObject != null && NewObject is Ingredient) {
            return ((KitchenTool) placedObject).AddIngredient((Ingredient) NewObject);
        } else if(placedObject != null && NewObject is Plate && ((Plate) NewObject).AddIngredient(((KitchenTool) placedObject).GetIngredient())) {
            ((KitchenTool) placedObject).DeleteIngredient();
            return false;
        }
        return false;
    }
}
