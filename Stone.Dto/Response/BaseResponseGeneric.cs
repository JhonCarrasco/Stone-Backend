namespace Stone.Dto.Response
{
    public class BaseResponseGeneric<T> : BaseResponse
    {
        public T? Data { get; set; }
        public int? Pages { get; set; }
        public int? Count { get; set; }
    }
}
