using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Elements
{
    public class HpBar : MonoBehaviour
    {
        [SerializeField] private Image _imageCurrent = null;

        public void SetValue(float current, float max) => 
            _imageCurrent.fillAmount = current / max;
    }
}