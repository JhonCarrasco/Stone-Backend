namespace Stone.Dto.Request
{
    public class PaginationDto
    {
        private readonly int MinSize = 10;
        public int OffSet { get; set; }
        private int limit;

        public int Limit
        {
            get { return limit; }
            set { limit = (value > MinSize) ? value : MinSize; }
        }

        //public int OffSet
        //{
        //    get { return offset; }
        //    set { offset = (value > 0) ? value : 1; }
        //}
    }
}
