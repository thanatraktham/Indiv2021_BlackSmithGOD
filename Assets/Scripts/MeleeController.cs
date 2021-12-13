using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeController : MonoBehaviour
{
    public PlayerController player;
    public GameManager gm;
    public HealthBar healthBar;
    public ArmorBar armorBar;
    public StaminaBar staminaBar;
    public ManaBar manaBar;
    public GameObject weaponImg;

    //Health
    public float maxHealth;
    //Armor
    public float maxArmor;
    //Melee
    public int meleeAttackDamage;
    public float meleeAttackRate;
    public float meleeAttackRange;
    public float meleeKnockBack;
    //Range
    public float primaryChargeSpeed;
    public float secondaryChargeSpeed;
    //Movement
    public float movementSpeed;
    //Stamine
    public float maxStamina;
    public float staminaRechargeRate;
    //Mana
    public float maxMana;
    public float manaRechargeRate;
    //WeaponSprite
    public Sprite meleeWeaponSprite;


    public void toggleMeleeStat() {
        Debug.Log("Melee Stat Toggled");
        // ++MaxHealth
        gm.setMaxHealth(maxHealth);
        healthBar.setMaxHealth(gm.getMaxHealth());
        healthBar.setHealth(gm.getCurrentHealth());
        // ++Armor
        gm.setMaxArmor(maxArmor);
        gm.setCurrentArmor(gm.getCurrentArmor() + 50);
        armorBar.setMaxArmor(gm.getMaxArmor());
        armorBar.setArmor(gm.getCurrentArmor() + 50);
        // ++MeleeRate
        player.setAttackDamage(meleeAttackDamage + 50);
        player.setAttackRate(meleeAttackRate);
        player.setAttackRange(meleeAttackRange);
        player.setEnemyKnockbackForce(meleeKnockBack);
        // ++AttackSpeed
        player.setPrimaryChargeSpeed(player.getPrimaryChargeSpeed() / primaryChargeSpeed);
        player.setSecondaryChargeSpeed(player.getSecondarySpeed() / secondaryChargeSpeed);
        // ++MovementSpeed
        player.setSpeed(movementSpeed);
        // ++MaxStamina
        gm.setMaxStamina(maxStamina);
        staminaBar.setMaxStamina(gm.getMaxStamina());
        staminaBar.setStamina(gm.getCurrentStamina());
        // ++StaminaRechargeRate
        gm.setStaminaRechargeRate(staminaRechargeRate);
        // ++MaxMana
        gm.setMaxMana(maxMana);
        manaBar.setMaxMana(gm.getMaxMana());
        manaBar.setMana(gm.getCurrentMana());
        // ++ManaRechargeRate
        gm.setManaRechargeRate(manaRechargeRate);
        // WeaponSprite
        weaponImg.GetComponent<SpriteRenderer>().sprite = meleeWeaponSprite;
    }
}
