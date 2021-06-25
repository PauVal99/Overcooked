using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtinguisherTable : Table {
    public GameObject extinguisher;

    private void Start() {
        fire = this.transform.Find("ParticleSystem").GetComponent<Fire>();
        placedObject = Instantiate(extinguisher).GetComponent<Object>();
        placedObject.transform.SetParent(transform.GetChild(0));
        placedObject.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        placedObject.transform.localRotation = Quaternion.Euler(180.0f, 90.0f, 90.0f);
    }
}