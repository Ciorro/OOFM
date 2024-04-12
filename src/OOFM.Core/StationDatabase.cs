﻿using OOFM.Core.Api.Models;
using System.Collections;

namespace OOFM.Core;

public class StationDatabase : IStationDatabase
{
    private readonly List<Station> _stations = new();

    bool ICollection<Station>.IsReadOnly => false;
    int ICollection<Station>.Count => _stations.Count;

    public void Add(Station station) 
        => _stations.Add(station);

    public Station? GetStationById(int id) 
        => _stations.FirstOrDefault(s => s.Id == id);

    public Station? GetStationBySlug(string slug) 
        => _stations.FirstOrDefault(s => s.Slug == slug);

    public IList<Station> GetStationsById(params int[] ids) 
        => _stations.Where(s => ids.Contains(s.Id)).ToList();

    public IList<Station> GetStationsBySlug(params string[] slugs) 
        => _stations.Where(s => slugs.Contains(s.Slug)).ToList();

    public bool Contains(Station station) 
        => _stations.Contains(station);

    public bool Remove(Station station)
        => _stations.Remove(station);

    public void Clear() 
        => _stations.Clear();

    void ICollection<Station>.CopyTo(Station[] array, int arrayIndex) 
        => _stations.CopyTo(array, arrayIndex);

    public IEnumerator<Station> GetEnumerator() 
        => _stations.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() 
        => _stations.GetEnumerator();
}
