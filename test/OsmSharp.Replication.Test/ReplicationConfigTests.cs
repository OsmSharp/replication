using System;
using System.Threading.Tasks;
using Xunit;

namespace OsmSharp.Replication.Test
{
    public class ReplicationConfigTests
    {
        [Fact]
        public void ReplicationConfig_Minutely_ShouldBeMinutely()
        {
            var replicationConfig = ReplicationConfig.Minutely;
            
            Assert.Equal(60, replicationConfig.Period);
            Assert.Equal("https://planet.openstreetmap.org/replication/minute/", replicationConfig.Url);
            Assert.True(replicationConfig.IsMinutely);
            Assert.False(replicationConfig.IsHourly);
            Assert.False(replicationConfig.IsDaily);
        }
        
        [Fact]
        public void ReplicationConfig_Hourly_ShouldBeHourly()
        {
            var replicationConfig = ReplicationConfig.Hourly;
            
            Assert.Equal(3600, replicationConfig.Period);
            Assert.Equal("https://planet.openstreetmap.org/replication/hour/", replicationConfig.Url);
            Assert.False(replicationConfig.IsMinutely);
            Assert.True(replicationConfig.IsHourly);
            Assert.False(replicationConfig.IsDaily);
        }
        
        [Fact]
        public void ReplicationConfig_Daily_ShouldBeDaily()
        {
            var replicationConfig = ReplicationConfig.Daily;
            
            Assert.Equal(24 * 3600, replicationConfig.Period);
            Assert.Equal("https://planet.openstreetmap.org/replication/day/", replicationConfig.Url);
            Assert.False(replicationConfig.IsMinutely);
            Assert.False(replicationConfig.IsHourly);
            Assert.True(replicationConfig.IsDaily);
        }

        [Fact]
        public async Task ReplicationConfig_GetReplicationState_0_ShouldReturnNull()
        {
            Http.HttpHandler.Default = new ReplicationServerMockHttpHandler();
            var result = await ReplicationConfig.Daily.GetReplicationState(0);
            Assert.Null(result);
        }

        [Fact]
        public async Task ReplicationConfig_GetReplicationState_SmallerThanZero_ShouldReturnNull()
        {
            Http.HttpHandler.Default = new ReplicationServerMockHttpHandler();
            var result = await ReplicationConfig.Daily.GetReplicationState(-10);
            Assert.Null(result);
        }

        [Fact]
        public async Task ReplicationConfig_GetReplicationState_AboveMax_ShouldReturnNull()
        {
            Http.HttpHandler.Default = new ReplicationServerMockHttpHandler();
            var result = await ReplicationConfig.Daily.GetReplicationState(long.MaxValue);
            Assert.Null(result);
        }

        [Fact]
        public async Task ReplicationConfig_GetReplicationState_NonExisting_ShouldReturnNull()
        {
            Http.HttpHandler.Default = new ReplicationServerMockHttpHandler();
            var result = await ReplicationConfig.Daily.GetReplicationState(ReplicationConfig.MaxSequenceNumber - 100024);
            Assert.Null(result);
        }
        
        [Fact]
        public void ReplicationConfig_MinutelyConfig_ShouldReturnMinutelyData()
        {
            var replicationConfig = new ReplicationConfig("https://planet.openstreetmap.org/replication/minute/", 60);
            
            Assert.Equal(60, replicationConfig.Period);
            Assert.Equal("https://planet.openstreetmap.org/replication/minute/", replicationConfig.Url);
            Assert.True(replicationConfig.IsMinutely);
            Assert.False(replicationConfig.IsHourly);
            Assert.False(replicationConfig.IsDaily);
        }
        
        [Fact]
        public void ReplicationConfig_HourlyConfig_ShouldReturnHourlyData()
        {
            var replicationConfig = new ReplicationConfig("https://planet.openstreetmap.org/replication/hour/", 3600);
            
            Assert.Equal(3600, replicationConfig.Period);
            Assert.Equal("https://planet.openstreetmap.org/replication/hour/", replicationConfig.Url);
            Assert.False(replicationConfig.IsMinutely);
            Assert.True(replicationConfig.IsHourly);
            Assert.False(replicationConfig.IsDaily);
        }
        
        [Fact]
        public void ReplicationConfig_DailyConfig_ShouldReturnDailyData()
        {
            var replicationConfig = new ReplicationConfig("https://planet.openstreetmap.org/replication/day/", 24 * 3600);
            
            Assert.Equal(24 * 3600, replicationConfig.Period);
            Assert.Equal("https://planet.openstreetmap.org/replication/day/", replicationConfig.Url);
            Assert.False(replicationConfig.IsMinutely);
            Assert.False(replicationConfig.IsHourly);
            Assert.True(replicationConfig.IsDaily);
        }
        
        [Fact]
        public async Task ReplicationConfig_DailyConfig_LatestShouldGiveStateSequence()
        {
            Http.HttpHandler.Default = new ReplicationServerMockHttpHandler();
            var replicationConfig = new ReplicationConfig("https://planet.openstreetmap.org/replication/day/", 24 * 3600);

            var result = await replicationConfig.LatestReplicationState();
            
            Assert.Equal(2569, result.SequenceNumber);
        }

        [Fact]
        public async Task ReplicationConfig_DailyConfig_GuessSequenceNumberAt_ShouldReturnSequenceNumberContainingDay()
        {
            Http.HttpHandler.Default = new ReplicationServerMockHttpHandler();
            var replicationConfig = new ReplicationConfig("https://planet.openstreetmap.org/replication/day/", 24 * 3600);

            var result = await replicationConfig.GuessSequenceNumberAt(new DateTime(2019, 08, 3, 8, 15, 0));
            
            Assert.Equal(2517, result);
        }

        [Fact]
        public async Task ReplicationConfig_HourlyConfig_GuessSequenceNumberAt_ShouldReturnSequenceNumberContainingHour()
        {
            Http.HttpHandler.Default = new ReplicationServerMockHttpHandler();
            var replicationConfig = new ReplicationConfig("https://planet.openstreetmap.org/replication/hour/", 3600);

            var result = await replicationConfig.GuessSequenceNumberAt(new DateTime(2019, 08, 3, 8, 15, 0));
            
            Assert.Equal(60386, result);
        }

        [Fact]
        public async Task ReplicationConfig_MinutelyConfig_GuessSequenceNumberAt_ShouldReturnSequenceNumberContainingMinute()
        {
            Http.HttpHandler.Default = new ReplicationServerMockHttpHandler();
            var replicationConfig = new ReplicationConfig("https://planet.openstreetmap.org/replication/minute/", 60);

            var result = await replicationConfig.GuessSequenceNumberAt(new DateTime(2019, 08, 3, 8, 15, 0));
            
            Assert.Equal(3610524, result);
        }

        [Fact]
        public async Task ReplicationConfig_DailyConfig_GetReplicationState_ShouldReturnSequenceNumberAndProperDate()
        {
            Http.HttpHandler.Default = new ReplicationServerMockHttpHandler();
            var replicationConfig = new ReplicationConfig("https://planet.openstreetmap.org/replication/day/", 24 * 3600);

            var result = await replicationConfig.GetReplicationState(2517);

            Assert.NotNull(result);
            Assert.Equal(2517, result.SequenceNumber);
            Assert.Equal(new DateTime(2019, 08, 04, 0, 0, 0, DateTimeKind.Utc), result.Timestamp);
            Assert.Equal(replicationConfig, result.Config);
        }

        [Fact]
        public async Task ReplicationConfig_DailyConfig_DownloadDiff_ShouldReturnParsedOsmChange()
        {
            Http.HttpHandler.Default = new ReplicationServerMockHttpHandler();
            var replicationConfig = new ReplicationConfig("https://planet.openstreetmap.org/replication/day/", 24 * 3600);

            var result = await replicationConfig.DownloadDiff(2517);
            
            Assert.NotNull(result);
            Assert.NotNull(result.Modify);
            Assert.NotNull(result.Create);
            Assert.NotNull(result.Delete);
        }
    }
}
