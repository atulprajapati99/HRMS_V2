namespace HRMS_V2.Core.Paging;

public class PagingArgs
{
    public int PageIndex { get; set; }

    public int PageSize { get; set; }

    public PagingStrategy PagingStrategy { get; set; }
}