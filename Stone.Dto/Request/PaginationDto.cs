namespace Stone.Dto.Request
{
    public class PaginationDto
    {
        private readonly int MaxSize = 5;
        public int Page { get; set; } = 1;

        private int limit;

        public int Limit
        {
            get { return limit; }
            set { limit = (value > MaxSize) ? value : MaxSize; }
        }
    }
}
