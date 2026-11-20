using System.ComponentModel.DataAnnotations.Schema;

namespace Stone.Entities
{
    public class FileDrive : EntityBase
    {
        [Column("nombre_archivo")]
        public string FileName { get; set; }
        [Column("extension")]
        public string FileExtention { get; set; }
        [Column("tipo_contenido")]
        public string ContentType { get; set; }
        [Column("id_archivo")]
        public string FileId { get; set; }
        [Column("carpeta_padre_id")]
        public string FolderParentId { get; set; }
        [Column("documento_id")]
        public int DocumentId { get; set; }
        [Column("tipo_documento")]
        public int? DocumentType { get; set; }
        [Column("peso_byte")]
        public long? Size { get; set; }
    }
}
