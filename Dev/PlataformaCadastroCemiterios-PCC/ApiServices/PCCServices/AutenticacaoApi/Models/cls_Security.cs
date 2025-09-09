namespace AutenticacaoApi.Models
{
    using System;
    using System.Security.Cryptography;
    using System.Text;


    public class cls_Security
    {
        public class cGetCrypto
        {
            private string wHashCode = "";
            private string wHashResult = "";
            private static readonly object _locker = new object();


            public string GetCrypto(string strValue)
            {
                lock (_locker)
                {
                    byte[] hashedDataBytes;
                    var encoder = new UTF8Encoding();
                    using (var md5Hasher = MD5.Create())
                    {
                        wHashCode = strValue;
                        hashedDataBytes = md5Hasher.ComputeHash(encoder.GetBytes(wHashCode));
                    }

                    wHashResult = BitConverter.ToString(hashedDataBytes);
                    wHashResult = wHashResult.Replace("-", "");

                    return wHashResult;
                }
            }

            public string Gen_Psw(int Lenght)
            {
                lock (_locker)
                {
                    var rNum = new Random();
                    var rLowerCase = new Random();
                    var rUpperCase = new Random();
                    var RandomSelect = new Random();

                    var psw = "";
                    int[] CNT = new int[3];
                    string[] Char_Sel = new string[3];
                    int iSel;

                    for (int i = 1; i <= Lenght; i++)
                    {
                        CNT[0] = rNum.Next(48, 57);
                        CNT[1] = rLowerCase.Next(65, 90);
                        CNT[2] = rUpperCase.Next(97, 122);

                        Char_Sel[0] = Convert.ToChar(CNT[0]).ToString();
                        Char_Sel[1] = Convert.ToChar(CNT[1]).ToString();
                        Char_Sel[2] = Convert.ToChar(CNT[2]).ToString();

                        iSel = RandomSelect.Next(0, 3);
                        psw += Char_Sel[iSel];
                        psw.Replace(psw, Char_Sel[iSel]);
                    }

                    return psw;
                }
            }

        }
    }

}
