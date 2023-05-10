using System;
using UnityEngine;

namespace VaporObservables
{
    public class ColorObservable : ObservableField
    {
        public static implicit operator Color(ColorObservable f) => f.Value;

        public Color Value { get; protected set; }
        public event Action<ColorObservable> ValueChanged;

        public ColorObservable(int fieldID, bool saveValue, Color value) : base(fieldID, saveValue)
        {
            Type = ObservableFieldType.Color;
            Value = value;
        }

        #region - Setters -
        internal bool InternalSet(Color value)
        {
            if (Value != value)
            {
                Value = value;
                ValueChanged?.Invoke(this);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Set(Color value)
        {
            return InternalSet(value);
        }
        #endregion

        #region - Saving -
        public override SavedObservable Save()
        {
            return new SavedObservable(FieldID, Type, $"{Value.r},{Value.g},{Value.b},{Value.a}");
        }

        public override ObservableField Clone()
        {
            return new ColorObservable(FieldID, SaveValue, Value);
        }
        #endregion
    }
}
