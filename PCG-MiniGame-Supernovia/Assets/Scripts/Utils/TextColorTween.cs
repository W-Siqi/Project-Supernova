using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace PCG {
    public class TextColorTween : ColorTween {
        [SerializeField]
        private TextMeshProUGUI TMP;
        protected override void SetColor(Color color) {
            TMP.color = color;
        }
    }
}
