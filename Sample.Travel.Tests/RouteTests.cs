using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Sample.Travel.Tests
{
    public sealed class RouteTests
    {
        [Fact]
        public void CorrentRouteShouldBeSortedSuccesfully()
        {
            var source = new Dictionary<string, string>
            {
                ["Мельбурн"] = "Кельн",
                ["Москва"] = "Париж",
                ["Кельн"] = "Москва"
            };
            var result = Route.Order(source.Select(p => (Source: new Waypoint(p.Key), Destination: new Waypoint(p.Value)))).ToImmutableList();
            result.Count.Should().Be(3);
            result[0].Source.Name.Should().Be("Мельбурн");
            result[1].Source.Name.Should().Be("Кельн");
            result[2].Source.Name.Should().Be("Москва");
            result[0].Destination.Name.Should().Be("Кельн");
            result[1].Destination.Name.Should().Be("Москва");
            result[2].Destination.Name.Should().Be("Париж");
        }
    }
}
