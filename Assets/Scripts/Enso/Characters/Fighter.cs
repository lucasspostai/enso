using Enso.CombatSystem;
using Enso.Enums;
using Framework;
using UnityEngine;

namespace Enso.Characters
{
    [RequireComponent(typeof(HealthSystem))]
    [RequireComponent(typeof(BalanceSystem))]
    [RequireComponent(typeof(CharacterAnimationHandler))]
    public abstract class Fighter : MonoBehaviour
    {
        private HealthSystem healthSystem;
        private BalanceSystem balanceSystem;

        [SerializeField] protected FighterProperties BaseProperties;
        
        public CharacterCollisions Collisions;
        public CharacterMovementController MovementController;
        public Team FighterTeam;

        [HideInInspector] public CharacterAnimationHandler AnimationHandler;
        [HideInInspector] public Transform Target;

        protected virtual void Awake()
        {
            healthSystem = GetComponent<HealthSystem>();
            balanceSystem = GetComponent<BalanceSystem>();
            AnimationHandler = GetComponent<CharacterAnimationHandler>();
        }

        protected virtual void Start()
        {
            MovementController.SetFighter(this);
        }

        public HealthSystem GetHealthSystem()
        {
            if(!healthSystem)
                healthSystem = GetComponent<HealthSystem>();
                
            return healthSystem;
        }
        
        public BalanceSystem GetBalanceSystem()
        {
            if(!balanceSystem)
                balanceSystem = GetComponent<BalanceSystem>();
                
            return balanceSystem;
        }

        public FighterProperties GetBaseProperties()
        {
            return BaseProperties;
        }

        public virtual void EnterCombatWith(Fighter fighter)
        {
            Target = fighter.transform;

            fighter.GetHealthSystem().Death += ExitCombat;
        }

        public void ExitCombat()
        {
            Target = null;
        }
    }
}
