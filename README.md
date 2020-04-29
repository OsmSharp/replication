# replication

[![Build status](https://build.anyways.eu/app/rest/builds/buildType:(id:Osmsharp_Replication)/statusIcon)](https://build.anyways.eu/viewType.html?buildTypeId=Osmsharp_Replication2)  

A package to use the OSM replication infrastructure.

- OsmSharp.Replication: [![NuGet Badge](https://buildstats.info/nuget/OsmSharp.Replication)](https://www.nuget.org/packages/OsmSharp.Replication/) [![NuGet Badge](https://buildstats.info/nuget/OsmSharp.Replication?includePreReleases=true)](https://www.nuget.org/packages/OsmSharp.Replication)  

OsmSharp enables you to use the OSM replication system from the OSM [planet server](https://planet.openstreetmap.org/) (and similar services) to keep local OSM data up to date. This package allows to:

- Get all changes between two dates/times.
- Download the associated diffs.

OsmSharp can then be used to apply the diffs.

### Install

    PM> Install-Package OsmSharp.Replication


### Usage

The most common use case is to stream diffs from a date/time in the past:  

```csharp
var thePast = DateTime.Now.AddHours(-2).AddDays(-5);
var catchupEnumerator = new CatchupReplicationDiffEnumerator(thePast);

while (await catchupEnumerator.MoveNext())
{
    var current = catchupEnumerator.State;

    var diff = await catchupEnumerator.Diff();
    
    // do something with the diff here!
}
```
