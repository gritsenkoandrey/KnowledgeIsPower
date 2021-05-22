using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.UI
{
    public class ActorUI : MonoBehaviour
    {
        [SerializeField] private HpBar _hpBar = null;
        
        private IHealth _health;

         private void Start()
         {
             IHealth health = GetComponent<IHealth>();
        
             if (health != null) 
                 Construct(health);
         }

        private void OnDestroy()
        {
            if (_health != null) 
                _health.HealthChanged -= UpdateHpBar;
        }

        public void Construct(IHealth health)
        {
            _health = health;
            _health.HealthChanged += UpdateHpBar;
        }

        private void UpdateHpBar() => 
            _hpBar.SetValue(_health.Current, _health.Max);
    }
}