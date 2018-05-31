using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Sample.Travel
{
    public static class Route
    {
        /// <summary>
        /// Order route
        /// Throw exception when route is incorrect: cycles, omissions, forks 
        /// </summary>
        /// <param name="route">Unordered segments</param>
        /// <returns>Ordered segments</returns>
        public static IEnumerable<(Waypoint Source, Waypoint Destination)> Order(IEnumerable<(Waypoint Source, Waypoint Destination)> route)
        {
            var backward = route.ToDictionary(s => s.Destination, s => s.Source);
            if (!backward.Keys.Union(backward.Values).TryTopologySort(s => backward.TryGetValue(s, out var d) ? new [] {d} : Enumerable.Empty<Waypoint>(), out var result))
                throw new ArgumentException($@"Cycle in route: {string.Join(",", result.Select(w => w.Name))}", nameof(route));
            var r = result.ToImmutableList();
            for (var i = 1; i < r.Count; i++)
                if (backward[r[i]] != r[i - 1])
                    throw new ArgumentException($@"Invalid route: expected segment {backward[r[i]]} -> {r[i]}, but source is {r[i - 1]} "); 
            return r.Skip(1).Select(w => (Source: backward[w], Destination: w));
        }
    }
}