namespace WebShop.Application.Dto.Framework
{
    public class ResultGetListDto<T>
    {
        public List<T> List { get; set; }
        public int rowsCount { get; set; }
    }
}
