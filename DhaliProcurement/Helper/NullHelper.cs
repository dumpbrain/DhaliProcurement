using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DhaliProcurement.Helper
{
    public static class NullHelper
    {
        public static string DateToString(object obj)
        {
            if (obj == null)
            {
                return "";
            }

            if ((DateTime)obj == DateTime.MinValue)
            {
                return "";
            }

            return ((DateTime)obj).ToString("dd/MM/yyyy");
        }

        public static string ObjectToString(object obj)
        {
            if (obj == null)
            {
                return "";
            }

            if (obj.ToString().Trim() == "")
            {
                return "";
            }


            return Convert.ToString(obj);
        }

        public static int ToIntNum(object objVal)
        {

            if(objVal == null) {
                return 0;
            }

            if (objVal.ToString().Trim() == "")
            {
                return 0;
            }
            
            return Convert.ToInt32(objVal);

        }


    }
}