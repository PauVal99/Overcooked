using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

internal class Order {
    public Order(Recipe recipe, float remainingTime) {
        this.recipe = recipe;
        this.remainingTime = remainingTime;
    }

    public Recipe recipe;
    public float remainingTime;
    public Slider slider;
}

public class ReceiptsEngine : MonoBehaviour {
    public List<Recipe> receipts = new List<Recipe>();
    public float levelTime = 240.0f;
    public float orderTime = 60.0f;
    public float minTimeToNewOrder = 60.0f, maxTimeToNewOrder = 50.0f;
    
    public Text uiMoney, uiCrono;
    public GameObject uiOrderPrefab;
    public GameObject EndMenu;
    public GameObject PauseMenu;
    public Transform uiOrders;

    private List<Order> queue = new List<Order>();
    private float timeToNewOrder = 0.0f;
    private int money;
    private bool cronoRunning = true;
    private bool paused = false;

    private static ReceiptsEngine _instance;
    public static ReceiptsEngine Instance { get { return _instance; } }

    [System.Serializable]
    public struct OrderImage{
        public string name;
        public Sprite image;
    }

    public OrderImage[] images;

    public void Start() {
        SetMoney(0);   
    }

    public Recipe First() {
        if(queue.Count > 0)
            return queue[0].recipe;
        return null;
    }

    public void Update() {
        UpdateNewOrder();
        UpdateOrders();
        UpdateCrono();

        if(Input.GetKeyDown(KeyCode.Escape)){
            if(paused){
                Time.timeScale = 1;
                PauseMenu.SetActive(false);
                paused = false;
            }
            else{
                Time.timeScale = 0;
                PauseMenu.SetActive(true);
                paused = true;
            }
        }
    }

    public void Resume(){
        
        Time.timeScale = 1;
        PauseMenu.SetActive(false);
        paused = false;
       
    }

    private void UpdateCrono(){
        if(levelTime > 0 && cronoRunning){
            levelTime -= Time.deltaTime;
        } else if(cronoRunning){
            cronoRunning = false;
            levelTime = 0;
            Time.timeScale = 0;
            EndMenu.SetActive(true);
        }

        int minutes = Mathf.FloorToInt(levelTime / 60);
        int seconds = Mathf.FloorToInt(levelTime % 60);

        string minutesString;
        string secondsString;

        if(minutes < 10){
            minutesString = string.Format("0{0}", minutes);
        }
        else minutesString = string.Format("{0}", minutes);

        if(seconds < 10){
            secondsString = string.Format("0{0}", seconds);
        }
        else secondsString = string.Format("{0}", seconds);

        uiCrono.text = minutesString + ":" + secondsString;

    }

    private void InstantiateOrderInUI(Order NewOrder) {
        GameObject UIOrder = Instantiate(uiOrderPrefab, uiOrders.transform);
        NewOrder.slider = UIOrder.transform.GetChild(0).GetComponent<Slider>();
        for(int i = 0; i < images.Length; i++){
            if(images[i].name == NewOrder.recipe.name) {
                UIOrder.GetComponent<Image>().sprite = images[i].image;
                break;
            }
        }
    }

    private void UpdateNewOrder() {
        timeToNewOrder -= Time.deltaTime;
        if(timeToNewOrder <= 0.0f && queue.Count < 4) {
            timeToNewOrder = Random.Range(minTimeToNewOrder, maxTimeToNewOrder);
            Order NewOrder = new Order(receipts[Random.Range(0, receipts.Count)], orderTime);
            queue.Add(NewOrder);
            InstantiateOrderInUI(NewOrder);
        }
    }

    private void DeleteOrderFromUI(int index){
        Destroy(uiOrders.GetChild(index).gameObject);
    }

    private void UpdateOrders() {
        foreach (Order order in queue.ToArray()) {
            order.remainingTime -= Time.deltaTime;
            order.slider.value = 1 - ((orderTime - order.remainingTime) / orderTime);
            if(order.remainingTime <= 0) {
                DeleteOrderFromUI(queue.IndexOf(order));
                queue.Remove(order);
                SetMoney(money - 15);
            }
        }
    }

    public void AddCompletedRecipe(Recipe recipe) {
        foreach(Order order in queue)
            if(order.recipe == recipe) {
                DeleteOrderFromUI(queue.IndexOf(order));
                queue.Remove(order);
                SetMoney(money + recipe.GetPrice() + (int) (order.remainingTime * 0.25f));
                return;
            }
        
        SetMoney(money - 30);
    }

    public Recipe GetRecipe(List<Ingredient> plate) {
        foreach (Recipe recipe in receipts) {
            if (recipe.IsRecipe(plate))
                return recipe;
        }
        return null;
    }


    private void SetMoney(int money) {
        if(money < 0)
            this.money = 0;
        else
            this.money = money;
        uiMoney.text = this.money.ToString();
    }

    private void Awake()
    {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    private void OnDestroy() {
        if (this == _instance)
            _instance = null;
    }
}
