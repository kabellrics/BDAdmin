﻿using System;

namespace ComicVineApi.Models
{
    public interface ISeriesFilterable
    {
        DateTimeOffset? DateAdded { get; set; }

        DateTimeOffset? DateLastUpdated { get; set; }

        int? Id { get; set; }

        string? Name { get; set; }
    }
}
