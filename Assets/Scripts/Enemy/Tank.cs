public class Tank : Enemy
{
    public Tank() : base("Tank", 50, 5, 2, 2, 10) { 
        
    }
    public override float GetAttackSpeed()
    {
        return attackSpeed;
    }

    public override int GetDamage()
    {
        return damage;
    }

    public override float GetMovingSpeed()
    {
        return movingSpeed;
    }

    /* return true if it is dead after receiving this damage. Otherwise, return false */
    public override bool ReceiveDamage(int amount)
    {
        return (currentHeath -= amount) <= 0;
    }

    public override float GetAttackRange() {
        return attackRange;
    }
}
