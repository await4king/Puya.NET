using System;
using System.Numerics;
using System.Threading;

namespace Puya.Base
{
    public static class TypeHelper
    {
        #region Type Properties
        public static Type TypeOfInt16 { get; private set; }
        public static Type TypeOfShort { get { return TypeOfInt16; } }
        public static Type TypeOfInt32 { get; private set; }
        public static Type TypeOfInt { get { return TypeOfInt32; } }
        public static Type TypeOfInt64 { get; private set; }
        public static Type TypeOfLong { get { return TypeOfInt64; } }
        public static Type TypeOfUInt16 { get; private set; }
        public static Type TypeOfUShort { get { return TypeOfUInt16; } }
        public static Type TypeOfUInt32 { get; private set; }
        public static Type TypeOfUInt { get { return TypeOfUInt32; } }
        public static Type TypeOfUInt64 { get; private set; }
        public static Type TypeOfULong { get { return TypeOfUInt64; } }
        public static Type TypeOfSingle { get; private set; }
        public static Type TypeOfFloat { get { return TypeOfSingle; } }
        public static Type TypeOfDouble { get; private set; }
        public static Type TypeOfDecimal { get; private set; }
        public static Type TypeOfByte { get; private set; }
        public static Type TypeOfSByte { get; private set; }
        public static Type TypeOfChar { get; private set; }
        public static Type TypeOfString { get; private set; }
        public static Type TypeOfBool { get; private set; }
        public static Type TypeOfBigInteger { get; private set; }
        public static Type TypeOfGuid { get; private set; }
        public static Type TypeOfDateTime { get; private set; }
        public static Type TypeOfDateTimeOffset { get; private set; }
        public static Type TypeOfTimeSpan { get; private set; }
        public static Type TypeOfByteArray { get; private set; }
        public static Type TypeOfCharArray { get; private set; }
        public static Type TypeOfNullable { get; private set; }
        public static Type TypeOfObject { get; private set; }
        public static Type TypeOfIListOfT { get; private set; }
        public static Type TypeOfIEnumerableOfT { get; private set; }
        public static Type TypeOfIEnumerable { get; private set; }
        public static Type TypeOfException { get; private set; }

        public static Type TypeOfNullableInt16 { get; private set; }
        public static Type TypeOfNullableShort { get { return TypeOfNullableInt16; } }
        public static Type TypeOfNullableInt32 { get; private set; }
        public static Type TypeOfNullableInt { get { return TypeOfNullableInt32; } }
        public static Type TypeOfNullableInt64 { get; private set; }
        public static Type TypeOfNullableLong { get { return TypeOfNullableInt64; } }
        public static Type TypeOfNullableUInt16 { get; private set; }
        public static Type TypeOfNullableUShort { get { return TypeOfNullableUInt16; } }
        public static Type TypeOfNullableUInt32 { get; private set; }
        public static Type TypeOfNullableUInt { get { return TypeOfNullableUInt32; } }
        public static Type TypeOfNullableUInt64 { get; private set; }
        public static Type TypeOfNullableULong { get { return TypeOfNullableUInt64; } }
        public static Type TypeOfNullableSingle { get; private set; }
        public static Type TypeOfNullableFloat { get { return TypeOfNullableSingle; } }
        public static Type TypeOfNullableDouble { get; private set; }
        public static Type TypeOfNullableDecimal { get; private set; }
        public static Type TypeOfNullableByte { get; private set; }
        public static Type TypeOfNullableChar { get; private set; }
        public static Type TypeOfNullableSByte { get; private set; }
        public static Type TypeOfNullableBool { get; private set; }
        public static Type TypeOfNullableBigInteger { get; private set; }
        public static Type TypeOfNullableDateTime { get; private set; }
        public static Type TypeOfNullableDateTimeOffset { get; private set; }
        public static Type TypeOfNullableTimeSpan { get; private set; }
        #endregion
        static TypeHelper()
        {
            #region initialize Type Properties
            TypeOfInt16 = typeof(System.Int16);
            TypeOfInt32 = typeof(System.Int32);
            TypeOfInt64 = typeof(System.Int64);
            TypeOfUInt16 = typeof(System.UInt16);
            TypeOfUInt32 = typeof(System.UInt32);
            TypeOfUInt64 = typeof(System.UInt64);

            TypeOfSingle = typeof(System.Single);
            TypeOfDouble = typeof(System.Double);
            TypeOfDecimal = typeof(System.Decimal);
            TypeOfByte = typeof(System.Byte);
            TypeOfSByte = typeof(System.SByte);
            TypeOfBool = typeof(System.Boolean);
            TypeOfBigInteger = typeof(System.Numerics.BigInteger);
            TypeOfChar = typeof(System.Char);
            TypeOfString = typeof(System.String);
            TypeOfGuid = typeof(System.Guid);
            TypeOfDateTime = typeof(System.DateTime);
            TypeOfDateTimeOffset = typeof(System.DateTimeOffset);
            TypeOfTimeSpan = typeof(System.TimeSpan);
            TypeOfByteArray = typeof(System.Byte[]);
            TypeOfCharArray = typeof(System.Char[]);
            TypeOfNullable = typeof(System.Nullable<>);
            TypeOfObject = typeof(object);
            TypeOfIListOfT = typeof(System.Collections.Generic.IList<>);
            TypeOfIEnumerable = typeof(System.Collections.IEnumerable);
            TypeOfIEnumerableOfT = typeof(System.Collections.Generic.IEnumerable<>);
            TypeOfException = typeof(System.Exception);

            TypeOfNullableInt16 = typeof(System.Nullable<System.Int16>);
            TypeOfNullableInt32 = typeof(System.Nullable<System.Int32>);
            TypeOfNullableInt64 = typeof(System.Nullable<System.Int64>);
            TypeOfNullableUInt16 = typeof(System.Nullable<System.UInt16>);
            TypeOfNullableUInt32 = typeof(System.Nullable<System.UInt32>);
            TypeOfNullableUInt64 = typeof(System.Nullable<System.UInt64>);

            TypeOfNullableSingle = typeof(System.Nullable<System.Single>);
            TypeOfNullableDouble = typeof(System.Nullable<System.Double>);
            TypeOfNullableDecimal = typeof(System.Nullable<System.Decimal>);
            TypeOfNullableByte = typeof(System.Nullable<System.Byte>);
            TypeOfNullableSByte = typeof(System.Nullable<System.SByte>);
            TypeOfNullableBool = typeof(System.Nullable<System.Boolean>);
            TypeOfNullableBigInteger = typeof(System.Nullable<System.Numerics.BigInteger>);
            TypeOfNullableChar = typeof(System.Nullable<System.Char>);
            TypeOfNullableDateTime = typeof(System.Nullable<System.DateTime>);
            TypeOfNullableDateTimeOffset = typeof(System.Nullable<System.DateTimeOffset>);
            TypeOfNullableTimeSpan = typeof(System.Nullable<System.TimeSpan>);
            #endregion
        }
        public static TAbstraction EnsureInitialized<TAbstraction, TConcretion>(ref TAbstraction value, bool threadSafe = false, object syncLock = null)
            where TConcretion : TAbstraction, new()
        {
            if (value == null)
            {
                var _lock = syncLock ?? AppDomain.CurrentDomain;
                bool _lockTaken = false;

                if (threadSafe)
                {
                    Monitor.Enter(_lock, ref _lockTaken);
                }

                try
                {
                    value = new TConcretion();
                }
                finally
                {
                    if (threadSafe && _lockTaken)
                    {
                        Monitor.Exit(_lock);
                    }
                }
            }

            return value;
        }
        public static TAbstraction EnsureInitialized<TAbstraction>(ref TAbstraction value, Func<TAbstraction> fnCreate, bool threadSafe = false, object syncLock = null)
        {
            if (value == null)
            {
                var _lock = syncLock ?? AppDomain.CurrentDomain;
                bool _lockTaken = false;

                if (threadSafe)
                {
                    Monitor.Enter(_lock, ref _lockTaken);
                }

                try
                {
                    value = fnCreate();
                }
                finally
                {
                    if (threadSafe && _lockTaken)
                    {
                        Monitor.Exit(_lock);
                    }
                }
            }

            return value;
        }
    }
}
