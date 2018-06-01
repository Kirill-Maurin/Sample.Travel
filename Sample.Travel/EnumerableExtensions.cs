using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Sample.Travel
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Generic topology sort
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="nodes"></param>
        /// <param name="getLinks"></param>
        /// <param name="order">Order if success, cycle if fail</param>
        /// <returns>True - success, False - fail (graph contains at least one cycle)</returns>
        public static bool TryTopologySort<T>(
            this IEnumerable<T> nodes,
            Func<T, IEnumerable<T>> getLinks,
            out IEnumerable<T> order) where T : class
        {
            var nodeList = nodes.ToImmutableList();
            if (nodeList.Count == 0)
            {
                order = Enumerable.Empty<T>();
                return true;
            }

            IEnumerator<T> next = nodeList.GetEnumerator();
            next.MoveNext();
            var path = new Stack<IEnumerator<T>>(new [] {next});
            var result = new List<T>();
            var colors = Enumerable.Range(0, nodeList.Count).ToDictionary(i => nodeList[i], i => Color.White);
            next = nodeList.Where(m => colors[m] == Color.White).GetEnumerator();
            var current = nodeList[0];
            for (; ; )
            {
                if (!next.MoveNext())
                {
                    if (path.Count == 1)
                        break;
                    // ReSharper disable once AssignNullToNotNullAttribute
                    colors[current] = Color.Black;
                    result.Add(current);
                    next = path.Pop();
                    current = path.Peek().Current;
                    continue;
                }
                current = next.Current;
                // ReSharper disable once AssignNullToNotNullAttribute
                if (colors[current] == Color.Gray)
                {
                    // ReSharper disable once PossibleNullReferenceException
                    order = path.Select(e => e.Current).TakeWhile(c => !c.Equals(current)).Concat(new[] { current });
                    return false;
                }
                path.Push(next);
                next = getLinks(current).Where(l => colors[l] != Color.Black).GetEnumerator();
                colors[current] = Color.Gray;
            }
            order = result;
            return true;
        }

        enum Color { White, Gray, Black }
    }
}
