using System;
using UnityEngine;

namespace VaporObservables
{
    public class QuaternionObservable : ObservableField
    {
        public static implicit operator Quaternion(QuaternionObservable f) => f.Value;

        public Quaternion Value { get; protected set; }
        public event Action<QuaternionObservable, float> ValueChanged;

        public QuaternionObservable(int fieldID, bool saveValue, Quaternion value) : base(fieldID, saveValue)
        {
            Type = ObservableFieldType.Quaternion;
            Value = value;
        }

        #region - Setters -
        internal bool InternalSet(Quaternion value)
        {
            if (Value != value)
            {
                var oldValue = Value;
                Value = value;
                ValueChanged?.Invoke(this, Quaternion.Angle(oldValue, Value));
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Set(Quaternion value)
        {
            return InternalSet(value);
        }
        #endregion

        #region - Saving -
        public override SavedObservable Save()
        {
            return new SavedObservable(FieldID, Type, $"{Value.x},{Value.y},{Value.z},{Value.w}");
        }

        public override ObservableField Clone()
        {
            return new QuaternionObservable(FieldID, SaveValue, Value);
        }
        #endregion
    }
}
