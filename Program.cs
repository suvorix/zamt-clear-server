using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ClearServer
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("\n\n**********************************************************************");
            Console.WriteLine("*                          ClearServer                               *");
            Console.WriteLine("*                                                                    *");
            Console.WriteLine("* Программа ClearServer предназначена для отчистки сервера от мусора *");
            Console.WriteLine("*                                                                    *");
            Console.WriteLine("**********************************************************************\n\n");

            string directory = "";
            try 
            {
                Console.Write("Введите путь (Если оставить поле пустым то программа будет работать \nв директории в которой она находится): ");
                directory = Console.ReadLine();
            }
            catch(Exception)
            {
                Console.WriteLine("Возникла ошибка при получении адреса");
            }

            if (directory == "") { directory = Directory.GetCurrentDirectory(); }

            Console.WriteLine();

            string[] allFiles = recSearch(directory);

            if (allFiles.Length != 0)
            {
                foreach (string el in allFiles)
                {
                    Console.WriteLine(el);
                }

                Console.Write("\nВы точно хотите удалить эти файлы? (y\\n): ");
                string res = Console.ReadLine();

                if (res == "y") 
                {
                    foreach (string el in allFiles)
                    {
                        Console.WriteLine("[DELETED]: " + el);
                        delElements(el);
                    }
                }
                else if (res == "n")
                {
                    Console.WriteLine("Файлы не были удалены");
                }
                else 
                {
                    Console.WriteLine("Неизвестный ответ");
                }
            }
            else 
            {
                Console.WriteLine("Папки не найдены");
            }

            Console.Write("\n\nДля выхода из программы нажмите любую кнопку . . . ");
            Console.ReadKey();
        }

        public static string[] recSearch(string path)
        {
            try
            {
                string[] bins = Directory.GetDirectories(path, "bin", SearchOption.AllDirectories);
                bins = clearArr(bins, path);
                string[] objs = Directory.GetDirectories(path, "obj", SearchOption.AllDirectories);
                objs = clearArr(objs, path);
                string[] vs = Directory.GetDirectories(path, ".vs", SearchOption.AllDirectories);
                vs = clearArr(vs, path);
                string[] hideFiles = Directory.GetFiles(path, "*.suo", SearchOption.AllDirectories);

                string[] files = new string[bins.Length + objs.Length + vs.Length + hideFiles.Length];

                for (int j = 0; j < bins.Length; j++) { files[j] = bins[j]; }
                for (int j = 0; j < objs.Length; j++) { files[bins.Length + j] = objs[j]; }
                for (int j = 0; j < vs.Length; j++) { files[bins.Length + objs.Length + j] = vs[j]; }
                for (int j = 0; j < hideFiles.Length; j++) { files[bins.Length + objs.Length + vs.Length + j] = hideFiles[j]; }
                
                return files;
            }
            catch(Exception)
            {
                Console.WriteLine("Возникла ошибка при поиске файлов и папок");
            }
            return null;
        }

        public static string[] clearArr(string[] arr, string startDir) 
        {
            try
            {
                int newArrayLength = 0;
                

                foreach(string el in arr)
                {
                    if (!parentDir(el, startDir))
                    {
                        newArrayLength++; 
                    }
                }

                string[] newArray = new string[newArrayLength];

                int j = 0;

                for (int i = 0; i < arr.Length; i++)
                {
                    if (!parentDir(arr[i], startDir))
                    {
                        newArray[j] = arr[i];
                        j++;
                    }
                }
                return newArray;
            }
            catch(Exception)
            {
                Console.WriteLine("Возникла ошибка при отчистки массива");
            }
            return null;
        }

        public static bool parentDir(string path, string startDir)
        {
            try 
            {
                string dir = path;
                while (dir != startDir)
                {
                    string dirName = new DirectoryInfo(dir).Name;
                    if (dirName == "БД")
                    {
                        return true;
                    }
                    dir = Directory.GetParent(dir).ToString();
                }
            }
            catch(Exception)
            {
                Console.WriteLine("Возникла ошибка при проверки родительских директорий");
            }
            return false;
        }

        public static void delElements(string path)
        {
            try {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                else 
                {
                    System.IO.DirectoryInfo di = new DirectoryInfo(path);

                    foreach (FileInfo file in di.GetFiles())
                    {
                        file.Delete();
                    }
                    foreach (DirectoryInfo dir in di.GetDirectories())
                    {
                        dir.Delete(true);
                    }
                    Directory.Delete(path);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Возникла ошибка при удалении файлов в папке bin или obj");
            }
        }
    }
}
