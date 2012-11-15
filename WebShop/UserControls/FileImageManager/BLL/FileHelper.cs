using System;
using System.Collections.Generic;
using System.Web;
using System.Text;
using System.IO;
using System.Data.SqlClient;
using WebShop.UserControls.FileImageManager.DAL;
using System.Data;
using System.Globalization;
using System.Drawing.Imaging;


namespace WebShop.UserControls.FileImageManager.BLL
{
    public class FileHelper
    {

        #region Class variables

        // A complete folder url path ex: D:\My Projects\WebShop\WebShop\WebShop\ProductImages
        // The folder Url where the file exists. Ex "~/ProductImages"
        private static string folderUrl;
        private static string folderUrlRoot;

        public static string FolderUrl
        {
            get
            {
                if (string.IsNullOrEmpty(folderUrl))
                    throw new Exception("The FolderUrl property tag is missing or has an empty value");
                return folderUrl;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new Exception("FolderUrl cannot be null or empty string.");

                // check if '/' token needs to be added
                if (value.Substring(value.Length - 1) != "/")
                {
                    folderUrl = value + "/";
                }
                else
                {
                    folderUrl = value;
                }
            }
        }

        public static string FolderUrlRoot
        {
            get
            {
                if (string.IsNullOrEmpty(folderUrlRoot))
                    throw new Exception("The FolderUrl property tag is missing or has an empty value");
                return folderUrlRoot;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new Exception("FolderUrl cannot be null or empty string.");

                // check if '/' token needs to be added
                if (value.Substring(value.Length - 1) != "\\")
                {
                    folderUrlRoot = value + "\\";
                }
                else
                {
                    folderUrlRoot = value;
                }
            }
        }

        // The file name. Ex book.png
        private static string filename;

        #endregion

        #region Initialization

        /// <summary>
        /// Initializes the class
        /// </summary>
        /// <param name="filename">The file name. Ex book.png</param>
        private static void Init(string filename)
        {
            if (string.IsNullOrEmpty(filename))
                throw new Exception("File name cannot be null or empty string.");

            FileHelper.filename = filename;
        }

        #endregion

        #region Delete file(s)

        /// <summary>
        /// Deletes a file in the specified folder
        /// </summary>
        /// <param name="filename">The file name. Ex book.png</param>
        public static bool DeleteFile(string filename)
        {
            try
            {
                // check if file exists before deleting
                FileInfo file = new FileInfo(folderUrlRoot + filename);
                if (file.Exists)
                {
                    File.Delete(folderUrlRoot + filename);   // delete file
                }
            }
            catch (Exception)
            {
                // make some loggin
                //throw new Exception("An exception occured when trying to delete the file: " + filename + ". ", ex);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Cleanes up files that have been marked for deletion
        /// </summary>
        public static void CleanupMarkedFilesForDeletion()
        {
            DataTable table = FileData.GetMarkedFilesForDeletion();

            foreach (DataRow row in table.Rows)
            {
                bool result = DeleteFile(row["FileName"].ToString());

                if (result)
                    FileData.DeleteFile(row["FileID"].ToString());
            }
        }

        /// <summary>
        /// Deletes all files connected to a Product item
        /// DataTable table = FileProductData.GetFilesByProductID(productID);
        /// </summary>
        public static void DeleteItemFiles(string itemID, DataTable table)
        {
            foreach (DataRow row in table.Rows)
            {
                bool result = DeleteFile(row["FileName"].ToString());

                if (result)
                    FileData.DeleteFile(row["FileID"].ToString());
            }
        }

        /// <summary>
        /// Deletes all files in a specified folderUrlRoot
        /// </summary>
        private void DeleteAllExistingFiles()
        {
            // Get all files in directory 
            string[] files = Directory.GetFiles(folderUrlRoot);
            // traverse the files
            foreach (string str in files)
            {
                try
                {
                    // check if file exists before deleting
                    FileInfo fileToDelete = new FileInfo(str);
                    if (fileToDelete.Exists)
                    {
                        File.Delete(str);   // delete file
                    }
                }
                catch (Exception)
                {
                    //throw ex; // could not delete file since it is in use
                }
            }
        }

        #endregion

        #region Make filename Unique

        /// <summary>
        /// Generates a unique fileName, by checking if it already exists and if so adds unique tail.
        /// </summary>
        /// <param name="filename">The file name. Ex book.png</param>
        /// <returns></returns>
        public static string GetUniqueFileName(string filename)
        {
            Init(filename);
            return Rename();
        }

        /// <summary>
        /// Renames a file name to be unique by adding a tail
        /// </summary>
        /// <returns></returns>
        private static string Rename()
        {
            // create info concerning file
            FileInfo file = new FileInfo(folderUrlRoot + filename);

            // check if file exists
            if (file.Exists)
            {
                // rename the file
                AddTail("_cc");
                Rename();  // recursive invocation if there are many files with same name
            }
            return filename;
        }

        /// <summary>
        /// Renames the filename by adding a number or increasing a number in the end of the filename. Ex MyFile_cc1.png
        /// </summary>
        /// <param name="tailString">The string mask that should be used when making the filename unique. Ex: "_cc" --> MyFile_cc1</param>
        /// <returns>renamed filename</returns>
        private static string AddTail(string tailString)
        {
            // find the index of '.' (dot) ex: myfile.png
            int dotTokenIndex = filename.IndexOf('.', filename.Length - 5);
            int lastFoundIndex = FindLastIndexOf(filename, tailString);
            // get the index after tailstring
            int tailOflastFoundIndex = lastFoundIndex + tailString.Length;
            int currentNumber;

            if (lastFoundIndex == -1)    // ie no tail-string found in current filename
            {
                // add a new tail before the '.'
                filename = filename.Insert(dotTokenIndex, tailString + "1");
            }

            else if (dotTokenIndex == tailOflastFoundIndex) // if they are same index then only add a tail number
            {
                // add a number at the tail of '_cc'
                filename = filename.Insert(tailOflastFoundIndex, "1");
            }
            // try to parse the characters between '_cc' and '.'
            else if (int.TryParse(filename.Substring(tailOflastFoundIndex, dotTokenIndex - tailOflastFoundIndex), out currentNumber))
            {
                // increase number
                currentNumber++;
                // change the filename to include the increased number
                filename = filename.Substring(0, lastFoundIndex) + tailString + currentNumber.ToString() + filename.Substring(dotTokenIndex, filename.Length - dotTokenIndex);
                //// replace the old number with increased number
                //oldNumber = filename.Substring(tailOflastFoundIndex, dotTokenIndex - tailOflastFoundIndex);
                //filename = filename.Replace(filename, currentNumber.ToString());
            }
            else // ie found a tail-string but it was not in correct format. Ex: My_ccFile.png => My_ccFile_cc1.png
            {
                // add a new tail before the '.'
                filename = filename.Insert(dotTokenIndex, tailString + "1");
            }

            return filename;
        }

        /// <summary>
        /// Finds the occurence-index of the last searchString in a string
        /// </summary>
        /// <param name="str">The string that contains a specified search string.</param>
        /// <param name="searchString">The word or token that should be found in the string</param>
        /// <returns></returns>
        private static int FindLastIndexOf(string str, string searchString)
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

        public static string RemoveLastSearchString(string str, string searchString)
        {
            int index = FindLastIndexOf(str, searchString);

            if (index != -1)
                str = str.Remove(FindLastIndexOf(str, searchString));
            return str;
        }

        #endregion

        #region Convert

        /// <summary>
        /// Converts a size from byte to either KB, MB, GB or TB
        /// </summary>
        /// <param name="sizeString">the size in byte</param>
        /// <returns></returns>
        public static string ConvertSizeToKBOrMBOrGBOrTB(string sizeString)
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

        /// <summary>
        /// A enum type for filetype
        /// </summary>
        public enum FileType
        {
            Image = 1,
            Flash,
            Doc,
            Excel,
            Unknown
        }

        public static FileType ConvertFileExtensionToFileType(string filename)
        {
            string extension = filename.ToLower().Substring(FindLastIndexOf(filename, ".") + 1);

            if (extension == "jpg" || extension == "jpeg" || extension == "png" || extension == "gif" || extension == "bmp" || extension == "tif" || extension == "tiff")
                return FileType.Image;
            else if (extension == "doc" || extension == "docx")
                return FileType.Doc;
            else if (extension == "xls" || extension == "xlsx")
                return FileType.Excel;
            else if (extension == "swf")
                return FileType.Flash;
            else
                return FileType.Unknown;
        }

         public static ImageFormat GetFileFormat(string filename)
        {
            string extension = FileExtension(filename).ToLower().Replace(".", ""); // remove dot (.)

            if (extension == "jpg" || extension == "jpeg")
                return ImageFormat.Jpeg;
            else if (extension == "png")
                return ImageFormat.Png;
            else if (extension == "gif")
                return ImageFormat.Gif;
            else if (extension == "bmp")
                return ImageFormat.Bmp;
            else if (extension == "tif" || extension == "tiff")
                return ImageFormat.Tiff;
            else
                return null;
        }

         public static bool ValidExtension(string filename)
         {
             string extension = FileExtension(filename).ToLower().Replace(".", ""); // remove dot (.)
             if (extension == "jpg" || extension == "jpeg" || extension == "png" || extension == "gif")
                 return true;
             else
                 return false;
         }

        public static string FileExtension(string filename)
        {
            // find the index of '.' (dot) ex: myfile.png
            if (!string.IsNullOrEmpty(filename))
            {
                int dotTokenIndex = filename.IndexOf('.', filename.Length - 5);
                return filename.Substring(dotTokenIndex);
            }
            else
                return filename;
        }

        public static string FileNameWithoutExtension(string filename)
        {
            // find the index of '.' (dot) ex: myfile.png
            if (!string.IsNullOrEmpty(filename))
            {
                int dotTokenIndex = filename.LastIndexOf('.');
                return filename.Substring(0, dotTokenIndex);
            }
            else
                return filename;
        }

        public static bool IsNumeric(string stringToTest)
        {
            double newVal;
            return double.TryParse(stringToTest, NumberStyles.Any, NumberFormatInfo.InvariantInfo, out newVal);
        }

        public static bool IsInteger(string stringToTest)
        {
            int newVal;
            return int.TryParse(stringToTest, NumberStyles.Any, NumberFormatInfo.InvariantInfo, out newVal);
        }

        #endregion
    }
}
