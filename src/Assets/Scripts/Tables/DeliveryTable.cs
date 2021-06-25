using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryTable : Table {
    public PlateTable plateTable;
    private ParticleSystem money;

    void Start() {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        fire = this.transform.Find("ParticleSystem").GetComponent<Fire>();
        money = transform.GetChild(0).GetComponent<ParticleSystem>();
    }

    override public bool PutObject(Object NewObject) {
        if(fire.IsPlaying())
            return false;

        if(NewObject is Plate && ((Plate) NewObject).IsRecipe()) {
            Plate plate = (Plate) NewObject;
            ReceiptsEngine.Instance.AddCompletedRecipe(plate.GetRecipe());
            plate.ThrowToBin();
            plateTable.AddPlate(plate);
            audioManager.Play("Delivery");
            money.Play();
            return true;
        } else
            return false;
    }
}
