using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VaporObservables
{
    public class UIntObservable : ObservableField
    {
        public static implicit operator uint(UIntObservable f) => f.Value;

        public uint Value { get; protected set; }
        public event Action<UIntObservable> ValueChanged;

        public UIntObservable(int fieldID, bool saveValue, uint value) : base(fieldID, saveValue)
        {
            Type = ObservableFieldType.UInt32;
            Value = value;
        }

        #region - Setters -
        internal bool InternalSet(uint value)
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

        public bool Set(uint value)
        {
            return InternalSet(value);
        }
        #endregion

        #region - Saving -
        public override SavedObservable Save()
        {
            return new SavedObservable(FieldID, Type, Value.ToString());
        }

        public override ObservableField Clone()
        {
            return new UIntObservable(FieldID, SaveValue, Value);
        }
        #endregion
    }
}
