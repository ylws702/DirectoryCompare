using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using static System.Console;

namespace DirectoryCompare
{
    class Program
    {
        static int path1Lenth;
        static int path2Lenth;
        static int trueCount = 0;
        static int falseCount = 0;
        static void Main(string[] args)
        {
            Write("Path1:");
            string path1 = ReadLine();
            if (path1[0] == '\"')
            {
                path1 = path1.Substring(1, path1.Length - 2);
            }
            path1Lenth = path1.Length;
            Write("Path2:");
            string path2 = ReadLine();
            if (path2[0] == '\"')
            {
                path2 = path2.Substring(1, path2.Length - 2);
            }
            path2Lenth = path2.Length;
            Compare(path1, path2);
            ResetColor();
            WriteLine($"True:{trueCount}, False:{falseCount}");
        }
        static void Compare(string path1, string path2)
        {
            DirectoryInfo folder = new DirectoryInfo(path1);
            foreach (FileSystemInfo fileSystemInfo in folder.GetFileSystemInfos())
            {
                string newPath1 = fileSystemInfo.FullName;
                string newPath2 = $"{path2}\\{fileSystemInfo.Name}";
                if (fileSystemInfo is DirectoryInfo)
                {
                    Compare(newPath1, newPath2);
                }
                else
                {
                    string relativePath = newPath1.Substring(path1Lenth + 1);
                    try
                    {
                        if (SHA1(newPath1) == SHA1(newPath2))
                        {
                            ResetColor();
                            WriteLine($"TRUE: .\\{relativePath}");
                            trueCount++;
                        }
                        else
                        {
                            ForegroundColor = ConsoleColor.Red;
                            WriteLine($"FALSE: .\\{relativePath}");
                            falseCount++;
                        }
                    }
                    catch (Exception ex)
                    {
                        ForegroundColor = ConsoleColor.Red;
                        WriteLine($"FALSE: .\\{relativePath}");
                        WriteLine(ex.Message);
                        falseCount++;
                    }
                }
            }
        }
        static string SHA1(string path)
        {
            try
            {
                FileStream file = new FileStream(path, FileMode.Open);
                SHA512 sha512 = new SHA512CryptoServiceProvider();
                byte[] retval = sha512.ComputeHash(file);
                file.Close();

                StringBuilder sc = new StringBuilder();
                for (int i = 0; i < retval.Length; i++)
                {
                    sc.Append(retval[i].ToString("x2"));
                }
                return sc.ToString();
            }
            catch (FileNotFoundException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                ForegroundColor = ConsoleColor.Red;
                WriteLine(ex.Message);
                return null;
            }
        }
    }
}
