﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public GameObject healthBar;
	public GameObject staminaBar;
	public GameObject firingPoint;
	public Projectile projectile;
	public LayerMask collisionMask;
	public GameObject lavaBurnEffect;

    public FireStream firestream;
    public HealSpell healspell;
    public AOESpell aoespell;
    public PushSpell pushspell;
    public ExMine exmine;
    public TrapMine trapmine;
    public GameObject trapEffect;
    public GameObject hitEffect;
    public GameObject deathEffect;

    Rigidbody rigidBody;
	Animator animator;
	public int playerNum { get; private set; }

	public float maxStamina;
	public float stamina { get; private set; }
	public float attackCost;
    public float aoeCost;
    public float streamCost;
    public float healCost;
    public float exminecost;
    public float healAmt;
    public float stamRechargeDelay = 1;
	private float stamRechargeTimer = 0;

	public float movingTurnSpeed = 360;
	public float stationaryTurnSpeed = 180;
	public float moveSpeedMultiplier = 1f;
	public float fireRate;
	private float lastShot = -10.0f;

	private bool isAiming;
	private bool isMoving;
    private bool isBlocking;
    private float stepDelay = 0.4f;
	private float stepTimer = 0f;

    private bool isBusy;

    private float trapTime;

    Vector3 inputVelocity;
	Vector3 pushVelocity;
	public Vector3 velocity { get; private set; }
	float friction = .05f;
	float squaredFrictionThreshold = .01f;

	public float hp = 100;
	float maxHp = 100;
	int lastDamagedBy;

	void Start(){ 
		animator = GetComponent<Animator>();
		rigidBody = GetComponent<Rigidbody>();
		playerNum = GetComponent<Player> ().playerNum;
		stamina = maxStamina;
		isMoving = false;
		lastDamagedBy = playerNum;
		GameRoundManager.instance.AddPlayer (this);
	}

	void Update(){
		CheckMovementStatus ();
		UpdateStamina ();
		UpdateHealthAndStamBars ();
	}

	void FixedUpdate()
    {
        VerticalCollisions();
        if (!isBlocking)
        {
            pushVelocity = pushVelocity * (1 / (friction + 1));
            if (pushVelocity.sqrMagnitude <= squaredFrictionThreshold)
            {
                pushVelocity = Vector3.zero;
            }
        }
        else
        {
            pushVelocity = Vector3.zero;
        }

        CheckBusy();
        if (!isBusy)
        {
            Vector3 velocity = (inputVelocity + pushVelocity) * Time.fixedDeltaTime;
            rigidBody.MovePosition(rigidBody.position + velocity);
        }
    }

	void UpdateHealthAndStamBars(){
		healthBar.transform.localScale = new Vector3(hp/maxHp, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
		staminaBar.transform.localScale = new Vector3(stamina/maxStamina, staminaBar.transform.localScale.y, staminaBar.transform.localScale.z);
	}

	void CheckForDeath(){
		if (hp <= 0) {
            GameObject d = Instantiate(deathEffect, transform.position, transform.rotation);
            AudioManager.instance.PlaySound("DeathSplosion", transform.position);
            AudioManager.instance.PlaySound("death", transform.position);
            GameRoundManager.instance.PlayerDeath(playerNum, lastDamagedBy);
            Destroy(gameObject);
            Destroy(d, 5);
        }
	}

	void UpdateStamina(){
		stamRechargeTimer += Time.deltaTime;
		if (stamina < maxStamina && stamRechargeTimer > stamRechargeDelay){
			stamina += Time.deltaTime/4;
			if (stamina > maxStamina) {
				stamina = maxStamina;
			}
		}
	}

	void CheckMovementStatus(){
		if (isMoving) {
			if (stepTimer < stepDelay){
				stepTimer += Time.deltaTime;
			} else {
				stepTimer = 0;
				AudioManager.instance.PlaySound("footsteps", transform.position);
			}
		}
		animator.SetBool("isRunning", isMoving);
		animator.SetBool ("isIdle", !isMoving);
	}

    bool CheckTrapped()
    {
        if (animator.GetBool("isTrapped"))
        {
            if(trapTime>0)
            {
                trapTime -= Time.deltaTime;
                return true;
            }
            else
            {
                animator.SetBool("isTrapped", false);
            }
            
        }
        return false;
    }

	public void Attack(){
		if (Time.time > (fireRate + lastShot) && stamina > attackCost)
		{  
            stamina -= attackCost;
            stamRechargeTimer = 0;            

            animator.SetBool("isCasting", true);
            animator.speed = 2.0f;
            Projectile p = Instantiate(projectile, firingPoint.transform.position, transform.rotation);
            p.setPlayerNo(playerNum);
            AudioManager.instance.PlaySound("fireball", p.transform.position);
            lastShot = Time.time;
        }
        else
        {
            //stamRechargeTimer = 0;
            return;
        }
	}

	public void Move(Vector3 _inputVelocity){
        if (_inputVelocity.magnitude > 1f)
        {
            _inputVelocity = _inputVelocity.normalized;
        }
        if (isMoving = _inputVelocity.magnitude > 0 && !CheckTrapped())
        {
            
            animator.SetBool("isCasting", false);
            animator.SetBool("isAOE", false);
            animator.SetBool("isHealing", false);
            animator.SetBool("isPushing", false);
            animator.SetBool("isStreaming", false);
            animator.SetBool("isMine", false);
            
        }
        inputVelocity = _inputVelocity * moveSpeedMultiplier;
        if (!isAiming)
        {
            FaceDirection(_inputVelocity);
        }
    }

    public void Stream()
    {
        if (Time.time > (fireRate + lastShot) && stamina > streamCost)
        {            
            stamina -= streamCost;
            stamRechargeTimer = 0;            

            animator.SetBool("isStreaming", true);
            animator.speed = 1.0f;
            FireStream f = Instantiate(firestream, new Vector4(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
            f.transform.SetParent(transform);
            f.setPlayerNo(playerNum);
            AudioManager.instance.PlaySound("stream", f.transform.position);
            lastShot = Time.time;
        }
        else
        {
            //stamRechargeTimer = 0;
            return;
        }
    }

    public void CheckBusy()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("idle") || animator.GetCurrentAnimatorStateInfo(0).IsName("run") || animator.GetCurrentAnimatorStateInfo(0).IsName("base") || animator.GetCurrentAnimatorStateInfo(0).IsName("push") || animator.GetCurrentAnimatorStateInfo(0).IsName("heal"))
        {
            isBusy = false;
        }
        else
        {
            isBusy = true;
        }
    }

    public void Aim(Vector3 aimInput){
        isAiming = aimInput.magnitude > 0;
        if (!isAiming)
        {
            return;
        }
        Vector3 aim = aimInput;
        if (aim.magnitude > 1f)
        {
            aim.Normalize();
        }
        
        animator.SetBool("isCasting", false);
        animator.SetBool("isAOE", false);
        animator.SetBool("isHealing", false);
        animator.SetBool("isPushing", false);
        animator.SetBool("isStreaming", false);
        
        FaceDirection(aim);
    }

    void makeAOE()
    {
        animator.speed = 2.0f;
        AOESpell a = Instantiate(aoespell, transform.position, transform.rotation);
        a.setPlayerNo(playerNum);
        AudioManager.instance.PlaySound("AOE", a.transform.position);
    }

    public void AOE()
    {
        if (Time.time > (fireRate + lastShot) && stamina > aoeCost)
        {
            stamina -= aoeCost;
            stamRechargeTimer = 0;
            
            animator.SetBool("isAOE", true);
            Invoke("makeAOE", 0.9f);
            
            
            lastShot = Time.time;
            //animator.SetBool("isAOE", false);
        }
        else
        {
            //stamRechargeTimer = 0;
            return;
        }
    }

    public void Heal()
    {
        if (Time.time > (fireRate + lastShot) && stamina > healCost)
        {
            
                stamina -= healCost;
                stamRechargeTimer = 0;
            

            animator.SetBool("isHealing", true);

            HealSpell h = Instantiate(healspell, transform.position, transform.rotation);
            h.transform.parent = transform;
            h.setPlayerNo(playerNum);

            hp += healAmt;
            if (hp > maxHp)
            {
                hp = maxHp;
            }

            AudioManager.instance.PlaySound("heal", h.transform.position);
            lastShot = Time.time;
        }
        else
        {
            //stamRechargeTimer = 0;
            return;
        }
    }

    public void Block()
    {
        isBlocking = true;
        animator.SetBool("isBlocking", true);

    }

    public void StopBlock()
    {
        isBlocking = false;
        animator.SetBool("isBlocking", false);

    }
    
    public void Force()
    {
        if (Time.time > (fireRate + lastShot))
        {
            animator.SetBool("isPushing", true);

            animator.speed = 2.0f;
            PushSpell pu = Instantiate(pushspell, firingPoint.transform.position, transform.rotation);
            pu.setPlayerNo(playerNum);
            AudioManager.instance.PlaySound("push", pu.transform.position);
            lastShot = Time.time;
            //animator.SetBool("isCasting", false);
        }
    }

    public void Push (Vector3 _pushVelocity, int attackingPlayerNum){
		lastDamagedBy = attackingPlayerNum;
		float missingHPCoefficient = 1 - (hp/maxHp); // 0 for full hp, 1 for no hp.
		float pushAdjustment = 2 + 4 * (missingHPCoefficient);
		pushVelocity += _pushVelocity * pushAdjustment;
		AudioManager.instance.PlaySound ("grunt", transform.position);
	}

	public void Damage(float damage){
		hp -= damage;
		GameRoundManager.instance.AddScore (lastDamagedBy, damage);
		CheckForDeath ();
	}

	public void Damage(float damage, int attackingPlayerNum){
        GameObject h = Instantiate(hitEffect, transform.position, transform.rotation);
        Destroy(h, 2);
        lastDamagedBy = attackingPlayerNum;
        Damage(damage);
    }

	void FaceDirection(Vector3 direction){
		if (animator.GetCurrentAnimatorStateInfo(0).IsName("CastPrimary") || animator.GetCurrentAnimatorStateInfo(0).IsName("CastAOE")){
			return;
		}
		Vector3 dir = direction;
		dir = transform.InverseTransformDirection(dir);
		dir = Vector3.ProjectOnPlane(dir, new Vector3(0,0,0));
		float turnAmount = Mathf.Atan2(dir.x, dir.z);
		float turnSpeed = Mathf.Lerp(stationaryTurnSpeed, movingTurnSpeed, dir.z);
		transform.Rotate(0, turnAmount * turnSpeed * Time.deltaTime, 0);
	}

	void VerticalCollisions(){
		RaycastHit hit;
		if (Physics.Raycast(transform.position + Vector3.up * 5, Vector3.up * -20, out hit, 50, collisionMask)){
			if (hit.transform.gameObject.GetComponent<Hazard> () != null) {
				hit.transform.gameObject.GetComponent<Hazard> ().ApplyEffect (this);
				lavaBurnEffect.gameObject.SetActive (true);
			} else {
				lavaBurnEffect.gameObject.SetActive (false);
			}
		}
		//Debug.DrawRay (transform.position + Vector3.up * 5, Vector3.up * -20, Color.red);
	}

    public void ExMine()
    {
        if (Time.time > (fireRate + lastShot) && stamina > exminecost)
        {
            
            stamina -= exminecost;
            stamRechargeTimer = 0;
            

            animator.SetBool("isMine", true);

            ExMine ex = Instantiate(exmine, transform.position, transform.rotation);
            ex.setPlayerNo(playerNum);
            AudioManager.instance.PlaySound("SetMine", ex.transform.position);
            lastShot = Time.time;
        }
        else
        {
            //stamRechargeTimer = 0;
            return;
        }
    }

    public void TrapMine()
    {
        if (Time.time > (fireRate + lastShot) && stamina > exminecost)
        {
            
            stamina -= exminecost;
            stamRechargeTimer = 0;
            

            animator.SetBool("isMine", true);

            TrapMine tr = Instantiate(trapmine, transform.position, transform.rotation);
            tr.setPlayerNo(playerNum);
            AudioManager.instance.PlaySound("SetMine", tr.transform.position);
            lastShot = Time.time;
        }
        else
        {
            //stamRechargeTimer = 0;
            return;
        }
    }

    public void Freeze(float time, int attackingPlayerNum)
    {
        GameObject tr = Instantiate(trapEffect, transform.position, transform.rotation);
        animator.SetBool("isTrapped", true);
        AudioManager.instance.PlaySound("TrapSound", tr.transform.position);

        trapTime = time;
        Destroy(tr, 5);
    }
}