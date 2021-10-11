using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace csharp_learning
{
    public static class EffectiveC__Item41
    {
        public static IEnumerable<string> ReadLines(this TextReader reader)
        {
            var text = reader.ReadLine();
            while (text != null)
            {
                yield return text;
                text = reader.ReadLine();
            }
        }

        public static int DefaultParse(this string input, int defaultValue)
        {
            return (int.TryParse(input, out var answer) ? answer : defaultValue);
        }

        public static IEnumerable<IEnumerable<int>> ReadNumbersFromStream(TextReader t)
        {
            var allines = from line in t.ReadLines() select line.Split(',');
            var matrixOfValue = from alline in allines select from item in alline select item.DefaultParse(0);
            return matrixOfValue;
        }

        public static IEnumerable<IEnumerable<int>> Use()
        {
            var t = new StreamReader(File.OpenRead("xxx.txt"));
            var arrayOfNumbers = ReadNumbersFromStream(t);

            //会报错，因为试图从已关闭的资源中读取内容
            IEnumerable<IEnumerable<int>> rowOfNumbers;
            using (TextReader text = new StreamReader(File.OpenRead("xxx.txt")))
                rowOfNumbers = ReadNumbersFromStream(text);
            foreach (var rowOfNumber in rowOfNumbers)
            {
                foreach (var i in rowOfNumber)
                {
                    Console.Write("");
                }
            }

            // 先读出rowOfNumbers中的内容
            using (TextReader text = new StreamReader(File.OpenRead("xxx.txt")))
            {
                rowOfNumbers = ReadNumbersFromStream(text);
                foreach (var rowOfNumber in rowOfNumbers)
                {
                    foreach (var i in rowOfNumber)
                    {
                        Console.Write("");
                    }
                }
            }

            //
            using (TextReader text = new StreamReader(File.OpenRead("xxx.txt")))
                return ReadNumbersFromStream(text);
        }

        // 直接在打开文件之后自己读取其中的内容，不是等调用方，这样可以控制关闭文件
        public static IEnumerable<string> ParseFile(string path)
        {
            using (TextReader text = new StreamReader(File.OpenRead("xxx.txt")))
            {
                var line = text.ReadLine();
                while (text != null)
                {
                    yield return line;
                    line = text.ReadLine();
                }
            }
        }

        public delegate TResult ProcessElementsFromFile<TResult>(IEnumerable<IEnumerable<int>> values);

        public static TResult ProcessFile<TResult>(string filePath, ProcessElementsFromFile<TResult> action)
        {
            using (TextReader text = new StreamReader(File.OpenRead("xxx.txt")))
            {
                var allines = from line in text.ReadLines() select line.Split(',');
                var matrixOfValue = from alline in allines select from item in alline select item.DefaultParse(0);
                return action(matrixOfValue);
            }
        }

        public static void AnothgerUse()
        {
            ProcessFile("xxx.txt", (arrayOfNumbers) =>
            {
                foreach (var arrayOfNumber in arrayOfNumbers)
                {
                    foreach (var i in arrayOfNumber)
                    {
                        Console.Write("");
                    }
                }

                return 0;
            });
        }

        public static int ImportantStatic(int input)
        {
            var filter = new ResourceHogFilter();
            return filter.PassFilter(input) ? input : 0;
        }

        private static int LeakingClosure(int mod)
        {
            var random = new Random();
            var result = random.Next(1,101);
            return result > ImportantStatic(result) ? result : -1;
        }
    }

    public class ResourceHogFilter : IDisposable
    {
        public void Dispose()
        {
            Console.Write("");
        }

        public bool PassFilter(int input)
        {
            if ((input % 2).Equals(0))
            {
                return true;
            }

            return false;
        }
    }
    
}