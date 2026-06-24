using Stone.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stone.Dto.Request
{
    public class MaterialRequestDto
    {
        public int? Id { get; set; }
        public bool Active { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public string? ProductCode { get; set; }
        public string Description { get; set; }
        public int? UnitMeasurement { get; set; }
        public decimal? Quantity { get; set; }
        public int? UnitValue { get; set; }
        public int? TotalValue { get; set; }
        public int? DocumentType { get; set; }//guia recepcion, guia despacho, vale de material
        public int? ProductId { get; set; }
        public int? VoucherId { get; set; }
        public int? ReceptionId { get; set; }
        public int? DispatchId { get; set; }
    }
}
