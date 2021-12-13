using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipWeapon : MonoBehaviour
{
    public string weaponClass;
    public int level;

    public Sprite weaponSprite;

    private GameManager gm;
    private playerMovement player;

    private void Start() {
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<playerMovement>();
    }

    public void equipWeapon() {
        gm.setActiveWeapon(weaponClass, level);
        player.setWeaponSprite(weaponSprite);
    }
}
