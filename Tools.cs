using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
 
namespace EugLib
{
    public static class Tools
    {
        public static List<string> toArgv(string args)
        {
            List<string> ret = new List<string>();
            string item = "";
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == ' ' && item != "")
                {
                    ret.Add(item);
                    item = "";
                }
                else
                {
                    if (args[i] != ' ')
                        item += args[i];
                    if (i == args.Length - 1 && item != "")
                        ret.Add(item);
                }
            }
            if (ret.Count == 0)
                ret.Add("");
            return ret;
        }
    }
    public class FileStream
    {
        public static string readFile(string name)
        {
            try
            {
                System.IO.StreamReader instream = new System.IO.StreamReader(name);
                string str = instream.ReadToEnd();
                instream.Close();
                return str;
            }
            catch (System.IO.FileNotFoundException)
            {
                writeFile(name, "");
                return "";
            }
            catch (System.IO.DirectoryNotFoundException)
            {
                writeFile(name, "");
                return "";
            }
            catch (Exception)
            {
                Console.WriteLine("FileStream.readFile : Erreur lors de la lecture du fichier " + name);
                return "";
            }
        }
        public static void writeFile(string name, string content)
        {
            try
            {
                System.IO.StreamWriter outstream = new System.IO.StreamWriter(name);
                outstream.Write(content);
                outstream.Close();
            }
            catch (System.IO.DirectoryNotFoundException)
            {
                try
                {
                    System.IO.Directory.CreateDirectory(name);
                    System.IO.Directory.Delete(name);
                    writeFile(name, content);
                }
                catch (Exception)
                {
                    Console.WriteLine("FileStream.writeFile : Erreur lors de la creation du repertoire " + name);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("FileStream.writeFile : Erreur lors de l'ecriture dans le fichier " + name);
            }
        }
        public static void toStdOut(Object content)
        {
            writeFile("stdout.txt", readFile("stdout.txt") + System.Environment.NewLine + content.ToString());
        }
        public static void clearStdOut()
        {
            writeFile("stdout.txt", "");
        }
    }
}
