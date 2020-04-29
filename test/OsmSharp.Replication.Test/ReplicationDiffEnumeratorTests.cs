using System;
using System.Threading.Tasks;
using Xunit;

namespace OsmSharp.Replication.Test
{
    public class ReplicationDiffEnumeratorTests
    {
         [Fact]
        public async Task ReplicationDiff_MoveTo_0_False()
        {
            Http.HttpHandler.Default = new ReplicationServerMockHttpHandler();
            var enumerator = new ReplicationDiffEnumerator(
                ReplicationConfig.Daily);
            Assert.False(await enumerator.MoveTo(0));
        }
        
        [Fact]
        public async Task ReplicationDiff_MoveTo_1_True()
        {
            Http.HttpHandler.Default = new ReplicationServerMockHttpHandler();
            var enumerator = new ReplicationDiffEnumerator(
                ReplicationConfig.Daily);
            Assert.True(await enumerator.MoveTo(1));
        }
        
        [Fact]
        public async Task ReplicationDiff_MoveTo_NonExisting_False()
        {
            Http.HttpHandler.Default = new ReplicationServerMockHttpHandler();
            var enumerator = new ReplicationDiffEnumerator(
                ReplicationConfig.Daily);
            Assert.False(await enumerator.MoveTo(
                ReplicationConfig.MaxSequenceNumber - 10249));
        }
        
        [Fact]
        public async Task ReplicationDiff_MoveTo_Last_True()
        {
            Http.HttpHandler.Default = new ReplicationServerMockHttpHandler();
            var enumerator = new ReplicationDiffEnumerator(
                ReplicationConfig.Daily);
            var latest = await ReplicationConfig.Daily.LatestReplicationState();
            Assert.True(await enumerator.MoveTo(
                latest.SequenceNumber));
        }
        
        [Fact]
        public async Task ReplicationDiff_MoveNext_1_2()
        {
            Http.HttpHandler.Default = new ReplicationServerMockHttpHandler();
            var enumerator = new ReplicationDiffEnumerator(
                ReplicationConfig.Daily);
            await enumerator.MoveTo(1);
            Assert.True(await enumerator.MoveNext());
            Assert.Equal(2, enumerator.State.SequenceNumber);
        }

        [Fact]
        public async Task ReplicationDiff_Daily_MoveTo_TimeStamp_ShouldOverlapTimestamp()
        {
            Http.HttpHandler.Default = new ReplicationServerMockHttpHandler();
            var enumerator = new ReplicationDiffEnumerator(
                ReplicationConfig.Daily);
            var timestamp = new DateTime(2019, 08, 3, 8, 15, 0);
            Assert.True(await enumerator.MoveTo(timestamp));
            Assert.True(enumerator.State.Overlaps(timestamp));
            Assert.Equal(2517, enumerator.State.SequenceNumber);
        }

        [Fact]
        public async Task ReplicationDiff_Hourly_MoveTo_TimeStamp_ShouldOverlapTimestamp()
        {
            Http.HttpHandler.Default = new ReplicationServerMockHttpHandler();
            var enumerator = new ReplicationDiffEnumerator(
                ReplicationConfig.Hourly);
            var timestamp = new DateTime(2019, 08, 3, 8, 15, 0);
            Assert.True(await enumerator.MoveTo(timestamp));
            Assert.True(enumerator.State.Overlaps(timestamp));
            Assert.Equal(60386, enumerator.State.SequenceNumber);
        }

        [Fact]
        public async Task ReplicationDiff_Minutely_MoveTo_TimeStamp_ShouldOverlapTimestamp()
        {
            Http.HttpHandler.Default = new ReplicationServerMockHttpHandler();
            var enumerator = new ReplicationDiffEnumerator(
                ReplicationConfig.Minutely);
            var timestamp = new DateTime(2019, 09, 22, 15, 15, 0, DateTimeKind.Utc);
            Assert.True(await enumerator.MoveTo(timestamp));
            Assert.True(enumerator.State.Overlaps(timestamp));
            Assert.Equal(3682948, enumerator.State.SequenceNumber);
        }
        
        // TODO: figure out how to test this, we can't test this really because the enumerator will not continue until there is a next diff available.
//        [Fact]
//        public async Task ReplicationDiff_MoveNext_Last_False()
//        {
//            Http.HttpHandler.Default = new ReplicationServerMockHttpHandler();
//            var enumerator = new ReplicationDiffEnumerator(
//                Tiled.Replication.Replication.Daily);
//            var latest = await Tiled.Replication.Replication.Daily.LatestReplicationState();
//            await enumerator.MoveTo(latest.SequenceNumber);
//            Assert.False(await enumerator.MoveNext());
//        }
    }
}