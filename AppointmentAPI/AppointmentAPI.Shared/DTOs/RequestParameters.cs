namespace AppointmentAPI.Shared.DTOs;

public abstract class RequestParameters
{
    const int maxPageSizae = 50;

    private int _pageNumber = 1;
    private int _pageSize = 15;

    public int PageNumber
    {
        get { return _pageNumber; }
        set { _pageNumber = (value < 1) ? _pageNumber : value; }
    }
    public int PageSize
    {
        get { return _pageSize; }
        set { _pageSize = (value > maxPageSizae) ? maxPageSizae : value; }
    }
}
