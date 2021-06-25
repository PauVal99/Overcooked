using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour {
    protected Object placedObject;
    protected float fireHealth = 120f;
    [SerializeField]
    private AudioSource fireSound;
    protected Fire fire;
    protected AudioManager audioManager;
    private float spreadFireCounter = 12f;

    void Start() {
        fireSound = GetComponent<AudioSource>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        fire = this.transform.Find("ParticleSystem").GetComponent<Fire>();
    }

    void Update() {
        SpreadFire();
    }

    protected void SpreadFire(){
        if(fire.IsPlaying()){
            spreadFireCounter -= Time.deltaTime;
            if(spreadFireCounter <= 0){
                Collider[] hitColliders = Physics.OverlapSphere(GetComponent<Renderer>().bounds.center, 1.25f);
                foreach(var hitCollider in hitColliders){
                    Table table = hitCollider.gameObject.GetComponent<Table>();
                    if(table != null)
                        table.ActivateFire();
                }
                spreadFireCounter = 10f;
            }
        }
    }

    protected virtual void ActivateFire(){
        fire.ActivateFire();
        fireSound.Play();
        
        if(placedObject != null)
            if(!(this is IngredientTable))
                placedObject.Burn();
    }

    public virtual void ExtinguisFire(){
        fire.ExtinguisFire();
        fireSound.Stop();
        fireHealth = 100;
        spreadFireCounter = 5;
    }

    public bool HasObject() {
        return placedObject != null;
    }

    public virtual Object GetObject() {
        Object objectToReturn = placedObject;

        placedObject = null;
        return objectToReturn;
    }


    public virtual bool PutObject(Object NewObject) {
        if(fire.IsPlaying())return false;
        if(placedObject == null){
            placedObject = NewObject;
            placedObject.transform.SetParent(transform.GetChild(0));
            placedObject.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            return true;
        } else if(NewObject is Ingredient) {
            if(placedObject is Plate) {
                return ((Plate) placedObject).AddIngredient((Ingredient) NewObject);
            } else if(placedObject is KitchenTool)
                return ((KitchenTool) placedObject).AddIngredient((Ingredient) NewObject);
        } else if(NewObject is KitchenTool && placedObject is Ingredient && ((KitchenTool) NewObject).AddIngredient(((Ingredient) placedObject))) {
            placedObject = null;
            return false;
        } else if(NewObject is KitchenTool && placedObject is Plate && ((Plate) placedObject).AddIngredient(((KitchenTool) NewObject).GetIngredient())) {
            ((KitchenTool) NewObject).DeleteIngredient();
            return false;
        } else if(NewObject is Plate && placedObject is Ingredient && ((Plate) NewObject).AddIngredient((Ingredient) placedObject)) {
            placedObject = null;
            return false;
        } else if(NewObject is Plate && placedObject is KitchenTool && ((Plate) NewObject).AddIngredient(((KitchenTool) placedObject).GetIngredient())) {
            ((KitchenTool) placedObject).DeleteIngredient();
            return false;
        }
        return false;
    }
}
