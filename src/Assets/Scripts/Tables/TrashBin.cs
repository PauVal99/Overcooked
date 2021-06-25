using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashBin : Table {
    override public bool PutObject(Object NewObject){
        NewObject.ThrowToBin();
        return false;
    }
}