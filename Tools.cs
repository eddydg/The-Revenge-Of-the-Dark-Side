using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
 
namespace EugLib
{
    public static class Tools
    {
        /**Prends en Parametres une chaine de caracteres
         * Retourne la liste de toutes les chaines separees par des espaces
         * */
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
        /**
         * Prends en parametre un nom de fichiers
         * Renvoie le contenu du fichier
         * Renvoie une chaine vide et cree le fichier
         * si il n'existait pas
         * */
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
        /**
         * Prends en parametres le nom et une chaine de caracteres
         * Ecrit la chaine de caracteres dans le fichier
         * Cree le fichier si il n'existe pas
         * */
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
        /**
         * Prends en parametre un objet
         * Ecrit le contenu de objet.ToString()
         * dans le fichier stdout.txt dans
         * le repertoire de l'executable
         * */
        public static void toStdOut(Object content)
        {
            writeFile("stdout.txt", readFile("stdout.txt") + System.Environment.NewLine + content.ToString());
        }
        /**
         * Cree le fichier stdout.txt ou le vide
         * */
        public static void clearStdOut()
        {
            writeFile("stdout.txt", "");
        }
    }
}
