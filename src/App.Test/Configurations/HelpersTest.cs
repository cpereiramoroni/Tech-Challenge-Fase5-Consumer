using System.Collections.Generic;

namespace App.Test.Configurations
{
    public static class HelpersTest
    {
        public static List<T> ToList<T>(this IEnumerator<T> e)
        {
            var list = new List<T>();
            while (e.MoveNext())
            {
                list.Add(e.Current);
            }
            return list;
        }
    }

}

