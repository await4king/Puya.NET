using System;
using System.Collections.Generic;
using System.Text;

namespace Puya.Base
{
    public static class NewLanguageConstructs
    {
        public static T Try<T>(Func<T> fn, out Exception e, T @default = default(T))
        {
            T result;

            try
            {
                result = fn();
                e = null;
            }
            catch (Exception ex)
            {
                result = @default;
                e = ex;
            }

            return result;
        }
        public static T Try<T>(Func<T> fn, T @default = default(T))
        {
            T result;

            try
            {
                result = fn();
            }
            catch
            {
                result = @default;
            }

            return result;
        }
        public static Exception TryOut(Action act)
        {
            Exception result = null;

            try
            {
                act();
            }
            catch (Exception e)
            {
                result = e;
            }

            return result;
        }
        public static bool Try(Action act)
        {
            var result = false;

            try
            {
                act();

                result = true;
            }
            catch { }

            return result;
        }
        public static bool Try<T>(Action<T> act, T arg)
        {
            var result = false;

            try
            {
                act(arg);

                result = true;
            }
            catch { }

            return result;
        }
        public static bool Try<T1, T2>(Action<T1, T2> act, T1 arg1, T2 arg2)
        {
            var result = false;

            try
            {
                act(arg1, arg2);

                result = true;
            }
            catch { }

            return result;
        }
        public static bool Try<T1, T2, T3>(Action<T1, T2, T3> act, T1 arg1, T2 arg2, T3 arg3)
        {
            var result = false;

            try
            {
                act(arg1, arg2, arg3);

                result = true;
            }
            catch { }

            return result;
        }
        public static bool Try<T1, T2, T3, T4>(Action<T1, T2, T3, T4> act, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            var result = false;

            try
            {
                act(arg1, arg2, arg3, arg4);

                result = true;
            }
            catch { }

            return result;
        }
        public static bool Try(Action act, out Exception e)
        {
            var result = false;

            try
            {
                act();

                e = null;

                result = true;
            }
            catch (Exception ex)
            {
                e = ex;
            }

            return result;
        }
    }
}
