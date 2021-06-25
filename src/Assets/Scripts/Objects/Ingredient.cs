using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public enum IngredientName {
    BREAD,
    MEAT,
    LETTUCE,
    TOMATO,
    CHEESE,
    BACON,
    EGG,
    HOTDOG_BREAD,
    SAUSAGE,
    KETCHUP_MUSTARD,
    RICE,
    SALMON,
    CARROT,
    PUMPKIN,
    TURNIP
};

public enum State {
    RAW, CHOPPED, COOKED, OVERCOOKED
};

public class Ingredient : Object {
    public IngredientName ingredientName;
    public State state = State.RAW;
    public KitchenTools kitchenTool;
  
    private AudioSource audioSource;
    public bool choppable, cookable, cookCheat = false;
    public float chopTime, cookTime;
    private float overcookTime, remainingOvercookTime, remainingChopTime, remainingCookTime;
    private bool overcooking = false;

    public GameObject raw, chopped, cooked, overcooked;
    private GameObject[] states = new GameObject[4];

    private Slider slider;
    private Image warning;

    public void Start() {
        audioSource = GetComponent<AudioSource>();
        states[(int)State.RAW] = raw;
        states[(int)State.CHOPPED] = chopped;
        states[(int)State.COOKED] = cooked;
        states[(int)State.OVERCOOKED] = overcooked;
        
        slider = transform.GetChild(0).GetChild(0).GetComponent<Slider>();
        slider.gameObject.SetActive(false);

        warning = transform.GetChild(0).GetChild(1).GetComponent<Image>();
        warning.gameObject.SetActive(false);

        remainingChopTime = chopTime;
        remainingCookTime = cookTime;
        remainingOvercookTime = overcookTime = cookTime * 0.6f;
    }

    public void Update() {
        if(!overcooking && warning.gameObject.activeSelf)
            warning.gameObject.SetActive(false);
        overcooking = false;

        if(state != State.OVERCOOKED && remainingOvercookTime < (0.75f * overcookTime)) {
            if(!audioSource.isPlaying)
                audioSource.Play();
        } else if(audioSource.isPlaying)
            audioSource.Stop();

        if(Input.GetKeyDown(KeyCode.Alpha3)) {
            audioManager.Play("Delivery");
            audioSource.Stop();
            cookCheat = !cookCheat;
        }
    }

    public IngredientName GetIngredientName() {
        return ingredientName;
    }

    public State GetState() {
        return state;
    }

    public bool IsChoppable() {
        return choppable;
    }

    public bool IsCookable() {
        return cookable;
    }

    public void SetState(State state) {
        if(this.state != state) {
            this.state = state;
            UpdateModel(state);
        }
    }

    public void Cut() {
        if(choppable && state == State.RAW) {
            remainingChopTime -= Time.deltaTime;
            slider.gameObject.SetActive(true);
            slider.value = (chopTime - remainingChopTime) / chopTime;
            if(remainingChopTime <= 0) {
                SetState(State.CHOPPED);
                slider.gameObject.SetActive(false);
            }
        }
    }

    public override void Burn()
    {
        if(cookable)
            SetState(State.OVERCOOKED);
        else{
            Destroy(gameObject);
        }
    }

    public bool Cook() {
        if(Input.GetKeyDown(KeyCode.Alpha2)) {
            audioManager.Play("Delivery");
            SetState(State.COOKED);
            slider.gameObject.SetActive(false);
        } else if(state != State.OVERCOOKED) {
            if(state == State.COOKED && !cookCheat) {
                overcooking = true;
                remainingOvercookTime -= Time.deltaTime;
                if(remainingOvercookTime <= 0) {
                    SetState(State.OVERCOOKED);
                    warning.gameObject.SetActive(false);
                    slider.gameObject.SetActive(false);
                    return false;
                } else if(remainingOvercookTime <= (0.4f * overcookTime)) {
                    warning.gameObject.SetActive(true);
                    warning.color = Color.red;
                } else if(remainingOvercookTime <= (0.75f * overcookTime)) {
                    warning.gameObject.SetActive(true);
                }
            } else {
                remainingCookTime -= Time.deltaTime;
                slider.gameObject.SetActive(true);
                slider.value = (cookTime - remainingCookTime) / cookTime;
                if(remainingCookTime <= 0) {
                    SetState(State.COOKED);
                    slider.gameObject.SetActive(false);
                }
            }
        }

        return true;
    }

    private void UpdateModel(State state) {
        GetComponent<MeshFilter>().mesh = states[(int)state].GetComponent<MeshFilter>().sharedMesh;
        GetComponent<Renderer>().materials = states[(int)state].GetComponent<Renderer>().sharedMaterials;
    }

    override public void ThrowToBin(){
        Destroy(gameObject);
    }
}
