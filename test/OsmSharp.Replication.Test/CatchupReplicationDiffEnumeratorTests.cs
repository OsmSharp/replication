using System;
using System.Threading.Tasks;
using Xunit;

namespace OsmSharp.Replication.Test
{
    public class CatchupReplicationDiffEnumeratorTests
    {
        [Fact]
        public async Task CatchupReplicationDiffEnumerator_MoveNext_Future_False()
        {
            Http.HttpHandler.Default = new ReplicationServerMockHttpHandler();
            var enumerator = new CatchupReplicationDiffEnumerator(
                new DateTime(2020, 01, 01));

            Assert.False(await enumerator.MoveNext());
        }

        [Fact]
        public async Task CatchupReplicationDiffEnumerator_MoveNext_FirstMinuteThenHourThenDayThenHourThenMinute()
        {
            Http.HttpHandler.Default = new ReplicationServerMockHttpHandler();
            var enumerator = new CatchupReplicationDiffEnumerator(
                new DateTime(2019, 09, 22, 20, 55, 0, DateTimeKind.Utc));

            // first minutes will happen.
            Assert.True(await enumerator.MoveNext());
            Assert.Equal(3683288, enumerator.State.SequenceNumber);
            Assert.True(await enumerator.MoveNext());
            Assert.Equal(3683289, enumerator.State.SequenceNumber);
            Assert.True(await enumerator.MoveNext());
            Assert.Equal(3683290, enumerator.State.SequenceNumber);
            Assert.True(await enumerator.MoveNext());
            Assert.Equal(3683291, enumerator.State.SequenceNumber);
            Assert.True(await enumerator.MoveNext());
            Assert.Equal(3683292, enumerator.State.SequenceNumber);
            Assert.True(await enumerator.MoveNext());
            Assert.Equal(3683293, enumerator.State.SequenceNumber);

            // then hours will happen.
            Assert.True(await enumerator.MoveNext());
            Assert.Equal(61599, enumerator.State.SequenceNumber);
            Assert.True(await enumerator.MoveNext());
            Assert.Equal(61600, enumerator.State.SequenceNumber);
            Assert.True(await enumerator.MoveNext());
            Assert.Equal(61601, enumerator.State.SequenceNumber);

            // then days will happen.
            Assert.True(await enumerator.MoveNext());
            Assert.Equal(2568, enumerator.State.SequenceNumber);
            Assert.True(await enumerator.MoveNext());
            Assert.Equal(2569, enumerator.State.SequenceNumber);

            // then hours again.
            Assert.True(await enumerator.MoveNext());
            Assert.Equal(61650, enumerator.State.SequenceNumber);
            Assert.True(await enumerator.MoveNext());
            Assert.Equal(61651, enumerator.State.SequenceNumber);
            Assert.True(await enumerator.MoveNext());
            Assert.Equal(61652, enumerator.State.SequenceNumber);
            Assert.True(await enumerator.MoveNext());
            Assert.Equal(61653, enumerator.State.SequenceNumber);
            Assert.True(await enumerator.MoveNext());
            Assert.Equal(61654, enumerator.State.SequenceNumber);
            Assert.True(await enumerator.MoveNext());
            Assert.Equal(61655, enumerator.State.SequenceNumber);
            Assert.True(await enumerator.MoveNext());
            Assert.Equal(61656, enumerator.State.SequenceNumber);
            Assert.True(await enumerator.MoveNext());
            Assert.Equal(61657, enumerator.State.SequenceNumber);
            Assert.True(await enumerator.MoveNext());
            Assert.Equal(61658, enumerator.State.SequenceNumber);
            Assert.True(await enumerator.MoveNext());
            Assert.Equal(61659, enumerator.State.SequenceNumber);
            Assert.True(await enumerator.MoveNext());
            Assert.Equal(61660, enumerator.State.SequenceNumber);
            Assert.True(await enumerator.MoveNext());
            Assert.Equal(61661, enumerator.State.SequenceNumber);
            Assert.True(await enumerator.MoveNext());
            Assert.Equal(61662, enumerator.State.SequenceNumber);
            Assert.True(await enumerator.MoveNext());
            Assert.Equal(61663, enumerator.State.SequenceNumber);

            // then minutes again.
            Assert.True(await enumerator.MoveNext());
            Assert.Equal(3687189, enumerator.State.SequenceNumber);
            Assert.True(await enumerator.MoveNext());
            Assert.Equal(3687190, enumerator.State.SequenceNumber);
            Assert.True(await enumerator.MoveNext());
            Assert.Equal(3687191, enumerator.State.SequenceNumber);
            Assert.True(await enumerator.MoveNext());
            Assert.Equal(3687192, enumerator.State.SequenceNumber);
            Assert.True(await enumerator.MoveNext());
            Assert.Equal(3687193, enumerator.State.SequenceNumber);
            Assert.True(await enumerator.MoveNext());
            Assert.Equal(3687194, enumerator.State.SequenceNumber);
            Assert.True(await enumerator.MoveNext());
            Assert.Equal(3687195, enumerator.State.SequenceNumber);
            Assert.False(await enumerator.MoveNext());
        }


        [Fact]
        public async Task CatchupReplicationDiffEnumerator_MoveNext_FewMinutesAfterMidnight()
        {
            Http.HttpHandler.Default = new ReplicationServerMockHttpHandler("data2");
            var enumerator = new CatchupReplicationDiffEnumerator(
                new DateTime(2023, 06, 02, 23, 57, 0, DateTimeKind.Utc));
            Assert.True(await enumerator.MoveNext());
            Assert.Equal(5602190, enumerator.State.SequenceNumber);
            Assert.True(await enumerator.MoveNext());
            Assert.Equal(5602191, enumerator.State.SequenceNumber);
            Assert.True(await enumerator.MoveNext());
            Assert.Equal(5602192, enumerator.State.SequenceNumber);
            Assert.True(await enumerator.MoveNext());
            Assert.Equal(5602193, enumerator.State.SequenceNumber);
            Assert.True(await enumerator.MoveNext());
            Assert.Equal(3917, enumerator.State.SequenceNumber);
            Assert.True(await enumerator.MoveNext());
            Assert.Equal(5603613, enumerator.State.SequenceNumber);
            Assert.True(await enumerator.MoveNext());
            Assert.Equal(5603614, enumerator.State.SequenceNumber);
            Assert.True(await enumerator.MoveNext());
            Assert.Equal(5603615, enumerator.State.SequenceNumber);
            Assert.True(await enumerator.MoveNext());
            Assert.Equal(5603616, enumerator.State.SequenceNumber);
            Assert.True(await enumerator.MoveNext());
            Assert.Equal(5603617, enumerator.State.SequenceNumber);
            Assert.True(await enumerator.MoveNext());
            Assert.Equal(5603618, enumerator.State.SequenceNumber);

            Assert.False(await enumerator.MoveNext());
        }
    }
}