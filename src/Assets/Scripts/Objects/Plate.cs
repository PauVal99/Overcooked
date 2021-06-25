using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : Object {
    private List<Ingredient> ingredients = new List<Ingredient>();
    private Recipe recipe;
    private ParticleSystem boom;

    new public void Awake() {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        boom = transform.GetChild(2).GetComponent<ParticleSystem>();
    }

    public bool AddIngredient(Ingredient ingredient) {
        if(IsRecipe() || ingredient == null || ingredients.Count >= 10)
            return false;

        ingredients.Add(ingredient);
        recipe = ReceiptsEngine.Instance.GetRecipe(ingredients);
        if(recipe == null) {
            ingredient.transform.SetParent(transform.GetChild(0));
            ingredient.transform.localPosition = new Vector3(0.0f, 0.0f, (ingredients.Count * 0.003f));
        } else
            SetRecipe(recipe);

        return true;
    }

    public void SetRecipe(Recipe recipe) {
        this.recipe = recipe;
        audioManager.Play("Delivery");
        boom.Play();
        ingredients.ForEach(ingredient => Destroy(ingredient.gameObject));
        ingredients = new List<Ingredient>();

        GameObject recipeModel = Instantiate(recipe.GetModel());
        recipeModel.transform.SetParent(transform.GetChild(1));
        recipeModel.transform.localPosition = new Vector3(0.0f, 0.0f, 0.003f);
    }

    public override void Burn()
    {
        foreach(var ingredient in ingredients){
            ingredient.Burn();
        }
    }

    public bool IsRecipe(){
        return recipe != null;
    }

    public Recipe GetRecipe(){
        return recipe;
    }

    override public void ThrowToBin() {
        if(IsRecipe()) {
            Destroy(transform.GetChild(1).GetChild(0).gameObject);
            recipe = null;
        } else {
            ingredients.ForEach(ingredient => Destroy(ingredient.gameObject));
            ingredients = new List<Ingredient>();
        }
    }
}
