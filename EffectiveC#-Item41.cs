using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace csharp_learning
{
    public static  class EffectiveC__Item41
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
            //不会立刻返回值，而是要等程序用到真正的值再去查询并返回
            var t = new StreamReader(File.OpenRead("xxx.txt"));
            var arrayOfNumbers = ReadNumbersFromStream(t);

            //若理解错生命周期，释放资源，就会报错，因为试图从已关闭的资源中读取内容
            IEnumerable<IEnumerable<int>> rowOfNumbers;
            using (TextReader text = new StreamReader(File.OpenRead("xxx.txt")))
            {
                rowOfNumbers = ReadNumbersFromStream(text);
            }
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

            //要由接收返回方法的例程去关闭，就会不知道究竟在哪段代码处被关闭
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
            var result = 0;
            ProcessFile<int>("xxx.txt", (arrayOfNumbers) =>
            {
                foreach (var arrayOfNumber in arrayOfNumbers)
                {
                    foreach (var i in arrayOfNumber)
                    {
                        result += i;
                    }
                }

                return result;
            });
            // other operations
        }
        
        public static void ThirdUse()
        {
            ProcessFile<int>("xxx.txt", (arrayOfNumbers) =>
            {
                return (from arrayOfNumber in arrayOfNumbers
                    select arrayOfNumber.Max()).Max();
            });
            // other operations
        }

        public static int ImportantStatic(int input)
        {
            var filter = new ResourceHogFilter();
            return filter.PassFilter(input) ? input : 0;
        }

        private static IEnumerable<int> LeakingClosure(int mod)
        {
            return from n in GetNumbers(mod)
                where n > ImportantStatic(n)
                select n;
        }

        private static ICollection<int> GetNumbers(int num)
        {
            var result = new List<int>();
            for (int i = 0; i <= num; i++)
            {
                result.Add(new Random().Next(1,101));
            }

            return result;
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