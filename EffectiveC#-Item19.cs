using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace csharp_learning
{
    /**
     * 通过运行期类型检查实现特定的泛型算法
     */
    public class ReverseEnumerable<T> : IEnumerable<T>
    {
        private class ReverseEnumerator : IEnumerator<T>
        {
            private int currentIndex;
            private IList<T> collection;

            public ReverseEnumerator(IList<T> srcCollection)
            {
                this.collection = srcCollection;
                this.currentIndex = collection.Count;
            }

            public bool MoveNext() => --currentIndex >= 0;

            public void Reset() => currentIndex = collection.Count;
            
            public T Current => collection[currentIndex];

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                throw new System.NotImplementedException();
            }
        }
        
        private sealed class ReverseStringEnumerator : IEnumerator<char>
        {
            private string sourceSequence;
            private int currentIndex;

            public ReverseStringEnumerator(string sourceSequence)
            {
                this.currentIndex = sourceSequence.Length;
                this.sourceSequence = sourceSequence;
            }

            public bool MoveNext() => --currentIndex >= 0;

            public void Reset() => currentIndex = sourceSequence.Length;

            public char Current => sourceSequence[currentIndex];

            object IEnumerator.Current => sourceSequence[currentIndex];

            public void Dispose()
            {
                throw new System.NotImplementedException();
            }
        }

        private IEnumerable<T> sourceSequence;
        private IList<T> originalSequence;

        public ReverseEnumerable(IEnumerable<T> sequence)
        {
            this.sourceSequence = sequence;
            
            // If sequence doesn't implement IList<T>, originalSequence is null, so this works fine
            originalSequence = sequence as IList<T>;
        }
        
        public ReverseEnumerable(IList<T> sequence)
        {
            this.sourceSequence = sequence;
            
            // If sequence doesn't implement IList<T>, originalSequence is null, so this works fine
            originalSequence = sequence;
        }

        public IEnumerator<T> GetEnumerator()
        {
            //string is a special case;
            if (sourceSequence is string sequence)
            {
                return new ReverseStringEnumerator(sequence) as IEnumerator<T>;
            }
            
            // create a copy of the original sequence
            // so it can be reversed
            if (originalSequence == null)
            {
                if (sourceSequence is ICollection<T>)
                {
                    ICollection<T> source = sourceSequence as ICollection<T>;
                    originalSequence = new List<T>(source.Count);
                }
                else
                {
                    originalSequence = new List<T>();
                }
                foreach (T item in sourceSequence)
                {
                    originalSequence.Add(item);
                }
            }

            return new ReverseEnumerator(originalSequence);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}