﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace CStrawberry3D.component
{
    class DirectionalLightComponent:Component
    {
        private Vector4 _diffuseColor = new Vector4(1,1,1,1);
        public Vector4 diffuseColor
        {
            get
            {
                return _diffuseColor;
            }
            set
            {
                _diffuseColor = value;
            }
        }
        private Vector4 _specularColor = new Vector4(1,1,1,1);
        public Vector4 specularColor
        {
            get
            {
                return _specularColor;
            }
            set
            {
                _specularColor = value;
            }
        }

        public DirectionalLightComponent():base()
        {
            _componentName = "DirectionalLightComponent";
        }
    }
}