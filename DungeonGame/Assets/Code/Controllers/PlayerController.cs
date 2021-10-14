using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rig2d;

    public WeaponArchetype Weapon;
    public ArmorArchetype Armor;

    public AudioClip DashClip, AttackClip;

    private const string IdleAnimation = "Idle";
    private const string RunAnimation = "Running";
    private const string DashAnimation = "Dash";
    private const string AttackAnimation = "Attacking";

    private const float MoveSpeed = 6.5f;

    private bool isDashing = false;
    private float dashTimer = DashTime;
    private const float DashTime = 0.2f;
    private const float DashMove = 20f;
    private Vector2 dashDirection = Vector2.zero;

    private bool isAttacking = false;
    private float attackTimer = AttackTime;
    private const float AttackTime = 0.2f;

    private string currentState;

    public static PlayerController Instance;

    private Vector2 movementDirection;

    private int health = 100;

    public bool IsControllerMode = true;

    public static bool IsPaused = false;

    [SerializeField] private Slider Healthbar, Xpbar;
    [SerializeField] private TMPro.TMP_Text HpText, XpText, LvlText, GoldText;

    public class StateInfo
    {
        public int LevelHealth = 100;
        public int MaxHealth
        {
            get
            {
                return LevelHealth + Instance.Armor.Health;
            }
        }
        public int Gold = 0;
        public int Level = 1;
        public int Xp;
        public int XpTill = 10;

        public void GainXp(int gain)
        {
            Xp += gain;
            while (Xp >= XpTill)
            {
                Xp -= XpTill;
                XpTill = (int)(XpTill * 1.15f);
                Level++;
                LevelHealth = (int)(LevelHealth * 1.1f);
                Instance.health = MaxHealth;
                // TODO: Give level up bonuses
            }
        }
    }
    public StateInfo Info { get; private set; }

    public int Damage
    {
        get
        {
            return Info.Level + Weapon.Damage;
        }
    }

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void RefreshStatUI()
    {
        Healthbar.value = (float)health / (float)Info.LevelHealth;
        Xpbar.value = (float)Info.Xp / (float)Info.XpTill;
        HpText.SetText($"{health}/{Info.MaxHealth}");
        XpText.SetText($"{Info.Xp}/{Info.XpTill}");
        LvlText.SetText(Info.Level.ToString());
        GoldText.SetText(Info.Gold.ToString());
    }

    public void EquipArmor(ArmorArchetype armor)
    {
        Armor = armor;
        health = Info.MaxHealth;
        RefreshStatUI();
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rig2d = GetComponent<Rigidbody2D>();
        currentState = IdleAnimation;
        
        Info = new StateInfo();

        RefreshStatUI();
    }

    public void Hurt(int damage)
    {
        health -= damage;
        RefreshStatUI();
    }

    public void DefeatedEnemy(EnemyArchetype enemy)
    {
        Info.Gold += enemy.GoldDrop;

        if (enemy.XpDrop > 0)
        {
            GlobalHitTextController.SpawnXpText(transform.position + new Vector3(-1, 1, 0), enemy.XpDrop);
        }
        if (enemy.GoldDrop > 0)
        {
            GlobalHitTextController.SpawnGoldText(transform.position + new Vector3(1, 1, 0), enemy.GoldDrop);
        }
        Info.GainXp(enemy.XpDrop);

        RefreshStatUI();
    }

    private void ChangeState(string newState)
    {
        if (currentState == newState) return;
        currentState = newState;

        animator.Play(newState);
    }

    void Update()
    {
        if (IsPaused) return;
        HandleMovementInput();
        HandleAttacking();
    }

    private void FixedUpdate()
    {
        if (IsPaused) return;
        HandleMovementAction();
    }

    void HandleAttacking()
    {
        if (isAttacking)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0)
            {
                attackTimer = AttackTime;
                isAttacking = false;
                ChangeState(movementDirection.magnitude > 0 ? RunAnimation : IdleAnimation);
            }
        }
        if (Input.GetMouseButton(0) && !isAttacking)
        {
            isAttacking = true;
            ChangeState(AttackAnimation);

            SoundEffectController.Instance.PlaySound(AttackClip);

            switch (Weapon.FireMode)
            {
                case WeaponArchetype.FiringMethod.Single:
                    {
                        FireBullet();
                        break;
                    }
                case WeaponArchetype.FiringMethod.Double:
                    {
                        FireBullet(-0.75f);
                        FireBullet( 0.75f);
                        break;
                    }
                case WeaponArchetype.FiringMethod.Triple:
                    {
                        FireBullet(-1);
                        FireBullet( 0);
                        FireBullet( 1);
                        break;
                    }
                case WeaponArchetype.FiringMethod.Crazy:
                    {
                        for (int i = 0; i < Random.Range(1, 3); i++)
                        {
                            FireBullet(Random.Range(-1, 1));
                        }
                        break;
                    }
            }
        }
    }

    void FireBullet(float hOffset = 0)
    {
        GameObject go = Instantiate(Weapon.Bullet.Prefab);

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 localPos = new Vector2(transform.position.x, transform.position.y);
        Vector2 mouseDirection = (mousePosition - localPos).normalized;

        float l = 1.5f;
        Vector2 k_pos = localPos + mouseDirection * l;
        float theta = Mathf.Asin((k_pos.y - localPos.y)/l);

        Vector2 v_pos = new Vector2(localPos.x, localPos.y + (theta == 0 ? 0 : (l/Mathf.Sin(theta))));

        Vector2 f_dir = (v_pos - k_pos).normalized * hOffset;

        Vector2 f_pos = k_pos + f_dir;

        go.transform.position = f_pos;

        BulletController ctr = go.GetComponent<BulletController>();
        ctr.Direction = mouseDirection;

        ctr.Archetype = Weapon.Bullet;
    }

    void HandleMovementInput()
    {
        movementDirection = Vector2.zero;
        if (IsControllerMode)
        {
            movementDirection.x = Input.GetAxis("Horizontal");
            movementDirection.y = Input.GetAxis("Vertical");
        }
        else
        {
            if (Input.GetKey(KeyCode.W))
            {
                movementDirection.y += 1;
            }
            if (Input.GetKey(KeyCode.S))
            {
                movementDirection.y -= 1;
            }
            if (Input.GetKey(KeyCode.A))
            {
                movementDirection.x -= 1;
            }
            if (Input.GetKey(KeyCode.D))
            {
                movementDirection.x += 1;
            }
        }
        movementDirection.Normalize();
        if (isDashing)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0)
            {
                dashTimer = DashTime;
                isDashing = false;
                ChangeState(RunAnimation);
            }
            else
            {
                ChangeState(DashAnimation);
            }
        }
        else if (movementDirection.magnitude > 0)
        {
            spriteRenderer.flipX = movementDirection.x < 0;
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                isDashing = true;
                dashDirection = movementDirection;
                SoundEffectController.Instance.PlaySound(DashClip);
            }
            else
            {
                if (!isAttacking)
                {
                    ChangeState(RunAnimation);
                }
            }
        }
        else if (!isAttacking)
        {
            ChangeState(IdleAnimation);
        }
    }

    private void HandleMovementAction()
    {
        if (isDashing)
        {
            rig2d.MovePosition(transform.position + (Vector3)(dashDirection * DashMove * Time.deltaTime));
        }
        else if (movementDirection.magnitude > 0)
        {
            rig2d.MovePosition(transform.position + (Vector3)(movementDirection * MoveSpeed * Time.deltaTime));
        }
    }
}
