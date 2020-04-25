using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ValueBar : MonoBehaviour
    {
        public Color fillColor;
        public Color backgroundColor;
        public float value;
        public float maxValue;

        public Image background;
        public Image fill;
        public Text text;
        private Slider _slider;

        // Start is called before the first frame update
        void Start()
        {
            _slider = GetComponent<Slider>();
            fill.color = fillColor;
            background.color = backgroundColor;
            SetValue(value);
            SetMaxValue(maxValue);
        }


        private string FormatBarValueString(float value, float maxValue)
        {
            var leftPart = $"{(int) (Mathf.Ceil(value))}".PadLeft(4);
            var rightPart = $"{(int) maxValue}".PadRight(4);
            return $"{leftPart} / {rightPart}";
        }

        public void SetValue(float v, float mv)
        {
            SetMaxValue(mv);
            SetValue(v);
            
        }

        public void SetValue(float v)
        {
            value = v;
            _slider.value = value;
            text.text = FormatBarValueString(value, maxValue);
        }

        public void SetMaxValue(float mv)
        {
            maxValue = mv;
            _slider.maxValue = mv;

            if (maxValue > 0)
            {
                gameObject.SetActive(true);
                text.text = FormatBarValueString(value, maxValue);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}