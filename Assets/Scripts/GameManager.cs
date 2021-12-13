using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //Health//
    public float maxHealth;
    public float currentHealth;
    public HealthBar healthBar;
    
    //Armor//
    public float maxArmor;
    public float currentArmor;
    public ArmorBar armorBar;
    public GameObject armorBarGameObject;

    //Stamina//
    public float maxStamina;
    public float currentStamina;
    public StaminaBar staminaBar;
    private float staminaRechargeRate = 10f;
    private float manaRachargeRate = 10f;

    //Mana//
    public float maxMana;
    public float currentMana;
    public ManaBar manaBar;

    //Dialog//
    private bool isTalking;

    //Material//
    [HideInInspector] public int material_1_total = 0;
    public Text material_1_text;
    [HideInInspector] public int material_2_total = 0;
    public Text material_2_text;
    [HideInInspector] public int material_3_total = 0;
    public Text material_3_text;

    //Weapons//
    private string activeClass;

    public GameObject melee1;
    public GameObject melee2;
    public GameObject melee3;

    public GameObject range1;
    public GameObject range2;
    public GameObject range3;

    public GameObject mage1;
    public GameObject mage2;
    public GameObject mage3;


    //OtherComponent//
    public GameObject player;
    public GameObject meleeController;
    public GameObject rangeController;
    public GameObject mageController;

    public GameObject meleeWeapon;
    public GameObject rotateWeapon;

    public LayerMask enemyLayers;

    public MenuController menuController;

    private int currentSceneIndex = 0;

    //SceneTransition//
    private GameObject transitionSquare;
    private CircleCollider2D signCollider;
    private SpriteRenderer signSprite;

    void Awake() {
        DontDestroyOnLoad(this.gameObject);
    }

    void Start() {
        Time.timeScale = 0;

        LoadMaterial();

        healthBar.setHealth(maxHealth);
        currentHealth = maxHealth * 0.9f;

        armorBar.setArmor(maxArmor);
        currentArmor = maxArmor * 0.5f;

        currentStamina = maxStamina;
        staminaBar.setStamina(currentStamina);

        manaBar.setMaxMana(maxMana);
        manaBar.setMana(currentMana);

        transitionSquare = GameObject.FindGameObjectWithTag("TransitionSquare");
        
        if (transitionSquare) {
            transitionSquare.SetActive(false);
        } else {
            Debug.Log("MyHomeScene not found");
        }

    }

    void Update() {
        if (currentStamina < maxStamina) {
            regenStamina(staminaRechargeRate);
        }

        if (currentMana < maxMana) {
            regenMana(manaRachargeRate);
        }

        if (Input.GetKeyDown(KeyCode.P)) {
            menuController.PauseGame();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            meleeController.GetComponent<MeleeController>().toggleMeleeStat();
            activeClass = "melee";
            meleeWeapon.SetActive(true);
            rotateWeapon.SetActive(false);
        } else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            rangeController.GetComponent<RangeController>().toggleRangeStat();
            activeClass = "range";
            meleeWeapon.SetActive(false);
            rotateWeapon.SetActive(true);
        } else if (Input.GetKeyDown(KeyCode.Alpha3)) {
            mageController.GetComponent<MageController>().toggleMageStat();
            activeClass = "mage";
            meleeWeapon.SetActive(false);
            rotateWeapon.SetActive(true);
        }

        // Enabled Sign
        Collider2D[] enemiesRemaining = Physics2D.OverlapCircleAll(transform.position, 1000f, enemyLayers);
        if (enemiesRemaining.Length == 0 && transitionSquare) {
            transitionSquare.SetActive(true);
            // signCollider.enabled = true;
            // signSprite.enabled = true;
        } else {
            // Debug.Log("enemies remaining : " + enemiesRemaining.Length);
            // signCollider.enabled = false;
            // signSprite.enabled = false;
        }
    }

    // Scene Transition Order
    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            if (currentSceneIndex == 0) {
                SwapSceen("Dungeon2");
            } else if (currentSceneIndex == 2) {
                SwapSceen("Dungeon3");
            } else if (currentSceneIndex == 3) {
                SwapSceen("StartingScene");
            } else if (currentSceneIndex == 4) {
                SwapSceen("Dungeon3");
            }
        }
    }

    //Health//
    public void takeDamage(float damage) {
        // float totalDamage = (damage - armor);
        // if (totalDamage < 0) {
        //     totalDamage = 0;
        // }
        float leftDamage = damage - currentArmor;
        if(currentArmor > 0) {
            if(leftDamage <= 0 ){
                currentArmor -= damage;
                armorBar.setArmor(currentArmor - damage);
            }
            else {
                currentArmor = 0;
                armorBar.setArmor(0);
                armorBarGameObject.SetActive(false);
            }
        }
        if(currentArmor <= 0) {
            currentHealth -= leftDamage;
            if (currentHealth < 0) {
                healthBar.setHealth(0);
                currentHealth = 0;
                SaveMatarial();
                Destroy(player);
                SwapSceen("StartingScene");
            } else {
                healthBar.setHealth(currentHealth);
            }

        }
    }

    public void heal(float health) {
        currentHealth += health;
        if (currentHealth > maxHealth) {
            healthBar.setHealth(maxHealth);
            currentHealth = maxHealth;
        } else {
            healthBar.setHealth(currentHealth);
        }
    }

    public void setMaxHealth(float newMaxHealth) {
        maxHealth = newMaxHealth;
    }

    public float getMaxHealth() {
        return maxHealth;
    }

    public void setCurrentHealth(float newHealth) {
        if (newHealth > maxHealth) {
            currentHealth = maxHealth;
        } else {
            currentHealth = newHealth;
        }
    }

    public float getCurrentHealth() {
        return currentHealth;
    }

    //Armor//
    public void setMaxArmor(float newMaxArmor){
        maxArmor = newMaxArmor;
    }

    public float getMaxArmor() {
        return maxArmor;
    }

    public void setCurrentArmor(float newArmor) {
        if(newArmor > maxArmor){
            currentArmor = maxArmor;
        } else {
            currentArmor = newArmor;
        }
    }

    public float getCurrentArmor() {
        return currentArmor;
    }

    //Stamina//
    public void useStamina(float stamina) {
        currentStamina -= stamina;
        if (currentStamina < 0) {
            staminaBar.setStamina(0);
            currentStamina = 0;
        } else {
            staminaBar.setStamina(currentStamina);
        }
    }

    public void regenStamina(float stamina) {
        // Debug.Log("stamina regening...");
        currentStamina += stamina * Time.deltaTime;
        staminaBar.setStamina(currentStamina);
    }

    public float getStamina() {
        return currentStamina;
    }

    public void setStaminaRechargeRate(float newRate) {
        staminaRechargeRate = newRate;
    }

    public float getStaminaRechargeRate() {
        return staminaRechargeRate;
    }

    public void setMaxStamina(float stamina) {
        maxStamina = stamina;
    }

    public float getMaxStamina() {
        return maxStamina;
    }

    public void setCurrentStamina(float newStamina) {
        if (newStamina > currentStamina) {
            currentStamina = maxStamina;
        } else {
            currentStamina = newStamina;
        }
    }

    public float getCurrentStamina() {
        return currentStamina;
    }

    //Mana//
    public void useMana(float mana) {
        currentMana -= mana;
        if (currentMana < 0) {
            manaBar.setMana(0);
            currentMana = 0;
        } else {
            manaBar.setMana(currentMana);
        }
    }

    public void regenMana(float mana) {
        currentMana += mana * Time.deltaTime;
        manaBar.setMana(currentMana);
    }

    public float getMana() {
        return currentMana;
    }

    public void setMaxMana(float newMaxMana) {
        maxMana = newMaxMana;
    }

    public float getMaxMana() {
        return maxMana;
    }

    public void setManaRechargeRate(float newRechargeRate) {
        manaRachargeRate = newRechargeRate;
    }

    public float getManaRechargeRate() {
        return manaRachargeRate;
    }

    public void setCurrentMana(float newMana) {
        if (newMana > currentMana) {
            currentMana = maxMana;
        } else {
            currentMana = newMana;
        }
    }

    public float getCurrentMana() {
        return currentMana;
    }

    //Dialog//
    public void setIsTalking(bool boolean) {
        isTalking = boolean;
    }

    public bool getIsTalking() {
        return isTalking;
    }

    //Material//
    public void collectMaterial(string str) {
        if (str == "material_1") {
            material_1_total += 1;
            material_1_text.text = material_1_total.ToString();
        } else if (str == "material_2") {
            material_2_total += 1;
            material_2_text.text = material_2_total.ToString();
        } else if (str == "material_3") {
            material_3_total += 1;
            material_3_text.text = material_3_total.ToString();
        }
    }

    public void removeMaterial(string str, int amount) {
        if (str == "material_1") {
            material_1_total -= amount;
            material_1_text.text = material_1_total.ToString();
        } else if (str == "material_2") {
            material_2_total -= amount;
            material_2_text.text = material_2_total.ToString();
        } else if (str == "material_3") {
            material_3_total -= amount;
            material_3_text.text = material_3_total.ToString();
        }
    }

    public int getMaterial(string str) {
        if (str == "material_1") {
            return material_1_total;
        } else if (str == "material_2") {
            return material_2_total;
        } else if (str == "material_3") {
           return material_3_total;
        }
        return -1;
    }

    //WeaponManagement
    public void setActiveWeapon(string str, int level) {
        Debug.Log("Pre set active weapon");
        resetAllWeapons();
        setActiveClass(str);

        // Sprite tmp_weapon_sprite = meleeWeapon.GetComponent<SpriteRenderer>().sprite;
        // Sprite tmp_rotate_sprite = rotateWeapon.GetComponent<SpriteRenderer>().sprite;
        if (str == "melee") {
            if (level == 1) {
                melee1.SetActive(true);
                // tmp_weapon_sprite = melee1.GetComponent<SpriteRenderer>().sprite;
            } else if (level == 2) {
                melee2.SetActive(true);
                // tmp_weapon_sprite = melee2.GetComponent<SpriteRenderer>().sprite;
            } else if (level == 3) {
                melee3.SetActive(true);
                // tmp_weapon_sprite = melee3.GetComponent<SpriteRenderer>().sprite;
            }
        } else if (str == "range") {
            if (level == 1) {
                range1.SetActive(true);
                // tmp_rotate_sprite = range1.GetComponent<SpriteRenderer>().sprite;
            } else if (level == 2) {
                range2.SetActive(true);
                // tmp_rotate_sprite = range2.GetComponent<SpriteRenderer>().sprite;
            } else if (level == 3) {
                range3.SetActive(true);
                // tmp_rotate_sprite = range3.GetComponent<SpriteRenderer>().sprite;
            }
        } else if (str == "mage") {
            if (level == 1) {
                mage1.SetActive(true);
                // tmp_rotate_sprite = mage1.GetComponent<SpriteRenderer>().sprite;
            } else if (level == 2) {
                mage2.SetActive(true);
                // tmp_rotate_sprite = mage2.GetComponent<SpriteRenderer>().sprite;
            } else if (level == 3) {
                mage3.SetActive(true);
                // tmp_rotate_sprite = mage3.GetComponent<SpriteRenderer>().sprite;
            }
        } else {
            Debug.Log("Not a class");
        }
    }

    public void resetAllWeapons() {
        Debug.Log("Reset All Active Weapons");

        melee1.SetActive(false);
        melee2.SetActive(false);
        melee3.SetActive(false);
        range1.SetActive(false);
        range2.SetActive(false);
        range3.SetActive(false);
        mage1.SetActive(false);
        mage2.SetActive(false);
        mage3.SetActive(false);
    }

    public string getActiveClass() {
        return activeClass;
    }

    public void setActiveClass(string newClass) {
        activeClass = newClass;
    }

    //SceneManagement//
    public void SwapSceen(string scene) {

        if (scene == "StartingScene") {
            currentSceneIndex = 0;
            SceneManager.LoadScene("StartingScene");
            player.transform.position = new Vector2(0f, 0f);
        } else if (scene == "Dungeon1") {
            currentSceneIndex = 1;
            SceneManager.LoadScene("Dungeon1");
            player.transform.position = new Vector2(0f, 0f);
        } else if (scene == "Dungeon2") {
            currentSceneIndex = 2;
            SceneManager.LoadScene("Dungeon2");
            player.transform.position = new Vector2(-14.5f, 0f);
        } else if (scene == "Dungeon3") {
            currentSceneIndex = 3;
            SceneManager.LoadScene("Dungeon3");
            player.transform.position = new Vector2(-23f, 0f);
        } else if (scene == "Dungeon4") {
            currentSceneIndex = 4;
            SceneManager.LoadScene("Dungeon4");
            player.transform.position = new Vector2(0f, 0f);
        } else if (scene == "MyHomeScene"){
            currentSceneIndex = 4;
            SceneManager.LoadScene("MyHomeScene");
            player.transform.position = new Vector2(10f, -21.5f);
        } else if (scene == "1-1-1"){
            currentSceneIndex = 4;
            SceneManager.LoadScene("1-1-1");
            player.transform.position = new Vector2(-29f, -2f);
        } else if (scene == "1-1-2"){
            currentSceneIndex = 4;
            SceneManager.LoadScene("1-1-2");
            player.transform.position = new Vector2(-18f, -9f);
        } else if (scene == "1-1-3"){
            currentSceneIndex = 4;
            SceneManager.LoadScene("1-1-3");
            player.transform.position = new Vector2(-22f, 15f);
        } else if (scene == "1-1-4"){
            currentSceneIndex = 4;
            SceneManager.LoadScene("1-1-4");
            player.transform.position = new Vector2(-14f, -8f);
        } else if (scene == "1-5"){
            currentSceneIndex = 4;
            SceneManager.LoadScene("1-5");
            player.transform.position = new Vector2(-36f, -10f);
        }

        StartCoroutine(waitForSceneToLoadBeforeTransition());

        SaveMatarial();

        // transitionSquare = GameObject.FindGameObjectWithTag("TransitionSquare");
        
        // if (transitionSquare) {
        //     transitionSquare.SetActive(false);
        // } else {
        //     Debug.Log("Scene not found");
        // }
    }

    private IEnumerator waitForSceneToLoadBeforeTransition() {
        yield return new WaitForSeconds(1f);
        transitionSquare = GameObject.FindGameObjectWithTag("TransitionSquare");
        
        if (transitionSquare) {
            transitionSquare.SetActive(false);
            Debug.Log("Set scene inactive sucessfully");
        } else {
            Debug.Log("Scene not found");
        }
    }

    //Save//
    public void SaveMatarial() {
        Debug.Log("System saved : " + Time.time);
        SaveSystem.SaveMatarial(this);
    }

    public void LoadMaterial() {
        Debug.Log("System loaded : " + Time.time);
        GameData data = SaveSystem.LoadMaterial();

        material_1_total = data.material_1_total;
        material_1_text.text = material_1_total.ToString();
        material_2_total = data.material_2_total;
        material_2_text.text = material_2_total.ToString();
        material_3_total = data.material_3_total;
        material_3_text.text = material_3_total.ToString();
    }

}
