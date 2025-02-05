﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PCG {
    public class ImageGUIColorTween : ColorTween {
        [SerializeField]
        private Image image;

        protected override void SetColor(Color color) {
            image.color = color;
        }
    }
}