using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateTable : Table {
    public int plates;
    public GameObject plate;

    private Stack<Plate> cleanPlates = new Stack<Plate>();

    private void Start() {
        fire = this.transform.Find("ParticleSystem").GetComponent<Fire>();
        for(int i = 0; i < plates; i++)
            AddPlate(Instantiate(plate).GetComponent<Plate>());
    }

    public void AddPlate(Plate plate) {
        if(cleanPlates.Count < plates) {
            cleanPlates.Push(plate);
            plate.transform.SetParent(transform.GetChild(0));
            plate.transform.localPosition = new Vector3(0.0f, 0.0f, (cleanPlates.Count * 0.0025f));
        } else
            Destroy(plate.gameObject);
    }

    override public bool PutObject(Object Object) {
        return false;
    }

    override public Object GetObject() {
        return cleanPlates.Pop();
    }
}
