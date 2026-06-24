namespace Stone.Dto.Request
{
    public class ProductRequestDto
    {
        public string? ProductCode { get; set; }
        public string Description { get; set; }
        public decimal? Long { get; set; }
        public decimal? Width { get; set; }
        public decimal? Thickness { get; set; }
        public string? Color { get; set; }
        public string? UnitMeasurement { get; set; }
        public int? UnitValue { get; set; }
        public int? ManufacturerId { get; set; }
        public int? CategoryId { get; set; }
        public int? ProviderId { get; set; }
        
        #region Fabricante
        public string? ManufacturerDescription { get; set; }
        #endregion

        #region Categoria
        public string? CategoriaDescription { get; set; }
        #endregion

        #region Proveedor
        public int PersonId { get; set; }
        public int LocationId { get; set; }
        public int BankAccountId { get; set; }
        #endregion
    }
}
