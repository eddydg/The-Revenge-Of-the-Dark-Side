using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EugLib.IO
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
        /// Genere  un fichier .spritefont
        /// </summary>
        /// <param name="outputFile">Fichier de sortie</param>
        /// <param name="fontName">Nom de la police</param>
        /// <param name="size">Taille de la police</param>
        /// <param name="style">Style de la police (ex : "Bold,Italic" )</param>
        public static void SpriteFontGenerator(string outputFile, string fontName = "Impact", int size = 14, string style = "Regular")
        {
            String xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                            "<XnaContent xmlns:Graphics=\"Microsoft.Xna.Framework.Content.Pipeline.Graphics\">" +
                            "<Asset Type=\"Graphics:FontDescription\">" +
                            " <FontName>";
            xml += fontName;
            xml += "</FontName><Size>";
            xml += size.ToString();
            xml+="</Size><Spacing>0</Spacing><UseKerning>true</UseKerning><Style>";
            xml += style;
            xml += "</Style><CharacterRegions><CharacterRegion><Start>&#32;</Start><End>&#256;</End></CharacterRegion></CharacterRegions></Asset></XnaContent>";
            FileStream.writeFile(outputFile, xml);
        }
    }
    public class FileStream
    {

        /// <summary>
        /// Prends en parametre un nom de fichiers
        /// Renvoie le contenu du fichier
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
            catch (Exception)
            {
                Console.WriteLine("FileStream.readFile : Erreur lors de la lecture du fichier " + name);
                return "";
            }
        }

        public static List<string> readFileLines(string name)
        {
            List<string> ret = new List<string>();
            string str = "";
            try
            {
                System.IO.StreamReader instream = new System.IO.StreamReader(name);
                while ((str = instream.ReadLine()) != null)
                {
                    ret.Add(str);
                }
                instream.Close();
            }
            catch (Exception)
            {
                Console.WriteLine("FileStream.readFile : Erreur lors de la lecture du fichier " + name);
            }
            return ret;
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
