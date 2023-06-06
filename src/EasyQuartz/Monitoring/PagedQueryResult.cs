﻿using System.Collections.Generic;

namespace EasyQuartz.Monitoring;

public class PagedQueryResult<T>
{
    public IList<T>? Items { get; set; }

    public long Totals { get; set; }

    public int PageIndex { get; set; }

    public int PageSize { get; set; }
}