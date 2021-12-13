using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBonus : MonoBehaviour
{
    public playerMovement player;
    public GameManager gm;
    public HealthBar healthBar;
    public ArmorBar armorBar;
    public StaminaBar staminaBar;
    public ManaBar manaBar;

    //Health
    public float bonusMaxHealth;
    //Armor
    public float BonusMaxArmor;
    //Melee
    public int bonusMeleeAttackDamage;
    public float meleeAttackRate;
    public float meleeAttackRange;
    public float meleeKnockBack;
    //Range
    public float primaryChargeSpeed;
    public float secondaryChargeSpeed;
    //Movement
    public float bonusMovementSpeed;
    //Stamine
    public float bonusMaxStamina;
    public float bonusStaminaRechargeRate;
    //Mana
    public float bonusMaxMana;
    public float bonusManaRechargeRate;

    private void Start() {
        resetStat();
        // Debug.Log("Active wWeapon Bonus Added");
        
        // ++MaxHealth
        gm.setMaxHealth(gm.getMaxHealth() + bonusMaxHealth);
        healthBar.setMaxHealth(gm.getMaxHealth() + bonusMaxHealth);
        healthBar.setHealth(gm.getCurrentHealth());
        // ++Armor
        gm.setMaxArmor(gm.getMaxArmor() + BonusMaxArmor);
        armorBar.setMaxArmor(gm.getMaxArmor() + BonusMaxArmor);
        armorBar.setArmor(gm.getCurrentArmor());
        // ++MeleeRate
        player.setAttackDamage(player.getAttackDamage() + bonusMeleeAttackDamage);
        player.setAttackRate(meleeAttackRate);
        player.setAttackRange(player.getAttackRange() * meleeAttackRange);
        player.setEnemyKnockbackForce(meleeKnockBack);
        // ++AttackSpeed
        player.setPrimaryChargeSpeed(player.getPrimaryChargeSpeed() / primaryChargeSpeed);
        player.setSecondaryChargeSpeed(player.getSecondarySpeed() / secondaryChargeSpeed);
        // ++MovementSpeed
        player.setSpeed(player.getSpeed() + bonusMovementSpeed);
        // ++MaxStamina
        gm.setMaxStamina(gm.getMaxStamina() + bonusMaxStamina);
        staminaBar.setMaxStamina(gm.getMaxStamina() + bonusMaxStamina);
        staminaBar.setStamina(gm.getCurrentStamina());
        // ++StaminaRechargeRate
        gm.setStaminaRechargeRate(gm.getStaminaRechargeRate() + bonusStaminaRechargeRate);
        // ++MaxMana
        gm.setMaxMana(gm.getMaxMana() + bonusMaxMana);
        manaBar.setMaxMana(gm.getMaxMana() + bonusMaxMana);
        manaBar.setMana(gm.getCurrentMana());
        // ++ManaRechargeRate
        gm.setManaRechargeRate(gm.getManaRechargeRate() + bonusManaRechargeRate);
    }

    public void resetStat() {
        // Debug.Log("Stat reseted");
        // // ++MaxHealth
        // gm.setMaxHealth(maxHealth);
        // healthBar.setMaxHealth(gm.getMaxHealth());
        // healthBar.setHealth(gm.getCurrentHealth());
        // // ++Armor
        // gm.setMaxArmor(maxArmor);
        // armorBar.setMaxArmor(gm.getMaxArmor());
        // armorBar.setArmor(gm.getCurrentArmor());
        // // ++MeleeRate
        // player.setAttackDamage(meleeAttackDamage);
        // player.setAttackRate(meleeAttackRate);
        // player.setAttackRange(player.getAttackRange() * meleeAttackRange);
        // player.setEnemyKnockbackForce(meleeKnockBack);
        // // ++AttackSpeed
        // player.setPrimaryChargeSpeed(player.getPrimaryChargeSpeed() / primaryChargeSpeed);
        // player.setSecondaryChargeSpeed(player.getSecondarySpeed() / secondaryChargeSpeed);
        // // ++MovementSpeed
        // player.setSpeed(movementSpeed);
        // // ++MaxStamina
        // gm.setMaxStamina(maxStamina);
        // staminaBar.setMaxStamina(gm.getMaxStamina());
        // staminaBar.setStamina(gm.getCurrentStamina());
        // // ++StaminaRechargeRate
        // gm.setStaminaRechargeRate(staminaRechargeRate);
        // // ++MaxMana
        // gm.setMaxMana(maxMana);
        // manaBar.setMaxMana(gm.getMaxMana());
        // manaBar.setMana(gm.getCurrentMana());
        // // ++ManaRechargeRate
        // gm.setManaRechargeRate(manaRechargeRate);
    }
}
