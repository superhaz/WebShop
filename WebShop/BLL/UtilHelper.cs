using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using WebShop.BLL.Interfaces;

namespace WebShop.BLL
{
    public class UtilHelper : IUtilHelper
    {
        public enum Roles { Users = 1, Administrators, SuperAdmins }

        public enum ControlMode { DefaultMode = 1, EditMode, NewEditMode }

        #region Rounding price

        public string RoundPrice(string price)
        {
            try
            {
                double priceDouble = Convert.ToDouble(price);
                price = RoundPrice(priceDouble).ToString();
                // replace ',' with '.'
                return price.Replace(",", ".");
            }
            catch (Exception)
            {
                return "";
            }

        }

        public double RoundPrice(double price)
        {
            double roundPrice = price - Math.Round(price, 0);
            if (roundPrice == 0)
                return Math.Round(price, 0);
            else
                return Math.Round(price, 2);

        }

        #endregion

        #region Random

        public int RandomNumber(int max)
        {
            // get millisecond - used as seed
            int seedMilliSecond = DateTime.Now.Millisecond;
            // create an instance and get a random quota
            Random random = new Random(seedMilliSecond);
            double randomQuota =  random.NextDouble();
            // generate the random value
            double randomValueDouble = max * randomQuota;
            // round value to nearest integer
            int randomValue = Convert.ToInt32(Math.Round(randomValueDouble, 0)); // 0= means that double should be rounded with no decimals
            return randomValue;
        } 

        #endregion

        #region Convert Size KB/MB/GB

        /// <summary>
        /// Converts a size from byte to either KB, MB, GB or TB
        /// </summary>
        /// <param name="sizeString">the size in byte</param>
        /// <returns></returns>
        public string ConvertSizeToKBOrMBOrGBOrTB(string sizeString)
        {
            if (!string.IsNullOrEmpty(sizeString))
            {
                try
                {
                    Double size = Convert.ToDouble(sizeString);

                    if (Math.Round(size / 1000) < 1000)
                        return Math.Round(size / 1000, 1).ToString() + " kb";  // return kb if size is less than 1MB
                    else if (Math.Round(size / 1000000) < 1000)
                        return Math.Round(size / 1000000, 2).ToString() + " MB";  // return MB if size is less than 1GB
                    else if (Math.Round(size / 1000000000) < 1000)
                        return Math.Round(size / 1000000000, 3).ToString() + " GB";  // return GB if size is less than 1TB
                    else
                        return Math.Round(size / 1000000000000, 4).ToString() + " TB";  // return GB if size is less than 1TB
                }
                catch (Exception ex)
                {
                    throw new Exception("An exception occured when trying to convert size. Input size: " + sizeString, ex);
                }

            }

            return "";
        }

        #endregion

        #region IsInteger & IsNumeric

        public bool IsNumeric(string stringToTest)
        {
            double newVal;
            return double.TryParse(stringToTest, NumberStyles.Any, NumberFormatInfo.InvariantInfo, out newVal);
        }

        public bool IsInteger(string stringToTest)
        {
            int newVal;
            return int.TryParse(stringToTest, NumberStyles.Any, NumberFormatInfo.InvariantInfo, out newVal);
        }

        #endregion

        #region string convertion

        public string ParseNewLine(object text)
        {
            try
            {
                if (text.ToString().Length > 0)
                {
                    return text.ToString().Replace("\n", "<br />");
                }
                else
                    return "";
            }
            catch (Exception)
            {

                return "";
            }
        }

        public string URLEncode(string s)
        {
            return s.Replace(" ", "%20").Replace("ö", "%F6").Replace("Ö", "%D6").Replace("å", "%E5").Replace("Å", "%C5").Replace("ä", "%E4").Replace("Ä", "%C4");
        }

        #endregion

        #region string manipulation

        /// <summary>
        /// Finds the occurence-index of the last searchString in a string
        /// </summary>
        /// <param name="str">The string that contains a specified search string.</param>
        /// <param name="searchString">The word or token that should be found in the string</param>
        /// <returns></returns>
        public int FindLastIndexOf(string str, string searchString)
        {
            int start = 0;
            int end = str.Length;
            int at = 0;
            int lastFoundIndex = -1;
            //
            while ((start <= end) && (at > -1))
            {
                // get the current index position 
                at = str.IndexOf(searchString, start);

                if (at == -1)
                    break;

                lastFoundIndex = at;
                // set the new start position
                start = at + 1;
            }

            return lastFoundIndex;
        }

        public string RemoveLastSearchString(string str, string searchString)
        {
            int index = FindLastIndexOf(str, searchString);

            if (index != -1)
                str = str.Remove(FindLastIndexOf(str, searchString));
            return str;
        }

        #endregion

    }
}
