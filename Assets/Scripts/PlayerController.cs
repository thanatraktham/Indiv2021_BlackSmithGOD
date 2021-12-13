using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //PlayerComponent//
    public Animator animator;
    public Rigidbody2D rb;
    private float moveSpeed = 8f;
    private Vector2 movement = new Vector2(0f, -1f);
    private Vector2 mousePos;

    //Dialog//
    public GameObject dialogueBox;
    public GameObject startDialogueButton;
    
    //Dashing//
    private Vector2 dashDirection;
    private bool isDashing;

    //Shooting//
    public float penaltySpeed;
    public GameObject primaryBullet;
    public GameObject secondaryBullet;
    public float primaryBulletSpeed;
    public float secondaryBulletSpeed;
    public float secondartBulletManaCost;
    private float primaryChargeSpeed = 0.3f;
    private float secondaryChargeSpeed = 1f;
    private bool IsShooting;
    private bool IsCharging;
    private bool ReadyToShoot;
    private bool ShootButtonReleased;

    //Melee//
    public float attackRange;
    public float attackRate;
    public int attackDamage;
    public LayerMask enemyLayers;
    public LayerMask crateLayers;
    public Transform attackPoint;
    public Animator meleeWeaponAnim;
    private float enemyKnockbackForce;
    private float nextAttackTime = 0f;

    //OtherComponent//
    public Camera cam;
    public GameManager gm;
    public GameObject weapon;
    public GameObject rotateWeapon;
    public spawnTreant spnT;
    private SpriteRenderer weaponImg;
    private SpriteRenderer rotateWeaponImg;
    private SpriteRenderer attackPointImg;


    void Awake() {
        DontDestroyOnLoad(this.gameObject);
    }

    void Start() {
        animator.SetBool("FaceDown", true);
        dashDirection = movement;
        enemyKnockbackForce = 1000f;
        weaponImg = weapon.GetComponent<SpriteRenderer>();
        rotateWeaponImg = rotateWeapon.GetComponent<SpriteRenderer>();
        attackPointImg = attackPoint.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (!gm.getIsTalking() && Time.timeScale != 0) {

            // Debug.Log(weaponImg.sortingOrder);

            //Movement//
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
            mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

            if (!isDashing) {
                if (movement.x == 1) {
                    // Debug.Log("Right");
                    resetAnimationState();
                    animator.SetBool("FaceRight", true);
                    dashDirection = movement;
                    weaponImg.sortingOrder = 4;
                } else if (movement.x == -1) {
                    // Debug.Log("Left");
                    resetAnimationState();
                    animator.SetBool("FaceLeft", true);
                    dashDirection = movement;
                    weaponImg.sortingOrder = 4;
                }
                if (movement.y == 1) {
                    // Debug.Log("Up");
                    resetAnimationState();
                    animator.SetBool("FaceUp", true);
                    dashDirection = movement;
                    weaponImg.sortingOrder = 6;
                } else if (movement.y == -1){
                    // Debug.Log("Down");
                    resetAnimationState();
                    animator.SetBool("FaceDown", true);
                    dashDirection = movement;
                    weaponImg.sortingOrder = 4;
                }
            }

            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            animator.SetFloat("Speed", movement.sqrMagnitude);

            //Dash//
            if (Input.GetKeyDown(KeyCode.LeftShift)) {
                if ((gm.getStamina() - 15) > 0) {
                    gm.useStamina(15);
                    animator.SetBool("IsDashing", true);
                    isDashing = true;
                    StartCoroutine(Dashing(0.15f));
                    rb.velocity = Vector2.zero;
                    if (movement.x != 0 && movement.y != 0) {
                        dashDirection = movement;
                    }
                }
            }

            //Melee//
            if (Input.GetKeyDown(KeyCode.Space)) {
                if (Time.time >= nextAttackTime) {
                    Melee();
                    nextAttackTime = Time.time + (1f / attackRate);
                }
            }

            //Shooting//
            if (Input.GetButtonDown("Fire1")) {
                if (!IsShooting && !IsCharging) {
                    StartCoroutine(Charge(primaryChargeSpeed));
                }
                IsShooting = true;
            } else if (Input.GetButtonUp("Fire1")) {
                attackPointImg.color = Color.white;
                // attackPointImg.enabled = false;

                StopAllCoroutines();
                IsCharging = false;
                if (ReadyToShoot) {
                    Shoot(primaryBullet, primaryBulletSpeed);
                }
                IsShooting = false;
            }
            if (gm.getMana() >= secondartBulletManaCost) {
                if (Input.GetButtonDown("Fire2")) {
                    if (!IsShooting && !IsCharging) {
                        StartCoroutine(Charge(secondaryChargeSpeed));
                    }
                    IsShooting = true;
                } else if (Input.GetButtonUp("Fire2")) {
                    attackPointImg.color = Color.white;
                    // attackPointImg.enabled = false;

                    StopAllCoroutines();
                    IsCharging = false;
                    if (ReadyToShoot) {
                        Shoot(secondaryBullet, secondaryBulletSpeed);
                        gm.useMana(secondartBulletManaCost);
                    }
                    IsShooting = false;
                }
            }

            Vector2 lookDir = mousePos - rb.position;
            lookDir.Normalize();
            
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
            attackPointImg.transform.rotation = Quaternion.Euler(0f, 0f, angle);
            // Debug.Log("weapon rotation = " + weaponImg.transform.rotation);
            if (gm.getActiveClass() == "range") {
                rotateWeaponImg.transform.rotation = Quaternion.Euler(0f, 0f , angle - 45f);
            } else if (gm.getActiveClass() == "mage") {
                rotateWeaponImg.transform.rotation = Quaternion.Euler(0f, 0f , angle + 45f);
            }
        }
    }

    void FixedUpdate()
    {
        if (!gm.getIsTalking()) {
            if (isDashing) {
                rb.MovePosition(rb.position + dashDirection * 20f * Time.deltaTime);
            } else if (IsShooting) {
                rb.MovePosition(rb.position + movement * moveSpeed * penaltySpeed * Time.fixedDeltaTime);
            } else {
                rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Pill") {
            Destroy(other.gameObject);
            gm.heal(5);
        } else if (other.gameObject.name == "HealingStation") {
            GameObject dialogueCanvas = GameObject.FindGameObjectWithTag("Dialogue");
            if (dialogueCanvas) {
                foreach(Transform tmp in dialogueCanvas.transform) {
                    if (tmp.gameObject.name == "DialogueBox") {
                        dialogueBox = tmp.gameObject;
                    } else if (tmp.gameObject.name == "StartDialogueButton") {
                        startDialogueButton = tmp.gameObject;
                    } else {
                        Debug.Log("Nothing found in dialogueCanvas");
                    }
                }
            } else {
                Debug.Log("Dialogue Canvas not found");
            }
            if (startDialogueButton) {
                startDialogueButton.SetActive(true);
                Animator buttonAnimator = startDialogueButton.GetComponent<Animator>();
                buttonAnimator.SetTrigger("FadeIn");
            } else {
                Debug.Log("Cannot found gameobject");
            }
        // } else if (other.gameObject.name == "MeleeUpgradeStation") {
        //     Debug.Log("Entering melee upgrade station");
        //     GameObject meleeUpgradeCanvas = GameObject.FindGameObjectWithTag("MeleeUpgradeCanvas");
        //     if (meleeUpgradeCanvas) {
        //         Debug.Log("Melee upgrade canvas found");
        //         meleeUpgradeCanvas.SetActive(true);
        //     } else {
        //         Debug.Log("Melee upgrade canvas not found");
        //     }
        } else if (other.gameObject.tag == "TransitionSquare") {
            switch (other.gameObject.name) {
                case "Dungeon1":
                    gm.SwapSceen("Dungeon1");
                    break;
                case "StartingScene":
                    gm.SwapSceen("StartingScene");
                    break;
                case "Dungeon2":
                    gm.SwapSceen("Dungeon2");
                    break;
                case "Dungeon3":
                    gm.SwapSceen("Dungeon3");
                    break;
                case "Dungeon4":
                    gm.SwapSceen("Dungeon4");
                    break;
                case "MyHomeScene":
                    gm.SwapSceen("MyHomeScene");
                    break;
                case "1-1":
                    gm.SwapSceen("1-1-1");
                    break;
                case "1-2":
                    gm.SwapSceen("1-1-2");
                    break;
                case "1-3":
                    gm.SwapSceen("1-1-3");
                    break;
                case "1-4":
                    gm.SwapSceen("1-1-4");
                    break;
                case "1-5":
                    gm.SwapSceen("1-5");
                    break;
            }
        }
         else if (other.gameObject.tag == "material_1") {
            gm.collectMaterial("material_1");
            Destroy(other.gameObject);
        } else if (other.gameObject.tag == "material_2") {
            gm.collectMaterial("material_2");
            Destroy(other.gameObject);
        } else if (other.gameObject.tag == "material_3") {
            gm.collectMaterial("material_3");
            Destroy(other.gameObject);
        } else if (other.gameObject.tag == "spawnTreant") {
            spnT.startSpawn();
            Debug.Log("SpawnTest");
        }
    }

    void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.tag == "PoisonAOE") {
            // Debug.Log("Damage AOE taken");
            gm.takeDamage(0.5f);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.name == "HealingStation") {
            StartCoroutine(BringDownStartDialogueButton());
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        string enemyTag = other.gameObject.tag;
        if (enemyTag == "Slime" && !isDashing) {
            TakeDamage(5);
        } else if (enemyTag == "Skeleton") {
            TakeDamage(5);
        } else if (other.gameObject.tag == "HomingBullet") {
            TakeDamage(40);
        }
    }

    public void TakeDamage(int damage) {
        gm.takeDamage(damage);
    }

    private void resetAnimationState() {
        animator.SetBool("FaceUp", false);
        animator.SetBool("FaceDown", false);
        animator.SetBool("FaceLeft", false);
        animator.SetBool("FaceRight", false);
    }

    //Dashing//
    private IEnumerator Dashing(float dashTimer) {
        yield return new WaitForSeconds(dashTimer);
        animator.SetBool("IsDashing", false);
        isDashing = false;
    }
    
    //Shooting//
    private void Shoot(GameObject bulletType, float bulletForce) {       
        ReadyToShoot = false; 
        GameObject bullet = Instantiate(bulletType, transform.position, Quaternion.identity);
        Bullet bulletScript = bullet.GetComponent<Bullet>();

        Vector2 lookDir = mousePos - rb.position;
        lookDir.Normalize();
        
        bulletScript.velocity = lookDir * bulletForce;
        bulletScript.player = gameObject;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        bullet.transform.Rotate(0, 0, angle);
    }

    private IEnumerator Charge(float chargeTime) {
        ReadyToShoot = false;
        IsCharging = true;
        // attackPointImg.enabled = true;
        yield return new WaitForSeconds(chargeTime);
        attackPointImg.color = Color.red;
        ReadyToShoot = true;
    }

    //Melee//
    void Melee() {
        animator.SetTrigger("Attack");

        if (gm.getActiveClass() == "melee") {
            meleeWeaponAnim.SetTrigger("melee");
        }

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach(Collider2D obj in hitEnemies) {
            if (obj.gameObject.CompareTag("Slime")) {
                obj.GetComponent<SlimeController>().TakeDamage(attackDamage);
            } else if (obj.gameObject.CompareTag("Skeleton")) {
                obj.GetComponent<SkeletonController>().TakeDamage(attackDamage);
            } else if (obj.gameObject.CompareTag("Treant")) {
                obj.GetComponent<TreantController>().TakeDamage(attackDamage);
            } else if (obj.gameObject.CompareTag("Mole")) {
                obj.GetComponent<MoleController>().TakeDamage(attackDamage);
            } else if (obj.gameObject.CompareTag("HomingBullet")) {
                obj.GetComponent<Bullet_Homing>().DestroyHomingBullet();
            }
            Rigidbody2D enemy_rb = obj.GetComponent<Rigidbody2D>();
            if (enemy_rb != null) {
                enemy_rb.isKinematic = false;
                Vector2 knockBackDir = obj.transform.position - transform.position;
                knockBackDir = knockBackDir.normalized * enemyKnockbackForce * 2;
                enemy_rb.AddForce(knockBackDir, ForceMode2D.Impulse);
                StartCoroutine(EnemyKnockback(enemy_rb));
            }
        }

        Collider2D[] hitCrate = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, crateLayers);

        foreach(Collider2D crate in hitCrate) {
            if (crate) {
                crate.GetComponent<CrateController>().dropMaterial();
                Destroy(crate.gameObject);
            }
        }
    }

    private IEnumerator EnemyKnockback(Rigidbody2D enemy_rb) {
        yield return new WaitForSeconds(0.25f);
        if (enemy_rb != null) {
            enemy_rb.velocity = Vector2.zero;
            enemy_rb.isKinematic = true;
        }
    }

    private void OnDrawGizmosSelected() {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public float getAttackRange(){
        return attackRange;
    }

    public void setAttackRange(float newAttackRange){
        attackRange = newAttackRange;
    }

    public float getAttackRate(){
        return attackRate;
    }

    public void setAttackRate(float newAttackRate){
        attackRate = newAttackRate;
    }
    
    public int getAttackDamage(){
        return attackDamage;
    }

    public void setAttackDamage(int newAttackDamage){
        attackDamage = newAttackDamage;
    }

    public float getEnemyKnockbackForce(){
        return enemyKnockbackForce;
    }

    public void setEnemyKnockbackForce(float newEnemyKnockbackForce){
        enemyKnockbackForce = newEnemyKnockbackForce;
    }

    //Dialog//
    private IEnumerator BringDownStartDialogueButton() {
        Animator buttonAnimator = startDialogueButton.GetComponent<Animator>();
        buttonAnimator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(0.25f);
        startDialogueButton.SetActive(false);
    }

    //Getter/Setter
    public float getSpeed() {
        return moveSpeed;
    }

    public void setSpeed(float newSpeed) {
        moveSpeed = newSpeed;
    }

    public float getPrimaryChargeSpeed() {
        return primaryChargeSpeed;
    }

    public void setPrimaryChargeSpeed(float newSpeed) {
        primaryChargeSpeed = newSpeed;
    }

    public float getSecondarySpeed() {
        return secondaryChargeSpeed;
    }

    public void setSecondaryChargeSpeed(float newSpeed) {
        secondaryChargeSpeed = newSpeed;
    }

    public void setWeaponSprite(Sprite newSprite) {
        // Debug.Log("Player Weapon Sprite Set");
        // Debug.Log(weapon.GetComponent<SpriteRenderer>().sprite);
        if (gm.getActiveClass() == "melee") {
            weapon.GetComponent<SpriteRenderer>().sprite = newSprite;
        } else if (gm.getActiveClass() == "range" || gm.getActiveClass() == "mage") {
            rotateWeapon.GetComponent<SpriteRenderer>().sprite = newSprite;
        } else {
            Debug.Log("PlayerController line 481");
        }
    }


}
