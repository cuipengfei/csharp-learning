using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using Xunit;

namespace csharp_learning
{
    // 尽量减少公有API中的动态对象
    public class MoreEffectiveC__Item47
    {
        private dynamic answer = Add(5, 5);

        public static dynamic Add(dynamic left, dynamic right)
        {
            return left + right;
        }

        public static int AddInt(int left, int right)
        {
            return left + right;
        }

        [Fact]
        public void TimeCompare()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < 10000; i++)
            {
                Add(5, 10);
            }

            stopwatch.Stop();
            var time1 = stopwatch.Elapsed.TotalMilliseconds;

            stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < 10000; i++)
            {
                AddInt(5, 10);
            }

            stopwatch.Stop();
            var time2 = stopwatch.Elapsed.TotalMilliseconds;
            // Assert.True(time1 > time2); // should correct
        }

        // 可以用泛型将Add改造一下
        public static TResult Add2<T1, T2, TResult>(T1 left, T2 right)
        {
            dynamic result = Add(left, right);
            return (TResult) result;
        }

        // https://github.com/JoshClose/CsvHelper
    }

    // 因为无法得知每一条记录有多少字段，叫什么名字是什么类型，所以用动态类型
    public class CSVDataContainer
    {
        private class CSVRow : DynamicObject
        {
            private List<(string, string)> values = new List<(string, string)>();

            public CSVRow(IEnumerable<string> headers, IEnumerable<string> items)
            {
                values.AddRange(headers.Zip(items, (header, value) => (header, value)));
            }

            public override bool TryGetMember(GetMemberBinder binder, out object result)
            {
                var answer = values.FirstOrDefault(n => n.Item1 == binder.Name);
                result = answer.Item2;
                return result != null;
            }
        }

        private List<string> columnNames = new List<string>();
        private List<CSVRow> data = new List<CSVRow>();

        public CSVDataContainer(System.IO.TextReader stream)
        {
            // read header
            var headers = stream.ReadLine();
            columnNames = (from header in headers.Split(',')
                select header.Trim()).ToList();
            var line = stream.ReadLine();
            while (line != null)
            {
                var items = line.Split(',');
                data.Add(new CSVRow(columnNames, items));
                line = stream.ReadLine();
            }
        }

        public dynamic this[int index] => data[index];
        public IEnumerable<dynamic> Rows => data;
    }
    // 能用静态类型尽量用静态类型
}