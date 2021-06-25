using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 2.0f;
    public GameObject plate;

    private Rigidbody rb;
    private Vector3 movement = Vector3.zero;

    private Object carriedObject;
    private Table nearTable;
    private CutTable cuttingTable;
    private AudioManager audioManager;

    private AnimationController animationController;
    public Transform rightFist;

    // Start is called before the first frame update
    public void Start() {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        rb = GetComponent<Rigidbody>();
        animationController = GetComponent<AnimationController>();
    }

    // Update is called once per frame
    public void Update() {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.z = Input.GetAxisRaw("Vertical");
        
        TableInteraction();
        CutIngredient();
        UseExtinguisher();
        RecipeCheat();

        if(carriedObject == null)
            animationController.StopCarry();
        else
            animationController.Carry();
    }

    private void RecipeCheat() {
        if(Input.GetKeyDown(KeyCode.Alpha1) && ReceiptsEngine.Instance.First() != null) {
            if(carriedObject != null)
                Destroy(carriedObject.gameObject);

            carriedObject = Instantiate(plate).GetComponent<Object>();
            carriedObject.transform.SetParent(transform.GetChild(0));
            carriedObject.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            ((Plate) carriedObject).SetRecipe(ReceiptsEngine.Instance.First());
        }
    }

    private void UseExtinguisher(){
        if(carriedObject != null && carriedObject is Extinguisher){
            if(Input.GetKey(KeyCode.F)){
                if(!audioManager.IsPlaying("Fire Extinguisher"))
                    audioManager.Play("Fire Extinguisher");

                ((Extinguisher) carriedObject).Activate();
            } else {
                ((Extinguisher) carriedObject).Deactivate();
                audioManager.Stop("Fire Extinguisher");
            } 
        }
    }

    private void CutIngredient(){
        if(!IsCutting() && Input.GetKey(KeyCode.C) && nearTable is CutTable && ((CutTable) nearTable).HasCuttableObject()) {
            cuttingTable = (CutTable) nearTable;
            rightFist.GetChild(0).gameObject.SetActive(true);
            animationController.Cut();
            cuttingTable.SetKnifeState(false);
            cuttingTable.Cut();
        } else if(IsCutting() && (!Input.GetKey(KeyCode.C) || !(nearTable is CutTable) || !((CutTable) nearTable).HasCuttableObject())) {
            animationController.StopCutting();
            audioManager.Stop("Cutting");
            rightFist.GetChild(0).gameObject.SetActive(false);
            cuttingTable.SetKnifeState(true);
            cuttingTable = null;
        } else if(IsCutting()) {
            cuttingTable.Cut();
        }
    }

    private bool IsCutting() {
        return cuttingTable != null;
    }

    private void TableInteraction(){
        if(Input.GetKeyDown(KeyCode.E) && nearTable != null) {
            audioManager.Play("Catch");
            if(carriedObject == null)
                GetObjectFromTable();
            else
                PutObjectOnTable();
        }
    }

    private void GetObjectFromTable(){
        Object placedObject = nearTable.GetObject();
        if(placedObject != null) {
            carriedObject = placedObject;
            carriedObject.transform.SetParent(transform.GetChild(0));
            carriedObject.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        }
    }

    private void PutObjectOnTable(){
        if(nearTable.PutObject(carriedObject))
            carriedObject = null;
    }

    public void FixedUpdate() {
        movement.Normalize();
        rb.velocity = movement * speed;
        transform.LookAt(transform.position + movement, Vector3.up);
        
        if(movement.magnitude > 0)
            animationController.Run();
        else
            animationController.Idle();
    }

    void OnTriggerEnter(Collider collision) {
        if(collision.gameObject.GetComponent<Table>() != null)
            nearTable = collision.gameObject.GetComponent<Table>();
    }

    void OnTriggerExit(Collider collision) {
        nearTable = null;
    }
}
