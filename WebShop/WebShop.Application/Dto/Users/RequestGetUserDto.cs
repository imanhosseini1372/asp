namespace WebShop.Application.Dto.Users
{
    public class RequestGetUserDto 
    {
        public string SearchKey { get; set; }
        public int PageNum { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
