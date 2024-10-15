using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TimeAide.Common.Helpers
{
   public class Encryption
    {
        #region Constants and Fields

        /// <summary>
        /// The iv.
        /// </summary>
        private static readonly byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

        /// <summary>
        /// The encryption key.
        /// </summary>
        private static string EncryptionKey = "!5623a#de";

        /// <summary>
        /// The key.
        /// </summary>
        private static byte[] key = { };

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The decrypt.
        /// </summary>
        /// <param name="Input">
        /// The input.
        /// </param>
        /// <returns>
        /// The decrypt.
        /// </returns>
        /// <exception cref="Exception">
        /// </exception>
        public static string Decrypt(string Input)
        {
            var inputByteArray = new byte[Input.Length];
            try
            {
                key = Encoding.UTF8.GetBytes(EncryptionKey.Substring(0, 8));
                var des = new DESCryptoServiceProvider();
                inputByteArray = Convert.FromBase64String(Input.Replace(" ", "+"));
                var ms = new MemoryStream();
                var cs = new CryptoStream(ms, des.CreateDecryptor(key, IV), CryptoStreamMode.Write);

                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();

                Encoding encoding = Encoding.UTF8;
                return encoding.GetString(ms.ToArray());
            }
            catch 
            {
                return string.Empty;

            }
        }

        /// <summary>
        /// The encrypt.
        /// </summary>
        /// <param name="Input">
        /// The input.
        /// </param>
        /// <returns>
        /// The encrypt.
        /// </returns>
        /// <exception cref="Exception">
        /// </exception>
        public static string Encrypt(string Input)
        {
            try
            {
                key = Encoding.UTF8.GetBytes(EncryptionKey.Substring(0, 8));
                var des = new DESCryptoServiceProvider();
                byte[] inputByteArray = Encoding.UTF8.GetBytes(Input);
                var ms = new MemoryStream();
                var cs = new CryptoStream(ms, des.CreateEncryptor(key, IV), CryptoStreamMode.Write);

                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return Convert.ToBase64String(ms.ToArray());
            }
            catch 
            {
                return string.Empty;
            }
        }

        #endregion
        public static string EncryptURLParm(string parm)
        {
            var retVal = Encrypt(parm);
            retVal = retVal.Replace("/", ",").Replace("+","-");
            return retVal;
        }
        public static string DecryptURLParm(string parm)
        {
            var scrVal = parm.Replace(",", "/").Replace( "-", "+");
            return Decrypt(scrVal);
        }
    }
}
