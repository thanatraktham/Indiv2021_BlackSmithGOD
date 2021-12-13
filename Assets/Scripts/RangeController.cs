using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeController : MonoBehaviour
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
    public Sprite rangeWeaponSprite;


    public void toggleRangeStat() {
        Debug.Log("Range Stat Toggled");
        // ++MaxHealth
        gm.setMaxHealth(maxHealth);
        healthBar.setMaxHealth(gm.getMaxHealth());
        healthBar.setHealth(gm.getCurrentHealth());
        // ++Armor
        gm.setMaxArmor(maxArmor);
        armorBar.setMaxArmor(gm.getMaxArmor());
        armorBar.setArmor(gm.getCurrentArmor());
        // ++MeleeRate
        player.setAttackDamage(meleeAttackDamage);
        player.setAttackRate(meleeAttackRate);
        player.setAttackRange(player.getAttackRange() * meleeAttackRange);
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
        weaponImg.GetComponent<SpriteRenderer>().sprite = rangeWeaponSprite;
    }
}
