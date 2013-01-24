using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
 
namespace EugLib
{
    public class Tools
    {

        /// <summary>
        /// Convertit une chaine en liste de chaines avec " " comme separateur.
        /// </summary>
        /// <param name="args">Chaine a separer</param>
        /// <returns>Liste de chaines</returns>
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

        /// <summary>
        /// Affiche tous les elements d'un tableau a 2 dimensions [X,Y]
        /// </summary>
        /// <typeparam name="T">Type des elements du tableau</typeparam>
        /// <param name="array">Tableau a ecrire</param>
        public static void ShowArray<T>(T[,] array)
        {
            Console.WriteLine();
            int xLen = array.GetLength(0);
            int yLen = array.GetLength(1);

            for (int i = 0; i < xLen; i++)
                Console.Write(i.ToString() + ' ');
            Console.WriteLine();

            for (int y = 0; y < yLen; y++)
            {
                for (int x = 0; x < xLen; x++)
                    Console.Write(array[x, y].ToString() + ' ');
                Console.Write(y.ToString() + '\n');
            }
        }

    }
    public class FileStream
    {

        /// <summary>
        /// Prends en parametre un nom de fichiers
		/// Renvoie le contenu du fichier
		/// Renvoie une chaine vide et cree le fichier
		/// si il n'existait pas
        /// </summary>
        /// <param name="name">Nom du fichier</param>
        /// <returns>Contenu du fichier</returns>
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

        /// <summary>
        /// Prends en parametres le nom et une chaine de caracteres
		/// Ecrit la chaine de caracteres dans le fichier
		/// Cree le fichier si il n'existe pas
        /// </summary>
        /// <param name="name">Nom du fichier</param>
        /// <param name="content">Contenu du fichier</param>
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

        /// <summary>
        /// Ajoute l'element a la fin du fichier stdout.txt
        /// </summary>
        /// <param name="content">content.ToString() sera ecrit a la fin du fichier</param>
        public static void toStdOut(Object content)
        {
            writeFile("stdout.txt", readFile("stdout.txt") + System.Environment.NewLine + content.ToString());
        }

        /// <summary>
        /// Cree le fichier stdout.txt ou le vide
        /// </summary>
        public static void clearStdOut()
        {
            writeFile("stdout.txt", "");
        }

    }
}