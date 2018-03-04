using System;

namespace LibTypeConverter
{
    public static class TypeConverter
    {
        public static bool StrToInt(string s, ref int i, out ErrorCode errorCode)
        {
            try
            {
                i = Convert.ToInt32(s);
                errorCode = ErrorCode.Ok;
                return true;
            }
            catch (FormatException)
            {
                errorCode = ErrorCode.FormatException;

            }
            catch (OverflowException)
            {
                errorCode = ErrorCode.OverflowException;
            }

            return false;
        }
    }

    public enum ErrorCode
    {
        Ok = 0,
        FormatException = 1,
        OverflowException = 2
    }
}
