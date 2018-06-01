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
            var result = Route.Order(CreateWaypoints(source)).ToImmutableList();
            result.Count.Should().Be(3);
            result[0].Source.Name.Should().Be("Мельбурн");
            result[1].Source.Name.Should().Be("Кельн");
            result[2].Source.Name.Should().Be("Москва");
            result[0].Destination.Name.Should().Be("Кельн");
            result[1].Destination.Name.Should().Be("Москва");
            result[2].Destination.Name.Should().Be("Париж");
        }

        static IEnumerable<(Waypoint Source, Waypoint Destination)> CreateWaypoints(Dictionary<string, string> source) 
            => source.Select(p => (Source: new Waypoint(p.Key), Destination: new Waypoint(p.Value)));

        [Fact]
        public void CycleShouldBeNotSorted()
        {
            var source = CreateWaypoints(new Dictionary<string, string>
            {
                ["Мельбурн"] = "Кельн",
                ["Москва"] = "Мельбурн",
                ["Кельн"] = "Москва"
            });
            Assert.ThrowsAny<Exception>(() => Route.Order(source));
        }

        [Fact]
        public void ForkShouldBeNotSorted()
        {
            var source = CreateWaypoints(new Dictionary<string, string>
            {
                ["Мельбурн"] = "Париж",
                ["Москва"] = "Мельбурн",
                ["Кельн"] = "Мельбурн"
            });
            Assert.ThrowsAny<Exception>(() => Route.Order(source));
        }

        [Fact]
        public void MissShouldBeNotSorted()
        {
            var source = CreateWaypoints(new Dictionary<string, string>
            {
                ["Мельбурн"] = "Париж",
                ["Кельн"] = "Москва"
            });
            Assert.ThrowsAny<Exception>(() => Route.Order(source));
        }

    }
}
