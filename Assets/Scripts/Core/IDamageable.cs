// /*------------------------------
// --------------------------------
// Creation Date: 2024-02-01
// Author: b22alesj
// Description: Interface for taking damage
// --------------------------------
// ------------------------------*/


namespace Game {
    namespace Core {
        public interface IDamageable
        {
            public int Health { get; set; }
            public bool Invincible { get; set; }

            public void Death();

            public void DamageTaken();

            public bool Damage(int _damage)
            {
                if (Invincible)
                {
                    return false;
                }
                
                Health -= _damage;
                if (Health <= 0)
                {
                    Death();
                    return true;
                }
                else
                {
                    DamageTaken();
                    return false;
                }
            }
        }
    }
}
