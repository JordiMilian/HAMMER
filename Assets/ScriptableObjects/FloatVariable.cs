using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

   [CreateAssetMenu]
   public class FloatVariable : ScriptableObject
   {
        public float Value;
        public Action OnValueSet;
        public void SetValue(float value)
        {
            Value = value;
            OnValueSet?.Invoke();
        }
   }






