using System.Text.Json.Serialization;

namespace Stone.Api.Enums
{
    /// <summary>
    /// Enumerador para definir tipo documento
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum DocumentType
    {
        /// <summary>
        /// Código para definir Factura Electrónica
        /// </summary>
        ElectronicInvoice = 1,
        /// <summary>
        /// Código para definir guia de recepción
        /// </summary>
        ReceptionGuide = 2,
        /// <summary>
        /// Código para definir guia de despacho
        /// </summary>
        DispatchGuide = 3,
        /// <summary>
        /// Código para definir guia de devolución
        /// </summary>
        ReturnGuide = 4,
        /// <summary>
        /// Código para definir Voucher material
        /// </summary>
        Voucher = 5,
        /// <summary>
        /// Código para definir Consignación de Recepción
        /// </summary>
        ReceiptConsignment = 6,
        /// <summary>
        /// Código para definir Consignación de Devolución
        /// </summary>
        ReturnConsignment = 7,
        /// <summary>
        /// Código para definir Presupuesto
        /// </summary>
        Budget = 8,
        /// <summary>
        /// Rendición de Gastos
        /// </summary>
        ExpenseReceipt = 9,
    }
}
