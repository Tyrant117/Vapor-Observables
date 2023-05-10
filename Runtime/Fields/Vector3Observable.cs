using System;
using UnityEngine;

namespace VaporObservables
{
    [Serializable]
    public class Vector3Observable : ObservableField
    {
        public static implicit operator Vector3(Vector3Observable f) => f.Value;

        public Vector3 Value { get; protected set; }
        public event Action<Vector3Observable, Vector3> ValueChanged;

        public Vector3Observable(int fieldID, bool saveValue, Vector3 value) : base(fieldID, saveValue)
        {
            Type = ObservableFieldType.Vector3;
            Value = value;
        }

        #region - Setters -
        internal bool InternalSet(Vector3 value)
        {
            if (Value != value)
            {
                var old = Value;
                Value = value;
                ValueChanged?.Invoke(this, Value - old);
                return true;
            }
            else
            {
                return false;
            }
        }

        internal bool InternalModify(Vector3 value, ObservableModifyType type)
        {
            return type switch
            {
                ObservableModifyType.Set => InternalSet(value),
                ObservableModifyType.Add => InternalSet(Value + value),
                ObservableModifyType.Multiplier => InternalSet(_Multiply(Value, value)),
                ObservableModifyType.PercentAdd => InternalSet(Value + _Multiply(Value, value)),
                _ => false,
            };

            static Vector3 _Multiply(Vector3 lhs, Vector3 rhs) => new(lhs.x * rhs.x, lhs.y * rhs.y, lhs.z * rhs.z);
        }

        public bool Set(Vector3 value)
        {
            return InternalSet(value);
        }

        public bool Modify(float multiplier)
        {
            return InternalSet(Value * multiplier);
        }

        public bool Modify(Vector3 value, ObservableModifyType type)
        {
            return InternalModify(value, type);
        }
        #endregion

        #region - Saving -
        public override SavedObservable Save()
        {
            return new SavedObservable(FieldID, Type, $"{Value.x},{Value.y},{Value.z}");
        }

        public override ObservableField Clone()
        {
            return new Vector3Observable(FieldID, SaveValue, Value);
        }
        #endregion
    }
}