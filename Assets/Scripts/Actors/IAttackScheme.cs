namespace Assets.Scripts
{
    public interface IAttackScheme
    {
        public void Apply(Attacker attacker);
        public void Cancel(Attacker attacker);
    }
}