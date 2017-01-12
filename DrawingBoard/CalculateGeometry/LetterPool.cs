using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawingBoard.CalculateGeometry
{
    internal class LetterPool
    {
        public static LetterPool Instance;
        private SortedSet<int> pool;
        List<string> list;
        static LetterPool()
        {
            Instance = new LetterPool();
        }
        private LetterPool()
        {
            int id = 0;
            list = new List<string>();
            pool = new SortedSet<int>();
            for (int i = 0; i < 9; ++i)
            {
                char s = (char)(i + '0');
                for (int j = 0; j < 26; ++j)
                {
                    char c = (char)(j + 'A');
                    if (i == 0)
                        list.Add(new string(new char[] { c }));
                    else
                        list.Add(new string(new char[] { c, s }));
                    pool.Add(id);
                    ++id;
                }
            }
        }
        public string Request()
        {
            string res = list[pool.First()];
            pool.Remove(pool.First());
            return res;
        }
        public void Recycle(string res)
        {
            if (list.IndexOf(res) == -1)
                throw new ArgumentException();
            pool.Add(list.IndexOf(res));
        }
    }
}
