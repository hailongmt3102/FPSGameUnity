public abstract class Enemy {
    protected int maxHeath;
    public int currentHeath;
    public string type;

    protected int damage;
    protected float movingSpeed;
    protected float attackSpeed;
    protected float attackRange;

    protected Enemy(string type, int maxHeath, int damage, float movingSpeed, float attackSpeed, float attackRange) {
        this.maxHeath = maxHeath;
        currentHeath = maxHeath;
        this.type = type;
        this.damage = damage;
        this.movingSpeed = movingSpeed;
        this.attackSpeed = attackSpeed;
        this.attackRange = attackRange;
    }

    abstract public bool ReceiveDamage(int amount);
    abstract public float GetMovingSpeed();
    abstract public float GetAttackSpeed();
    abstract public int GetDamage();
    abstract public float GetAttackRange();
}
